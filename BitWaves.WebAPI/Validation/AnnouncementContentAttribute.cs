using System.ComponentModel.DataAnnotations;

namespace BitWaves.WebAPI.Validation
{
    /// <summary>
    /// 为全站公告内容提供数据验证逻辑。
    /// </summary>
    public sealed class AnnouncementContentAttribute : ValidationAttributeWrapper
    {
        /// <summary>
        /// 全站公告内容最短长度。
        /// </summary>
        public const int MinLength = 1;

        /// <summary>
        /// 初始化 <see cref="AnnouncementContentAttribute"/> 类的新实例。
        /// </summary>
        public AnnouncementContentAttribute()
            : base(new RequiredAttribute(),
                   new MinLengthAttribute(MinLength))
        { }
    }
}
