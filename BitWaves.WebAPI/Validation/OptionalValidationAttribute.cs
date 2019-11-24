using System;
using System.ComponentModel.DataAnnotations;
using BitWaves.WebAPI.Utils;

namespace BitWaves.WebAPI.Validation
{
    /// <summary>
    /// 为 <see cref="Optional{T}"/> 提供数据验证标记。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public sealed class OptionalValidationAttribute : ValidationAttribute
    {
        private readonly Type _innerAttributeType;
        private readonly object[] _innerAttributeArgs;

        /// <summary>
        /// 初始化 <see cref="OptionalValidationAttribute"/> 类的新实例。
        /// </summary>
        /// <param name="innerAttributeType">要使用的内部 <see cref="ValidationAttribute"/> 类型。</param>
        /// <param name="innerAttributeArgs">用于构造内部 <see cref="ValidationAttribute"/> 类型的对象的参数。</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="innerAttributeType"/> 为 null
        ///     或
        ///     <paramref name="innerAttributeArgs"/> 为 null。
        /// </exception>
        public OptionalValidationAttribute(Type innerAttributeType, params object[] innerAttributeArgs)
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

            if (!OptionalUtils.IsOptionalType(value.GetType()))
            {
                return true;
            }

            if (!OptionalUtils.TryGetOptionalValue(value, out var innerValue))
            {
                return true;
            }

            var innerAttr = ActivateInnerValidationAttribute();
            return innerAttr.IsValid(innerValue);
        }
    }
}
