using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BitWaves.WebAPI.Authentication;
using BitWaves.WebAPI.Models;
using BitWaves.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BitWaves.WebAPI.Controllers
{
    [ApiController]
    [Route("judges")]
    public sealed class JudgesController : ControllerBase
    {
        private readonly IJudgeService _judgeService;
        private readonly IMapper _mapper;

        public JudgesController(IJudgeService judgeService, IMapper mapper)
        {
            _judgeService = judgeService;
            _mapper = mapper;
        }

        // GET: /judges
        [HttpGet]
        [Authorize(Policy = BitWavesAuthPolicies.AdminOnly)]
        public async Task<List<JudgeNodeInfoModel>> GetJudges()
        {
            var judges = await _judgeService.GetJudgeNodesAsync();
            return _mapper.Map<List<JudgeNodeInfo>, List<JudgeNodeInfoModel>>(judges);
        }

        // PUT: /judges/{address}/blocked
        [HttpPut("{address}/blocked")]
        [Authorize(Policy = BitWavesAuthPolicies.AdminOnly)]
        public async Task<IActionResult> SetBlocked(
            string address,
            [FromQuery(Name = "blocked")] bool blocked = true)
        {
            try
            {
                await _judgeService.BlockJudgeNodeAsync(address, blocked);
            }
            catch (ArgumentException)
            {
                ModelState.AddModelError(nameof(address), "Invalid judge node address.");
                return ValidationProblem();
            }

            return Ok();
        }
    }
}
