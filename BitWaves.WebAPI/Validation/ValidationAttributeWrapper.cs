using System.ComponentModel.DataAnnotations;

namespace BitWaves.WebAPI.Validation
{
    /// <summary>
    /// 提供在 validation attributes 中调用其他 validation attributes 的能力。
    /// </summary>
    public abstract class ValidationAttributeWrapper : ValidationAttribute
    {
        private readonly ValidationAttribute[] _inner;

        /// <summary>
        /// 初始化 <see cref="ValidationAttributeWrapper"/> 类的新实例。
        /// </summary>
        /// <param name="inner">内部使用的数据验证标注。</param>
        protected ValidationAttributeWrapper(params ValidationAttribute[] inner)
        {
            Contract.NotNull(inner, nameof(inner));

            _inner = inner;
        }

        /// <inheritdoc />
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            foreach (var innerValidator in _inner)
            {
                var innerResult = innerValidator.GetValidationResult(value, validationContext);
                if (innerResult != ValidationResult.Success)
                {
                    return innerResult;
                }
            }

            return ValidationResult.Success;
        }
    }
}
