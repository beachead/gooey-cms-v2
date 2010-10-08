using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business;
using Gooeycms.Business.Subscription;
using Gooeycms.Data.Model.Subscription;

namespace gooeycms.webrole.betasignup.auth
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.TxtInviteEmail.Text = GooeyConfigManager.InviteEmailTemplate;
                this.TxtApproveEmail.Text = GooeyConfigManager.ApprovedEmailTemplate;

                InviteRequests.DataSource = InviteManager.Instance.GetInvites();
                InviteRequests.DataBind();
            }
        }


        protected void InviteRequests_OnItemDataBound(object sender, GridViewRowEventArgs e)
        {
            CmsInvite invite = (CmsInvite)e.Row.DataItem;
            if (invite != null)
            {
                Button approve = (Button)e.Row.FindControl("BtnApprove");
                if (invite.Issued.Year < 5000)
                {
                    approve.Text = "Re-Issue";
                    approve.OnClientClick = "return confirm('Are you sure you want to reissue this invite?');";
                }
            }
        }

        protected void InviteRequests_OnItemCommand(object source, GridViewCommandEventArgs e)
        {
            String guid = (String)e.CommandArgument;
            if (e.CommandName.Equals("InviteApprove"))
            {
                InviteManager.Instance.Approve(guid);
            }
            else if (e.CommandName.Equals("InviteDelete"))
            {
                InviteManager.Instance.Delete(guid);
            }

            InviteRequests.DataSource = InviteManager.Instance.GetInvites();
            InviteRequests.DataBind();
        }

        protected void BtnSaveInviteEmail_Click(Object sender, EventArgs e)
        {
            GooeyConfigManager.InviteEmailTemplate = this.TxtInviteEmail.Text;
        }

        protected void BtnSaveApproveEmail_Click(Object sender, EventArgs e)
        {
            GooeyConfigManager.ApprovedEmailTemplate = this.TxtApproveEmail.Text;
        }
    }
}