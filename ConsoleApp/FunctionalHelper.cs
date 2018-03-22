using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public static class FunctionalHelper
    {
        public static void CatchOutOfMemoryEx(this Action action)
        {
            try
            {
                action();
            }
            catch (OutOfMemoryException ex)
            {
                Console.WriteLine($"Catched out of memory exception");
            }
        }

        public static IEnumerable<TSource> DoAction<TSource>(this IEnumerable<TSource> items, Action<TSource> action)
        {
            foreach (var item in items)
            {
                action(item);
                yield return item;
            }
        }

        public static IEnumerable<TResult> DoFunction<TSource, TResult>(this IEnumerable<TSource> items, Func<TSource, TResult> func)
        {
            foreach (var item in items)
            {
                yield return func(item);
            }
        }
    }
}
