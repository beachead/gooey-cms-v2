using System;
using System.Collections.Generic;

namespace Gooeycms.Extensions
{
    public static class Extensions
    {
        public static String StringValue(this Boolean item)
        {
            return (item) ? Boolean.TrueString.ToLower() : Boolean.FalseString.ToLower();
        }

        public static bool EqualsCaseInsensitive(this String item, String param)
        {
            return String.Equals(item, param, StringComparison.CurrentCultureIgnoreCase);
        }

        public static V GetValue<K, V>(this IDictionary<K, V> dictionary, K key)
        {
            V result;
            if (dictionary.TryGetValue(key, out result))
            {
                return result;
            }
            return default(V);
        }

        public static IList<String> SplitAsList(this String item, char value)
        {
            IList<String> results = new List<String>();

            if (item != null)
            {
                String[] items = item.Split(value);
                foreach (String temp in items)
                {
                    if (!String.IsNullOrEmpty(temp))
                        results.Add(temp);
                }
            }

            return results;
        }
    }
}
