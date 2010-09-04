using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Business.Forms
{
    public class FormManager
    {
        private static Dictionary<String, Boolean> InvalidFields = new Dictionary<string, bool>();

        static FormManager()
        {
            String fields = GooeyConfigManager.DefaultSystemFormFields;
            String[] temp = fields.Split(',');
            foreach (String field in temp)
                InvalidFields.Add(field.ToLower(), true);
        }

        public static Boolean IsValidField(String key)
        {
            return !FormManager.InvalidFields.ContainsKey(key.ToLower());
        }
    }
}
