using System.Diagnostics.CodeAnalysis;

namespace Tools.Extensions
{
    public static class LinqExtensions
    {
        /// <summary>
        /// </summary>
        /// <example>
        ///     string[] colors = { "Red", "Green", "Blue" };
        ///     string[] sizes = { "Small", "Medium", "Large" };
        ///     var data = new List<![CDATA[string[]]]>();
        ///     data.Add(colors);
        ///     data.Add(sizes);
        ///     var result = CartesianProduct(data);
        ///     the result will be:
        ///     Red,    Small
        ///     Red,    Medium
        ///     Red,    Large
        ///     Green,  Small
        ///     Green,  Medium
        ///     Green,  Large
        ///     Blue,   Small
        ///     Blue,   Medium
        ///     Blue,   Large
        /// </example>
        /// <param name="sequences"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
        {
            IEnumerable<IEnumerable<T>> result = new[] {Enumerable.Empty<T>()};

            foreach (var sequence in sequences)
            {
                var s = sequence;

                result =
                    from seq in result
                    from item in s
                    select seq.Concat(new[] {item});
            }

            return result;
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random random)
        {
            var elements = source.ToArray();

            if (elements.Length == 0)
                yield break;

            // Note i > 0 to avoid final pointless iteration
            for (var i = elements.Length - 1; i > 0; i--)
            {
                // Swap element "i" with a random earlier element it (or itself)
                var swapIndex = random.Next(0, i + 1);
                yield return elements[swapIndex];
                elements[swapIndex] = elements[i];
                // we don't actually perform the swap, we can forget about the
                // swapped element because we already returned it.
            }

            // there is one item remaining that was not returned - we return it now
            yield return elements[0];
        }

        public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> items)
        {
            foreach (var item in items)
                set.Add(item);
        }

        [return: MaybeNull]
        public static T CustomMaxOrDefault<T, TKey>(this IEnumerable<T> source, Func<T, TKey> comparator)
            where TKey : IComparable
        {
            using var enumerator = source.GetEnumerator();

            if (!enumerator.MoveNext())
                return default;

            var max = enumerator.Current;
            var value = comparator(enumerator.Current);

            while (enumerator.MoveNext())
            {
                var temp = comparator(enumerator.Current);

                if (temp.CompareTo(value) > 0)
                {
                    max = enumerator.Current;
                    value = temp;
                }
            }

            return max;
        }

        [return: MaybeNull]
        public static T CustomMinOrDefault<T, TKey>(this IEnumerable<T> source, Func<T, TKey> comparator)
            where TKey : IComparable
        {
            using var enumerator = source.GetEnumerator();

            if (!enumerator.MoveNext())
                return default;

            var min = enumerator.Current;
            var value = comparator(enumerator.Current);

            while (enumerator.MoveNext())
            {
                var temp = comparator(enumerator.Current);

                if (temp.CompareTo(value) < 0)
                {
                    min = enumerator.Current;
                    value = temp;
                }
            }

            return min;
        }

        public static IEnumerable<T> FromParams<T>(params T[] source)
        {
            return source.AsEnumerable();
        }

        public static IEnumerable<T> GetEnumValues<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            using var enumerator = source.GetEnumerator();

            while (enumerator.MoveNext())
                action?.Invoke(enumerator.Current);
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            using var enumerator = source.Select((data, i) => (data, i)).GetEnumerator();

            while (enumerator.MoveNext())
                action?.Invoke(enumerator.Current.data, enumerator.Current.i);
        }
    }
}