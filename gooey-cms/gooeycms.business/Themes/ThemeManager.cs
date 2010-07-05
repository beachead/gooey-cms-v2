using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Data.Model.Theme;

namespace Gooeycms.Business.Themes
{
    public class ThemeManager
    {
        private static ThemeManager instance = new ThemeManager();
        private ThemeManager() { }
        public static ThemeManager Instance
        {
            get { return ThemeManager.instance; }
        }

        public IList<CmsTheme> GetAllBySite(String guid)
        {
            CmsThemeDao dao = new CmsThemeDao();
            return dao.FindAllThemes(guid);
        }
    }
}
