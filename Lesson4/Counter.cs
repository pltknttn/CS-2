using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson4
{
    static class Counter<T>
    {
        public static int CountElements(List<T> collections, T item, Func<T, T, bool> fun)
        {
            if (collections == null || collections.Count == 0) return 0;

            int count = 0;
            foreach (var element in collections) if (fun?.Invoke(element, item) == true) count++;
            return count;
        }
        public static int CountElements(List<T> collections, Func<T, bool> fun)
        {
            if (collections == null || collections.Count == 0) return 0;

            int count = 0;
            foreach (var element in collections) if (fun?.Invoke(element) == true) count++;
            return count;
        }
    }
}
