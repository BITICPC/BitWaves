using System.ComponentModel.DataAnnotations;

namespace BitWaves.WebAPI.Validation
{
    /// <summary>
    /// 为学号字段提供数据验证逻辑。
    /// </summary>
    public sealed class StudentIdAttribute : ValidationAttributeWrapper
    {
        /// <summary>
        /// 学号的最短长度。
        /// </summary>
        public const int MinLength = 4;

        /// <summary>
        /// 学号的最长长度。
        /// </summary>
        public const int MaxLength = 32;

        /// <summary>
        /// 初始化 <see cref="StudentIdAttribute"/> 类的新实例。
        /// </summary>
        public StudentIdAttribute()
            : base(new MinLengthAttribute(MinLength),
                   new MaxLengthAttribute(MaxLength))
        { }
    }
}
