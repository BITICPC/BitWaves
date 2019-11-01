using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BitWaves.Data;
using BitWaves.Data.Entities;
using BitWaves.WebAPI.Authentication;
using BitWaves.WebAPI.Extensions;
using BitWaves.WebAPI.Models;
using BitWaves.WebAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BitWaves.WebAPI.Controllers
{
    [ApiController]
    [Route("contents")]
    public sealed class ContentsController : ControllerBase
    {
        private readonly Repository _repo;

        public ContentsController(Repository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [Authorize(BitWavesAuthPolicies.AdminOnly)]
        public async Task<IActionResult> GetObjectList(
            [FromQuery] string name = null,
            [FromQuery] string mimeType = null,
            [FromQuery] [Range(0, int.MaxValue)] int page = 0,
            [FromQuery] [Range(1, int.MaxValue)] int itemsPerPage = 20)
        {
            var filter = Builders<Content>.Filter.Empty;
            if (name != null)
            {
                filter &= Builders<Content>.Filter.Eq(content => content.Name, name);
            }
            if (mimeType != null)
            {
                filter &= Builders<Content>.Filter.Eq(content => content.MimeType, mimeType);
            }

            var query = _repo.Contents.Find(filter);
            var totalCount = await query.CountDocumentsAsync();

            var entities = await query.Sort(Builders<Content>.Sort.Descending(content => content.CreationTime))
                                      .Project(Builders<Content>.Projection.Exclude(content => content.Data))
                                      .Paginate(page, itemsPerPage)
                                      .ToListAsync();
            return new ListResult<ContentObjectInfo>(
                totalCount, entities.Select(content => new ContentObjectInfo(content)));
        }

        [HttpPost]
        [Authorize(BitWavesAuthPolicies.AdminOnly)]
        public async Task<IActionResult> CreateObject(
            [FromForm(Name = "content")] [FileMaxLength(16 * 1024 * 1024)] IFormFile file)
        {
            var buffer = new byte[file.Length];
            using (var bufferStream = new MemoryStream(buffer))
            {
                await file.CopyToAsync(bufferStream);
            }

            var content = Data.Entities.Content.Create(file.FileName, file.ContentType, buffer);
            await _repo.Contents.InsertOneAsync(content);

            return new ObjectResult(new { id = content.Id.ToString() });
        }

        [HttpGet("{contentId}")]
        public async Task<IActionResult> GetObject(
            string contentId,
            [FromQuery] bool attachment = false)
        {
            if (!ObjectId.TryParse(contentId, out var id))
            {
                ModelState.AddModelError(nameof(contentId), "Invalid content ID.");
                return ValidationProblem();
            }

            var content = await _repo.Contents.Find(Builders<Content>.Filter.Eq(e => e.Id, id))
                                     .FirstOrDefaultAsync();
            if (content == null)
            {
                return NotFound();
            }

            return new BitWavesContentResult(content) { IsAttachment = attachment };
        }

        [HttpDelete("{contentId}")]
        [Authorize(Policy = BitWavesAuthPolicies.AdminOnly)]
        public async Task<IActionResult> DeleteObject(
            string contentId)
        {
            if (!ObjectId.TryParse(contentId, out var id))
            {
                ModelState.AddModelError(nameof(contentId), "Invalid content ID.");
                return ValidationProblem();
            }

            var deleteResult = await _repo.Contents.DeleteOneAsync(
                Builders<Content>.Filter.Eq(content => content.Id, id));
            if (deleteResult.DeletedCount == 0)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
