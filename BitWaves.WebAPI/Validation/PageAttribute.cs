using System.ComponentModel.DataAnnotations;

namespace BitWaves.WebAPI.Validation
{
    /// <summary>
    /// 为页面编号提供数据验证逻辑。
    /// </summary>
    public sealed class PageAttribute : ValidationAttributeWrapper
    {
        /// <summary>
        /// 初始化 <see cref="PageAttribute"/> 类的新实例。
        /// </summary>
        public PageAttribute()
            : base(new RangeAttribute(0, int.MaxValue))
        { }
    }
}
