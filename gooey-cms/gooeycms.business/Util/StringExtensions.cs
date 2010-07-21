using System;

namespace Gooeycms.Business.Util
{
    public static class StringExtensions
    {
        public static bool EqualsCaseInsensitive(this String item, String param)
        {
            return String.Equals(item, param, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
