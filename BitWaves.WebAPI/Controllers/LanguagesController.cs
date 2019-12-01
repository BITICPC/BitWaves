using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BitWaves.Data.DML;
using BitWaves.Data.Entities;
using BitWaves.Data.Repositories;
using BitWaves.WebAPI.Authentication;
using BitWaves.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace BitWaves.WebAPI.Controllers
{
    [ApiController]
    [Route("languages")]
    public sealed class LanguagesController : ControllerBase
    {
        private readonly Repository _repo;
        private readonly IMapper _mapper;

        public LanguagesController(Repository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // GET: /languages
        [HttpGet]
        public async Task<ActionResult<LanguageInfo[]>> GetLanguages()
        {
            var languages = await _repo.Languages.FindManyAsync(LanguageFindPipeline.Default);

            return languages.ResultSet.Select(e => _mapper.Map<Language, LanguageInfo>(e)).ToArray();
        }

        // POST: /languages
        [HttpPost]
        [Authorize(Policy = BitWavesAuthPolicies.AdminOnly)]
        public async Task<IActionResult> CreateLanguage(
            [FromBody] CreateLanguageModel model)
        {
            var entity = _mapper.Map<CreateLanguageModel, Language>(model);
            await _repo.Languages.InsertOneAsync(entity);

            return new ObjectResult(new { id = entity.Id });
        }

        // DELETE: /languages/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = BitWavesAuthPolicies.AdminOnly)]
        public async Task<IActionResult> DeleteLanguage(
            string id)
        {
            if (!ObjectId.TryParse(id, out var objId))
            {
                ModelState.AddModelError(nameof(id), "Invalid object ID.");
                return ValidationProblem();
            }

            var deleteResult = await _repo.Languages.DeleteOneAsync(objId);
            if (!deleteResult)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
