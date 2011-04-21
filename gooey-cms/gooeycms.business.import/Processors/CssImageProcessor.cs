using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using NCrawler.Extensions;
using NCrawler.HtmlProcessor.Properties;

namespace Gooeycms.Business.Import.Processors
{
    public class CssImageProcessor : IPipelineStep
    {
        private Regex pattern = new Regex("url\\('?([\\./\\w\\W]+?(png|gif|jpg|jpeg))'?\\)", RegexOptions.IgnoreCase);

        private String siteHash;
        public string ImportSiteHash
        {
            get
            {
                return this.siteHash;
            }
            set
            {
                this.siteHash = value;
            }
        }

        public void Process(NCrawler.Crawler crawler, NCrawler.PropertyBag propertyBag)
        {
            if (propertyBag.StatusCode != HttpStatusCode.OK)
            {
                return;
            }

            if (!("text/css".Equals(propertyBag.ContentType,StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            String fullpath = propertyBag.ResponseUri.GetLeftPart(UriPartial.Path);
            String relative = fullpath.Substring(0, fullpath.LastIndexOf("/"));

            //Look for images within the css file
			using (Stream reader = propertyBag.GetResponse())
			using (StreamReader sr = new StreamReader(reader))
			{
                String css = sr.ReadToEnd();
                MatchCollection matches = pattern.Matches(css);
                foreach (Match match in matches)
                {
                    String link = match.Groups[1].Value;
                    if (!link.Contains("http"))
                    {
                        String normalized = link.NormalizeUri(fullpath);
                        if (normalized.IsNull())
                            continue;

				        crawler.AddStep(new Uri(normalized), propertyBag.Step.Depth + 1,
					        propertyBag.Step, new Dictionary<string, object>
						        {
						        });
                    }
                }
            }
        }
    }
}
