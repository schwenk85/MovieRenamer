using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;

namespace MovieRenamer
{
    /// <summary>
    /// Source: http://kiwigis.blogspot.de/2010/03/how-to-sort-obversablecollection.html
    /// </summary>
    public static class ListExtensions
    {
        public static void BubbleSort(this IList list)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                for (int j = 1; j <= i; j++)
                {
                    object item1 = list[j - 1];
                    object item2 = list[j];
                    if (((IComparable)item1).CompareTo(item2) > 0)
                    {
                        list.Remove(item1);
                        list.Insert(j, item1);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Source: http://stackoverflow.com/questions/146134/how-to-remove-illegal-characters-from-path-and-filenames
    /// </summary>
    public static class StringExtensions
    {
        public static string RemoveBadCharacters(this string str)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(str, string.Empty);
        }
    }
}
