using System.ComponentModel.DataAnnotations;

namespace BitWaves.WebAPI.Validation
{
    /// <summary>
    /// 为内存限制提供数据验证逻辑。
    /// </summary>
    public sealed class MemoryLimitAttribute : ValidationAttributeWrapper
    {
        /// <summary>
        /// 内存限制的最小值。
        /// </summary>
        public const int MinValue = 32;

        /// <summary>
        /// 内存限制的最大值。
        /// </summary>
        public const int MaxValue = 1024;

        /// <summary>
        /// 初始化 <see cref="MemoryLimitAttribute"/> 类的新实例。
        /// </summary>
        public MemoryLimitAttribute()
            : base(new RangeAttribute(MinValue, MaxValue))
        { }
    }
}
