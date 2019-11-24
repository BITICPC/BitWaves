using System.ComponentModel.DataAnnotations;

namespace BitWaves.WebAPI.Validation
{
    /// <summary>
    /// 为学校字段提供数据验证逻辑。
    /// </summary>
    public sealed class SchoolAttribute : ValidationAttributeWrapper
    {
        /// <summary>
        /// 学校字段的最短长度。
        /// </summary>
        public const int MinLength = 1;

        /// <summary>
        /// 学校字段的最长长度。
        /// </summary>
        public const int MaxLength = 64;

        /// <summary>
        /// 初始化 <see cref="SchoolAttribute"/> 类的新实例。
        /// </summary>
        public SchoolAttribute()
            : base(new MinLengthAttribute(MinLength),
                new MaxLengthAttribute(MaxLength))
        { }
    }
}
