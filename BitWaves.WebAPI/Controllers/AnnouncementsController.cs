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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace BitWaves.WebAPI.Controllers
{
    [ApiController]
    [Route("announcements")]
    public sealed class AnnouncementsController : ControllerBase
    {
        private readonly Repository _repo;
        private readonly IMapper _mapper;

        public AnnouncementsController(Repository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // GET: /announcements
        [HttpGet]
        public async Task<PaginatedListActionResult<AnnouncementListInfo>> GetAnnouncementsList(
            [FromQuery] [Range(0, int.MaxValue)] int page = 0,
            [FromQuery] [Range(1, int.MaxValue)] int itemsPerPage = 20)
        {
            var findPipeline = new AnnouncementFindPipeline()
            {
                Pagination = new Pagination(page, itemsPerPage)
            };
            var findResult = await _repo.Announcements.FindManyAsync(findPipeline);

            var models = findResult.ResultSet.Select(e => _mapper.Map<Announcement, AnnouncementListInfo>(e))
                                   .ToList();
            return (findResult.TotalCount, models);
        }

        // POST: /announcements
        [HttpPost]
        [Authorize(Policy = BitWavesAuthPolicies.AdminOnly)]
        public async Task<IActionResult> CreateAnnouncement(
            [FromBody] CreateAnnouncementModel model)
        {
            model.Author = User.Identity.Name;
            var entity = _mapper.Map<CreateAnnouncementModel, Announcement>(model);

            await _repo.Announcements.InsertOneAsync(entity);
            return new ObjectResult(new { id = entity.Id });
        }

        // GET: /announcements/{announcementId}
        [HttpGet("{announcementId}")]
        public async Task<ActionResult<AnnouncementInfo>> GetAnnouncement(
            string announcementId)
        {
            if (!ObjectId.TryParse(announcementId, out var id))
            {
                ModelState.AddModelError(nameof(announcementId), "Invalid announcement ID.");
                return ValidationProblem();
            }

            var entity = await _repo.Announcements.FindOneAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            return _mapper.Map<Announcement, AnnouncementInfo>(entity);
        }

        // PUT: /announcements/{announcementId}
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

            var update = _mapper.Map<UpdateAnnouncementModel, AnnouncementUpdateInfo>(model);
            var updateResult = await _repo.Announcements.UpdateOneAsync(id, update);
            if (!updateResult)
            {
                return NotFound();
            }

            return Ok();
        }

        // DELETE: /announcements/{announcementId}
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

            var deleteResult = await _repo.Announcements.DeleteOneAsync(id);
            if (!deleteResult)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
