using System.ComponentModel.DataAnnotations;
using System.Linq;
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
    [Route("problems")]
    [ApiController]
    public sealed class ProblemsController : ControllerBase
    {
        private readonly IAuthorizationService _authorization;
        private readonly Repository _repo;

        public ProblemsController(IAuthorizationService authorization, Repository repo)
        {
            _authorization = authorization;
            _repo = repo;
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
            return new ListResult<ProblemInfo>(
                totalCount, entities.Select(p => new ProblemInfo(p, ProblemInfoScheme.List)));
        }

        [HttpPost]
        [Authorize(Policy = BitWavesAuthPolicies.AdminOnly)]
        public async Task<IActionResult> CreateProblem(
            [FromBody] CreateProblemModel model)
        {
            var entity = model.ToEntity(User.Identity.Name);
            await _repo.Problems.InsertOneAsync(entity);
            return Ok();
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

            return new ObjectResult(new ProblemInfo(entity, ProblemInfoScheme.Full));
        }
    }
}
