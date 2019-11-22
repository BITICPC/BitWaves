using System;
using System.Linq;
using BitWaves.Data.Entities;
using BitWaves.WebAPI.Utils;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为题目信息提供数据模型。
    /// </summary>
    public sealed class ProblemInfo
    {
        /// <summary>
        /// 初始化 <see cref="ProblemInfo"/> 类的新实例。
        /// </summary>
        /// <param name="entity">题目实体对象。</param>
        /// <param name="scheme">创建的 <see cref="ProblemInfo"/> 数据模型的应用场景。</param>
        /// <exception cref="ArgumentNullException"><paramref name="entity"/> 为 null。</exception>
        /// <exception cref="ArgumentException">无效的 JudgeMode 字段。</exception>
        public ProblemInfo(Problem entity, ProblemInfoScheme scheme)
        {
            Contract.NotNull(entity, nameof(entity));

            Id = entity.Id.ToString();
            ArchiveId = entity.ArchiveId;
            Title = entity.Title;
            CreationTime = entity.CreationTime;
            LastUpdateTime = entity.LastUpdateTime;
            Author = entity.Author;
            Source = entity.Source;
            Difficulty = entity.Difficulty;
            Tags = entity.Tags.ToArray();
            TotalSubmissions = entity.TotalSubmissions;
            AcceptedSubmissions = entity.AcceptedSubmissions;
            TotalAttemptedUsers = entity.TotalAttemptedUsers;
            TotalSolvedUsers = entity.TotalSolvedUsers;
            LastSubmissionTime = entity.LastSubmissionTime;

            if (scheme == ProblemInfoScheme.Full)
            {
                Legend = entity.Description?.Legend;
                Input = entity.Description?.Input;
                Output = entity.Description?.Output;
                SampleTests = entity.Description?.SampleTests
                                    ?.Select(st => new ProblemSampleTestInfo(st))
                                    .ToArray();
                Notes = entity.Description?.Notes;
                TimeLimit = entity.JudgeInfo.TimeLimit;
                MemoryLimit = entity.JudgeInfo.MemoryLimit;
                IsTestReady = entity.JudgeInfo.TestDataArchiveFileId != null;

                JudgeMode = entity.JudgeInfo.JudgeMode;
                BuiltinCheckerOptions = entity.JudgeInfo.CheckerOptions;
            }

            Scheme = scheme;
        }

        /// <summary>
        /// 获取题目 ID。
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; }

        /// <summary>
        /// 获取题目在 Archive 中的 ID。
        /// </summary>
        [JsonProperty("archiveId")]
        public int? ArchiveId { get; }

        /// <summary>
        /// 获取题目标题。
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; }

        /// <summary>
        /// 获取题目的创建时间。
        /// </summary>
        [JsonProperty("creationTime")]
        public DateTime CreationTime { get; }

        /// <summary>
        /// 获取题目的上次更新时间。
        /// </summary>
        [JsonProperty("lastUpdateTime")]
        public DateTime LastUpdateTime { get; }

        /// <summary>
        /// 获取题目的作者。
        /// </summary>
        [JsonProperty("author")]
        public string Author { get; }

        /// <summary>
        /// 获取题目的来源。
        /// </summary>
        [JsonProperty("source")]
        public string Source { get; }

        /// <summary>
        /// 获取题目的正文叙述。
        /// </summary>
        [JsonProperty("legend")]
        public string Legend { get; }

        /// <summary>
        /// 获取题目的输入描述。
        /// </summary>
        [JsonProperty("input")]
        public string Input { get; }

        /// <summary>
        /// 获取题目的输出描述。
        /// </summary>
        [JsonProperty("output")]
        public string Output { get; }

        /// <summary>
        /// 获取题目的测试样例。
        /// </summary>
        [JsonProperty("samples")]
        public ProblemSampleTestInfo[] SampleTests { get; }

        /// <summary>
        /// 获取题目的提示信息。
        /// </summary>
        [JsonProperty("notes")]
        public string Notes { get; }

        /// <summary>
        /// 获取题目的难度系数。
        /// </summary>
        [JsonProperty("difficulty")]
        public int Difficulty { get; }

        /// <summary>
        /// 获取题目的标签。
        /// </summary>
        [JsonProperty("tags")]
        public string[] Tags { get; }

        /// <summary>
        /// 获取题目的时间限制，单位为毫秒。
        /// </summary>
        [JsonProperty("timeLimit")]
        public int TimeLimit { get; }

        /// <summary>
        /// 获取题目的空间限制，单位为 MB。
        /// </summary>
        [JsonProperty("memoryLimit")]
        public int MemoryLimit { get; }

        /// <summary>
        /// 获取题目的评测模式。
        /// </summary>
        [JsonProperty("judgeMode")]
        public ProblemJudgeMode JudgeMode { get; }

        /// <summary>
        /// 当评测模式为 Standard 时，获取内建答案检查器的选项列表。
        /// </summary>
        [JsonProperty("builtinCheckerOptions")]
        public BuiltinCheckerOptions? BuiltinCheckerOptions { get; }

        /// <summary>
        /// 获取题目的总提交数量。
        /// </summary>
        [JsonProperty("totalSubmissions")]
        public int TotalSubmissions { get; }

        /// <summary>
        /// 获取题目的 AC 提交数量。
        /// </summary>
        [JsonProperty("acceptedSubmissions")]
        public int AcceptedSubmissions { get; }

        /// <summary>
        /// 获取尝试过该题目的用户数量。
        /// </summary>
        [JsonProperty("totalAttemptedUsers")]
        public int TotalAttemptedUsers { get; }

        /// <summary>
        /// 获取成功解答该题目的用户数量。
        /// </summary>
        [JsonProperty("totalSolvedUsers")]
        public int TotalSolvedUsers { get; }

        /// <summary>
        /// 获取上次提交本题目的时间。
        /// </summary>
        [JsonProperty("lastSubmissionTime")]
        public DateTime LastSubmissionTime { get; }

        /// <summary>
        /// 获取题目是否包含有效的测试数据集。
        /// </summary>
        [JsonProperty("testReady")]
        public bool IsTestReady { get; }

        /// <summary>
        /// 获取或设置当前对象的序列化选项。
        /// </summary>
        [JsonIgnore]
        public ProblemInfoScheme Scheme { get; set; }

        #region Conditional Property Serialization Checks

        // TODO: Refactor this hell: use a different approach to conditionally serialize properties of ProblemInfo.

        public bool ShouldSerializeLegend()
        {
            return Scheme == ProblemInfoScheme.Full;
        }

        public bool ShouldSerializeInput()
        {
            return Scheme == ProblemInfoScheme.Full;
        }

        public bool ShouldSerializeOutput()
        {
            return Scheme == ProblemInfoScheme.Full;
        }

        public bool ShouldSerializeSampleTests()
        {
            return Scheme == ProblemInfoScheme.Full;
        }

        public bool ShouldSerializeNotes()
        {
            return Scheme == ProblemInfoScheme.Full;
        }

        public bool ShouldSerializeTimeLimit()
        {
            return Scheme == ProblemInfoScheme.Full;
        }

        public bool ShouldSerializeMemoryLimit()
        {
            return Scheme == ProblemInfoScheme.Full;
        }

        public bool ShouldSerializeJudgeMode()
        {
            return Scheme == ProblemInfoScheme.Full;
        }

        public bool ShouldSerializeBuiltinCheckerOptions()
        {
            return Scheme == ProblemInfoScheme.Full && JudgeMode == ProblemJudgeMode.Standard;
        }

        public bool ShouldSerializeIsTestReady()
        {
            return Scheme == ProblemInfoScheme.Full;
        }

        #endregion
    }
}
