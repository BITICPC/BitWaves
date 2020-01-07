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
        [Authorize(Roles = BitWavesAuthRoles.Admin)]
        public async Task<List<JudgeNodeInfoModel>> GetJudges()
        {
            var judges = await _judgeService.GetJudgeNodesAsync();
            return _mapper.Map<List<JudgeNodeInfo>, List<JudgeNodeInfoModel>>(judges);
        }
    }
}
