using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Membership;
using Gooeycms.Business.Util;
using Gooeycms.Constants;
using Gooeycms.Business;
using Telerik.Web.UI;

namespace Gooeycms.Webrole.Control.auth.global_admin
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckSetupStatus();
        }

        private void CheckSetupStatus()
        {
            DisplayButtonStatus(GooeyStatus.IsMembershipConfigured(), "red", "Configured", "Setup", this.MembershipStatusImage, null, this.BtnSetupMembership, "The membership system is already configured. Are you sure you want to reconfigure the membership system?", null);
            DisplayButtonStatus(GooeyStatus.IsFlashConfigured(), "red", "Configured", "Setup", this.FlashStatusImage, null, this.BtnSetupFlash, "Flash is already configured. Are you sure you want to reconfigure flash support?", null);
            DisplayButtonStatus(!GooeyStatus.IsDevelopmentMode(), "yellow", "Running within Windows Azure (PRODUCTION MODE)", "Running outside of Windows Azure (DEVELOPMENT MODE)", this.DevModeStatusImage, this.LblDevMode);
            DisplayButtonStatus(!GooeyStatus.IsPaypalSandbox(), "yellow", "Enable Paypal SANDBOX mode", "Enable Paypal LIVE mode",this.PaypalStatusImage, null, this.BtnTogglePaypal, "Are you sure you want to DISABLE live paypal payments?","Are you sure you want to ENABLE live paypal payments?");

            DisplayLinkStatus(GooeyStatus.IsValueConfigured(ConfigConstants.DefaultHomepage), "yellow", "Modify Default Homepage Template", "Define Default Homepage Template (currently using default template)", this.DefaultTemplateImage, this.LnkDefaultTemplate, this.TooltipDefaultTemplate, GooeyConfigManager.DefaultTemplate, ConfigConstants.DefaultTemplate, "DefaultTemplate");
            DisplayLinkStatus(GooeyStatus.IsValueConfigured(ConfigConstants.DefaultTemplate), "yellow", "Modify Default Theme Template", "Define Default Theme Template (currently using default template)", this.DefaultHomepageImage, this.LnkDefaultHomepage, this.TooltipDefaultHomepage, GooeyConfigManager.DefaultHomepage, ConfigConstants.DefaultHomepage, "DefaultHomepage");
            DisplayLinkStatus(GooeyStatus.IsValueConfigured(ConfigConstants.DefaultCmsDomain), "red", GooeyConfigManager.DefaultCmsDomain, "Define Domain", this.DefaultCmsDomainImage, this.DefaultCmsDomainLink, this.DefaultCmsDomainTooltip, GooeyConfigManager.DefaultCmsDomain, ConfigConstants.DefaultCmsDomain, "DefaultCmsDomain");

            DisplayLinkStatus(GooeyStatus.IsValueConfigured(ConfigConstants.PaypalPdt), "red", GooeyConfigManager.PaypalPdtToken, "Configure Token (currently using default sandbox token)", this.PaypalPdtTokenImage, this.PaypalPdtTokenLink, this.PaypalPdtTokenTooltip, GooeyConfigManager.PaypalPdtToken, ConfigConstants.PaypalPdt, "PaypalPdtToken");
            DisplayLinkStatus(GooeyStatus.IsValueConfigured(ConfigConstants.PaypalUsername), "red", GooeyConfigManager.PaypalUsername, "Define Paypal API Username", this.PaypalApiUsernameImage, this.PaypalApiUsernameLink, this.PaypalApiUsernameTooltip, GooeyConfigManager.PaypalUsername, ConfigConstants.PaypalUsername, "PaypalUsername");
            DisplayLinkStatus(GooeyStatus.IsValueConfigured(ConfigConstants.PaypalPassword), "red", GooeyConfigManager.PaypalPassword, "Define Paypal API Password", this.PaypalApiPasswordImage, this.PaypalApiPasswordLink, this.PaypalApiPasswordTooltip, GooeyConfigManager.PaypalPassword, ConfigConstants.PaypalPassword, "PaypalPassword");
            DisplayLinkStatus(GooeyStatus.IsValueConfigured(ConfigConstants.PaypalSignature), "red", GooeyConfigManager.PaypalSignature, "Define Paypal API Signature", this.PaypalApiSignatureImage, this.PaypalApiSignatureLink, this.PaypalApiSignatureTooltip, GooeyConfigManager.PaypalSignature, ConfigConstants.PaypalSignature, "PaypalSignature");

            DisplayLinkStatus(GooeyStatus.IsValueConfigured(ConfigConstants.DefaultAdminDomain), "yellow", GooeyConfigManager.AdminSiteHost, "Define Admin Domain (Currently using default: " + GooeyConfigManager.AdminSiteHost + ")", this.AdminSiteHostImage, this.AdminSiteHostLink, this.AdminSiteHostTooltip, GooeyConfigManager.AdminSiteHost, ConfigConstants.DefaultAdminDomain, "AdminSiteHost");
            DisplayLinkStatus(GooeyStatus.IsValueConfigured(ConfigConstants.DefaultStagingPrefix), "yellow", GooeyConfigManager.DefaultStagingPrefix, "Define Staging Prefix (Currently using default: " + GooeyConfigManager.DefaultStagingPrefix + ")", this.DefaultStaingPrefixImage, this.DefaultStaingPrefixLink, this.DefaultStaingPrefixTooltip, GooeyConfigManager.DefaultStagingPrefix, ConfigConstants.DefaultStagingPrefix, "DefaultStagingPrefix");
            DisplayLinkStatus(GooeyStatus.IsValueConfigured(ConfigConstants.DefaultPageName), "yellow", GooeyConfigManager.DefaultPageName, "Define Default Page Name (Currently using default: " + GooeyConfigManager.DefaultPageName + ")", this.DefaultPageNameImage, this.DefaultPageNameLink, this.DefaultPageNameTooltip, GooeyConfigManager.DefaultPageName, ConfigConstants.DefaultPageName, "DefaultPageName");
            DisplayLinkStatus(GooeyStatus.IsValueConfigured(ConfigConstants.DefaultAsyncTimeout), "green", GooeyConfigManager.DefaultAsyncTimeout.ToString() + " seconds", "Configure Async Timeout (Default: " + GooeyConfigManager.DefaultAsyncTimeout + " seconds) [advanced]", this.DefaultAsyncTimeoutImage, this.DefaultAsyncTimeoutLink, this.DefaultAsyncTimeoutTooltip, GooeyConfigManager.DefaultAsyncTimeout.ToString(), ConfigConstants.DefaultAsyncTimeout, "DefaultAsyncTimeout");

            DisplayLinkStatus(GooeyStatus.IsValueConfigured(ConfigConstants.DefaultTemplateName), "yellow", GooeyConfigManager.DefaultTemplateName, "Define Default Template Name (Default: " + GooeyConfigManager.DefaultTemplateName + ")", this.DefaultTemplateNameImage, this.DefaultTemplateNameLink, this.DefaultTemplateNameTooltip, GooeyConfigManager.DefaultTemplateName, ConfigConstants.DefaultTemplateName, "DefaultTemplateName");
            DisplayLinkStatus(GooeyStatus.IsValueConfigured(ConfigConstants.DefaultThemeName), "yellow", GooeyConfigManager.DefaultThemeName, "Define Default Theme Name (Default: " + GooeyConfigManager.DefaultThemeName + ")", this.DefaultThemeNameImage, this.DefaultThemeNameLink, this.DefaultThemeNameTooltip, GooeyConfigManager.DefaultThemeName, ConfigConstants.DefaultThemeName, "DefaultThemeName");
            DisplayLinkStatus(GooeyStatus.IsValueConfigured(ConfigConstants.DefaultThemeDescription), "yellow", GooeyConfigManager.DefaultThemeDescription, "Define Default Theme Description (Default: " + GooeyConfigManager.DefaultThemeDescription + ")", this.DefaultThemeDescriptionImage, this.DefaultThemeDescriptionLink, this.DefaultThemeDescriptionTooltip, GooeyConfigManager.DefaultThemeDescription, ConfigConstants.DefaultThemeDescription, "DefaultThemeDescription");

            this.LblSubscriptionProcessor.Text = GooeyConfigManager.SubscriptionProcessorClassType.FullName;
        }

        private void DisplayButtonStatus(Boolean isValidStatus, String invalidBallColor, 
                                         String validLinkText, String invalidLinkText, 
                                         Image linkImage, Label statusLabel = null,
                                         LinkButton linkButton = null,
                                         String onClientClickValidText = null, String onClientClickInvalidText = null)
        {
            if (isValidStatus)
            {
                linkImage.ImageUrl = "~/images/green-ball.gif";
                if (linkButton != null)
                {
                    linkButton.Text = validLinkText;
                    if (onClientClickValidText != null)
                        linkButton.OnClientClick = "return confirm('" + onClientClickValidText + "');";
                }
                if (statusLabel != null)
                    statusLabel.Text = validLinkText;
            }
            else
            {
                linkImage.ImageUrl = "~/images/" + invalidBallColor + "-ball.gif";
                if (linkButton != null)
                {
                    linkButton.Text = invalidLinkText;
                    if (onClientClickInvalidText != null) 
                        linkButton.OnClientClick = "return confirm('" + onClientClickInvalidText + "');";
                }

                if (statusLabel != null)
                    statusLabel.Text = invalidLinkText;
            }
        }



        private void DisplayLinkStatus(Boolean isValidStatus, String invalidBallColor, String validLinkText, String invalidLinkText, Image statusImage, HyperLink statusLink, RadToolTip statusTooltip, String statusTooltipText, String configurationName, String methodName)
        {
            if (isValidStatus)
            {
                statusImage.ImageUrl = "~/images/green-ball.gif";
                statusLink.Text = validLinkText;
            }
            else
            {
                statusImage.ImageUrl = "~/images/" + invalidBallColor + "-ball.gif";
                statusLink.Text = invalidLinkText;
            }
            if ((statusTooltip != null) && (statusTooltipText != null))
                statusTooltip.Text = Server.HtmlEncode(statusTooltipText).Replace("\n", "<br />");
            statusLink.NavigateUrl = "#";
            statusLink.Attributes["onclick"] = "window.radopen('Configuration.aspx?c=" + configurationName + "&d=" + methodName + "'); return false;";
        }

        protected void BtnSetupMembership_Click(Object sender, EventArgs e)
        {
            MembershipUtil.ConfigureRoles();
            CheckSetupStatus();
        }

        protected void BtnSetupFlash_Click(Object sender, EventArgs e)
        {
            GooeyStatus.SetupFlash();
            CheckSetupStatus();
        }

        protected void BtnTogglePaypal_Click(Object sender, EventArgs e)
        {
            GooeyStatus.TogglePaypalMode();
            CheckSetupStatus();
        }
    }
}