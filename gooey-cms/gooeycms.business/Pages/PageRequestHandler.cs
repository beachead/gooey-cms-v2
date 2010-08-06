using System;
using System.Text;
using System.Web.UI;
using System.Threading;
using Beachead.Core.Markup;
using Beachead.Core.Markup.Engine;
using Gooeycms.Business.Css;
using Gooeycms.Business.Javascript;
using Gooeycms.Business.Membership;
using Gooeycms.Business.Themes;
using Gooeycms.Business.Util;
using Gooeycms.Business.Web;
using Gooeycms.Data.Model.Page;
using Gooeycms.Data.Model.Theme;
using Gooeycms.Business.Crypto;
using System.Threading.Tasks;
using System.ServiceModel.Activation;
using Gooeycms.Business.Storage;

namespace Gooeycms.Business.Pages
{
    public class PageRequestHandler : Page
    {
        private static String HtmlShell = String.Empty;

        private CmsPage page = null;
        private CmsTheme theme = null;
        private StringBuilder output = new StringBuilder();
        private String analytics = "";

        /// <summary>
        /// Load any static data which should be loaded only once
        /// </summary>
        static PageRequestHandler()
        {
            HtmlShell = EmbeddedResource.Read("Gooeycms.Business.Pages.HtmlTemplate.htm");
        }

        /// <summary>
        /// Called before any other events during the page lifecycle.
        /// 
        /// Retrieve the page template during this stage based upon the path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            String preview = Request.QueryString["pvw"];
            String cacheKey = Request.QueryString["pvw_id"];
            String token = Request.QueryString["token"];

            CmsUrl url = CmsUrl.Parse(Request.RawUrl);
            String culture = CurrentSite.Culture;

            if (String.IsNullOrEmpty(preview))
            {
                ValidateSite();
                this.page = PageManager.Instance.GetLatestPage(url);
            }
            else
            {
                //Make sure there's an authenticated user making this request
                if (!TokenManager.IsValid(cacheKey, token))
                    throw new ApplicationException("The specified security token is not valid for this preview request.");

                this.page = (CmsPage)Cache.Get(cacheKey);
                if (this.page == null)
                {
                    this.page = PageManager.Instance.GetLatestPage(url, false); ;
                    Cache.Insert(cacheKey, this.page, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(15));
                }

                QueueManager queue = new QueueManager(QueueManager.GetPreviewQueueName(CurrentSite.Guid));
                PreviewDto dto = queue.GetFirst<PreviewDto>();

                this.page.Content = dto.Content;
                this.page.Title = dto.Title;
                this.page.Template = dto.TemplateName;
            }

            if (this.page != null)
                this.theme = ThemeManager.Instance.GetDefaultBySite(Data.Guid.New(this.page.SubscriptionId));
            else
                throw new PageNotFoundException(url.Path);
        }

        private void ValidateSite()
        {
            if (CurrentSite.GetCurrentTheme() == null)
                throw new ApplicationException("This site could not be displayed. Reason: The site has not been properly configured with a default theme.");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            output = new StringBuilder(HtmlShell);

            //Set all of the header and meta tag information
            output = output.Replace("{head.title}", this.page.Title);
            output = output.Replace("{head.meta.description}", this.page.Description);
            output = output.Replace("{head.meta.keywords}", this.page.Keywords);
            output = output.Replace("{head.meta.custom}", "");

            //Include all of the javascript files
            JavascriptManager js = new JavascriptManager(this.page);
            CssManager css = new CssManager(this.page);
            output = output.Replace("{head.scripts.include}", js.GetJavascriptIncludes(this.page));
            output = output.Replace("{head.css.include}", css.GetCssIncludes());

            //Include any custom body options
            if (!String.IsNullOrEmpty(this.page.OnBodyLoad))
            {
                output = output.Replace("{body.options}", " onload=\"" + this.page.OnBodyLoad + "\"");
            }
            else
            {
                output = output.Replace("{body.options}", "");
            }

            //Insert the page template
            CmsTemplate cmsTemplate = TemplateManager.Instance.GetTemplate(this.page.Template);

            DynamicContentFormatter engine = new DynamicContentFormatter();
            StringBuilder template = engine.Convert(new StringBuilder(cmsTemplate.Content));

            //Add any campaign elements into the template
            /* TODO Code the campaign functionality
            CampaignManager campaignManager = new CampaignManager();
            IList<Element> top = campaignManager.GetCampaignElementsForPage(CampaignManager.ElementPosition.Top, this.page);
            IList<Element> middle = campaignManager.GetCampaignElementsForPage(CampaignManager.ElementPosition.Middle, this.page);
            IList<Element> bottom = campaignManager.GetCampaignElementsForPage(CampaignManager.ElementPosition.Bottom, this.page);
            */

            StringBuilder topElements = new StringBuilder();//CampaignMarkupFormatter.Format(top);
            StringBuilder middleElements = new StringBuilder();//CampaignMarkupFormatter.Format(middle);
            StringBuilder bottomElements = new StringBuilder();//CampaignMarkupFormatter.Format(bottom);

            template = template.Replace("{campaign-top}", topElements.ToString());
            template = template.Replace("{campaign-middle}", middleElements.ToString());
            template = template.Replace("{campaign-bottom}", bottomElements.ToString());

            output = output.Replace("{page.html}", template.ToString());
            output = output.Replace("{analytics.tracking}", this.analytics);

            WebRequestContext context = new WebRequestContext();
            String downloadId = Request.QueryString["d"];
            String downloadlink = "";
            if (downloadId != null)
            {
                downloadlink = "downloadFile('" + context.CurrentHttpContext.Server.UrlEncode(downloadId) + "')";
            }
            output = output.Replace("{download-link}", downloadlink);

        }

        protected override void Render(HtmlTextWriter writer)
        {
            IMarkupEngine engine = MarkupEngineFactory.Instance.GetDefaultEngine();

            String header = engine.Convert(this.theme.Header);
            String footer = engine.Convert(this.theme.Footer);
            String content = engine.Convert(this.page.Content);

            this.output = output.Replace("{header}", header);
            this.output = output.Replace("{footer}", footer);
            this.output = output.Replace("{content}", content);
            this.output = engine.GetHeaderFormatter().Convert(output);
            this.output = engine.GetHeaderTextFormatter().Convert(output);

            NavigationControl navigation = new NavigationControl();
            output = output.Replace("{navigation}", navigation.ToHtml());

            ResourceControl resources = new ResourceControl();
            output = resources.Replace(output);

            VariableControl variables = new VariableControl();
            output = variables.Replace(output);

            Control resolver = new Control();
            String path = resolver.ResolveUrl("~/");

            output = output.Replace("~/", path);

            //HACK to fix IIS 6.0 root directory issue where it doesn't resolve correctly
            output = output.Replace("~/", "/");

            writer.Write(output.ToString());
            base.Render(writer);
        }
    }
}
