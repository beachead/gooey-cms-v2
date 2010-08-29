using System;
using System.Collections.Generic;
using Beachead.Persistence.Hibernate;
using Gooeycms.Business.Util;
using Gooeycms.Data.Model.Theme;
using Gooeycms.Business.Cache;

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

        public CmsTheme GetByGuid(Data.Guid themeGuid)
        {
            Data.Guid guid = SiteHelper.GetActiveSiteGuid(true);
            return GetByGuid(guid, themeGuid);
        }

        public CmsTheme GetByGuid(Data.Guid siteGuid, Data.Guid themeGuid)
        {
            CmsThemeDao dao = new CmsThemeDao();
            return dao.FindBySiteAndGuid(siteGuid, themeGuid);
        }

        /// <summary>
        /// Gets the current site's theme based upon the theme name
        /// </summary>
        /// <returns></returns>
        public CmsTheme GetByName(String name)
        {
            Data.Guid guid = SiteHelper.GetActiveSiteGuid(true);
            return GetByName(guid, name);
        }

        /// <summary>
        /// Gets the specified site's theme based upon the theme name
        /// </summary>
        /// <param name="siteGuid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public CmsTheme GetByName(Data.Guid siteGuid, String name)
        {
            CmsThemeDao dao = new CmsThemeDao();
            return dao.FindBySiteAndName(siteGuid, name);
        }

        public IList<CmsTheme> GetAllBySite(Data.Guid guid)
        {
            CmsThemeDao dao = new CmsThemeDao();
            return dao.FindAllThemes(guid);
        }

        /// <summary>
        /// Gets all of the themes for the current site.
        /// This call requires that the selected-site cookie be set.
        /// </summary>
        /// <returns></returns>
        public IList<CmsTheme> GetAllBySite()
        {
            Data.Guid guid = SiteHelper.GetActiveSiteGuid(true);
            return GetAllBySite(guid);
        }

        /// <summary>
        /// Adds a new theme to the current site.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public CmsTheme Add(string name, string description)
        {
            Data.Guid guid = SiteHelper.GetActiveSiteGuid(true);
            return Add(guid,name, description);
        }

        /// <summary>
        /// Adds a new theme to the specified site.
        /// </summary>
        /// <param name="guid">The guid of the site to associate the theme with</param>
        /// <param name="name">Theme name</param>
        /// <param name="description">Theme description</param>
        /// <returns></returns>
        public CmsTheme Add(Data.Guid guid, String name, String description)
        {
            //Make sure this theme doesn't exist
            CmsTheme theme = this.GetByName(name);
            if (theme != null)
                throw new ArgumentException("This theme name is already in use and may not be used again.");

            theme = new CmsTheme();

            theme.Name = name;
            theme.SubscriptionGuid = guid.Value;
            theme.ThemeGuid = System.Guid.NewGuid().ToString();
            theme.Description = description;

            CmsThemeDao dao = new CmsThemeDao();
            using (Transaction tx = new Transaction())
            {
                dao.Save<CmsTheme>(theme);
                tx.Commit();
            }

            return theme;
        }

        public void Save(CmsTheme theme)
        {
            CurrentSite.Cache.Clear("default-theme");
            CmsThemeDao dao = new CmsThemeDao();
            using (Transaction tx = new Transaction())
            {
                dao.Save(theme);
                tx.Commit();
            }

            SitePageCacheRefreshInvoker.InvokeRefresh(CurrentSite.Guid.Value, SitePageRefreshRequest.PageRefreshType.Staging);
        }

        internal CmsTheme GetDefaultBySite(Data.Guid siteGuid)
        {
            CacheInstance cache = CurrentSite.Cache;
            CmsTheme theme = cache.Get<CmsTheme>("default-theme");
            if (theme == null)
            {
                CmsThemeDao dao = new CmsThemeDao();
                theme = dao.FindEnabledBySite(siteGuid);
                cache.Add("default-theme", theme);
            }

            return theme;
        }
    }
}
