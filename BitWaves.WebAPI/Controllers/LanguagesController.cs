using System.Linq;
using System.Threading.Tasks;
using BitWaves.Data;
using BitWaves.Data.Entities;
using BitWaves.WebAPI.Authentication;
using BitWaves.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BitWaves.WebAPI.Controllers
{
    [ApiController]
    [Route("languages")]
    public sealed class LanguagesController : ControllerBase
    {
        private readonly Repository _repo;

        public LanguagesController(Repository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetLanguages()
        {
            var languages = await _repo.Languages.Find(Builders<Language>.Filter.Empty)
                                       .ToListAsync();
            return new ObjectResult(languages.Select(e => new LanguageInfo(e)));
        }

        [HttpPost]
        [Authorize(Policy = BitWavesAuthPolicies.AdminOnly)]
        public async Task<IActionResult> CreateLanguage(
            [FromBody] CreateLanguageModel model)
        {
            var entity = model.ToEntity();
            await _repo.Languages.InsertOneAsync(entity);

            return Ok();
        }

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

            var deleteResult = await _repo.Languages.DeleteOneAsync(Builders<Language>.Filter.Eq(e => e.Id, objId));
            if (deleteResult.DeletedCount == 0)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
