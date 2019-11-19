using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BitWaves.Data.Entities;
using BitWaves.WebAPI.Utils;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为更新题目信息操作提供数据模型。
    /// </summary>
    public sealed class UpdateProblemModel
    {
        /// <summary>
        /// 获取题目的标题。
        /// </summary>
        [JsonProperty("title")]
        [OptionalValidation(typeof(RequiredAttribute))]
        [OptionalValidation(typeof(MinLengthAttribute), 1)]
        public Utils.Optional<string> Title { get; private set; }

        /// <summary>
        /// 获取题目的背景描述。
        /// </summary>
        [JsonProperty("legend")]
        public Utils.Optional<string> Legend { get; private set; }

        /// <summary>
        /// 获取题目的输入格式描述。
        /// </summary>
        [JsonProperty("input")]
        public Utils.Optional<string> Input { get; private set; }

        /// <summary>
        /// 获取题目的输出格式描述。
        /// </summary>
        [JsonProperty("output")]
        public Utils.Optional<string> Output { get; private set; }

        /// <summary>
        /// 获取题目的提示信息。
        /// </summary>
        [JsonProperty("notes")]
        public Utils.Optional<string> Notes { get; private set; }

        /// <summary>
        /// 获取题目的难度系数。
        /// </summary>
        [JsonProperty("difficulty")]
        [OptionalValidation(typeof(RangeAttribute), 0, 100)]
        public Utils.Optional<int> Difficulty { get; private set; }

        /// <summary>
        /// 获取题目的标签。
        /// </summary>
        [JsonProperty("tags")]
        public Utils.Optional<string[]> Tags { get; private set; }

        /// <summary>
        /// 获取题目单个测试点的时间限制，单位为毫秒。
        /// </summary>
        [JsonProperty("timeLimit")]
        [OptionalValidation(typeof(RangeAttribute), 500, 10000)]
        public Utils.Optional<int> TimeLimit { get; private set; }

        /// <summary>
        /// 获取题目单个测试点的内存限制，单位为 MB。
        /// </summary>
        [JsonProperty("memoryLimit")]
        [OptionalValidation(typeof(RangeAttribute), 32, 1024)]
        public Utils.Optional<int> MemoryLimit { get; private set; }

        /// <summary>
        /// 获取题目的评测模式。
        /// </summary>
        [JsonProperty("judgeMode")]
        public Utils.Optional<ProblemJudgeMode> JudgeMode { get; private set; }

        /// <summary>
        /// 从当前的题目信息更新数据模型创建对应的数据库更新定义。
        /// </summary>
        /// <returns>创建的数据库更新定义。</returns>
        public UpdateDefinition<Problem> CreateUpdateDefinition()
        {
            // TODO: 重构 UpdateProblemModel.CreateUpdateDefinition 方法

            var updates = new List<UpdateDefinition<Problem>>();
            updates.Add(Builders<Problem>.Update.Set(p => p.LastUpdateTime, DateTime.UtcNow));

            if (Title.HasValue)
            {
                updates.Add(Builders<Problem>.Update.Set(p => p.Title, Title.Value));
            }

            if (Legend.HasValue)
            {
                updates.Add(Builders<Problem>.Update.Set(p => p.Description.Legend, Legend.Value));
            }

            if (Input.HasValue)
            {
                updates.Add(Builders<Problem>.Update.Set(p => p.Description.Input, Input.Value));
            }

            if (Output.HasValue)
            {
                updates.Add(Builders<Problem>.Update.Set(p => p.Description.Output, Output.Value));
            }

            if (Notes.HasValue)
            {
                updates.Add(Builders<Problem>.Update.Set(p => p.Description.Notes, Notes.Value));
            }

            if (Difficulty.HasValue)
            {
                updates.Add(Builders<Problem>.Update.Set(p => p.Difficulty, Difficulty.Value));
            }

            if (Tags.HasValue)
            {
                updates.Add(Builders<Problem>.Update.Set(p => p.Tags, Tags.Value.ToList()));
            }

            if (TimeLimit.HasValue)
            {
                updates.Add(Builders<Problem>.Update.Set(p => p.JudgeInfo.TimeLimit, TimeLimit.Value));
            }

            if (MemoryLimit.HasValue)
            {
                updates.Add(Builders<Problem>.Update.Set(p => p.JudgeInfo.MemoryLimit, MemoryLimit.Value));
            }

            if (JudgeMode.HasValue)
            {
                updates.Add(Builders<Problem>.Update.Set(p => p.JudgeInfo.JudgeMode, JudgeMode.Value));
            }

            return Builders<Problem>.Update.Combine(updates);
        }
    }
}
