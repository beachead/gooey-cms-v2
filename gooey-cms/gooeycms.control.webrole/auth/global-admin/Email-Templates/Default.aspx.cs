using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business;
using Gooeycms.Business.Web;

namespace Gooeycms.Webrole.Control.auth.global_admin.Email_Templates
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.TxtSmtpUsername.Text = GooeyConfigManager.SmtpUsername;
                this.TxtSmtpPassword.Text = GooeyConfigManager.SmtpPassword;
                this.TxtSmtpServer.Text = GooeyConfigManager.SmtpServer;
                this.TxtSmtpPort.Text = GooeyConfigManager.SmtpPort;
            }
        }

        protected void BtnUpdateSmtp_Click(Object sender, EventArgs e)
        {
            GooeyConfigManager.SmtpServer = this.TxtSmtpServer.Text;
            GooeyConfigManager.SmtpPort = this.TxtSmtpPort.Text;
            GooeyConfigManager.SmtpUsername = this.TxtSmtpUsername.Text;
            GooeyConfigManager.SmtpPassword = this.TxtSmtpPassword.Text;

            this.LblStatus.Text = "Settings updated";
        }

        protected void BtnTestSettings_Click(Object sender, EventArgs e)
        {
            try
            {
                EmailClient client = EmailClient.GetDefaultClient();
                client.FromAddress = "gooeycms-test@gooeycms.com";
                client.ToAddress = GooeyConfigManager.EmailAddresses.SiteAdmin;
                client.Send("Test Email", "This is a test of the settings");

                this.LblStatus.Text = "Test Passed";
            }
            catch (Exception ex)
            {
                this.LblStatus.Text = "TEST FAILED: " + ex.Message;
            }
        }

        protected void LnkModifyTemplate_Click(Object sender, CommandEventArgs e)
        {
            String template = (String)e.CommandArgument;

            this.TemplateName.Value = template;
            this.TxtTemplate.Text = GooeyConfigManager.GetEmailTemplate(template);

            this.TxtTemplate.Visible = true;
        }

        protected void BtnSave_Click(Object sender, EventArgs e)
        {
            String template = this.TemplateName.Value;
            GooeyConfigManager.SetEmailTemplate(template, this.TxtTemplate.Text);
        }
    }
}