using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Beachead.Persistence.Hibernate;
using Gooeycms.Business.Crypto;
using Gooeycms.Business.Web;
using Gooeycms.Data.Model.Help;

namespace Gooeycms.Business.Help
{
    public class HelpManager
    {
        private static HelpManager instance = new HelpManager();
        public static HelpManager Instance
        {
            get { return HelpManager.instance; }
        }

        private HelpManager() { }

        /// <summary>
        /// Gets the help page for the specified path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public HelpPage GetHelpPage(String path)
        {
            throw new NotImplementedException();
        }

        public static Data.Model.Help.HelpPage GetCurrent() 
        {
            WebRequestContext context = new WebRequestContext();
            
            CmsUrl url = WebRequestContext.CurrentPage();
            Data.Hash hash = TextHash.MD5(url.Path);

            HelpPageDao dao = new HelpPageDao();
            HelpPage result = dao.FindByPageHash(hash);

            if (result != null)
            {
                String separator = "?";
                String path = context.Request.Url.PathAndQuery;
                if (path.Contains("?"))
                    separator = "&";

                path = path + separator + "hide=1";
                result.Text = result.Text.Replace("{action}", path);
            }
            return result;
        }

        /// <summary>
        /// Hides the help page for the current user
        /// </summary>
        /// <param name="current"></param>
        public void Hide(Data.Model.Help.HelpPage current)
        {
            HashSet<String> pages = GetViewedHelpPages();
            pages.Add(current.Hash);

            StringBuilder builder = new StringBuilder();
            foreach (String value in pages)
            {
                builder.Append(value).Append(",");
            }
            builder.Remove(builder.Length - 1, 1);
            String values = builder.ToString();

            HttpCookie cookie = new HttpCookie("help-system");
            cookie.Value = values;
            cookie.Expires = DateTime.Now.AddYears(25);

            WebRequestContext context = new WebRequestContext();
            context.CurrentHttpContext.Response.Cookies.Add(cookie);
        }

        public bool IsHelpVisible(HelpPage current)
        {
            HashSet<String> result = GetViewedHelpPages();
            return (result.Contains(current.Hash)) ? false : true;
        }

        private HashSet<String> GetViewedHelpPages()
        {
            HashSet<String> result = new HashSet<string>();

            WebRequestContext context = new WebRequestContext();
            HttpCookie cookie = context.CurrentHttpContext.Request.Cookies["help-system"];
            if (cookie != null)
            {
                String[] temp = cookie.Value.Split(',');
                foreach (String item in temp)
                {
                    result.Add(item.Trim());
                }
            }

            return result;
        }

        public void Add(HelpPage page)
        {
            page.Hash = TextHash.MD5(page.Path).Value;

            HelpPageDao dao = new HelpPageDao();
            using (Transaction tx = new Transaction())
            {
                dao.Save<HelpPage>(page);
                tx.Commit();
            }
        }

        public IList<HelpPage> GetAll()
        {
            HelpPageDao dao = new HelpPageDao();
            return dao.FindAll<HelpPage>();
        }

        public HelpPage Get(int id)
        {
            HelpPageDao dao = new HelpPageDao();
            return dao.FindByPrimaryKey<HelpPage>(id);
        }
    }
}
