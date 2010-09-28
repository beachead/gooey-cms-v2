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
using Gooeycms.Business.Subscription;
using Gooeycms.Constants;
using Gooeycms.Business.Membership;
using Gooeycms.Business.Web;
using Gooeycms.Data.Model.Site;

namespace Gooeycms.Business.Store
{
    public class SitePackageManager
    {
        public const String PackageContainer = "packaged-sites";
        public const String PackageDirectory = "binary-data";
        public const String PackageExtension = ".zip";
        //public const String DemoSitePrefix = "gooeycmsdemo";

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
            IList<CmsSitePath> sitePaths = new List<CmsSitePath>();

            PackageThemes(siteGuid, packageThemes);
            PackagePages(siteGuid, packagePages);
            PackageContentTypes(siteGuid, packageContentTypes);

            sitepackage.Themes = packageThemes;
            sitepackage.SiteMapPaths = CmsSiteMap.Instance.GetAllPaths(siteGuid);
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
            client.Save(PackageContainer,PackageDirectory, packageGuid + PackageExtension, data, Permissions.Private);


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
                PageManager.LoadPageData(page);
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

                packagePage.Images = ImageManager.Instance.GetImagesWithData(siteGuid, StorageClientConst.RootFolder);
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

                packageTheme.Header = theme.Header;
                packageTheme.Footer = theme.Footer;
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

        public IList<Package> GetPackages(string packageType, int lastMaxPos)
        {
            PackageDao dao = new PackageDao();
            IList<Package> packages = dao.FindByPackageType(packageType);

            int end = (lastMaxPos + 8);
            if (end > packages.Count)
                end = packages.Count;

            IList<Package> results = new List<Package>();
            for (int i = lastMaxPos; i < end; i++)
                results.Add(packages[i]);

            return results;
        }

        public void DeployDemoPackage(Data.Guid packageGuid)
        {
            PackageDao dao = new PackageDao();
            Package package = dao.FindByPackageGuid(packageGuid);
            if (package != null)
            {
                //Get the owner's subscription for this package
                CmsSubscription owner = SubscriptionManager.GetSubscription(package.OwnerSubscriptionId);

                //Check if our demo account exists
                MembershipUserWrapper wrapper = MembershipUtil.FindByUsername(MembershipUtil.DemoAccountUsername);
                if (!wrapper.IsValid())
                    wrapper = MembershipUtil.CreateDemoAccount();
                

                //Create a new subscription for the demo account
                CmsSubscription subscription = new CmsSubscription();
                subscription.Guid = package.Guid;
                subscription.Created = DateTime.Now;
                subscription.Subdomain = packageGuid + "-" + owner.Subdomain;
                subscription.StagingDomain = subscription.Subdomain + GooeyConfigManager.DefaultCmsDomain;
                subscription.SubscriptionPlanId = (int)SubscriptionPlans.Demo;
                subscription.PrimaryUserGuid = wrapper.UserInfo.Guid;
                subscription.IsDemo = true;
                subscription.Expires = DateTime.MaxValue;
                SubscriptionManager.Create(wrapper, subscription);

                //Deploy the package into the demo site
                IStorageClient client = StorageHelper.GetStorageClient();
                byte [] zipped = client.Open(PackageContainer, PackageDirectory, package.Guid + PackageExtension);

                Compression.ZipHandler zip = new Compression.ZipHandler(zipped);
                byte [] serialized = zip.Decompress()[0].Data;

                SitePackage sitepackage = Serializer.ToObject<SitePackage>(serialized);

                Data.Guid guid = Data.Guid.New(subscription.Guid);

                DeployThemes(sitepackage, guid);
                DeployPages(sitepackage, guid);
                DeployContentTypes(sitepackage, guid);
            }
        }

        private static void DeployContentTypes(SitePackage sitepackage, Data.Guid guid)
        {
            foreach (SiteContentType ct in sitepackage.ContentTypes)
            {
                CmsContentType type = ct.ContentType;
                type.Id = 0;
                type.Guid = null;

                ContentManager.Instance.AddContentType(guid, type);

                foreach (CmsContentTypeField field in ct.Fields)
                {
                    field.Id = 0;
                    field.Parent = null;

                    ContentManager.Instance.AddContentTypeField(type, field);
                }
            }
        }

        private static void DeployPages(SitePackage sitepackage, Data.Guid guid)
        {
            //Deploy the sitemap
            foreach (CmsSitePath path in sitepackage.SiteMapPaths)
            {
                path.Id = 0;
                path.SubscriptionGuid = guid.Value;

                CmsSiteMap.Instance.Save(path);
            }

            //Deploy the pages into the site
            foreach (SitePackagePage pageWrapper in sitepackage.Pages)
            {
                CmsPage page = pageWrapper.Page;

                page.Id = 0;
                page.SubscriptionId = guid.Value;
                page.IsApproved = false;
                page.Guid = System.Guid.NewGuid().ToString();
                PageManager.Instance.Save(page);

                //Save the javascript files for this theme
                foreach (JavascriptFile js in pageWrapper.Javascript)
                {
                    JavascriptManager.Instance.Save(guid, page, js);
                }

                //Save the css files for this theme
                foreach (CssFile css in pageWrapper.Css)
                {
                    CssManager.Instance.Save(guid, page, css);
                }

                //Save all the images for this theme
                foreach (StorageFile file in pageWrapper.Images)
                {
                    ImageManager.Instance.AddImage(guid, StorageClientConst.RootFolder, file.Filename, file.ContentType, file.Data);
                }
            }
        }

        private static void DeployThemes(SitePackage sitepackage, Data.Guid guid)
        {
            //Deploy the themes into the site
            foreach (SitePackageTheme themeWrapper in sitepackage.Themes)
            {
                CmsTheme theme = themeWrapper.Theme;

                Boolean isEnabled = theme.IsEnabled;
                theme = ThemeManager.Instance.Add(guid, theme.Name, theme.Description);

                theme.IsEnabled = isEnabled;
                theme.Header = themeWrapper.Header;
                theme.Footer = themeWrapper.Footer;
                ThemeManager.Instance.Save(theme);

                //Save all of the templates for this theme
                foreach (CmsTemplate template in themeWrapper.Templates)
                {
                    CmsTemplate newTemplate = new CmsTemplate();
                    newTemplate.IsGlobalTemplateType = template.IsGlobalTemplateType;
                    newTemplate.Name = template.Name;
                    newTemplate.SubscriptionGuid = guid.Value;
                    newTemplate.Theme = theme;
                    newTemplate.Content = template.Content;
                    newTemplate.LastSaved = template.LastSaved;

                    TemplateManager.Instance.Save(newTemplate);
                }

                //Save the javascript files for this theme
                foreach (JavascriptFile js in themeWrapper.Javascript)
                {
                    JavascriptManager.Instance.Save(guid, theme, js);
                }

                //Save the css files for this theme
                foreach (CssFile css in themeWrapper.Css)
                {
                    CssManager.Instance.Save(guid, theme, css);
                }

                //Save all the images for this theme
                foreach (StorageFile file in themeWrapper.Images)
                {
                    ImageManager.Instance.AddImage(guid, theme.ThemeGuid, file.Filename, file.ContentType, file.Data);
                }
            }
        }

        public void AddToUser(Data.Guid userGuid, Package package)
        {
            //Make sure this package isn't already assocaited
            UserPackageDao dao = new UserPackageDao();

            UserPackage userPackage = dao.FindByUserAndPackage(userGuid, package.Guid);
            if (userPackage == null)
            {
                userPackage = new UserPackage();
                userPackage.UserGuid = userGuid.Value;
                userPackage.PackageGuid = package.Guid;
                userPackage.PackageTitle = package.Title;
                userPackage.PackageType = package.PackageTypeString;

                using (Transaction tx = new Transaction())
                {
                    dao.Save<UserPackage>(userPackage);
                    tx.Commit();
                }
            }
        }

        public IList<Package> GetPurchasedPackages(UserInfo user)
        {
            IList<Package> packages = new List<Package>();

            UserPackageDao dao = new UserPackageDao();
            PackageDao packageDao = new PackageDao();

            IList<UserPackage> ups = dao.FindByUserAndPackage(user.Guid);
            foreach (UserPackage up in ups)
            {
                Package package = packageDao.FindByPackageGuid(up.PackageGuid);
                if (package != null)
                    packages.Add(package);
            }

            return packages;
        }

        public void DeletePackage(string packageGuid)
        {
            Package package = GetPackage(packageGuid);
            if (package != null)
            {
                //Delete the entries in the database
                CmsSubscription subscription = SubscriptionManager.GetSubscription(package.Guid);
                SubscriptionManager.Delete(subscription);

                PackageDao packageDao = new PackageDao();
                using (Transaction tx = new Transaction())
                {
                    packageDao.Delete<Package>(package);
                    tx.Commit();
                }

                //Delete the file from the cloud storage
                try
                {
                    IStorageClient client = StorageHelper.GetStorageClient();
                    foreach (String screenshot in package.ScreenshotList)
                    {
                        client.Delete("package-screenshots", package.PackageTypeString, screenshot); 
                    }
                    client.Delete(PackageContainer, PackageDirectory, package.Guid + PackageExtension);
                }
                catch (Exception e)
                {
                    Logging.Error("There was a problem deleting the package " + package.Guid, e);
                }
            }
        }

        public void Save(Package package)
        {
            PackageDao dao = new PackageDao();
            using (Transaction tx = new Transaction())
            {
                dao.Save<Package>(package);
                tx.Commit();
            }
        }
    }
}
