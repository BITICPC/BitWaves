using System.ComponentModel.DataAnnotations;

namespace BitWaves.WebAPI.Validation
{
    /// <summary>
    /// 为用户名字段提供数据验证逻辑。
    /// </summary>
    public sealed class UsernameAttribute : ValidationAttributeWrapper
    {
        /// <summary>
        /// 用户名的最短长度。
        /// </summary>
        public const int MinLength = 3;

        /// <summary>
        /// 用户名的最长长度。
        /// </summary>
        public const int MaxLength = 64;

        /// <summary>
        /// 初始化 <see cref="UsernameAttribute"/> 类的新实例。
        /// </summary>
        public UsernameAttribute()
            : base(new RequiredAttribute(),
                   new MinLengthAttribute(MinLength),
                   new MaxLengthAttribute(MaxLength))
        { }
    }
}
