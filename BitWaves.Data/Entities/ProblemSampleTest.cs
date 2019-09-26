namespace BitWaves.Data.Entities
{
    /// <summary>
    /// 为题目提供一组样例。
    /// </summary>
    public sealed class ProblemSampleTest
    {
        /// <summary>
        /// 获取或设置样例输入数据。
        /// </summary>
        public string Input { get; set; }

        /// <summary>
        /// 获取或设置样例输出数据。
        /// </summary>
        public string Output { get; set; }
    }
}
