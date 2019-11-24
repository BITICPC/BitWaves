using System.ComponentModel.DataAnnotations;

namespace BitWaves.WebAPI.Validation
{
    /// <summary>
    /// 为密码字段提供数据验证逻辑。
    /// </summary>
    public sealed class PasswordAttribute : ValidationAttributeWrapper
    {
        /// <summary>
        /// 最短密码长度。
        /// </summary>
        public const int MinLength = 6;

        /// <summary>
        /// 初始化 <see cref="PasswordAttribute"/> 类的新实例。
        /// </summary>
        public PasswordAttribute()
            : base(new RequiredAttribute(),
                   new MinLengthAttribute(MinLength))
        { }
    }
}
