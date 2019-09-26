using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BitWaves.Data;
using BitWaves.Data.Entities;
using BitWaves.WebAPI.Extensions;
using BitWaves.WebAPI.Models;
using BitWaves.WebAPI.Utils;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace BitWaves.WebAPI.Controllers
{
    [Route("archive")]
    [ApiController]
    public sealed class ArchiveController : ControllerBase
    {
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
            var viewList = viewEntityList.Select(entity => new ProblemInfo(entity));

            return new ListResult<ProblemInfo>(totalCount, viewList);
        }
    }
}
