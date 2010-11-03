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

        private const Int32 CreatePackageSteps = 7;
        private const Int32 DeployPackageSteps = 8;
        public const Int32 MaxSteps = CreatePackageSteps + DeployPackageSteps;

        //public const String DemoSitePrefix = "gooeycmsdemo";

        private SitePackageManager() { }
        public static SitePackageManager NewInstance { get { return new SitePackageManager(); } }

        private String guid;
        private int currentStepCount = 1;

        public interface IPackageStatusNotifier
        {
            void OnNotify(String guid, String eventName, int stepCount, int maxSteps);
        }

        public IList<Package> GetSitePackagesForUser(UserInfo user)
        {
            PackageDao dao = new PackageDao();
            return dao.FindByUserId(user.Id);
        }

        public Data.Guid CreatePackage(Package package, IPackageStatusNotifier notifier)
        {
            PackageDao dao = new PackageDao();

            this.guid = package.Guid;
            String siteGuid = package.OwnerSubscriptionId;

            SitePackage sitepackage = new SitePackage();
            IList<SitePackageTheme> packageThemes = new List<SitePackageTheme>();
            IList<SitePackagePage> packagePages = new List<SitePackagePage>();
            IList<SiteContentType> packageContentTypes = new List<SiteContentType>();
            IList<SiteContent> packageContent = new List<SiteContent>();
            IList<CmsSitePath> sitePaths = new List<CmsSitePath>();

            DoNotify(notifier, "Packaging Themes");
            PackageThemes(siteGuid, packageThemes, notifier);

            DoNotify(notifier, "Packaging Pages");
            PackagePages(siteGuid, packagePages);

            DoNotify(notifier, "Packaging Content Types");
            PackageContentTypes(siteGuid, packageContentTypes);

            DoNotify(notifier, "Packaging Content");
            PackageContent(siteGuid, packageContent);

            IStorageClient client = StorageHelper.GetStorageClient();
            String imageDirectory = SiteHelper.GetStorageKey(SiteHelper.ImagesContainerKey, siteGuid);

            DoNotify(notifier, "Creating Page Image Snapshots (this may take a while)");
            sitepackage.PageImages = client.CreateSnapshot(imageDirectory, StorageClientConst.RootFolder);
            sitepackage.Themes = packageThemes;
            sitepackage.SiteMapPaths = CmsSiteMap.Instance.GetAllPaths(siteGuid);
            sitepackage.Pages = packagePages;
            sitepackage.ContentTypes = packageContentTypes;
            sitepackage.SiteContent = packageContent;
            sitepackage.OriginalSiteGuid = siteGuid;

            DoNotify(notifier, "Creating Package Archive");
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

            DoNotify(notifier, "Saving Package Archive");
            client = StorageHelper.GetStorageClient();
            client.Save(PackageContainer,PackageDirectory, package.Guid + PackageExtension, data, Permissions.Private);

            return package.Guid;
        }

        private void DoNotify(IPackageStatusNotifier notifier, String eventName)
        {
            if (notifier != null)
                notifier.OnNotify(this.guid, eventName, currentStepCount++,MaxSteps);
        }

        private void PackageContentTypes(Data.Guid siteGuid, IList<SiteContentType> packageContentTypes)
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

        private void PackageContent(Data.Guid siteGuid, IList<SiteContent> packageContents)
        {
            IList<CmsContent> content = ContentManager.Instance.GetAllContent(siteGuid);
            foreach (CmsContent item in content)
            {
                /* currently does not support file type content types in the package */
                if (!item.ContentType.IsFileType)
                {
                    SiteContent packageContent = new SiteContent();
                    packageContent.Content = item;

                    packageContents.Add(packageContent);
                }
            }
        }

        private void PackagePages(Data.Guid siteGuid, IList<SitePackagePage> packagePages)
        {
            IList<CmsPage> pages = PageManager.Instance.Filter(siteGuid, null);
            foreach (CmsPage item in pages)
            {
                CmsPage loadedPage = PageManager.Instance.GetLatestPage(siteGuid, item.Url, true);
                SitePackagePage packagePage = new SitePackagePage();

                IList<JavascriptFile> jsFiles = JavascriptManager.Instance.List(loadedPage);
                foreach (JavascriptFile file in jsFiles)
                {
                    JavascriptFile temp = JavascriptManager.Instance.Get(loadedPage, file.FullName);
                    file.Content = temp.Content;
                }

                IList<CssFile> cssFiles = CssManager.Instance.List(loadedPage);
                foreach (CssFile file in cssFiles)
                {
                    CssFile temp = CssManager.Instance.Get(loadedPage, file.FullName);
                    file.Content = temp.Content;
                }

                packagePage.Javascript = jsFiles;
                packagePage.Css = cssFiles;
                packagePage.Page = loadedPage;

                packagePages.Add(packagePage);
            }
        }

        private void PackageThemes(Data.Guid siteGuid, IList<SitePackageTheme> packageThemes, IPackageStatusNotifier notifier)
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

                //Create a snapshot of the images
                IStorageClient client = StorageHelper.GetStorageClient();
                String imageDirectory = SiteHelper.GetStorageKey(SiteHelper.ImagesContainerKey, siteGuid.Value);

                DoNotify(notifier, "Creating Theme Image Snapshots (This may take a while)");
                packageTheme.Header = theme.Header;
                packageTheme.Footer = theme.Footer;
                packageTheme.Templates = TemplateManager.Instance.GetTemplates(theme);
                packageTheme.Images = client.CreateSnapshot(imageDirectory, theme.ThemeGuid);
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

        public IList<Package> GetApprovedPackages(string packageType, int lastMaxPos)
        {
            PackageDao dao = new PackageDao();
            IList<Package> packages = dao.FindByPackageType(packageType, PackageDao.ApprovalStatus.Approved);

            int end = (lastMaxPos + 8);
            if (end > packages.Count)
                end = packages.Count;

            IList<Package> results = new List<Package>();
            for (int i = lastMaxPos; i < end; i++)
                results.Add(packages[i]);

            return results;
        }

        public void DeployDemoPackage(Data.Guid packageGuid, IPackageStatusNotifier notifier)
        {
            DoNotify(notifier, "Deploying Demo Package");

            PackageDao dao = new PackageDao();
            Package package = dao.FindByPackageGuid(packageGuid);
            if (package != null)
            {
                //Get the owner's subscription for this package
                CmsSubscription owner = SubscriptionManager.GetSubscription(package.OwnerSubscriptionId);

                DoNotify(notifier, "Creating Demo Account");
                //Check if our demo account exists
                MembershipUserWrapper wrapper = MembershipUtil.FindByUsername(MembershipUtil.DemoAccountUsername);
                if (!wrapper.IsValid())
                    wrapper = MembershipUtil.CreateDemoAccount();
                

                //Create a new subscription for the demo account
                CmsSubscription subscription = new CmsSubscription();
                subscription.Guid = package.Guid;
                subscription.Created = DateTime.Now;
                subscription.Subdomain = "demo-" + owner.Subdomain;
                subscription.StagingDomain = subscription.Subdomain + GooeyConfigManager.DefaultCmsDomain;
                subscription.SubscriptionPlanId = (int)SubscriptionPlans.Demo;
                subscription.PrimaryUserGuid = wrapper.UserInfo.Guid;
                subscription.IsDemo = true;
                subscription.IsCampaignEnabled = true;
                subscription.Expires = DateTime.MaxValue;
                SubscriptionManager.Create(wrapper, subscription);

                DoNotify(notifier, "Reading Package From Archive");
                //Deploy the package into the demo site
                IStorageClient client = StorageHelper.GetStorageClient();
                byte [] zipped = client.Open(PackageContainer, PackageDirectory, package.Guid + PackageExtension);

                Compression.ZipHandler zip = new Compression.ZipHandler(zipped);
                byte [] serialized = zip.Decompress()[0].Data;

                SitePackage sitepackage = Serializer.ToObject<SitePackage>(serialized);

                Data.Guid guid = Data.Guid.New(subscription.Guid);

                DoNotify(notifier, "Deploying Package Themes To Site");
                DeployThemes(sitepackage, guid, notifier);

                DoNotify(notifier, "Deploying Package Pages To Site");
                DeployPages(sitepackage, guid, notifier);

                DoNotify(notifier, "Deploying Package Content Types To Site");
                DeployContentTypes(sitepackage, guid);

                DoNotify(notifier, "Deploying Package Content To Site");
                DeployContent(sitepackage, guid);
            }
        }

        private void DeployContent(SitePackage sitepackage, Data.Guid guid)
        {
            foreach (SiteContent ct in sitepackage.SiteContent)
            {
                CmsContent newcontent = new CmsContent();
                CmsContent content = ct.Content;
                IList<CmsContentField> fields = new List<CmsContentField>(content.Fields);

                newcontent.Guid = System.Guid.NewGuid().ToString();
                newcontent.SubscriptionId = guid.Value;
                newcontent.ContentType = content.ContentType;
                newcontent.Content = content.Content;
                newcontent.Culture = content.Culture;
                newcontent.ExpireDate = content.ExpireDate;
                newcontent.IsApproved = content.IsApproved;
                newcontent.LastSaved = content.LastSaved;
                newcontent.PublishDate = content.PublishDate;
                newcontent.RegistrationPage = content.RegistrationPage;
                newcontent.RequiresRegistration = content.RequiresRegistration;
                foreach (CmsContentField field in fields)
                {
                    CmsContentField newfield = new CmsContentField();
                    newfield.Name = field.Name;
                    newfield.ObjectType = field.ObjectType;
                    newfield.Value = field.Value;

                    newcontent.AddField(newfield);
                }

                ContentManager.Instance.Save(newcontent);
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

        private void DeployPages(SitePackage sitepackage, Data.Guid guid, IPackageStatusNotifier notifier)
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
            }

            DoNotify(notifier, "Copying Page Images to Site. (This may take a few minutes) ");
            //Save all the page-level images for this site
            String copyFromImageContainer = SiteHelper.GetStorageKey(SiteHelper.ImagesContainerKey, sitepackage.OriginalSiteGuid.Value);
            String copyToImageContainer = SiteHelper.GetStorageKey(SiteHelper.ImagesContainerKey, guid.Value);

            IStorageClient client = StorageHelper.GetStorageClient();
            client.CopyFromSnapshots(sitepackage.PageImages, copyFromImageContainer, copyToImageContainer, StorageClientConst.RootFolder, Permissions.Public);
        }

        private void DeployThemes(SitePackage sitepackage, Data.Guid guid, IPackageStatusNotifier notifier)
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
                DoNotify(notifier, "Copying Theme Images From Snapshot to Site. (This may take a few minutes) ");
                String copyFromImageContainer = SiteHelper.GetStorageKey(SiteHelper.ImagesContainerKey, sitepackage.OriginalSiteGuid.Value);
                String copyToImageContainer = SiteHelper.GetStorageKey(SiteHelper.ImagesContainerKey, guid.Value);

                IStorageClient client = StorageHelper.GetStorageClient();
                client.CopyFromSnapshots(themeWrapper.Images, copyFromImageContainer, copyToImageContainer, theme.ThemeGuid, Permissions.Public);
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
                IStorageClient client = StorageHelper.GetStorageClient();
                try
                {
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

                //Delete the snapshots associated with this package
                String imageDirectory = SiteHelper.GetStorageKey(SiteHelper.ImagesContainerKey, package.OwnerSubscriptionId);
                client.DeleteSnapshots(imageDirectory, StorageClientConst.RootFolder);
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

        public IList<Package> GetUnapprovedPackages()
        {
            PackageDao dao = new PackageDao();
            return dao.FindByApprovalStatus(false);
        }

        public void ApprovePackage(Data.Guid guid)
        {
            Package package = this.GetPackage(guid);
            if (package != null)
            {
                package.IsApproved = true;
                package.Approved = DateTime.Now;

                PackageDao dao = new PackageDao();
                using (Transaction tx = new Transaction())
                {
                    dao.Save<Package>(package);
                    tx.Commit();
                }
            }
        }
    }
}
