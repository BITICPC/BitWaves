namespace BitWaves.WebAPI.Services
{
    /// <summary>
    /// 为 <see cref="IJudgeService"/> 提供选项。
    /// </summary>
    public sealed class JudgeServiceOptions
    {
        /// <summary>
        /// 获取或设置评测集群的控制节点的地址。
        /// </summary>
        public string JudgeBoardAddress { get; set; }
    }
}
