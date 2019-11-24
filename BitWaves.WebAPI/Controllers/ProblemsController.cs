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
using MongoDB.Bson;
using MongoDB.Driver;

namespace BitWaves.WebAPI.Controllers
{
    [Route("problems")]
    [ApiController]
    public sealed class ProblemsController : ControllerBase
    {
        private readonly IAuthorizationService _authorization;
        private readonly Repository _repo;
        private readonly IMapper _mapper;

        public ProblemsController(IAuthorizationService authorization, Repository repo, IMapper mapper)
        {
            _authorization = authorization;
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Policy = BitWavesAuthPolicies.AdminOnly)]
        public async Task<IActionResult> GetProblems(
            [FromQuery] [Range(0, int.MaxValue)] int page = 0,
            [FromQuery] [Range(1, int.MaxValue)] int itemsPerPage = 20)
        {
            var query = _repo.Problems.Find(Builders<Problem>.Filter.Empty)
                             .SortByDescending(p => p.LastUpdateTime);
            var totalCount = await query.CountDocumentsAsync();
            var entities = await query.Paginate(page, itemsPerPage)
                                .ToListAsync();
            var models = entities.Select(e => _mapper.Map<Problem, ProblemListInfo>(e));
            return new PaginatedListResult<ProblemListInfo>(totalCount, models);
        }

        [HttpPost]
        [Authorize(Policy = BitWavesAuthPolicies.AdminOnly)]
        public async Task<IActionResult> CreateProblem(
            [FromBody] CreateProblemModel model)
        {
            var entity = _mapper.Map<CreateProblemModel, Problem>(model);
            await _repo.Problems.InsertOneAsync(entity);

            return new ObjectResult(new { id = entity.Id });
        }

        [HttpPut("{problemId}")]
        [Authorize(Policy = BitWavesAuthPolicies.AdminOnly)]
        public async Task<IActionResult> UpdateProblem(
            string problemId,
            [FromBody] UpdateProblemModel model)
        {
            if (!ObjectId.TryParse(problemId, out var id))
            {
                ModelState.AddModelError(nameof(problemId), "Invalid problem ID.");
                return ValidationProblem();
            }

            var updateDefinition = model.CreateUpdateDefinition();
            var entity = await _repo.Problems.FindOneAndUpdateAsync(
                Builders<Problem>.Filter.Eq(p => p.Id, id),
                updateDefinition);

            if (entity == null)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpGet("{problemId}")]
        public async Task<IActionResult> GetProblem(
            string problemId)
        {
            if (!ObjectId.TryParse(problemId, out var id))
            {
                ModelState.AddModelError(nameof(problemId), "Invalid problem ID.");
                return ValidationProblem();
            }

            var entity = await _repo.Problems.Find(Builders<Problem>.Filter.Eq(p => p.Id, id))
                                    .FirstOrDefaultAsync();
            if (entity == null)
            {
                return NotFound();
            }

            // Check whether the user has access to the problem.
            var authResult = await _authorization.AuthorizeAsync(User, entity, BitWavesAuthPolicies.GetProblemDetail);
            if (!authResult.Succeeded)
            {
                return Forbid();
            }

            var model = _mapper.Map<Problem, ProblemInfo>(entity);
            return new ObjectResult(model);
        }
    }
}
