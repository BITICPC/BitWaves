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

            var entities = await query.Paginate(page, itemsPerPage)
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
    }
}
