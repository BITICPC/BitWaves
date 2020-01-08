using System.ComponentModel.DataAnnotations;
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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

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

        // GET: /problems
        [HttpGet]
        [Authorize(Policy = BitWavesAuthPolicies.AdminOnly)]
        public async Task<PaginatedListActionResult<ProblemListInfo>> GetProblems(
            [FromQuery] ProblemSortKey by = ProblemSortKey.CreationTime,
            [FromQuery] bool descend = true,
            [FromQuery] [Range(0, int.MaxValue)] int page = 0,
            [FromQuery] [Range(1, int.MaxValue)] int itemsPerPage = 20)
        {
            var findPipeline = new ProblemFindPipeline(ProblemFilterBuilder.Empty)
            {
                SortKey = by,
                SortByDescending = descend,
                Pagination = new Pagination(page, itemsPerPage)
            };

            var findResult = await _repo.Problems.FindManyAsync(findPipeline);
            var models = findResult.ResultSet.Select(e => _mapper.Map<Problem, ProblemListInfo>(e));
            return (findResult.TotalCount, models);
        }

        // POST: /problems
        [HttpPost]
        [Authorize(Policy = BitWavesAuthPolicies.AdminOnly)]
        public async Task<IActionResult> CreateProblem(
            [FromBody] CreateProblemModel model)
        {
            model.Author = User.Identity.Name;
            var entity = _mapper.Map<CreateProblemModel, Problem>(model);
            await _repo.Problems.InsertOneAsync(entity);

            return new ObjectResult(new { id = entity.Id });
        }

        // PUT: /problems/{problemId}
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

            var update = _mapper.Map<UpdateProblemModel, ProblemUpdateInfo>(model);
            var updateResult = await _repo.Problems.UpdateOneAsync(id, update);
            if (!updateResult)
            {
                return NotFound();
            }

            return Ok();
        }

        // POST: /problems/{problemId}/tags
        [HttpPost("{problemId}/tags")]
        [Authorize(Policy = BitWavesAuthPolicies.AdminOnly)]
        public async Task<IActionResult> AddProblemTags(
            string problemId,
            [FromBody] [Required] [EnumerableValidation(typeof(ProblemTagNameAttribute))] string[] tagNames)
        {
            if (!ObjectId.TryParse(problemId, out var id))
            {
                ModelState.AddModelError(nameof(problemId), "invalid problem ID");
                return ValidationProblem();
            }

            var updateResult = await _repo.Problems.AddTagsToProblemAsync(id, tagNames);
            if (!updateResult)
            {
                return NotFound();
            }

            return Ok();
        }

        // DELETE: /problems/{problemId}/tags
        [HttpDelete("{problemId}/tags")]
        [Authorize(Policy = BitWavesAuthPolicies.AdminOnly)]
        public async Task<IActionResult> DeleteProblemTags(
            string problemId,
            [FromBody] [Required] [EnumerableValidation(typeof(ProblemTagNameAttribute))] string[] tagNames)
        {
            if (!ObjectId.TryParse(problemId, out var id))
            {
                ModelState.AddModelError(nameof(problemId), "invalid problem ID");
                return ValidationProblem();
            }

            var updateResult = await _repo.Problems.DeleteTagsFromProblemAsync(id, tagNames);
            if (!updateResult)
            {
                return NotFound();
            }

            return Ok();
        }

        // GET: /problems/{problemId}
        [HttpGet("{problemId}")]
        public async Task<ActionResult<ProblemInfo>> GetProblem(
            string problemId)
        {
            if (!ObjectId.TryParse(problemId, out var id))
            {
                ModelState.AddModelError(nameof(problemId), "Invalid problem ID.");
                return ValidationProblem();
            }

            var entity = await _repo.Problems.FindOneAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            // Check whether the user has access to the problem.
            var authResult = await _authorization.AuthorizeAsync(User, entity, BitWavesAuthPolicies.GetProblemDetail);
            if (!authResult.Succeeded)
            {
                if (User == null)
                {
                    return Challenge();
                }

                return Forbid();
            }

            return _mapper.Map<Problem, ProblemInfo>(entity);
        }

        // GET: /problems/tags
        [HttpGet("tags")]
        [Authorize(Policy = BitWavesAuthPolicies.AdminOnly)]
        public async Task<ActionResult<ProblemTagInfo[]>> GetProblemTags()
        {
            var entities = await _repo.Problems.FindAllTagsAsync(ProblemFilterBuilder.Empty);
            return entities.Select(e => _mapper.Map<ProblemTag, ProblemTagInfo>(e)).ToArray();
        }

        // POST: /problems/{problemId}/testset
        [HttpPost("{problemId}/testset")]
        [Authorize(Policy = BitWavesAuthPolicies.AdminOnly)]
        [RequestSizeLimit(256 * 1024 * 1024)] // The maximal size of request is 256 MB for this action.
        [RequestFormLimits(MultipartBodyLengthLimit = 256 * 1024 * 1024)]
        public async Task<IActionResult> UploadTestArchive(
            string problemId,
            [FromForm(Name = "archive")] IFormFile file)
        {
            var validateOk = true;
            if (!ObjectId.TryParse(problemId, out var id))
            {
                ModelState.AddModelError("problemId", "invalid problem ID.");
                validateOk = false;
            }

            if (file == null)
            {
                ModelState.AddModelError("archive", "no test archive found.");
                validateOk = false;
            }

            if (!validateOk)
            {
                return ValidationProblem();
            }

            bool succeeded;
            using (var fileStream = file.OpenReadStream())
            {
                succeeded = await _repo.Problems.UploadProblemTestDataArchive(id, fileStream);
            }

            if (!succeeded)
            {
                return NotFound();
            }

            return Ok();
        }

        // DELETE: /problems/{problemId}/testset
        [HttpDelete("{problemId}/testset")]
        [Authorize(Policy = BitWavesAuthPolicies.AdminOnly)]
        public async Task<IActionResult> DeleteTestArchive(
            string problemId)
        {
            if (!ObjectId.TryParse(problemId, out var id))
            {
                ModelState.AddModelError("problemId", "invalid problem ID.");
                return ValidationProblem();
            }

            await _repo.Problems.DeleteProblemTestDataArchive(id);
            return Ok();
        }
    }
}
