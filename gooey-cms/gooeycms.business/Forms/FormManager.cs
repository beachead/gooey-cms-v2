using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Data.Model.Form;
using Beachead.Persistence.Hibernate;

namespace Gooeycms.Business.Forms
{
    public class FormManager
    {
        private static Dictionary<String, Boolean> InvalidFields = new Dictionary<string, bool>();

        private static FormManager instance = new FormManager();
        private FormManager() { }
        public static FormManager Instance { get { return FormManager.instance; } }

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

        public void Save(CmsForm form)
        {
            if (form.Guid == null)
                form.Guid = System.Guid.NewGuid().ToString();

            CmsFormDao dao = new CmsFormDao();
            using (Transaction tx = new Transaction())
            {
                dao.Save<CmsForm>(form);
                tx.Commit();
            }
        }
    }
}
