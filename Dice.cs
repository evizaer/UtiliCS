using System;
using System.Collections.Generic;
using System.Linq;

namespace UtiliCS
{
    public static class Dice
    {
        public static void Initialize(int seed)
        {
            _Rand = new Random(seed);
        }

        private static Random _Rand = new Random();

        public static int Roll(int sides)
        {
            return _Rand.Next(sides) + 1;
        }

        public static T Choose<T>(this List<T> objects)
        {
            return objects[Roll(objects.Count) - 1];
        }

        public static List<T> Choose<T>(this IEnumerable<T> objects, int count)
        {
            if (count < 1)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            return objects.OrderBy(_ => GetDouble()).Take(count).ToList();
        }

        public static T Choose<T>(this IEnumerable<T> objects)
        {
            return objects.Choose(1).Single();
        }

        public static List<int> ChooseInRange(int rangeStart, int rangeEnd, int count)
        {
            return Enumerable.Range(rangeStart, rangeEnd - rangeStart).OrderBy(_ => GetDouble()).Take(count).ToList();
        }

        public static double GetDouble()
        {
            return _Rand.NextDouble();
        }

        /// <summary>
        /// Get random int between first and second. Doesn't matter which is higher. Range is low-inclusive, high-exclusive.
        /// </summary>
        public static int GetInt(int first, int second = 0)
        {
            int low = first, high = second;
            if (first > second)
            {
                low = second;
                high = first;
            }
            return _Rand.Next(high - low) + low;
        }

        public static int GetIntWithVariance(int baseValue, double variance)
        {
            double factor = 1 + (GetDouble() * (variance * 2) - variance);

            return Convert.ToInt32(baseValue * factor);
        }

        /// <summary>
        /// Returns base value + a random number between -jitter and +jitter.
        /// </summary>
        public static int GetIntWithJitter(int baseValue, int jitter)
        {
            return baseValue + GetInt(-jitter, jitter);
        }

        // ReSharper disable once MethodOverloadWithOptionalParameter
        public static double GetDouble(double min = 0.0, double max = 1.0) 
        {
            return _Rand.NextDouble() * (max - min) + min;
        }

        public static double GetDoubleWithJitter(double baseValue, double jitter, double min = double.MinValue, double max = double.MaxValue)
        {
            return (baseValue + GetDouble(-jitter, jitter)).Clamp(min, max);
        }

        public static T ChooseByWeight<T>(this IEnumerable<T> items, Func<T, double> getProbability) where T :class
        {
            return new ChanceStack<T>(items, getProbability).Choose();
        }

        public class ChanceStack<T> where T : class
        {
            readonly List<Tuple<double, T>> _Stack;

            public ChanceStack(IEnumerable<T> inputs, Func<T, double> getProbability)
            {
                double sum = 0;
                _Stack = inputs.Select(item => Tuple.Create(sum += getProbability(item), item)).Reverse().ToList();
            }

            public T Choose()
            {
                var target = GetDouble();
                return _Stack.TakeWhile((pair, _) => pair.Item1 > target)
                            .Select(pair => pair.Item2).LastOrDefault();
            }
        }
        /*
        public static List<T> ChooseFast<T>(List<T> from, int count)
        {
            List<int> targetIndexes = new List<int>() { Dice.GetInt(0, from.Count) };

            int cur = 0;

            while (cur < count)
            {
                var i = Dice.GetInt(0, from.Count - cur);

                cur++;
            }
        }*/
    }
}