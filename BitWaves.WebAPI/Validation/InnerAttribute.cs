using System;
using System.ComponentModel.DataAnnotations;
using BitWaves.Data.Utils;

namespace BitWaves.WebAPI.Validation
{
    /// <summary>
    /// 为 <see cref="Maybe{T}"/> 类型的内部值提供数据验证标记。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public sealed class InnerAttribute : ValidationAttribute
    {
        private readonly Type _innerAttributeType;
        private readonly object[] _innerAttributeArgs;

        /// <summary>
        /// 初始化 <see cref="InnerAttribute"/> 类的新实例。
        /// </summary>
        /// <param name="innerAttributeType">要使用的内部 <see cref="ValidationAttribute"/> 类型。</param>
        /// <param name="innerAttributeArgs">用于构造内部 <see cref="ValidationAttribute"/> 类型的对象的参数。</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="innerAttributeType"/> 为 null
        ///     或
        ///     <paramref name="innerAttributeArgs"/> 为 null。
        /// </exception>
        public InnerAttribute(Type innerAttributeType, params object[] innerAttributeArgs)
        {
            Contract.NotNull(innerAttributeType, nameof(innerAttributeType));
            Contract.NotNull(innerAttributeArgs, nameof(innerAttributeArgs));

            _innerAttributeType = innerAttributeType;
            _innerAttributeArgs = innerAttributeArgs;
        }

        private ValidationAttribute ActivateInnerValidationAttribute()
        {
            if (!_innerAttributeType.IsSubclassOf(typeof(ValidationAttribute)))
                throw new InvalidOperationException("给定的类型不是 ValidationAttribute 的子类。");

            return (ValidationAttribute) Activator.CreateInstance(_innerAttributeType, _innerAttributeArgs);
        }

        /// <inheritdoc />
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            if (!MaybeUtils.IsMaybe(value))
            {
                return true;
            }

            var maybe = MaybeUtils.Unbox(value);
            if (!maybe.HasValue)
            {
                return true;
            }

            var innerAttr = ActivateInnerValidationAttribute();
            return innerAttr.IsValid(maybe.Value);
        }
    }
}
