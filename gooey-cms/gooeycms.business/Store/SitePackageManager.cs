using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Data.Model.Store;
using Ionic.Zip;
using Gooeycms.Business.Themes;
using Gooeycms.Data.Model.Theme;
using Gooeycms.Business.Javascript;
using Gooeycms.Business.Css;
using Gooeycms.Business.Util;
using System.IO;
using Gooeycms.Business.Storage;
using Gooeycms.Business.Images;
using Gooeycms.Data.Model.Page;
using Gooeycms.Business.Pages;
using Gooeycms.Data.Model.Content;
using Gooeycms.Business.Content;
using Beachead.Persistence.Hibernate;
using Gooeycms.Data.Model.Subscription;

namespace Gooeycms.Business.Store
{
    public class SitePackageManager
    {
        private static SitePackageManager instance = new SitePackageManager();
        private SitePackageManager() { }
        public static SitePackageManager Instance { get { return SitePackageManager.instance; } }

        public IList<Package> GetSitePackagesForUser(UserInfo user)
        {
            PackageDao dao = new PackageDao();
            return dao.FindByUserId(user.Id);
        }

        public Data.Guid CreatePackage(Data.Guid siteGuid, String title, String features, String category, Double salePrice)
        {
            PackageDao dao = new PackageDao();
            Package package = dao.FindBySiteGuid(siteGuid);

            //Delete the existing package, before creating a new one
            if (package != null)
            {
                //TODO Delete the existing package before creating a new one
            }

            String packageGuid = System.Guid.NewGuid().ToString();

            SitePackage sitepackage = new SitePackage();
            IList<SitePackageTheme> packageThemes = new List<SitePackageTheme>();
            IList<SitePackagePage> packagePages = new List<SitePackagePage>();
            IList<SiteContentType> packageContentTypes = new List<SiteContentType>();

            PackageThemes(siteGuid, packageThemes);
            PackagePages(siteGuid, packagePages);
            PackageContentTypes(siteGuid, packageContentTypes);

            sitepackage.Themes = packageThemes;
            sitepackage.Pages = packagePages;
            sitepackage.ContentTypes = packageContentTypes;

            byte[] data = null;
            using (MemoryStream outputstream = new MemoryStream())
            {
                //Serialize the object and then compress the serialized object
                data = Serializer.ToByteArray<SitePackage>(sitepackage);
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddEntry("sitepackage.bin", data);
                    zip.Save(outputstream);
                }
                data = null;

                //Write the serialized data to the container
                data = outputstream.ToArray();
            }

            IStorageClient client = StorageHelper.GetStorageClient();
            client.Save("packaged-sites", "binary-data", packageGuid + ".bin", data, Permissions.Private);


            //Save this package into the database
            package = new Package();
            package.OwnerSubscriptionId = siteGuid.Value;
            package.PackageTypeString = PackageTypes.Site.ToString();
            package.Price = salePrice;
            package.Guid = packageGuid;
            package.Features = features;
            package.Title = title;
            package.Category = category;
            package.PageCount = packagePages.Count;
            package.IsApproved = false;
            package.Created = DateTime.Now;
            package.Approved = DateTime.MaxValue;
            using (Transaction tx = new Transaction())
            {
                dao.Save<Package>(package);
                tx.Commit();
            }

            return packageGuid;
        }

        private static void PackageContentTypes(Data.Guid siteGuid, IList<SiteContentType> packageContentTypes)
        {
            IList<CmsContentType> contentTypes = ContentManager.Instance.GetContentTypes(siteGuid);
            foreach (CmsContentType contentType in contentTypes)
            {
                SiteContentType packageContentType = new SiteContentType();

                IList<CmsContentTypeField> fields = ContentManager.Instance.GetContentTypeFields(contentType.Guid);

                packageContentType.ContentType = contentType;
                packageContentType.Fields = fields;
                packageContentTypes.Add(packageContentType);
            }
        }

        private static void PackagePages(Data.Guid siteGuid, IList<SitePackagePage> packagePages)
        {
            IList<CmsPage> pages = PageManager.Instance.Filter(siteGuid, null);
            foreach (CmsPage page in pages)
            {
                SitePackagePage packagePage = new SitePackagePage();

                IList<JavascriptFile> jsFiles = JavascriptManager.Instance.List(page);
                foreach (JavascriptFile file in jsFiles)
                {
                    JavascriptFile temp = JavascriptManager.Instance.Get(page, file.FullName);
                    file.Content = temp.Content;
                }

                IList<CssFile> cssFiles = CssManager.Instance.List(page);
                foreach (CssFile file in cssFiles)
                {
                    CssFile temp = CssManager.Instance.Get(page, file.FullName);
                    file.Content = temp.Content;
                }

                packagePage.Images = ImageManager.Instance.GetImagesWithData(siteGuid, page.Guid);
                packagePage.Javascript = jsFiles;
                packagePage.Css = cssFiles;
                packagePage.Page = page;

                packagePages.Add(packagePage);
            }
        }

        private static void PackageThemes(Data.Guid siteGuid, IList<SitePackageTheme> packageThemes)
        {
            IList<CmsTheme> themes = ThemeManager.Instance.GetAllBySite(siteGuid);
            foreach (CmsTheme theme in themes)
            {

                SitePackageTheme packageTheme = new SitePackageTheme();

                IList<JavascriptFile> jsFiles = JavascriptManager.Instance.List(theme);
                foreach (JavascriptFile file in jsFiles)
                {
                    JavascriptFile temp = JavascriptManager.Instance.Get(theme, file.FullName);
                    file.Content = temp.Content;
                }

                IList<CssFile> cssFiles = CssManager.Instance.List(theme);
                foreach (CssFile file in cssFiles)
                {
                    CssFile temp = CssManager.Instance.Get(theme, file.FullName);
                    file.Content = temp.Content;
                }

                packageTheme.Templates = TemplateManager.Instance.GetTemplates(theme);
                packageTheme.Images = ImageManager.Instance.GetImagesWithData(siteGuid, theme.ThemeGuid);
                packageTheme.Javascript = jsFiles;
                packageTheme.Css = cssFiles;
                packageTheme.Theme = theme;

                packageThemes.Add(packageTheme);
            }
        }

        public void AddScreenshot(Data.Guid packageGuid, String filename, byte[] imagedata)
        {
            Package package = GetPackage(packageGuid);
            if (package != null)
            {
                String imageFilename = packageGuid + "-" + filename;

                package.Screenshots = package.Screenshots + imageFilename + Package.ScreenshotSeparator;

                IStorageClient client = StorageHelper.GetStorageClient();
                client.Save("package-screenshots", package.PackageTypeString, imageFilename, imagedata, Permissions.Public);

                PackageDao dao = new PackageDao();
                using (Transaction tx = new Transaction())
                {
                    dao.Save<Package>(package);
                    tx.Commit();
                }
            }
        }

        public Package GetPackage(Data.Guid packageGuid)
        {
            PackageDao dao = new PackageDao();
            return dao.FindByPackageGuid(packageGuid);
        }

        public IList<String> GetScreenshotUrls(Package package)
        {
            IStorageClient client = StorageHelper.GetStorageClient();
            IList<String> results = new List<String>();

            foreach (String screenshot in package.ScreenshotList)
            {
                StorageFile file = client.GetFile("package-screenshots", package.PackageTypeString, screenshot);
                results.Add(file.Url);
            }

            return results;
        }
    }
}
