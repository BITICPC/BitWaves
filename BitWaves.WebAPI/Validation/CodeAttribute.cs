using System.ComponentModel.DataAnnotations;

namespace BitWaves.WebAPI.Validation
{
    /// <summary>
    /// 为源代码字段提供数据验证逻辑。
    /// </summary>
    public sealed class CodeAttribute : ValidationAttributeWrapper
    {
        /// <summary>
        /// 源代码的最小长度。
        /// </summary>
        public const int MinLength = 1;

        /// <summary>
        /// 初始化 <see cref="CodeAttribute"/> 类的新实例。
        /// </summary>
        public CodeAttribute()
            : base(new RequiredAttribute(),
                   new MinLengthAttribute(MinLength))
        { }
    }
}
