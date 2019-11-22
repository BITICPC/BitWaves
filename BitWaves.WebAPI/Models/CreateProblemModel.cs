using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BitWaves.Data.Entities;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为创建题目操作提供数据模型。
    /// </summary>
    public sealed class CreateProblemModel
    {
        /// <summary>
        /// 初始化 <see cref="CreateProblemModel"/> 的新实例。
        /// </summary>
        public CreateProblemModel()
        {
            TimeLimit = 1000;
            MemoryLimit = 256;
            JudgeMode = ProblemJudgeMode.Standard;
        }

        /// <summary>
        /// 获取题目的标题。
        /// </summary>
        [Required]
        [MinLength(1)]
        [JsonProperty("title")]
        public string Title { get; private set; }

        /// <summary>
        /// 获取题目的背景描述。
        /// </summary>
        [JsonProperty("legend")]
        public string Legend { get; private set; }

        /// <summary>
        /// 获取题目的输入描述。
        /// </summary>
        [JsonProperty("input")]
        public string Input { get; private set; }

        /// <summary>
        /// 获取题目的输出描述。
        /// </summary>
        [JsonProperty("output")]
        public string Output { get; private set; }

        /// <summary>
        /// 获取题目的提示信息。
        /// </summary>
        [JsonProperty("notes")]
        public string Notes { get; private set; }

        /// <summary>
        /// 获取题目的难度系数。
        /// </summary>
        [JsonProperty("difficulty")]
        [Range(0, 100)]
        public int Difficulty { get; private set; }

        /// <summary>
        /// 获取题目的标签。
        /// </summary>
        [JsonProperty("tags")]
        public string[] Tags { get; private set; }

        /// <summary>
        /// 获取题目单个测试点的时间限制，单位为毫秒。
        /// </summary>
        [JsonProperty("timeLimit")]
        [Range(500, 10000)]
        public int TimeLimit { get; private set; }

        /// <summary>
        /// 获取题目单个测试点的内存限制打，单位为MB。
        /// </summary>
        [JsonProperty("memoryLimit")]
        [Range(32, 1024)]
        public int MemoryLimit { get; private set; }

        /// <summary>
        /// 获取题目的评测模式。
        /// </summary>
        [JsonProperty("judgeMode")]
        public ProblemJudgeMode JudgeMode { get; private set; }

        /// <summary>
        /// 当评测模式为 Standard 时，获取传递给内建答案检查器的选项。
        /// </summary>
        [JsonProperty("builtinCheckerOptions")]
        public BuiltinCheckerOptions? BuiltinCheckerOptions { get; private set; }

        /// <summary>
        /// 从当前的数据模型创建题目实体对象。
        /// </summary>
        /// <param name="username">创建题目的用户的用户名。</param>
        /// <returns>从当前的数据模型创建的题目实体对象。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="username"/> 为 null。</exception>
        public Problem ToEntity(string username)
        {
            Contract.NotNull(username, nameof(username));

            var entity = Problem.Create();
            entity.Author = username;
            entity.Title = Title;
            entity.Description.Legend = Legend;
            entity.Description.Input = Input;
            entity.Description.Output = Output;
            entity.Description.Notes = Notes;
            entity.Difficulty = Difficulty;
            entity.Tags = Tags?.ToList() ?? new List<string>();
            entity.JudgeInfo.TimeLimit = TimeLimit;
            entity.JudgeInfo.MemoryLimit = MemoryLimit;
            entity.JudgeInfo.JudgeMode = JudgeMode;
            entity.JudgeInfo.BuiltinCheckerOptions = BuiltinCheckerOptions;

            return entity;
        }
    }
}
