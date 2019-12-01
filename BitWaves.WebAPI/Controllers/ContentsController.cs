using System.ComponentModel.DataAnnotations;
using System.IO;
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
    [ApiController]
    [Route("contents")]
    public sealed class ContentsController : ControllerBase
    {
        private readonly Repository _repo;
        private readonly IMapper _mapper;

        public ContentsController(Repository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // GET: /contents
        [HttpGet]
        [Authorize(BitWavesAuthPolicies.AdminOnly)]
        public async Task<PaginatedListActionResult<ContentInfo>> GetObjectList(
            [FromQuery] string name = null,
            [FromQuery] string mimeType = null,
            [FromQuery] [Range(0, int.MaxValue)] int page = 0,
            [FromQuery] [Range(1, int.MaxValue)] int itemsPerPage = 20)
        {
            var filterBuilder = ContentFilterBuilder.Empty;

            if (name != null)
            {
                filterBuilder = filterBuilder.Name(name);
            }

            if (mimeType != null)
            {
                filterBuilder = filterBuilder.MimeType(mimeType);
            }

            var findPipeline = new ContentFindPipeline(filterBuilder)
            {
                Pagination = new Pagination(page, itemsPerPage)
            };

            var findResult = await _repo.Contents.FindManyAsync(findPipeline);
            var models = findResult.ResultSet.Select(e => _mapper.Map<Content, ContentInfo>(e));
            return (findResult.TotalCount, models);
        }

        // POST: /contents
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

            var content = new Content(file.FileName, file.ContentType, buffer);
            await _repo.Contents.InsertOneAsync(content);

            return new ObjectResult(new { id = content.Id });
        }

        // GET: /contents/{contentId}
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

            var content = await _repo.Contents.FindOneAsync(id);
            if (content == null)
            {
                return NotFound();
            }

            return new BitWavesContentResult(content) { IsAttachment = attachment };
        }

        // DELETE: /contents/{contentId}
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

            var deleteResult = await _repo.Contents.DeleteOneAsync(id);
            if (!deleteResult)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
