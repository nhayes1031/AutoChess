using System;
using System.Collections.Generic;

namespace Server.Extensions {
    static class ListExtensions {

        private static readonly Random rng = new();

        public static void Shuffle<T>(this IList<T> list) {
            int n = list.Count;
            while (n > 1) {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static T PickRandom<T>(this IList<T> list) {
            return list[rng.Next(list.Count)];
        }
    }
}
