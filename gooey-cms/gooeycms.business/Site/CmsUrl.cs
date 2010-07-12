using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Data.Model.Site;
using Gooeycms.Business.Util;

namespace Gooeycms.Business.Site
{
    public class CmsUrl
    {
        public String Path { get; set; }
        public String Domain { get; set; }
        public String Querystring { get; set; }

        public CmsUrl(String path)
        {
            try
            {
                Uri temp = new Uri(path);
                this.Domain = temp.Host;
                this.Path = temp.AbsolutePath;
            }
            catch (Exception)
            {
                this.Domain = CurrentSite.ProductionDomain;
                this.Path = path;
            }
        }

        public CmsUrl(String domain, String path) 
        {
            this.Path = path;
            this.Domain = domain;
        }

        public Uri ToUri()
        {
            return new Uri(this.Domain + this.Path + "?" + this.Querystring);
        }

        internal static CmsUrl Parse(string path)
        {
            CmsUrl url = new CmsUrl(path);
            return url;
        }

        public static CmsUrl Parse(CmsSitePath path)
        {
            //TODO Implement the parse CmsSitePath
            throw new NotImplementedException();
        }
    }
}
