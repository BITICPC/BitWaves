using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Services
{
    /// <summary>
    /// 为 <see cref="IJudgeService"/> 提供默认实现。
    /// </summary>
    internal sealed class DefaultJudgeService : IJudgeService
    {
        private readonly HttpClient _http;

        /// <summary>
        /// 初始化 <see cref="DefaultJudgeService"/> 类的新实例。
        /// </summary>
        /// <param name="client"></param>
        public DefaultJudgeService(HttpClient client)
        {
            Contract.NotNull(client, nameof(client));

            _http = client;
        }

        /// <inheritdoc />
        public async Task<List<JudgeNodeInfo>> GetJudgeNodesAsync()
        {
            var response = await _http.GetStringAsync("/judges");
            return JsonConvert.DeserializeObject<List<JudgeNodeInfo>>(response);
        }
    }
}
