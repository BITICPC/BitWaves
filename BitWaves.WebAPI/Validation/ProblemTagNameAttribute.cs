using System.ComponentModel.DataAnnotations;

namespace BitWaves.WebAPI.Validation
{
    /// <summary>
    /// 为题目标签名称提供数据验证逻辑。
    /// </summary>
    public sealed class ProblemTagNameAttribute : ValidationAttributeWrapper
    {
        /// <summary>
        /// 题目标签名称的最短长度。
        /// </summary>
        public const int MinLength = 1;

        /// <summary>
        /// 题目标签名称的最长长度。
        /// </summary>
        public const int MaxLength = 32;

        /// <summary>
        /// 初始化 <see cref="ProblemTagNameAttribute"/> 类的新实例。
        /// </summary>
        public ProblemTagNameAttribute()
            : base(new RequiredAttribute(),
                   new MinLengthAttribute(MinLength),
                   new MaxLengthAttribute(MaxLength))
        { }
    }
}
