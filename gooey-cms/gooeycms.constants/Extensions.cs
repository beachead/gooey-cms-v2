﻿using System;
using System.Collections.Generic;
using System.Text;

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

        public static Boolean IsEmpty(this String item)
        {
            return (String.IsNullOrEmpty(item));
        }

        public static String AsString<T>(this IList<T> items, String separator)
        {
            StringBuilder builder  = new StringBuilder();
            foreach (T item in items)
            {
                builder.Append(item.ToString()).Append(separator);
            }
            builder = builder.Remove(builder.Length - 1, 1);

            return builder.ToString();
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
