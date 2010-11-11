using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Extensions;
using Gooeycms.Business.Storage;
using Gooeycms.Data.Model.Subscription;
using Beachead.Persistence.Hibernate;

namespace Gooeycms.Business.Util
{
    public class Logos
    {
        private const String Container = "gooeycms-logos";
        public const String Directory = StorageClientConst.RootFolder;

        public static String GetImageSrc(String logoname)
        {
            String result = "~/images/___placeholder_logo.png";
            if (logoname != null)
            {
                if (logoname.IsEmpty())
                    result = "~/images/___placeholder_logo.png";
                else
                {
                    StorageFile logo = GetLogoFile(logoname);
                    result = logo.Url + "?" + DateTime.Now.Ticks;
                }
            }

            return result;
        }

        public static void SaveLogoFile(CmsSubscription subscription, String logoname, byte [] data) 
        {
            if (logoname.IsEmpty())
                logoname = System.Guid.NewGuid().ToString();

            subscription.LogoName = logoname;
            CmsSubscriptionDao dao = new CmsSubscriptionDao();
            using (Transaction tx = new Transaction())
            {
                dao.Save<CmsSubscription>(subscription);
                tx.Commit();
            }

            IStorageClient client = StorageHelper.GetStorageClient();
            client.Save(Container, Directory, logoname, data, Permissions.Public);
        }

        public static StorageFile GetLogoFile(String logoname)
        {
            IStorageClient client = StorageHelper.GetStorageClient();
            return client.GetInfo(Container, Directory, logoname);
        }
    }
}
