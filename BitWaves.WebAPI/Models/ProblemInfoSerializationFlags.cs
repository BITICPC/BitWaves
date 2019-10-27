namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为 <see cref="ProblemInfo"/> 实例对象的序列化操作提供选项。
    /// </summary>
    public enum ProblemInfoSerializationFlags
    {
        /// <summary>
        /// 仅序列化出现在题目列表中的数据成员。
        /// </summary>
        Less,

        /// <summary>
        /// 序列化 <see cref="ProblemInfo"/> 实例对象的所有数据成员。
        /// </summary>
        Full
    }
}
