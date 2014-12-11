using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace UtiliCS
{
    public static class Extensions
    {
        public static void DebugPrint(this int[,] plane, int columnWidth) {
            var height = plane.GetLength(1);
            var width = plane.GetLength(0);

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    Debug.Write(plane[x, y].ToString().PadLeft(columnWidth));
                }
                Debug.Write("\n");
            }
        }

        public static void DebugPrint(this double[,] plane, int columnWidth, string format) {
            var height = plane.GetLength(1);
            var width = plane.GetLength(0);

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    Debug.Write(plane[x, y].ToString(format).PadLeft(columnWidth));
                }
                Debug.Write("\n");
            }
        }

        public static T GetAt<T>(this IList<T> flattenedPlane, int x, int y, int width)
        {
            return flattenedPlane[y * width + x];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Clamp(this int value, int min = Int32.MinValue, int max = Int32.MaxValue)
        {
            if (value > max) return max;
            if (value < min) return min;
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Clamp(this double value, double min = double.MinValue, double max = double.MaxValue)
        {
            if (value > max) return max;
            if (value < min) return min;
            return value;
        }

        public static IEnumerable<T> IterateAll<T>(this Enum e)
        {
            return from object item in Enum.GetValues(typeof(T)) select (T)item;
        }

        // MinBy and MaxBy methods are modified versions of methods found in the MoreLinq library https://code.google.com/p/morelinq/

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector)
        {
            return source.MinBy(selector, Comparer<TKey>.Default);
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            using (IEnumerator<TSource> sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence was empty");
                }
                TSource min = sourceIterator.Current;
                TKey minKey = selector(min);
                while (sourceIterator.MoveNext())
                {
                    TSource candidate = sourceIterator.Current;
                    TKey candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, minKey) < 0)
                    {
                        min = candidate;
                        minKey = candidateProjected;
                    }
                }
                return min;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector)
        {
            return source.MaxBy(selector, Comparer<TKey>.Default);
        }

        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            using (IEnumerator<TSource> sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence was empty");
                }
                TSource max = sourceIterator.Current;
                TKey maxKey = selector(max);
                while (sourceIterator.MoveNext())
                {
                    TSource candidate = sourceIterator.Current;
                    TKey candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, maxKey) > 0)
                    {
                        max = candidate;
                        maxKey = candidateProjected;
                    }
                }
                return max;
            }
        }

        public static IEnumerable<int> To(this int start, int end, int step = 1, bool endInclusive = false)
        {
            var stepValue = end.CompareTo(start) * Math.Abs(step);

            if (stepValue == 0)
            {
                yield return start;
            }
            else
            {
                if (endInclusive) ++end;
                int cur = start;
                while ((stepValue > 0 && cur < end) || (stepValue < 0 && end < cur))
                {
                    yield return cur;
                    cur += stepValue;
                }
            }
        }

        public static void Do<T>(this IEnumerable<T> ls, Action<T> action)
        {
            foreach (var item in ls)
            {
                action(item);
            }
        }

        public static void Do<TKey, TValue>(this IDictionary<TKey, TValue> dict, Action<TKey, TValue> action)
        {
            foreach (var pair in dict)
            {
                action(pair.Key, pair.Value);
            }
        }

        public static IEnumerable<int> Times(this int count)
        {
            return Enumerable.Range(1, count);
        } 
    }
}
