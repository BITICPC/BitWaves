using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BitWaves.Data.DML;
using BitWaves.Data.Entities;
using BitWaves.Data.Repositories;
using BitWaves.WebAPI.Authentication;
using BitWaves.WebAPI.Models;
using BitWaves.WebAPI.Utils;
using BitWaves.WebAPI.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BitWaves.WebAPI.Controllers
{
    [Route("archive")]
    [ApiController]
    public sealed class ArchiveController : ControllerBase
    {
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
            [FromQuery] ProblemSortKey by = ProblemSortKey.ArchiveId,
            [FromQuery] bool descend = false,
            [FromQuery] [Page] int page = 0,
            [FromQuery] [ItemsPerPage] int itemsPerPage = 20)
        {
            var findPipeline = new ProblemFindPipeline(new ProblemFilterBuilder().InArchive(true))
            {
                SortKey = by,
                SortByDescending = descend,
                Pagination = new Pagination(page, itemsPerPage)
            };
            var findResult = await _repo.Problems.FindManyAsync(findPipeline);

            var models = findResult.ResultSet.Select(p => _mapper.Map<Problem, ProblemListInfo>(p));
            return (findResult.TotalCount, models);
        }

        // GET: /archive/{archiveId}
        [HttpGet("{archiveId}")]
        public async Task<ActionResult<ProblemInfo>> GetArchiveProblem(
            int archiveId)
        {
            var entity = await _repo.Problems.FindOneArchiveProblemAsync(archiveId);
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
            bool updateResult;
            try
            {
                updateResult = await _repo.Problems.AddProblemToArchiveAsync(model.ProblemId, model.ArchiveId);
            }
            catch (RepositoryException ex)
            {
                if (ex.IsDuplicateKey)
                {
                    return Conflict();
                }

                throw;
            }

            if (!updateResult)
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
            await _repo.Problems.DeleteProblemsFromArchiveAsync(problemIds);

            return Ok();
        }
    }
}
