using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BitWaves.Data;
using BitWaves.Data.Entities;
using BitWaves.WebAPI.Authentication;
using BitWaves.WebAPI.Extensions;
using BitWaves.WebAPI.Models;
using BitWaves.WebAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BitWaves.WebAPI.Controllers
{
    [Route("archive")]
    [ApiController]
    public sealed class ArchiveController : ControllerBase
    {
        /// <summary>
        /// 为保证 ArchiveId 的唯一性，在应用端设置的对象互斥锁。
        /// </summary>
        private static readonly object UpdateArchiveIdLock = new object();

        private readonly Repository _repo;

        public ArchiveController(Repository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetProblemList(
            [FromQuery] ArchiveListKey by = ArchiveListKey.Id,
            [FromQuery][Range(0, int.MaxValue)] int page = 0,
            [FromQuery][Range(1, int.MaxValue)] int itemsPerPage = 20)
        {
            var query = _repo.Problems.Find(Builders<Problem>.Filter.Ne(problem => problem.ArchiveId, null));

            // 执行排序
            var sortKey = by.GetFieldSelector();
            query = query.SortBy(sortKey);

            // 获取符合筛选条件的总题目数量
            var totalCount = await query.CountDocumentsAsync();

            // 执行分页
            query = query.Paginate(page, itemsPerPage);

            var viewEntityList = await query.ToListAsync();
            var viewList = viewEntityList.Select(entity => new ProblemInfo(entity, ProblemInfoScheme.List));

            return new ListResult<ProblemInfo>(totalCount, viewList);
        }

        [HttpGet("{archiveId}")]
        public async Task<IActionResult> GetArchiveProblem(
            int archiveId)
        {
            var entity = await _repo.Problems.Find(Builders<Problem>.Filter.Eq(problem => problem.ArchiveId, archiveId))
                                             .FirstOrDefaultAsync();
            if (entity == null)
            {
                return NotFound();
            }

            return new ObjectResult(new ProblemInfo(entity, ProblemInfoScheme.Full));
        }

        // FIXME: Remove lock blocks used below and use db's synchronization mechanisms instead.

        [HttpPost]
        [Authorize(Policy = BitWavesAuthPolicies.AdminOnly)]
        public IActionResult AddProblems([FromBody] ArchiveAddProblemModel[] model)
        {
            var succeeded = new List<ObjectId>();
            var notFound = new List<ObjectId>();
            var conflict = new List<ObjectId>();

            // 检查数据模型中是否有冲突的 ArchiveId
            var archiveIds = new Dictionary<int, ArchiveAddProblemModel>();
            var candidates = new List<ArchiveAddProblemModel>();
            foreach (var updateModel in model)
            {
                if (archiveIds.ContainsKey(updateModel.ArchiveId))
                {
                    conflict.Add(updateModel.ProblemId);
                }
                else
                {
                    archiveIds.Add(updateModel.ArchiveId, updateModel);
                    candidates.Add(updateModel);
                }
            }

            lock (UpdateArchiveIdLock)
            {
                // 检查数据模型与数据库中冲突的 ArchiveId
                var conflictIds = _repo.Problems.Find(
                                           Builders<Problem>.Filter.In(p => p.ArchiveId,
                                                                       archiveIds.Keys.Cast<int?>()))
                                       .Project(p => p.ArchiveId)
                                       .ToEnumerable()
                                       .Cast<int>()
                                       .ToHashSet();
                foreach (var conflictId in conflictIds)
                {
                    conflict.Add(archiveIds[conflictId].ProblemId);
                }

                // 更新数据库
                foreach (var updateModel in candidates.Where(x => !conflictIds.Contains(x.ArchiveId)))
                {
                    var (filter, update) = updateModel.CreateUpdateDefinition();
                    var entity = _repo.Problems.FindOneAndUpdate(filter, update);
                    if (entity == null)
                    {
                        notFound.Add(updateModel.ProblemId);
                    }
                    else
                    {
                        succeeded.Add(updateModel.ProblemId);
                    }
                }
            }

            if (succeeded.Count == model.Length)
            {
                return Ok();
            }
            else
            {
                return new ObjectResult(new { succeeded, notFound, conflict })
                {
                    StatusCode = (int) HttpStatusCode.UnprocessableEntity
                };
            }
        }

        [HttpDelete]
        [Authorize(Policy = BitWavesAuthPolicies.AdminOnly)]
        public IActionResult DeleteProblems([FromBody] int[] problemIds)
        {
            var ids = problemIds.ToHashSet();

            var succeeded = new List<int>();
            var notFound = new List<int>();

            lock (UpdateArchiveIdLock)
            {
                foreach (var archiveId in ids)
                {
                    var entity = _repo.Problems.FindOneAndUpdate(
                        Builders<Problem>.Filter.Eq(p => p.ArchiveId, archiveId),
                        Builders<Problem>.Update.Set(p => p.ArchiveId, null));
                    if (entity == null)
                    {
                        notFound.Add(archiveId);
                    }
                    else
                    {
                        succeeded.Add(archiveId);
                    }
                }
            }

            if (succeeded.Count == problemIds.Length)
            {
                return Ok();
            }
            else
            {
                return new ObjectResult(new { succeeded, notFound })
                {
                    StatusCode = (int) HttpStatusCode.UnprocessableEntity
                };
            }
        }
    }
}
