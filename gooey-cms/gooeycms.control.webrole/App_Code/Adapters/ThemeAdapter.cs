using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gooeycms.Business.Themes;
using Gooeycms.Data.Model.Theme;

namespace Gooeycms.Webrole.Control.App_Code.Adapters
{
    public class ThemeAdapter
    {
        private CmsTheme instance;
        public ThemeAdapter(CmsTheme theme)
        {
            this.instance = theme;
        }

        public CmsTheme @Theme
        {
            get { return this.instance; }
        }

        public static IList<ThemeAdapter> GetThemesBySite(String guid)
        {
            IList<ThemeAdapter> results = new List<ThemeAdapter>();

            foreach (CmsTheme theme in ThemeManager.Instance.GetAllBySite(guid))
                results.Add(new ThemeAdapter(theme));

            return results;
        }
    }
}
