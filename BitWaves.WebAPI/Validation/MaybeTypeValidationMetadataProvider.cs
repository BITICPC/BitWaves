using System;
using BitWaves.Data.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace BitWaves.WebAPI.Validation
{
    /// <summary>
    /// 为 <see cref="Maybe{T}"/> 提供模型验证元数据。
    /// </summary>
    public sealed class MaybeTypeValidationMetadataProvider : IValidationMetadataProvider
    {
        /// <inheritdoc />
        public void CreateValidationMetadata(ValidationMetadataProviderContext context)
        {
            if (MaybeUtils.IsMaybeType(context.Key.ModelType))
            {
                // Maybe<T> should be considered as a whole during validation. Prevent ASP.NET Core from validating
                // the children of a Maybe<T> instance.
                context.ValidationMetadata.ValidateChildren = false;
            }
        }
    }

    /// <summary>
    /// 为 <see cref="MaybeTypeValidationMetadataProvider"/> 类型提供扩展方法。
    /// </summary>
    public static class MaybeTypeValidationMetadataProviderExtensions
    {
        /// <summary>
        /// 将 <see cref="MaybeTypeValidationMetadataProvider"/> 类型的实例添加到给定的 MVC 选项中。
        /// </summary>
        /// <param name="options">MVC 选项。</param>
        /// <exception cref="ArgumentNullException"><paramref name="options"/> 为 null。</exception>
        public static MvcOptions AddMaybeTypeValidationMetadataProvider(this MvcOptions options)
        {
            Contract.NotNull(options, nameof(options));

            options.ModelMetadataDetailsProviders.Add(new MaybeTypeValidationMetadataProvider());
            return options;
        }
    }
}
