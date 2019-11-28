using System;
using System.ComponentModel.DataAnnotations;

namespace BitWaves.WebAPI.Validation
{
    /// <summary>
    /// 为每一页上的元素数量提供数据验证逻辑。
    /// </summary>
    public sealed class ItemsPerPageAttribute : ValidationAttributeWrapper
    {
        /// <summary>
        /// 初始化 <see cref="ItemsPerPageAttribute"/> 类的新实例。
        /// </summary>
        public ItemsPerPageAttribute()
            : base(new RangeAttribute(1, Int32.MaxValue))
        { }
    }
}
