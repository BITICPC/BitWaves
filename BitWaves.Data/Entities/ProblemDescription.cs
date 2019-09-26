using System.Collections.Generic;

namespace BitWaves.Data.Entities
{
    /// <summary>
    /// 为题目提供描述信息。
    /// </summary>
    public sealed class ProblemDescription
    {
        /// <summary>
        /// 获取或设置题目背景描述。
        /// </summary>
        public string Legend { get; set; }

        /// <summary>
        /// 获取或设置题目输入描述。
        /// </summary>
        public string Input { get; set; }

        /// <summary>
        /// 获取或设置题目输出描述。
        /// </summary>
        public string Output { get; set; }

        /// <summary>
        /// 获取或设置题目样例列表。
        /// </summary>
        public List<ProblemSampleTest> SampleTests { get; set; }

        /// <summary>
        /// 获取或设置题目的提示信息。
        /// </summary>
        public string Notes { get; set; }
    }
}
