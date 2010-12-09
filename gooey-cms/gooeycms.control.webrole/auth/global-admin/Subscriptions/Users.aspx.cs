using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Data.Model.Subscription;
using Gooeycms.Business.Subscription;
using Gooeycms.Business.Membership;
using Telerik.Web.UI;

namespace Gooeycms.Webrole.Control.auth.global_admin.Subscriptions
{
    public partial class Users : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
            }
        }

        protected void LnkAddUser_Click(Object sender, EventArgs e)
        {
            this.UserGridView.MasterTableView.InsertItem();
            this.UserGridView.DataBind();
        }

        protected void UserGridView_InsertItemCommand(object sender, GridCommandEventArgs e)
        {
            GridDataInsertItem item = (GridDataInsertItem)e.Item;

            try
            {
                UserInfo info = new UserInfo();
                info.Email = (item["Email"].Controls[0] as TextBox).Text;
                info.Password = (item["Password"].Controls[0] as TextBox).Text;
                info.Firstname = (item["Firstname"].Controls[0] as TextBox).Text;
                info.Lastname = (item["Lastname"].Controls[0] as TextBox).Text;

                MembershipDataSource ds = new MembershipDataSource();
                ds.InsertUser(Request.QueryString["g"], info);
            }
            catch (Exception ex)
            {
                UserGridView.Controls.Add(new LiteralControl("Unable to add user to subscription. Reason: " + ex.Message));
            }

            this.UserGridView.DataBind();
            e.Canceled = true;
        }

        protected void UserGridView_DeleteItemCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem item = (GridDataItem)e.Item;

            UserInfo info = new UserInfo();
            info.Email = (item["Email"] as TableCell).Text;
            info.Username = info.Email;
            info.Firstname = (item["Firstname"] as TableCell).Text;
            info.Lastname = (item["Lastname"] as TableCell).Text;

            MembershipDataSource ds = new MembershipDataSource();
            ds.RemoveUserFromSite(Request.QueryString["g"], info);

            this.UserGridView.DataBind();
            e.Canceled = true;
        }
    }
}