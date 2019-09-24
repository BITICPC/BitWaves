namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 表示用户排名列表的排名依据。
    /// </summary>
    public enum RanklistKey
    {
        /// <summary>
        /// 按照用户总提交数量进行排名。
        /// </summary>
        TotalSubmissions,

        /// <summary>
        /// 按照用户总 AC 提交数量进行排名。
        /// </summary>
        TotalAccepted,

        /// <summary>
        /// 按照用户总共尝试的题目数量进行排名。
        /// </summary>
        TotalProblemsAttempted,

        /// <summary>
        /// 按照用户通过的题目数量进行排名。
        /// </summary>
        TotalProblemsAccepted
    }
}
