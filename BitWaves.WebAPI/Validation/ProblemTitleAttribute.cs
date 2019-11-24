using System.ComponentModel.DataAnnotations;

namespace BitWaves.WebAPI.Validation
{
    /// <summary>
    /// 为题目标题字段提供数据验证逻辑。
    /// </summary>
    public sealed class ProblemTitleAttribute : ValidationAttributeWrapper
    {
        /// <summary>
        /// 题目标题的最短长度。
        /// </summary>
        public const int MinLength = 1;

        /// <summary>
        /// 题目标题的最长长度。
        /// </summary>
        public const int MaxLength = 128;

        /// <summary>
        /// 初始化 <see cref="ProblemTitleAttribute"/> 类的新实例。
        /// </summary>
        public ProblemTitleAttribute()
            : base(new RequiredAttribute(),
                   new MinLengthAttribute(MinLength),
                   new MaxLengthAttribute(MaxLength))
        { }
    }
}
