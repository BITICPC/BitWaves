using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BitWaves.WebAPI.Utils
{
    /// <summary>
    /// 为 <see cref="IFormFile"/> 提供最长长度验证逻辑。
    /// </summary>
    public sealed class FileMaxLengthAttribute : ValidationAttribute
    {
        /// <summary>
        /// 初始化 <see cref="FileMaxLengthAttribute"/> 类的新实例。
        /// </summary>
        /// <param name="maxLength">文件的最长字节长度。</param>
        public FileMaxLengthAttribute(long maxLength)
        {
            MaxLength = maxLength;
        }

        /// <summary>
        /// 获取文件的最长字节长度。
        /// </summary>
        public long MaxLength { get; }

        /// <inheritdoc />
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = (IFormFile) value;
            if (file.Length > MaxLength)
            {
                return new ValidationResult("File is too large.");
            }

            return ValidationResult.Success;
        }
    }
}
