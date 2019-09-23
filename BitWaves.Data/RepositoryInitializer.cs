using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace BitWaves.Data
{
    using Entities;

    /// <summary>
    /// 提供 BitWaves 数据库的初始化逻辑。
    /// </summary>
    public sealed class RepositoryInitializer
    {
        private readonly ILogger<RepositoryInitializer> _logger;
        private readonly Repository _repo;

        /// <summary>
        /// 初始化 <see cref="RepositoryInitializer"/> 的新实例。
        /// </summary>
        /// <param name="repo">要初始化的数据库。</param>
        /// <param name="logger">日志组件。</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="repo"/> 为 null。
        /// </exception>
        public RepositoryInitializer(Repository repo, ILogger<RepositoryInitializer> logger = null)
        {
            Contract.NotNull(repo, nameof(repo));
            Contract.NotNull(logger, nameof(logger));

            _logger = logger;
            _repo = repo;
        }

        /// <summary>
        /// 初始化用户数据集。
        /// </summary>
        /// <param name="force">是否强制初始化。</param>
        private void InitializeUserCollection(bool force)
        {
            _logger?.LogTrace("初始化用户数据集...");

            if (!force && _repo.Users.Exists())
            {
                _logger?.LogTrace("用户数据集已经存在且初始化模式不是强制初始化模式。");
                return;
            }

            var indexesList = new List<CreateIndexModel<User>>
            {
                // Username 上的唯一升序索引
                // 注意：MongoDB 哈希索引不能保证唯一性，因此使用普通索引
                new CreateIndexModel<User>(Builders<User>.IndexKeys.Ascending(user => user.Username),
                                           new CreateIndexOptions { Unique = true })
            };

            _repo.Users.Indexes.CreateMany(indexesList);
            _logger?.LogDebug("在用户数据集上创建了{0}个索引。", indexesList.Count);
        }

        /// <summary>
        /// 初始化数据库。
        /// </summary>
        /// <param name="force">是否强制初始化。</param>
        public void Initialize(bool force = false)
        {
            _logger?.LogTrace("初始化 BitWaves 数据库...");

            InitializeUserCollection(force);
        }

        /// <summary>
        /// 向用户数据集添加种子数据。
        /// </summary>
        private void SeedUserCollection()
        {
            _logger?.LogTrace("向用户数据集添加种子数据...");

            var adminUser = User.Create();
            adminUser.Username = "admin";
            adminUser.SetPassword("bitwaves2019");
            adminUser.IsAdmin = true;

            try
            {
                _repo.Users.InsertOne(adminUser);
            }
            catch (MongoWriteException ex)
            {
                if (ex.WriteError.Category != ServerErrorCategory.DuplicateKey)
                {
                    throw;
                }

                // 管理员用户已经存在。不执行任何操作。
            }

            _logger?.LogDebug("已经向用户数据集中插入了管理员用户。");
        }

        /// <summary>
        /// 向数据库添加种子数据。
        /// </summary>
        public void Seed()
        {
            _logger?.LogTrace("向 BitWaves 数据集中添加种子数据...");

            SeedUserCollection();
        }
    }
}
