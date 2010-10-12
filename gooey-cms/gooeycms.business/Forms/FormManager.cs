using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Data.Model.Form;
using Beachead.Persistence.Hibernate;
using Gooeycms.Business.Util;

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

        public void Save(CmsSavedForm savedForm)
        {
            if (savedForm.Guid == null)
                savedForm.Guid = System.Guid.NewGuid().ToString();
            if (savedForm.SubscriptionId == null)
                savedForm.SubscriptionId = CurrentSite.Guid.Value;

            CmsSavedFormDao dao = new CmsSavedFormDao();
            
            CmsSavedForm exists = dao.FindBySiteAndName(savedForm.SubscriptionId, savedForm.Name);
            if (exists != null)
                throw new ArgumentException("The saved form name is already in use and may not be used again.");

            using (Transaction tx = new Transaction())
            {
                dao.Save<CmsSavedForm>(savedForm);
                tx.Commit();
            }
        }

        public CmsSavedForm GetSavedForm(Data.Guid siteGuid, Data.Guid guid)
        {
            CmsSavedFormDao dao = new CmsSavedFormDao();
            return dao.FindBySiteAndGuid(siteGuid, guid);
        }

        public IList<CmsSavedForm> GetSavedForms(Data.Guid siteGuid)
        {
            CmsSavedFormDao dao = new CmsSavedFormDao();
            return dao.FindBySite(siteGuid);
        }

    }
}
