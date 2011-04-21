using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Data.Model.Import;
using Gooeycms.Business.Storage;
using System.Net;
using System.IO;
using Gooeycms.Business.Images;
using System.Drawing;
using Gooeycms.Business.Web;
using Gooeycms.Business.Css;
using Gooeycms.Data.Model.Subscription;
using Gooeycms.Business.Subscription;
using Gooeycms.Business.Themes;
using Gooeycms.Data.Model.Theme;
using Gooeycms.Business.Javascript;
using Gooeycms.Business.Pages;
using Gooeycms.Data.Model.Site;
using Gooeycms.Data.Model.Page;
using HtmlAgilityPack;
using Gooeycms.Business.Crypto;
using Gooeycms.Business.Util;

namespace Gooeycms.Business.Import
{
    public enum ImportType
    {
        Page,
        Javascript,
        Css,
        Image,
        Unknown
    }

    public class ImportManager
    {
        private static ImportManager instance = new ImportManager();
        private ImportManager() { }
        public static ImportManager Instance
        {
            get { return ImportManager.instance; } 
        }

        public IDictionary<ImportType, IList<ImportedItem>> GetImportedItems(Data.Hash hash)
        {
            ImportedItemDao dao = new ImportedItemDao();
            IList<ImportedItem> items = dao.FindbyHash(hash);

            IDictionary<ImportType,IList<ImportedItem>> results = new Dictionary<ImportType,IList<ImportedItem>>();
            foreach (ImportedItem item in items)
            {
                ImportType type = GetImportItemType(item);
                if (!results.ContainsKey(type))
                    results[type] = new List<ImportedItem>();

                results[type].Add(item);
            }

            //Make sure that each item type is accounted for
            foreach (String name in Enum.GetNames(typeof(ImportType)))
            {
                ImportType type = (ImportType)Enum.Parse(typeof(ImportType), name);
                if (!results.ContainsKey(type))
                    results[type] = new List<ImportedItem>();
            }

            return results;
        }

        public static ImportType GetImportItemType(ImportedItem item)
        {
            ImportType result;
            if (item.ContentType.Contains("text/html"))
                result = ImportType.Page;
            else if (item.ContentType.Contains("css"))
                result = ImportType.Css;
            else if (item.ContentType.Contains("javascript"))
                result = ImportType.Javascript;
            else if (item.ContentType.Contains("image/"))
                result = ImportType.Image;
            else if (item.ContentType.Contains("application/"))
                result = ImportType.Unknown;
            else
            {
                if (!String.IsNullOrWhiteSpace(item.ContentType))
                    Logging.Database.Write("import-manager", "Detected unknown content type:" + item.ContentType);

                result = ImportType.Unknown;
            }

            return result;
        }

        public void AddToImportQueue(Data.Hash importHash, Data.Guid subscriptionId, String emailTo, IList<Data.Guid> removed, Boolean deleteExisting)
        {
            //First thing is to remove any pages which aren't being imported
            ImportedItemDao dao = new ImportedItemDao();
            dao.RemoveImportedItems(removed);

            //Add the import hash to the queue for processing later
            ImportSiteMessage message = new ImportSiteMessage();
            message.ImportHash = importHash.Value;
            message.SubscriptionId = subscriptionId.Value;
            message.DeleteExisting = deleteExisting;
            message.CompletionEmail = emailTo;

            QueueManager queue = new QueueManager(QueueNames.ImportSiteQueue);
            queue.Put<ImportSiteMessage>(message);
        }

        public List<String> Import(Data.Hash importHash, Data.Guid subscriptionId, Boolean deleteExisting)
        {
            List<String> status = new List<String>();

            byte[] data;

            AddStatus(importHash, status, "Site import started at " + UtcDateTime.Now.ToString());
            CmsSubscription subscription = SubscriptionManager.GetSubscription(subscriptionId);
            CmsTheme defaultTheme = ThemeManager.Instance.GetDefaultBySite(subscriptionId);
            
            //Check if the import-template already exists
            CmsTemplate template = TemplateManager.Instance.GetTemplate(subscriptionId, "import-template");
            if (template == null)
            {
                template = new CmsTemplate();
                template.Content = "{content}";
                template.Name = "import-template";
                template.IsGlobalTemplateType = false;
                template.LastSaved = UtcDateTime.Now;
                template.SubscriptionGuid = subscriptionId.Value;
                template.Theme = defaultTheme;

                TemplateManager.Instance.Save(template);
                AddStatus(importHash, status, "Successfully created and associated import template to deafult theme");
            }

            IDictionary<ImportType, IList<ImportedItem>> items = this.GetImportedItems(importHash);

            //Check if we need to delete the existing site
            if (deleteExisting)
            {
                //Check if the existing site contains snapshots, if so, we can't delete the content
                if (!ImageManager.Instance.ContainsSnapshots(subscription.Guid, StorageClientConst.RootFolder))
                {
                    IList<CmsPage> deleted = PageManager.Instance.Filter(subscription.Guid, PageManager.Filters.AllPages);
                    foreach (CmsPage page in deleted)
                        PageManager.Instance.DeleteAll(page);

                    ImageManager.Instance.DeleteAllImages(subscription.Guid, StorageClientConst.RootFolder);
                    AddStatus(importHash, status, "Successfully deleted existing pages and images");
                }
                else
                {
                    AddStatus(importHash, status, "Unable to delete existing pages and images because the current site is associated with a package");
                }
            }

            //First, import all of the images
            IList<ImportedItem> images = items[ImportType.Image];
            foreach (ImportedItem image in images)
            {
                CmsUrl uri = new CmsUrl(image.Uri);
                data = SimpleWebClient.GetResponse(uri.ToUri());

                ImageManager.Instance.AddImage(subscriptionId, StorageClientConst.RootFolder, uri.Path, image.ContentType, data);
                AddStatus(importHash, status, "Successfully imported image: " + uri.ToString() + " (" + image.ContentType + ")");
            }

            //Import the css
            IList<ImportedItem> css = items[ImportType.Css];

            int sortOrder = 0;
            CssManager cssManager = new CssManager(null);
            cssManager.SubscriptionId = subscriptionId;
            foreach (ImportedItem item in css)
            {
                CmsUrl uri = new CmsUrl(item.Uri);
                data = SimpleWebClient.GetResponse(uri.ToUri());

                cssManager.Save(defaultTheme.ThemeGuid, uri.Path, data, true, 0);
                cssManager.UpdateSortInfo(defaultTheme, uri.Path, sortOrder++);
                AddStatus(importHash, status, "Successfully imported css file: " + uri.ToString());
            }

            //Import the javascript
            IList<ImportedItem> js = items[ImportType.Javascript];

            sortOrder = 0;
            JavascriptManager jsManager = new JavascriptManager(null);
            jsManager.SubscriptionId = subscriptionId;
            foreach (ImportedItem item in js)
            {
                CmsUrl uri = new CmsUrl(item.Uri);
                data = SimpleWebClient.GetResponse(uri.ToUri());

                jsManager.Save(defaultTheme.ThemeGuid, uri.Path, data, true, 0);
                jsManager.UpdateSortInfo(defaultTheme, uri.Path, sortOrder++);
                AddStatus(importHash, status, "Successfully imported javascript file: " + uri.ToString());
            }

            //Create the sitemap and then add the page itself
            CmsSitePath root = CmsSiteMap.Instance.GetPath(subscriptionId, CmsSiteMap.RootPath);

            IList<ImportedItem> pages = items[ImportType.Page];
            foreach (ImportedItem page in pages)
            {
                try
                {
                    CmsUrl uri = new CmsUrl(page.Uri);
                    CmsUrlWalker walker = new CmsUrlWalker(uri);

                    while (walker.Next())
                    {
                        String parent = walker.GetParentPath();
                        String current = walker.GetIndividualPath();
                        String fullpath = CmsSiteMap.PathCombine(parent, current);
                        int depth = walker.Depth;

                        if (!walker.IsLast)
                        {
                            //Check if the current path exists, if not, create it
                            if (!CmsSiteMap.Instance.Exists(subscriptionId, fullpath))
                                CmsSiteMap.Instance.AddChildDirectory(subscriptionId, parent, current);
                        }
                    }

                    String pageName = walker.GetIndividualPath();
                    CmsPage newpage = GetPage(template.Name, subscription.Culture, pageName, page);
                    newpage.SubscriptionId = subscriptionId.Value;

                    //Add the page to the cms system
                    PageManager.Instance.AddNewPage(walker.GetParentPath(), pageName, newpage);
                    AddStatus(importHash, status, "Successfully imported page " + page.Uri);
                }
                catch (Exception ex)
                {
                    if (!ex.Message.Contains("404"))
                        Logging.Database.Write("import-site-manager", "Failed to import page: " + page.Uri + ", cause:" + ex.Message + ", stack:" + ex.StackTrace);
                    AddStatus(importHash, status, "Failed to import page " + page.Uri + ", Reason:" + ex.Message);
                }
            }
            AddStatus(importHash, status, "Site import completed successfully at " + UtcDateTime.Now.ToString());

            ImportedItemDao dao = new ImportedItemDao();
            dao.DeleteAllByImportHash(importHash);

            return status;
        }

        private void AddStatus(Data.Hash importHash, List<string> status, string item)
        {
            status.Add(item);
        }

        /// <summary>
        /// Downloads the page content and parses it
        /// </summary>
        /// <param name="pagename"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public CmsPage GetPage(String defaultTemplate, String culture, String pagename, ImportedItem item)
        {
            CmsUrl uri = new CmsUrl(item.Uri);
            String html = Encoding.UTF8.GetString(SimpleWebClient.GetResponse(uri.ToUri()));

            HtmlAgilityPack.HtmlDocument htmldoc = new HtmlAgilityPack.HtmlDocument();
            htmldoc.OptionFixNestedTags = true;
            htmldoc.LoadHtml(html);

            HtmlNode titleNode = htmldoc.DocumentNode.SelectSingleNode("//title");
            HtmlNode bodyNode = htmldoc.DocumentNode.SelectSingleNode("//body");

            String description = "";
            HtmlNodeCollection descriptionNode = htmldoc.DocumentNode.SelectNodes("//meta[@name='description']");
            if ((descriptionNode != null) && (descriptionNode.Count > 0))
                description = descriptionNode[0].GetAttributeValue("content", String.Empty);

            String keywords = "";
            HtmlNodeCollection keywordNode = htmldoc.DocumentNode.SelectNodes("//meta[@name='keywords']");
            if ((keywordNode != null) && (keywordNode.Count > 0))
                keywords = keywordNode[0].GetAttributeValue("content", String.Empty);

            String path = uri.Path;
            if (path.EndsWith("/"))
                path = path + GooeyConfigManager.DefaultPageName;

            String body = bodyNode.InnerHtml;
            body = "<!-- nomarkup-begin -->\r\n" + body + "\r\n<!-- nomarkup-end -->";
            body = body + "<!-- Imported by Gooeycms Import Tool. Site:" + item.Uri + " at " + UtcDateTime.Now.ToString() + " -->\r\n";

            CmsPage page = new CmsPage();
            page.SubscriptionId = item.SubscriptionId;
            page.Author = "Site Importer";
            page.Content = body;
            page.Culture = culture;
            page.DateSaved = UtcDateTime.Now;
            page.Description = description;
            page.Guid = System.Guid.NewGuid().ToString();
            page.Url = path;
            page.UrlHash = TextHash.MD5(page.Url).Value;
            page.Template = defaultTemplate;
            page.Keywords = keywords;
            page.Title = (titleNode != null) ? titleNode.InnerText : "";

            return page;
        }

        public static void SendStartEmail(string toEmail)
        {
            String from = "siteimport-noreply@gooeycms.com";
            String subject = "Site Import - Started";
            String message = "Your site import process has started. We will send you another email after the process has completed.";

            try
            {
                EmailClient client = EmailClient.GetDefaultClient();
                client.FromAddress = from;
                client.ToAddress = toEmail;
                client.Send(subject, message);
            }
            catch (Exception e)
            {
                Logging.Database.Write("site-import-email", "The start email failed to send to:" + toEmail + ", Reason:" + e.Message);
            }
        }

        internal static void SendCompletionEmail(List<string> status, string toEmail, Boolean success)
        {
            StringBuilder builder = new StringBuilder();
            if (success)
            {
                builder.Append("Your site import process has completed.");
                builder.AppendLine("Detailed Status:").AppendLine();
                foreach (String line in status)
                    builder.AppendLine("--" + line + "--").AppendLine();
            }
            else
            {
                builder.Append("There was a problem processing your site using our site import tool.").AppendLine();
                builder.AppendLine("You will need to contact customer support for more assitance.").AppendLine();
                builder.AppendLine("\r\nWe sincerely apologize for the trouble you are having.");
            }
            

            String completionStatus = (success) ? "Completed Successfully" : "Failed";

            String from = "siteimport-noreply@gooeycms.com";
            String subject = "Site Import - " + completionStatus;
            String message = builder.ToString();

            try
            {
                EmailClient client = EmailClient.GetDefaultClient();
                client.FromAddress = from;
                client.ToAddress = toEmail;
                client.Send(subject, message);
            }
            catch (Exception e)
            {
                Logging.Database.Write("site-import-email", "The complete email failed to send to:" + toEmail + ", Reason:" + e.Message);
            }
        }
    }
}
