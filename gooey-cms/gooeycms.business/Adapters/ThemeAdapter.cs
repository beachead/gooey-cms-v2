using System;
using System.Collections.Generic;
using Gooeycms.Business.Themes;
using Gooeycms.Data.Model.Theme;

namespace Gooeycms.Business.Adapters
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

            foreach (CmsTheme theme in ThemeManager.Instance.GetAllBySite(Data.Guid.New(guid)))
                results.Add(new ThemeAdapter(theme));

            return results;
        }
    }
}
