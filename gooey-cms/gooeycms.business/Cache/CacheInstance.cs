using System;
using System.Collections.Generic;
using Gooeycms.Business.Util;

namespace Gooeycms.Business.Cache
{
    public class CacheInstance
    {
        private Dictionary<String, Object> table = new Dictionary<string, object>();

        public CacheInstance()
        {
        }

        public ResultType Get<ResultType>(String key)
        {
            return (ResultType)table.GetValue(key);
        }

        public void Add(String key, Object item)
        {
            table[key] = item;
        }

        public void Clear()
        {
            table.Clear();
        }
    }
}
