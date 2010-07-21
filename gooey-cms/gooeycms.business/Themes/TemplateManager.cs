using System;
using System.Collections.Generic;
using System.Linq;
using Beachead.Persistence.Hibernate;
using Gooeycms.Business.Crypto;
using Gooeycms.Business.Util;
using Gooeycms.Data.Model.Theme;

namespace Gooeycms.Business.Themes
{
    public class TemplateManager
    {
        private static TemplateManager instance = new TemplateManager();
        private TemplateManager() { }

        public static TemplateManager Instance
        {
            get { return TemplateManager.instance; }
        }

        /// <summary>
        /// Gets the templates which are currently associated with the specified theme.
        /// </summary>
        /// <param name="theme"></param>
        /// <returns></returns>
        public IList<CmsTemplate> GetTemplates(CmsTheme theme)
        {
            CmsTemplateDao dao = new CmsTemplateDao();
            return dao.FindByThemeId(theme.Id);
        }

        /// <summary>
        /// Gets the templates which still need to be added to this theme
        /// </summary>
        /// <param name="theme"></param>
        /// <returns></returns>
        public IList<String> GetAvailableGlobalTemplateTypeNames(CmsTheme theme)
        {
            IList<CmsTemplate> associated = GetTemplates(theme);

            CmsTemplateDao dao = new CmsTemplateDao();
            IList<CmsGlobalTemplateType> globals = dao.FindGlobalTemplateTypes();

            IList<String> temp1 = new List<String>();
            IList<String> temp2 = new List<String>();

            foreach (CmsTemplate item in associated)
                temp1.Add(item.Name);

            foreach (CmsGlobalTemplateType global in globals)
                temp2.Add(global.Name);

            IEnumerable<String> available = temp2.Except<String>(temp1);

            return new List<String>(available);
        }

        public void Save(CmsTemplate template)
        {
            using (Transaction tx = new Transaction())
            {
                CmsTemplateDao dao = new CmsTemplateDao();
                dao.Save<CmsTemplate>(template);

                tx.Commit();
            }
        }

        public CmsTemplate GetTemplate(String templateName)
        {
            return GetTemplate(CurrentSite.Guid, templateName);
        }

        public CmsTemplate GetTemplate(Data.Guid siteGuid, String templateName)
        {
            CmsTemplateDao dao = new CmsTemplateDao();
            return dao.FindBySiteAndName(siteGuid, templateName);
        }

        public CmsTemplate GetTemplate(int primaryKey)
        {
            CmsTemplateDao dao = new CmsTemplateDao();
            return dao.FindByPrimaryKey<CmsTemplate>(primaryKey);
        }

        public CmsTemplate GetTemplate(Data.EncryptedValue encryptedId)
        {
            int id = 0;
            Int32.TryParse(TextEncryption.Decode(encryptedId.Value), out id);
            return GetTemplate(id);
        }
    }
}
