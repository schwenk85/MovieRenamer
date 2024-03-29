using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MovieRenamer
{
    /// <summary>
    ///     Extended Collection/List functionality
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        ///     Algorithm to sort a list or a collection.
        ///     Source: http://kiwigis.blogspot.de/2010/03/how-to-sort-obversablecollection.html
        /// </summary>
        public static void BubbleSort(this IList list)
        {
            for (var i = list.Count - 1; i >= 0; i--)
            {
                for (var j = 1; j <= i; j++)
                {
                    var item1 = list[j - 1];
                    var item2 = list[j];
                    if (((IComparable)item1).CompareTo(item2) > 0)
                    {
                        list.Remove(item1);
                        list.Insert(j, item1);
                    }
                }
            }
        }

        public static T GetPrevious<T>(this IList<T> list, T item)
        {
            var index = list.IndexOf(item);

            if (index < 0)
            {
                throw new ItemNotInCollectionException<T>(item);
            }

            return index == 0 ? list.Last() : list[index - 1];
        }

        public static T GetNext<T>(this IList<T> list, T item)
        {
            var index = list.IndexOf(item);

            if (index < 0)
            {
                throw new ItemNotInCollectionException<T>(item);
            }

            return index == list.Count - 1 ? list.First() : list[index + 1];
        }

        private class ItemNotInCollectionException<T> : Exception
        {
            public ItemNotInCollectionException(T item) : base($"Collection does not contain '{item}'.") { }
        }
    }

    /// <summary>
    ///     Source: http://stackoverflow.com/questions/146134/how-to-remove-illegal-characters-from-path-and-filenames
    /// </summary>
    public static class StringExtensions
    {
        public static string RemoveBadCharacters(this string str)
        {
            var regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var regex = new Regex($"[{Regex.Escape(regexSearch)}]");
            return regex.Replace(str, string.Empty);
        }
    }
}