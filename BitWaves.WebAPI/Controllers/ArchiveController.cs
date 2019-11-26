using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BitWaves.Data;
using BitWaves.Data.Entities;
using BitWaves.WebAPI.Authentication;
using BitWaves.WebAPI.Extensions;
using BitWaves.WebAPI.Models;
using BitWaves.WebAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMapper _mapper;

        public ArchiveController(Repository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // GET: /archive
        [HttpGet]
        public async Task<PaginatedListActionResult<ProblemListInfo>> GetProblemList(
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
            var viewList = viewEntityList.Select(entity => _mapper.Map<Problem, ProblemListInfo>(entity));

            return (totalCount, viewList);
        }

        // GET: /archive/{archiveId}
        [HttpGet("{archiveId}")]
        public async Task<ActionResult<ProblemInfo>> GetArchiveProblem(
            int archiveId)
        {
            var entity = await _repo.Problems.Find(Builders<Problem>.Filter.Eq(problem => problem.ArchiveId, archiveId))
                                             .FirstOrDefaultAsync();
            if (entity == null)
            {
                return NotFound();
            }

            return _mapper.Map<Problem, ProblemInfo>(entity);
        }

        // POST: /archive
        [HttpPost]
        [Authorize(Policy = BitWavesAuthPolicies.AdminOnly)]
        public async Task<IActionResult> AddProblem(
            [FromBody] ArchiveAddProblemModel model)
        {
            UpdateResult updateResult;

            try
            {
                updateResult = await _repo.Problems.UpdateOneAsync(model.GetFilter(), model.GetUpdateDefinition());
            }
            catch (MongoWriteException ex)
            {
                if (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
                {
                    return Conflict();
                }

                throw;
            }

            if (updateResult.MatchedCount == 0)
            {
                return NotFound();
            }

            return Ok();
        }

        // DELETE: /archive
        [HttpDelete]
        [Authorize(Policy = BitWavesAuthPolicies.AdminOnly)]
        public async Task<IActionResult> DeleteProblems(
            [FromBody] int[] problemIds)
        {
            var filter = Builders<Problem>.Filter.In(p => p.ArchiveId, problemIds.Select<int, int?>(x => x));
            await _repo.Problems.UpdateManyAsync(filter, Builders<Problem>.Update.Unset(p => p.ArchiveId));

            return Ok();
        }
    }
}
