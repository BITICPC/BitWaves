using System.ComponentModel.DataAnnotations;

namespace BitWaves.WebAPI.Validation
{
    /// <summary>
    /// 为 URL 提供数据验证逻辑。
    /// </summary>
    public sealed class UrlAttribute : ValidationAttributeWrapper
    {
        /// <summary>
        /// 进行数据验证时所使用的正则表达式。
        /// </summary>
        public const string RegularExpression =
            @"^(https?:\/\/)?(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&\/\/=]*)$";

        /// <summary>
        /// 初始化 <see cref="UrlAttribute"/> 类的新实例。
        /// </summary>
        public UrlAttribute()
            : base(new RegularExpressionAttribute(RegularExpression))
        { }
    }
}
