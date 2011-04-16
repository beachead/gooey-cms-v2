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
            else if (item.ContentType.Contains("text/css"))
                result = ImportType.Css;
            else if (item.ContentType.Contains("text/javascript"))
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

        public void AddToImportQueue(Data.Hash importHash, Data.Guid subscriptionId, IList<Data.Guid> removed)
        {
            //First thing is to remove any pages which aren't being imported
            ImportedItemDao dao = new ImportedItemDao();
            dao.RemoveImportedItems(removed);

            //Add the import hash to the queue for processing later
            ImportSiteMessage message = new ImportSiteMessage();
            message.ImportHash = importHash.Value;
            message.SubscriptionId = subscriptionId.Value;

            QueueManager queue = new QueueManager(QueueNames.ImportSiteQueue);
            queue.Put<ImportSiteMessage>(message);
        }

        public void Import(Data.Hash importHash, Data.Guid subscriptionId)
        {
            byte[] data;

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
                template.LastSaved = DateTime.Now;
                template.SubscriptionGuid = subscriptionId.Value;
                template.Theme = defaultTheme;

                TemplateManager.Instance.Save(template);
            }

            IDictionary<ImportType, IList<ImportedItem>> items = this.GetImportedItems(importHash);

            //First, import all of the images
            /*
            IList<ImportedItem> images = items[ImportType.Image];
            foreach (ImportedItem image in images)
            {
                CmsUrl uri = new CmsUrl(image.Uri);
                data = SimpleWebClient.GetResponse(uri.ToUri());

                ImageManager.Instance.AddImage(subscriptionId, StorageClientConst.RootFolder, uri.Path, image.ContentType, data);
            }
            */

            //Import the css
            IList<ImportedItem> css = items[ImportType.Css];

            CssManager cssManager = new CssManager(null);
            cssManager.SubscriptionId = subscriptionId;
            foreach (ImportedItem item in css)
            {
                CmsUrl uri = new CmsUrl(item.Uri);
                data = SimpleWebClient.GetResponse(uri.ToUri());

                cssManager.Save(defaultTheme.ThemeGuid, uri.Path, data, true, 0);
            }

            //Import the javascript
            IList<ImportedItem> js = items[ImportType.Javascript];

            JavascriptManager jsManager = new JavascriptManager(null);
            jsManager.SubscriptionId = subscriptionId;
            foreach (ImportedItem item in js)
            {
                CmsUrl uri = new CmsUrl(item.Uri);
                data = SimpleWebClient.GetResponse(uri.ToUri());

                jsManager.Save(defaultTheme.ThemeGuid, uri.Path, data, true, 0);
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
                            if (!CmsSiteMap.Instance.Exists("99a2a4a3-6748-4155-8bf2-fb781b7d8ccc", fullpath))
                                CmsSiteMap.Instance.AddChildDirectory("99a2a4a3-6748-4155-8bf2-fb781b7d8ccc", parent, current);
                        }
                    }

                    String pageName = walker.GetIndividualPath();
                    CmsPage newpage = GetPage(template.Name, subscription.Culture, pageName, page);
                    newpage.SubscriptionId = subscriptionId.Value;

                    //Add the page to the cms system
                    PageManager.Instance.AddNewPage(walker.GetParentPath(), pageName, newpage);
                }
                catch (Exception ex)
                {
                    Logging.Database.Write("import-site-manager", "Failed to import page: " + page.Uri + ", cause:" + ex.Message + ", stack:" + ex.StackTrace);
                }
            }
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
            HtmlNode body = htmldoc.DocumentNode.SelectSingleNode("//body");

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

            CmsPage page = new CmsPage();
            page.SubscriptionId = item.SubscriptionId;
            page.Author = "Site Importer";
            page.Content = body.InnerHtml.Trim();
            page.Culture = culture;
            page.DateSaved = DateTime.Now;
            page.Description = description;
            page.Guid = System.Guid.NewGuid().ToString();
            page.Url = path;
            page.UrlHash = TextHash.MD5(page.Url).Value;
            page.Template = defaultTemplate;
            page.Keywords = keywords;
            page.Title = (titleNode != null) ? titleNode.InnerText : "";

            return page;
        }
    }
}
