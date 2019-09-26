using System;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace BitWaves.Data
{
    using Entities;

    /// <summary>
    /// 表示 BitWaves 的数据仓库。
    /// </summary>
    public sealed class Repository
    {
        /// <summary>
        /// 初始化 <see cref="Repository"/> 类的新实例。
        /// </summary>
        /// <param name="connectionString">到 MongoDB 实例的连接字符串。</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="connectionString"/> 为 null。
        /// </exception>
        public Repository(string connectionString)
        {
            Contract.NotNull(connectionString, nameof(connectionString));

            MongoClient = new MongoClient(connectionString);
            Database = MongoClient.GetDatabase("BitWaves");
        }

        /// <summary>
        /// 获取包含到 MongoDB 实例的连接池的 <see cref="IMongoClient"/> 对象。
        /// </summary>
        public IMongoClient MongoClient { get; }

        /// <summary>
        /// 获取 BitWaves 数据库实例对象。
        /// </summary>
        public IMongoDatabase Database { get; }

        /// <summary>
        /// 获取用户数据集。
        /// </summary>
        public IMongoCollection<User> Users => Database.GetCollection<User>("Users");

        /// <summary>
        /// 获取题目数据集。
        /// </summary>
        public IMongoCollection<Problem> Problems => Database.GetCollection<Problem>("Problems");

        /// <summary>
        /// 获取 BitWaves 的 MongoDB 实例上的 GridFS 数据集。
        /// </summary>
        public IGridFSBucket Files => new GridFSBucket(Database);
    }

    namespace DependencyInjection
    {
        /// <summary>
        /// 为 <see cref="Repository"/> 提供依赖注入逻辑。
        /// </summary>
        public static class RepositoryExtensions
        {
            /// <summary>
            /// 将 <see cref="Repository"/> 依赖项添加到指定的服务集合中。
            /// </summary>
            /// <param name="services">服务集合。</param>
            /// <param name="connectionString">连接字符串。</param>
            /// <returns>服务集合。</returns>
            /// <exception cref="ArgumentNullException">
            ///     <paramref name="services"/> 为 null
            ///     或
            ///     <paramref name="connectionString"/> 为 null。
            /// </exception>
            public static IServiceCollection AddBitWavesRepository(this IServiceCollection services,
                                                                   string connectionString)
            {
                Contract.NotNull(services, nameof(services));
                Contract.NotNullOrEmpty(connectionString, "连接字符串不能为空。", nameof(connectionString));

                return services.AddScoped(serviceProvider => new Repository(connectionString));
            }
        }
    }
}
