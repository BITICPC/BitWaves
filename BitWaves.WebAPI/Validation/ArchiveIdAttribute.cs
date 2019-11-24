using System.ComponentModel.DataAnnotations;

namespace BitWaves.WebAPI.Validation
{
    /// <summary>
    /// 为公开题目集中的题目 ID 提供数据验证逻辑。
    /// </summary>
    public sealed class ArchiveIdAttribute : ValidationAttributeWrapper
    {
        /// <summary>
        /// 题目 ID 最小值。
        /// </summary>
        public const int MinValue = 0;

        /// <summary>
        /// 题目 ID 最大值。
        /// </summary>
        public const int MaxValue = int.MaxValue;

        /// <summary>
        /// 初始化 <see cref="ArchiveIdAttribute"/> 类的新实例。
        /// </summary>
        public ArchiveIdAttribute()
            : base(new RangeAttribute(MinValue, MaxValue))
        { }
    }
}
