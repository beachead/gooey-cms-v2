using System;
using System.Web.UI.WebControls;
using Gooeycms.Business.Help;

namespace Gooeycms.Webrole.Control.App_Code
{
    public abstract class HelpPage : ValidatedPage
    {
        protected override void OnPreRender(EventArgs e)
        {
            Data.Model.Help.HelpPage current = HelpManager.GetCurrent();

            Boolean showHelp = false;
            if (current != null)
            {
                Boolean hideHelp = (Request.QueryString["hide"] != null);
                if (hideHelp)
                    HelpManager.Instance.Hide(current);

                //Check if we need to show the help page
                showHelp = HelpManager.Instance.IsHelpVisible(current);
            }

            if (showHelp)
            {
                ContentPlaceHolder holder = (ContentPlaceHolder)this.Page.Master.FindControl("Editor");
                ContentPlaceHolder instructions = (ContentPlaceHolder)this.Page.Master.FindControl("Instructions");

                if (holder == null)
                    throw new ApplicationException("Could not find the 'Editor' control to replace with the help content. This is a critical progamming error and must be corrected before this page can be displayed.");

                holder.Visible = false;
                instructions.Controls.Clear();
                
                Literal helpControl = new Literal();
                helpControl.Text = current.Text;

                Table table = new Table();
                TableRow row = new TableRow();                
                TableCell cell = new TableCell();
                cell.ColumnSpan = 2;
                cell.Controls.Add(helpControl);
                row.Cells.Add(cell);
                table.Rows.Add(row);

                instructions.Controls.Add(helpControl);
            }
            else
            {
                base.OnPreRender(e);
            }
        }
    }
}
