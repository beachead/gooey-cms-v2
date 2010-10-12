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

        public void Save(CmsSavedForm tempForm)
        {
            CmsSavedFormDao dao = new CmsSavedFormDao();
            CmsSavedForm exists = dao.FindBySiteAndName(tempForm.SubscriptionId, tempForm.Name);
            if (exists == null)
                exists = new CmsSavedForm();

            if (tempForm.Guid == null)
                exists.Guid = System.Guid.NewGuid().ToString();
            if (tempForm.SubscriptionId == null)
                exists.SubscriptionId = CurrentSite.Guid.Value;
            exists.Markup = tempForm.Markup;
            exists.DateSaved = tempForm.DateSaved;
            exists.Name = tempForm.Name;

            using (Transaction tx = new Transaction())
            {
                dao.Save<CmsSavedForm>(exists);
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
