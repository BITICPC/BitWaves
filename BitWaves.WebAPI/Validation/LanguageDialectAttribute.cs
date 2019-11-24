using System.ComponentModel.DataAnnotations;

namespace BitWaves.WebAPI.Validation
{
    /// <summary>
    /// 为语言方言字段提供数据验证逻辑。
    /// </summary>
    public sealed class LanguageDialectAttribute : ValidationAttributeWrapper
    {
        /// <summary>
        /// 语言方言的最短长度。
        /// </summary>
        public const int MinLength = 1;

        /// <summary>
        /// 语言方言的最长长度。
        /// </summary>
        public const int MaxLength = 32;

        /// <summary>
        /// 初始化 <see cref="LanguageDialectAttribute"/> 类的新实例。
        /// </summary>
        public LanguageDialectAttribute()
            : base(new RequiredAttribute(),
                   new MinLengthAttribute(MinLength),
                   new MaxLengthAttribute(MaxLength))
        { }
    }
}
