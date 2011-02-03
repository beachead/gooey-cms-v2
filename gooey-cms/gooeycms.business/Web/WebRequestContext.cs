using System;
using System.Web;
using System.Globalization;
namespace Gooeycms.Business.Web
{
    public class WebRequestContext
    {
        public static WebRequestContext Instance
        {
            get { return new WebRequestContext(); }
        }

        public bool IsModifiedSince(DateTime lastModified)
        {
            CultureInfo enUS = new CultureInfo("en-US"); 
            Boolean result = false;

            String header = Request.Headers["If-Modified-Since"];
            if (!String.IsNullOrEmpty(header))
            {
                DateTime modifiedSince;
                if (DateTime.TryParse(header, out modifiedSince))
                {
                    DateTime utc = modifiedSince.ToUniversalTime();
                    result = (utc >= lastModified);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns the current HTTP context of the call
        /// </summary>
        public HttpContext CurrentHttpContext
        {
            get
            {
                if (HttpContext.Current == null)
                    throw new ApplicationException("HttpContext.Current is null. May be outside the request or the request may have been recycled.");
                return HttpContext.Current;
            }
        }


        /// <summary>
        /// Returns the current HTTP request
        /// </summary>
        public HttpRequest Request
        {
            get { return this.CurrentHttpContext.Request; }
        }

        /// <summary>
        /// Retrieves the physical application path that the web site is running at
        /// </summary>
        public String PhysicalApplicationPath
        {
            get
            {
                return this.MapRelativePath("~/");
            }
        }

        /// <summary>
        /// Maps the specified relative path to the physical directory.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public String MapRelativePath(String path)
        {
            String root = CmsUrl.ResolveUrl(path);
            String rootPath = this.CurrentHttpContext.Server.MapPath(root);

            return rootPath;
        }

        public static CmsUrl CurrentPage()
        {
            return CmsUrl.Parse(HttpContext.Current.Request.RawUrl);
        }
    }
}
