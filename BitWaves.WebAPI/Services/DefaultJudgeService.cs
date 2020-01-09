using System;
using System.Collections.Generic;
using System.Net;
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

        /// <inheritdoc />
        public async Task<bool> BlockJudgeNodeAsync(string address, bool blocked = true)
        {
            Contract.NotNull(address, nameof(address));
            Contract.NotEmpty(address, nameof(address));

            var uri = new UriBuilder
            {
                Path = $"/judges/{address}/block",
                Query = $"blocked={blocked}"
            }.Uri;
            var request = new HttpRequestMessage(HttpMethod.Put, uri);

            var response = await _http.SendAsync(request);
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return true;

                case HttpStatusCode.BadRequest:
                    throw new ArgumentException("无效的评测节点地址。", nameof(address));

                case HttpStatusCode.NotFound:
                    return false;

                default:
                    throw new InvalidOperationException($"评测集群控制返回了非预期的状态码：{response.StatusCode}");
            }
        }
    }
}
