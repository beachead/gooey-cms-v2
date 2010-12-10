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

            UserInfo info = new UserInfo();
            info.Email = (item["Email"].Controls[0] as TextBox).Text;
            info.Password = (item["Password"].Controls[0] as TextBox).Text;
            info.Firstname = (item["Firstname"].Controls[0] as TextBox).Text;
            info.Lastname = (item["Lastname"].Controls[0] as TextBox).Text;

            MembershipDataSource ds = new MembershipDataSource();
            MembershipDataSource.MembershipInsertResult result = ds.InsertUser(Request.QueryString["g"], info);

            if (result.IsSuccess)
            {
                if (result.IsExistingUser)
                    this.LblStatus.Text = "Successfully associated existing user " + info.Email + ", using the user's existing password, to the subscription.";
                else
                    this.LblStatus.Text = "Successfully created new user " + info.Email + " and associated user to the subscription";
            }
            else
            {
                String message = (result.HasException) ? result.Exception.Message : "";
                this.LblStatus.Text = "There was a problem associating this user to the subscription. Reason: " + message;
            }

            this.UserGridView.DataBind();
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

            this.LblStatus.Text = "Successfully removed user " + info.Email + " from subscription";
        }
    }
}