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
using Gooeycms.Business.Cache;
using Gooeycms.Business.Markup.Dynamic;
using Gooeycms.Business.Campaigns;
using Gooeycms.Data.Model.Site;
using System.Web;
using System.Collections.Generic;
using Gooeycms.Data.Model.Campaign;
using Gooeycms.Business.Markup.Markdown;
using Gooeycms.Business.Images;
using Gooeycms.Business.Subscription;
using Gooeycms.Data.Model.Subscription;

namespace Gooeycms.Business.Pages
{
    public class PageRequestHandler : Page
    {
        private static String HtmlShell = String.Empty;

        private CmsPage page = null;
        private CmsTheme theme = null;
        private StringBuilder output = new StringBuilder();
        private Boolean isInCache = false;

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
            if (!CurrentSite.IsAvailable)
                Response.Redirect(GooeyConfigManager.SignupSiteHost, true);

            String preview = Request.QueryString["pvw"];
            String cacheKey = Request.QueryString["pvw_id"];
            String token = Request.QueryString["token"];

            CmsUrl url = CmsUrl.Parse(Request.RawUrl);
            String culture = CurrentSite.Culture;

            if (Request.QueryString["nocache"] == null)
            {
                this.isInCache = SitePageCache.Instance.GetIfExists(url, ref output);
                
                //if it's the staging server, check if the site is dirty
                //if so, we need to wait till the page is saved prior to displaying it
                if (CurrentSite.IsStagingHost)
                {
                    int count = 0;
                    while ((CurrentSite.IsDirty) && (count++ < 7))
                    {
                        this.isInCache = false;
                        System.Threading.Thread.Sleep(1000);
                    }

                    if (count >= 6)
                    {
                        CmsSubscription temp = CurrentSite.Subscription;
                        temp.IsDirty = false;
                        SubscriptionManager.Save(temp);

                        CurrentSite.Cache.Clear();
                        Logging.Database.Write("page-save-error", "Subscription: " + temp.Guid + " failed to detect the page save. Is dirty flag automatically cleared.");
                    }
                }
            }

            if (String.IsNullOrEmpty(preview))
            {
                //Make sure the subscription is still active
                if (CurrentSite.Subscription.IsDisabled)
                    Response.Redirect(GooeyConfigManager.SiteDisabledRedirect + "?" + Server.UrlEncode("The requested site is not currently active"), true);

                if (!this.isInCache)
                {
                    //Check if this page is a redirect
                    String path = "~" + url.Path;
                    CmsSitePath sitepath = CmsSiteMap.Instance.GetPath(path);
                    if (sitepath != null)
                    {
                        if (sitepath.IsRedirect)
                        {
                            Control resolver = new Control();
                            String redirectTo = resolver.ResolveClientUrl(sitepath.RedirectTo);
                            Response.Redirect(redirectTo, true);
                        }
                    }
                }

                CampaignManager.Instance.TrackCampaigns();

                //If the page is in the cache, return immediately
                if (isInCache)
                    return;

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


        private void CheckIfProcessRedirect(CmsUrl url)
        {
            if (url.Path.Contains(".swf"))
            {
                /*
                String imageContainerUrl = CurrentSite.GetContainerUrl(SiteHelper.ImagesContainerKey);
                String redirect = imageContainerUrl + url.Path;
                Response.Redirect(redirect, true);
                */
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if the page is in the cache return immediately
            if (isInCache)
                return;

            output = new StringBuilder(HtmlShell);

            //Set all of the header and meta tag information
            output = output.Replace("{head.title}", this.page.Title);
            output = output.Replace("{head.meta.description}", this.page.Description);
            output = output.Replace("{head.meta.keywords}", this.page.Keywords);
            output = output.Replace("{head.meta.custom}", "");

            //Include all of the javascript files
            JavascriptManager js = new JavascriptManager(this.page);
            CssManager css = new CssManager(this.page);
            output = output.Replace("{head.scripts.include}", js.GetIncludes(this.page));
            output = output.Replace("{head.css.include}", css.GetIncludes(this.page));

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
            StringBuilder template = new StringBuilder(cmsTemplate.Content);

            //Get all of the campaign elements and put them into the page/template
            IDictionary<String, String> elements = CampaignManager.Instance.Elements.GetElementsForPage(this.page);
            template = template.Replace("{campaign-top}", elements["top"]);
            template = template.Replace("{campaign-middle}", elements["middle"]);
            template = template.Replace("{campaign-bottom}", elements["bottom"]);

            template = ImageRewriter.ThemesImageRewriter.Rewrite(template);

            //Replace the page campaign placeholders
            this.page.Content = this.page.Content.Replace("{campaign-top}", elements["top"]);
            this.page.Content = this.page.Content.Replace("{campaign-middle}", elements["middle"]);
            this.page.Content = this.page.Content.Replace("{campaign-bottom}", elements["bottom"]);

            DynamicContentFormatter engine = new DynamicContentFormatter();
            template = engine.Convert(template);

            output = output.Replace("{page.html}", template.ToString());
            output = output.Replace("{analytics.tracking}", CampaignManager.Instance.GetCampaignEngine().GetTrackingScript());

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
            if (!isInCache)
            {
                IMarkupEngine engine = MarkupEngineFactory.Instance.GetDefaultEngine();

                String header = engine.Convert(this.theme.Header, true);
                String footer = engine.Convert(this.theme.Footer, true);
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

                CmsUrl url = CmsUrl.Parse(Request.RawUrl);
                SitePageCache.Instance.AddToCache(url, output);
            }

            //Replace the phone number after any caching has already been done (dynamic per-user)
            String local = output.ToString();

            String phone = CampaignManager.Instance.GetActivePhoneNumber();
            local = local.Replace("{phone}", phone);

            writer.Write(local);
            base.Render(writer);
        }

        protected void Page_Error(Object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            Server.ClearError();

            if (ex is PageNotFoundException)
            {
                PageNotFoundException notfound = (PageNotFoundException)ex;
                if (notfound.NotFoundPath.Contains("404.aspx"))
                {
                    Response.Redirect("/gooeycms/errors/404.aspx?path=" + Server.UrlEncode(notfound.NotFoundPath), true);
                }
                else
                {
                    Control resolver = new Control();
                    String path = resolver.ResolveUrl("~/");

                    Response.Redirect(path + "404.aspx?path=" + Server.UrlEncode(notfound.NotFoundPath), true);
                }
            }
            else
            {
                Logging.Database.Write("Unhandled Exception", Logging.FormatException(ex));
                Response.Redirect("/gooeycms/errors/500.aspx", true);
            }
        }

        private void ValidateSite()
        {
            if (CurrentSite.GetCurrentTheme() == null)
                throw new ApplicationException("This site could not be displayed. Reason: The site has not been properly configured with a default theme.");
        }
    }
}
