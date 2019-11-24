using System.ComponentModel.DataAnnotations;

namespace BitWaves.WebAPI.Validation
{
    /// <summary>
    /// 为语言标识符提供数据验证逻辑。
    /// </summary>
    public sealed class LanguageIdentifierAttribute : ValidationAttributeWrapper
    {
        /// <summary>
        /// 语言标识符的最短长度。
        /// </summary>
        public const int MinLength = 1;

        /// <summary>
        /// 语言标识符的最长长度。
        /// </summary>
        public const int MaxLength = 32;

        /// <summary>
        /// 初始化 <see cref="LanguageIdentifierAttribute"/> 类的新实例。
        /// </summary>
        public LanguageIdentifierAttribute()
            : base(new RequiredAttribute(),
                   new MinLengthAttribute(MinLength),
                   new MaxLengthAttribute(MaxLength))
        { }
    }
}
