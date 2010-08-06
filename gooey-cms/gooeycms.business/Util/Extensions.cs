using System;
using System.Collections.Generic;

namespace Gooeycms.Business.Util
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
    }
}
