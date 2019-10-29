using BitWaves.Data.Entities;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 提供 <see cref="ProblemInfo"/> 的应用场景。
    /// </summary>
    public enum ProblemInfoScheme
    {
        /// <summary>
        /// <see cref="ProblemInfo"/> 用于题目列表中的数据模型。此时仅从 <see cref="Problem"/> 实体对象中抽取必要的部分信息。
        /// </summary>
        List,

        /// <summary>
        /// <see cref="ProblemInfo"/> 用于题目详细信息的数据模型。此时将抽取 <see cref="Problem"/> 实体对象的所有数据成员。
        /// </summary>
        Full
    }
}
