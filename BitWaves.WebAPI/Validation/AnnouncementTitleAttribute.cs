using System.ComponentModel.DataAnnotations;

namespace BitWaves.WebAPI.Validation
{
    /// <summary>
    /// 为全站公告标题提供数据验证逻辑。
    /// </summary>
    public sealed class AnnouncementTitleAttribute : ValidationAttributeWrapper
    {
        /// <summary>
        /// 全站公告标题最短长度。
        /// </summary>
        public const int MinLength = 1;

        /// <summary>
        /// 全站公告标题最长长度。
        /// </summary>
        public const int MaxLength = 128;

        /// <summary>
        /// 初始化 <see cref="AnnouncementTitleAttribute"/> 类的新实例。
        /// </summary>
        public AnnouncementTitleAttribute()
            : base(new RequiredAttribute(),
                   new MinLengthAttribute(MinLength),
                   new MaxLengthAttribute(MaxLength))
        { }
    }
}
