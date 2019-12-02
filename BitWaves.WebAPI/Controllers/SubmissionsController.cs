using System;
using System.Threading.Tasks;
using AutoMapper;
using BitWaves.Data.Entities;
using BitWaves.Data.Repositories;
using BitWaves.WebAPI.Authentication;
using BitWaves.WebAPI.Models;
using BitWaves.WebAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            [FromQuery] string status = null,
            [FromQuery] string verdict = null,
            [FromQuery] string language = null)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateSubmission(
            [FromBody] CreateSubmissionModel model)
        {
            model.Author = User.Identity.Name;
            var entity = _mapper.Map<CreateSubmissionModel, Submission>(model);

            await _repo.Submissions.InsertOneAsync(entity);
            return Ok();
        }

        [HttpGet("{submissionId}")]
        public async Task<ActionResult<SubmissionInfo>> GetSubmission(
            string submissionId)
        {
            throw new NotImplementedException();
        }

        [HttpPatch("{submissionId}")]
        [Authorize(Policy = BitWavesAuthPolicies.AdminOnly)]
        public async Task<IActionResult> PatchSubmission(
            string submissionId)
        {
            throw new NotImplementedException();
        }
    }
}
