using System.ComponentModel.DataAnnotations;

namespace BitWaves.WebAPI.Validation
{
    /// <summary>
    /// 为语言显示名称字段提供数据验证逻辑。
    /// </summary>
    public sealed class LanguageDisplayNameAttribute : ValidationAttributeWrapper
    {
        /// <summary>
        /// 语言显示名称的最短长度。
        /// </summary>
        public const int MinLength = 1;

        /// <summary>
        /// 语言显示名称的最长长度。
        /// </summary>
        public const int MaxLength = 128;

        /// <summary>
        /// 初始化 <see cref="LanguageDisplayNameAttribute"/> 类的新实例。
        /// </summary>
        public LanguageDisplayNameAttribute()
            : base(new RequiredAttribute(),
                   new MinLengthAttribute(MinLength),
                   new MaxLengthAttribute(MaxLength))
        { }
    }
}
