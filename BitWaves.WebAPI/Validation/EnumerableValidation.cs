using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace BitWaves.WebAPI.Validation
{
    /// <summary>
    /// 为 <see cref="IEnumerable"/> 类型的对象提供迭代元素验证逻辑。
    /// </summary>
    public sealed class EnumerableValidation : ValidationAttribute
    {
        private readonly Type _elementValidationType;
        private readonly object[] _elementValidatorArgs;

        /// <summary>
        /// 初始化 <see cref="EnumerableValidation"/> 类的新实例。
        /// </summary>
        /// <param name="elementValidationType">将应用于迭代元素的数据验证标注类型。</param>
        /// <param name="elementValidatorArgs">
        ///     用于构造 <paramref name="elementValidationType"/> 类型的实例对象的参数。
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="elementValidationType"/> 为 null。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="elementValidationType"/> 所表示的类型不是 <see cref="ValidationAttribute"/> 的子类型。
        /// </exception>
        public EnumerableValidation(Type elementValidationType, params object[] elementValidatorArgs)
        {
            Contract.NotNull(elementValidationType, nameof(elementValidationType));

            if (!elementValidationType.IsSubclassOf(typeof(ValidationAttribute)))
                throw new ArgumentException($"{elementValidationType} 不是 {typeof(ValidationAttribute)} 的子类。");

            _elementValidationType = elementValidationType;
            _elementValidatorArgs = elementValidatorArgs;
        }

        /// <inheritdoc />
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is IEnumerable enumerable))
            {
                return ValidationResult.Success;
            }

            var innerValidator =
                (ValidationAttribute) Activator.CreateInstance(_elementValidationType, _elementValidatorArgs);

            if (validationContext == null)
            {
                validationContext = new ValidationContext(value);
            }

            foreach (var element in enumerable)
            {
                var innerResult = innerValidator.GetValidationResult(element, validationContext);
                if (innerResult != ValidationResult.Success)
                {
                    return innerResult;
                }
            }

            return ValidationResult.Success;
        }
    }
}
