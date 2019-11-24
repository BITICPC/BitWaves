using System.ComponentModel.DataAnnotations;

namespace BitWaves.WebAPI.Validation
{
    /// <summary>
    /// 为难度字段提供数据验证逻辑。
    /// </summary>
    public sealed class DifficultyAttribute : ValidationAttributeWrapper
    {
        /// <summary>
        /// 难度的最小值。
        /// </summary>
        public const int MinValue = 0;

        /// <summary>
        /// 难度的最大值。
        /// </summary>
        public const int MaxValue = 100;

        /// <summary>
        /// 初始化 <see cref="DifficultyAttribute"/> 类的新实例。
        /// </summary>
        public DifficultyAttribute()
            : base(new RangeAttribute(MinValue, MaxValue))
        { }
    }
}
