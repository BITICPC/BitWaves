using System.Collections.Generic;
using MongoDB.Driver;

namespace BitWaves.Data
{
    /// <summary>
    /// 为 MongoDB 组件提供扩展方法。
    /// </summary>
    internal static class MongoExtensions
    {
        /// <summary>
        /// 收集给定 <see cref="IAsyncCursor{T}"/> 中的数据到 <see cref="HashSet{T}"/> 中。
        /// </summary>
        /// <param name="cursor">指向目标数据的 <see cref="IAsyncCursor{T}"/> 对象。</param>
        /// <typeparam name="T">目标数据类型。</typeparam>
        /// <returns>收集到的 <see cref="HashSet{T}"/> 对象。</returns>
        public static HashSet<T> ToHashSet<T>(this IAsyncCursor<T> cursor)
        {
            var set = new HashSet<T>();
            while (cursor.MoveNext())
            {
                foreach (var value in cursor.Current)
                {
                    set.Add(value);
                }
            }

            return set;
        }

        /// <summary>
        /// 测试给定的 MongoDB 数据集是否存在。
        /// </summary>
        /// <param name="collection">MongoDB 数据集。</param>
        /// <typeparam name="T">数据集中的数据类型。</typeparam>
        /// <returns>给定的数据集是否存在。</returns>
        public static bool Exists<T>(this IMongoCollection<T> collection)
        {
            return collection.Database.ListCollectionNames().ToHashSet().Contains(
                collection.CollectionNamespace.CollectionName);
        }
    }
}
