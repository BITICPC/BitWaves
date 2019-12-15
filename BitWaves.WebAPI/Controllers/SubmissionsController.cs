using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BitWaves.Data.DML;
using BitWaves.Data.Entities;
using BitWaves.Data.Repositories;
using BitWaves.Data.Utils;
using BitWaves.WebAPI.Authentication;
using BitWaves.WebAPI.Models;
using BitWaves.WebAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace BitWaves.WebAPI.Controllers
{
    [ApiController]
    [Route("submissions")]
    public sealed class SubmissionsController : ControllerBase
    {
        private readonly Repository _repo;
        private readonly IMapper _mapper;

        public SubmissionsController(Repository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<PaginatedListActionResult<SubmissionListInfo>> GetSubmissions(
            [FromQuery] int page = 0,
            [FromQuery] int itemsPerPage = 20,
            [FromQuery] string author = null,
            [FromQuery] string problem = null,
            [FromQuery] JudgeStatus? status = null,
            [FromQuery] Verdict? verdict = null,
            [FromQuery] string language = null)
        {
            var filter = new SubmissionFilterBuilder();

            if (author != null)
            {
                filter = filter.Author(author);
            }

            if (problem != null)
            {
                // TODO: Change the type of problem from `string` to `ObjectId`.
                if (!ObjectId.TryParse(problem, out var problemId))
                {
                    ModelState.AddModelError(nameof(problem), "Invalid problem ID.");
                    return ValidationProblem();
                }

                filter = filter.Problem(problemId);
            }

            if (status != null)
            {
                filter = filter.Status(status.Value);
            }

            if (verdict != null)
            {
                filter = filter.Verdict(verdict.Value);
            }

            if (language != null)
            {
                filter = filter.Language(language);
            }

            var pipeline = new SubmissionFindPipeline(filter)
            {
                Pagination = new Pagination(page, itemsPerPage)
            };

            var findResult = await _repo.Submissions.FindManyAsync(pipeline);
            var models = findResult.ResultSet.Select(s => _mapper.Map<Submission, SubmissionListInfo>(s))
                                   .ToList();
            return new PaginatedListActionResult<SubmissionListInfo>(findResult.TotalCount, models);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ObjectId>> CreateSubmission(
            [FromBody] CreateSubmissionModel model)
        {
            // 查找对应的语言实体对象
            var language = await _repo.Languages.FindOneAsync(model.LanguageId);
            if (language == null)
            {
                return NotFound();
            }

            // 检查题目是否存在
            if (!await _repo.Problems.ExistAsync(model.ProblemId))
            {
                return NotFound();
            }

            model.Author = User.Identity.Name;
            model.LanguageTriple = new LanguageTriple(language.Identifier, language.Dialect, language.Version);
            model.LanguageDisplayName = language.DisplayName;
            var entity = _mapper.Map<CreateSubmissionModel, Submission>(model);

            await _repo.Submissions.InsertOneAsync(entity);
            return entity.Id;
        }

        [HttpGet("{submissionId}")]
        public async Task<ActionResult<SubmissionInfo>> GetSubmission(
            string submissionId)
        {
            // TODO: Change the type of submissionId from `string` to `ObjectId`.
            if (!ObjectId.TryParse(submissionId, out var id))
            {
                ModelState.AddModelError(nameof(submissionId), "Invalid submission ID.");
                return ValidationProblem();
            }

            var entity = await _repo.Submissions.FindOneAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            return _mapper.Map<Submission, SubmissionInfo>(entity);
        }

        [HttpPatch("{submissionId}")]
        [Authorize(Policy = BitWavesAuthPolicies.AdminOnly)]
        public async Task<IActionResult> PatchSubmission(
            string submissionId)
        {
            // TODO: Change the type of submissionId from `string` to `ObjectId`.
            if (!ObjectId.TryParse(submissionId, out var id))
            {
                ModelState.AddModelError(nameof(submissionId), "Invalid submission ID.");
                return ValidationProblem();
            }

            var update = new SubmissionUpdateInfo
            {
                Result = new Maybe<JudgeResult>(null),
                Status = new Maybe<JudgeStatus>(JudgeStatus.Pending)
            };

            var updateResult = await _repo.Submissions.UpdateOneAsync(id, update);
            if (!updateResult)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
