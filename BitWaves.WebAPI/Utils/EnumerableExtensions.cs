using System;
using System.Collections.Generic;

namespace BitWaves.WebAPI.Utils
{
    /// <summary>
    /// 为 <see cref="IEnumerable{T}"/> 提供扩展方法。
    /// </summary>
    internal static class EnumerableExtensions
    {
        /// <summary>
        /// 将给定的 <see cref="IEnumerable{TElement}"/> 中的元素加入到给定的 <see cref="IDictionary{TKey, TValue}"/> 中。
        /// 若发生字典键冲突，原有的字典键值将被覆盖。
        /// </summary>
        /// <param name="enumerable">包含输入元素的 <see cref="IEnumerable{TElement}"/> 实例。</param>
        /// <param name="dict">要加入到的 <see cref="IDictionary{TKey, TValue}"/> 实例。</param>
        /// <param name="keySelector">从输入元素中选择字典键。</param>
        /// <param name="valueSelector">从输入元素中选择字典值。</param>
        /// <typeparam name="TElement">输入元素类型。</typeparam>
        /// <typeparam name="TKey">字典键类型。</typeparam>
        /// <typeparam name="TValue">字典值类型。</typeparam>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="enumerable"/> 为 null
        ///     或
        ///     <paramref name="dict"/> 为 null
        ///     或
        ///     <paramref name="keySelector"/> 为 null
        ///     或
        ///     <paramref name="valueSelector"/> 为 null。
        /// </exception>
        public static void PopulateToDictionary<TElement, TKey, TValue>(
            this IEnumerable<TElement> enumerable, IDictionary<TKey, TValue> dict,
            Func<TElement, TKey> keySelector, Func<TElement, TValue> valueSelector)
        {
            Contract.NotNull(enumerable, nameof(enumerable));
            Contract.NotNull(dict, nameof(dict));
            Contract.NotNull(keySelector, nameof(keySelector));
            Contract.NotNull(valueSelector, nameof(valueSelector));

            foreach (var el in enumerable)
            {
                var key = keySelector(el);
                var value = valueSelector(el);
                dict.Add(key, value);
            }
        }

        /// <summary>
        /// 将给定的键值对二元组序列加入到给定的字典中。
        /// </summary>
        /// <param name="enumerable">包含二元组序列的 <see cref="IEnumerable{T}"/> 对象。</param>
        /// <param name="dict">要加入的字典对象。</param>
        /// <typeparam name="TKey">字典键类型。</typeparam>
        /// <typeparam name="TValue">字典值类型。</typeparam>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="enumerable"/> 为 null
        ///     或
        ///     <paramref name="dict"/> 为 null。
        /// </exception>
        public static void PopulateToDictionary<TKey, TValue>(
            this IEnumerable<(TKey, TValue)> enumerable, IDictionary<TKey, TValue> dict)
        {
            PopulateToDictionary(enumerable, dict, i => i.Item1, i => i.Item2);
        }
    }
}
