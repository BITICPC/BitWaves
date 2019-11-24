using System;
using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 提供一个 <see cref="IMappingAction{TSource, TDestination}"/> 的实现，用于将当前 HTTP 上下文中的登录用户名映射到
    /// 目标类型的指定属性上。
    /// </summary>
    /// <typeparam name="TEntity">实体对象类型。</typeparam>
    internal sealed class SetAuthorAction<TEntity> : IMappingAction<object, TEntity>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<SetAuthorAction<TEntity>> _logger;

        private readonly PropertyInfo _targetProperty;

        /// <summary>
        /// 初始化 <see cref="SetAuthorAction{TEntity}"/> 类的新实例。该构造器应仅由 IoC 容器调用。
        /// </summary>
        /// <param name="httpContextAccessor">用于访问 HTTP 上下文信息的 IoC 包装。</param>
        /// <param name="logger">日志记录器。</param>
        /// <exception cref="InvalidOperationException">
        ///     无法在类型 <typeparamref name="TEntity"/> 上找到 Author 属性
        ///     或
        ///     类型 <typeparamref name="TEntity"/> 上的 Author 属性不可写。
        /// </exception>
        public SetAuthorAction(IHttpContextAccessor httpContextAccessor,
                               ILogger<SetAuthorAction<TEntity>> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;

            _targetProperty = typeof(TEntity).GetProperty("Author");
            if (_targetProperty == null)
                throw new InvalidOperationException($"无法在类型 {typeof(TEntity)} 上找到 Author 属性。");
            if (!_targetProperty.CanWrite)
                throw new InvalidOperationException($"类型 {typeof(TEntity)} 上的 Author 属性不可写。");
        }

        /// <summary>
        /// 获取 HTTP 上下文。
        /// </summary>
        private HttpContext HttpContext => _httpContextAccessor.HttpContext;

        /// <inheritdoc />
        public void Process(object source, TEntity destination, ResolutionContext context)
        {
            var author = HttpContext.User.Identity.Name;
            _targetProperty.GetSetMethod().Invoke(destination, new object[] { author });
        }
    }
}
