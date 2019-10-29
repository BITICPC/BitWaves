using System;
using BitWaves.Data.Entities;
using BitWaves.WebAPI.Utils;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为题目样例提供数据模型。
    /// </summary>
    public sealed class ProblemSampleTestInfo
    {
        /// <summary>
        /// 初始化 <see cref="ProblemSampleTestInfo"/> 类的新实例。
        /// </summary>
        /// <param name="entity">题目样例实体对象。</param>
        /// <exception cref="ArgumentNullException"><paramref name="entity"/> 为 null。</exception>
        public ProblemSampleTestInfo(ProblemSampleTest entity)
        {
            Contract.NotNull(entity, nameof(entity));
        }

        /// <summary>
        /// 获取输入数据。
        /// </summary>
        [JsonProperty("input")]
        public string Input { get; }

        /// <summary>
        /// 获取输出数据。
        /// </summary>
        [JsonProperty("output")]
        public string Output { get; }
    }
}
