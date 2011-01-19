
using System.Net.Mail;
using System;
using System.IO;
using gooeycms.business.salesforce;
using System.Collections.Generic;
using Gooeycms.Business.Import;
using Gooeycms.Business.Twilio;


namespace Gooeycms.test
{
    class Program
    {
        static void Main(string[] args)
        {
            TwilioClient client = new TwilioClient("ACe6a95690dcdee460400d44ae94e4e637", "707b003f01b09e45d998a487828c8625");
            client.SearchAvailableLocalNumbers("123");

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
    }
}
