using System.ComponentModel.DataAnnotations;

namespace BitWaves.WebAPI.Validation
{
    /// <summary>
    /// 为时间限制字段提供数据验证逻辑。
    /// </summary>
    public sealed class TimeLimitAttribute : ValidationAttributeWrapper
    {
        /// <summary>
        /// 时间限制的最小值。
        /// </summary>
        public const int MinValue = 500;

        /// <summary>
        /// 时间限制的最大值。
        /// </summary>
        public const int MaxValue = 10000;

        /// <summary>
        /// 初始化 <see cref="TimeLimitAttribute"/> 类的新实例。
        /// </summary>
        public TimeLimitAttribute()
            : base(new RangeAttribute(MinValue, MaxValue))
        { }
    }
}
