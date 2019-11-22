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
    [ApiController]
    [Route("announcements")]
    public sealed class AnnouncementsController : ControllerBase
    {
        private readonly Repository _repo;

        public AnnouncementsController(Repository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAnnouncementsList(
            [FromQuery] [Range(0, int.MaxValue)] int page = 0,
            [FromQuery] [Range(1, int.MaxValue)] int itemsPerPage = 20)
        {
            var query = _repo.Announcements.Find(Builders<Announcement>.Filter.Empty)
                             .Sort(Builders<Announcement>.Sort.Combine(
                                       Builders<Announcement>.Sort.Descending(ann => ann.IsPinned),
                                       Builders<Announcement>.Sort.Descending(ann => ann.LastUpdateTime)));
            var totalCount = await query.CountDocumentsAsync();
            var entities = await query.Paginate(page, itemsPerPage)
                                      .ToListAsync();

            return new PaginatedListResult<AnnouncementInfo>(
                totalCount, entities.Select(e => new AnnouncementInfo(e, AnnouncementInfoScheme.List)));
        }

        [HttpPost]
        [Authorize(Policy = BitWavesAuthPolicies.AdminOnly)]
        public async Task<IActionResult> CreateAnnouncement(
            [FromBody] CreateAnnouncementModel model)
        {
            var entity = model.ToAnnouncementEntity(User.Identity.Name);

            await _repo.Announcements.InsertOneAsync(entity);
            return Ok();
        }

        [HttpGet("{announcementId}")]
        public async Task<IActionResult> GetAnnouncement(
            string announcementId)
        {
            if (!ObjectId.TryParse(announcementId, out var id))
            {
                ModelState.AddModelError(nameof(announcementId), "Invalid announcement ID.");
                return ValidationProblem();
            }

            var entity = await _repo.Announcements.Find(Builders<Announcement>.Filter.Eq(ann => ann.Id, id))
                                    .FirstOrDefaultAsync();
            if (entity == null)
            {
                return NotFound();
            }

            return new ObjectResult(new AnnouncementInfo(entity, AnnouncementInfoScheme.Full));
        }

        [HttpPut("{announcementId}")]
        [Authorize(Policy = BitWavesAuthPolicies.AdminOnly)]
        public async Task<IActionResult> UpdateAnnouncement(
            string announcementId,
            [FromBody] UpdateAnnouncementModel model)
        {
            if (!ObjectId.TryParse(announcementId, out var id))
            {
                ModelState.AddModelError(nameof(announcementId), "Invalid announcement ID.");
                return ValidationProblem();
            }

            var update = model.GetUpdateDefinition();
            var updateResult = await _repo.Announcements.UpdateOneAsync(
                Builders<Announcement>.Filter.Eq(ann => ann.Id, id), update);
            if (updateResult.MatchedCount == 0)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("{announcementId}")]
        [Authorize(Policy = BitWavesAuthPolicies.AdminOnly)]
        public async Task<IActionResult> DeleteAnnouncement(
            string announcementId)
        {
            if (!ObjectId.TryParse(announcementId, out var id))
            {
                ModelState.AddModelError(nameof(announcementId), "Invalid announcement ID.");
                return ValidationProblem();
            }

            var deleteResult = await _repo.Announcements.DeleteOneAsync(
                Builders<Announcement>.Filter.Eq(ann => ann.Id, id));
            if (deleteResult.DeletedCount == 0)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
