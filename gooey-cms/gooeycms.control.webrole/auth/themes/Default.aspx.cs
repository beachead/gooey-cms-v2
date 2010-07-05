using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using gooeycms.webrole.control.App_Code;

namespace gooeycms.webrole.control.auth.themes
{
    public partial class Default : ValidatedPage
    {
        protected override void OnLoad(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Because the gridview doesn't support radiogroups,
        /// we need to manually determine which one to set and uncheck
        /// any other ones to enforce the "one" only rule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnEnableClick_Change(object sender, EventArgs e)
        {
            //Clear the existing selected row 
            foreach (GridViewRow oldrow in GridView1.Rows)
            {
                ((RadioButton)oldrow.FindControl("Enabled")).Checked = false;
            }

            //Set the new selected row
            RadioButton rb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)rb.NamingContainer;
            ((RadioButton)row.FindControl("Enabled")).Checked = true;
        }

        protected void OnDelete_Click(object sender, EventArgs e)
        {
            /*
            HiddenField field = GridViewHelper.FindControl<HiddenField>(sender, "HiddenId");

            try
            {
                ThemeManager.Instance.Delete(Int32.Parse(field.Value));
                this.StatusLabel.Text = "Successfully deleted theme";
                this.GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                this.StatusLabel.Text = "There was an error deleting theme: " + ex.Message;
            }
            this.StatusLabel.Visible = true;
            */
        }
    }
}
