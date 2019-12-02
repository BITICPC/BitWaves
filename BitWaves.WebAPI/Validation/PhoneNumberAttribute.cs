using System.ComponentModel.DataAnnotations;

namespace BitWaves.WebAPI.Validation
{
    /// <summary>
    /// 为手机号字段提供数据验证逻辑。
    /// </summary>
    public sealed class PhoneNumberAttribute : ValidationAttributeWrapper
    {
        /// <summary>
        /// 验证手机号正确性所使用的正则表达式。
        /// </summary>
        public const string RegularExpression = @"\d{11}";

        /// <summary>
        /// 初始化 <see cref="PhoneNumberAttribute"/> 类的新实例。
        /// </summary>
        public PhoneNumberAttribute()
            : base(new RegularExpressionAttribute(RegularExpression))
        { }
    }
}
