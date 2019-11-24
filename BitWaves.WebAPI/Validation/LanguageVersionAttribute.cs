using System.ComponentModel.DataAnnotations;

namespace BitWaves.WebAPI.Validation
{
    /// <summary>
    /// 为语言版本字段提供验证逻辑。
    /// </summary>
    public sealed class LanguageVersionAttribute : ValidationAttributeWrapper
    {
        /// <summary>
        /// 语言版本的最短长度。
        /// </summary>
        public const int MinLength = 1;

        /// <summary>
        /// 语言版本的最长长度。
        /// </summary>
        public const int MaxLength = 32;

        /// <summary>
        /// 初始化 <see cref="LanguageVersionAttribute"/> 类的新实例。
        /// </summary>
        public LanguageVersionAttribute()
            : base(new RequiredAttribute(),
                   new MinLengthAttribute(MinLength),
                   new MaxLengthAttribute(MaxLength))
        { }
    }
}
