
using System.Net.Mail;
using System;
using System.IO;
using gooeycms.business.salesforce;
using System.Collections.Generic;
using Gooeycms.Business.Import;
using Gooeycms.Business.Twilio;
using Gooeycms.Business.Import.Processors;
using Gooeycms.Data.Model.Import;
using Beachead.Persistence.Hibernate;
using Gooeycms.Business.Web;
using Gooeycms.Data.Model.Page;
using Gooeycms.Data.Model.Site;
using Gooeycms.Business.Images;


namespace Gooeycms.test
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            ImportedItem item = new ImportedItem();
            item.Uri = "tesT";
            item.Expires = DateTime.Now;
            item.Inserted = DateTime.Now;

            ImportedItemDao dao = new ImportedItemDao();
            using (Transaction tx = new Transaction())
            {
                dao.Save<ImportedItem>(item);
                tx.Commit();
            }
            */

            String html = "<img src=\"../images/test.jpg\" /><img src=\"http://www.google.com/image.jpg\" /><img src='images/blah.gif'>";
            html = new ImageRewriter("location").Rewrite(html);

            /*
            GooeyCrawler crawler = new GooeyCrawler(new Uri("http://tribecadentaldesign.com"));
            crawler.AddPipelineStep(new CssImageProcessor());
            crawler.AddPipelineStep(new ConsoleOutputProcessor());
            //crawler.AddPipelineStep(new DatabasePersistenceProcessor(Data.Guid.Empty));
            crawler.Progress += new EventHandler<Business.Import.Events.CrawlProgressEventArgs>(crawler_Progress);
            crawler.Crawl();
            */

            /*
            CmsUrl url = new CmsUrl("http://www.a123systems.com/Collateral/Templates/English-US/images/topnav_about_on.gif");
            CmsUrlWalker walker = new CmsUrlWalker(url);
            while (walker.Next())
            {
                int depth = walker.Depth;
                String indivdual = walker.GetIndividualPath();
                String path = walker.GetCurrentPath();

                Console.WriteLine(walker.GetParentPath() + "," + walker.GetCurrentPath() + "," + depth);
            }
            */

            /*
            ImportedItem item = new ImportedItem();
            item.Uri = "http://www.a123systems.com/Collateral/Flash/English-US/Animation/bae_pack.html";
            CmsUrl uri = new CmsUrl(item.Uri);
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
            CmsPage page = ImportManager.Instance.GetPage("copyright.htm", "en-us", "import-template", item);
            */
             
            /*
            TwilioClient client = new TwilioClient("ACe6a95690dcdee460400d44ae94e4e637", "707b003f01b09e45d998a487828c8625");
            client.SearchAvailableLocalNumbers("123");
            */

            /*
            client.PurchasePhoneNumber("816", "http://control.gooeycms.com/twilio-handler.aspx");
            */

            /*
            IList<AvailablePhoneNumber> numbers = client.SearchAvailableLocalNumbers("816");
            foreach (AvailablePhoneNumber number in numbers)
            {
                System.Console.WriteLine(number.FriendName);
            }

            IList<AvailablePhoneNumber> numbers2 = client.SearchAvailableTollFreeNumbers();
            foreach (AvailablePhoneNumber number in numbers2)
            {
                System.Console.WriteLine(number.FriendName);
            }
            System.Console.ReadLine();
            */

            /*
            GooeyCrawler crawler = new GooeyCrawler(new Uri("http://www.emaildatasource.com/"));
            crawler.GetSiteMap();
            */

            /*
            WebSiteDownloaderOptions options = new WebSiteDownloaderOptions();
            options.DownloadUri = new Uri("http://www.firstmgt.com");
            options.MaximumLinkDepth = 3;
            options.StayOnSite = true;

            WebSiteDownloader downloader = new WebSiteDownloader(options);
            downloader.Process();
             */

            /*
            String test = "\u001FTESTING\u001FTESTING1\u001FTESTING2";
            Console.WriteLine(test);

            String[] items = test.Split('\u001F');
            foreach (String item in items)
                Console.WriteLine(item);
            Console.ReadLine();
             */
            /*
            string test =
@"
#H1 Header  
##H2 Header  
###H3 Header  
####H4 Header  

*Hello World*  
**Hello World2**

Link Style 1 [Google][1]  
Link Style 2 [Google](http://www.google.com ""Title Attribute"")  
Link Style 3 [Google][google]
Link Style 4 http://www.google.com  
Link Style 5 <http://www.google.com>?

[1]: http:/www.google.com ""Title Attribute""
[google]: http://www.google.com ""Title Attribute""

This is a test <email@address.com>
  
+ Item 1
+ Item 2
+ Item 3 With Sub Items
    - Must be tabbed or indented
        * Sub list again
    - More content
    
    This paragraph is still part of item 3
  

> This is a blockquote  
> Blockquote Line 2
>
> Newline in the blockquote

![Image Alt Text](http://w3.org/Icons/valid-xhtml10)

Testing manual line break{BR}
This is a test{BR}

[form responseTemplate=""myresponse.tpl"" emailTo=""my@test.com"" submitText=""Register""]
[table]
    --------------
    <textbox fname>
    ||
    Hello World
    ---------------
    ---------------
    This is a test
    ||
    <textarea cols=15 rows=5 comments>
    -----------------
[/table]
[/form]
";
            MarkdownSharp.Markdown m = new MarkdownSharp.Markdown();
            m.AutoHyperlink = true;
            m.LinkEmails = true;
            String result = m.Transform(test);

            Console.WriteLine(test);
            Console.WriteLine("----------------");
            Console.WriteLine(result);
            Console.ReadLine();

            File.WriteAllText("c:\\markup.txt",test + "\r\n----------------\r\n" + result);
            File.WriteAllText("c:\\markup.html", "<html><head></head><body>" + result + "</body></html>");
        }
        */
        }

        static void crawler_Progress(object sender, Business.Import.Events.CrawlProgressEventArgs e)
        {
            System.Console.WriteLine(e.EventType.ToString() + " - " + e.Uri.ToString());
        }
    }
}
