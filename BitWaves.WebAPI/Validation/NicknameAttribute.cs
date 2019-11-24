using System.ComponentModel.DataAnnotations;

namespace BitWaves.WebAPI.Validation
{
    /// <summary>
    /// 为用户昵称提供数据验证逻辑。
    /// </summary>
    public sealed class NicknameAttribute : ValidationAttributeWrapper
    {
        /// <summary>
        /// 用户昵称的最短长度。
        /// </summary>
        public const int MinLength = 1;

        /// <summary>
        /// 用户昵称的最长长度。
        /// </summary>
        public const int MaxLength = 64;

        /// <summary>
        /// 初始化 <see cref="NicknameAttribute"/> 类的新实例。
        /// </summary>
        public NicknameAttribute()
            : base(new RequiredAttribute(),
                   new RangeAttribute(MinLength, MaxLength))
        { }
    }
}
