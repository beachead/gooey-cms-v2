using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gooeycms.Webrole.Controls
{
    public partial class StatusPanel : System.Web.UI.UserControl
    {
        public enum StatusTypes
        {
            General,
            Error
        }

        private StatusTypes statusType = StatusTypes.General;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.statusType == StatusTypes.Error)
            {
                this.CalloutText.ForeColor = System.Drawing.Color.Red;
                this.StatusMessage.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                this.CalloutText.ForeColor = System.Drawing.Color.Black;
                this.StatusMessage.ForeColor = System.Drawing.Color.Black;
            }
        }

        public StatusTypes StatusType
        {
            set { this.statusType = value; }
            get { return this.statusType; }
        }

        public String Text
        {
            set { this.StatusMessage.Text = value; }
            get { return this.StatusMessage.Text; }
        }

        public bool ShowStatus
        {
            set { this.PanelSingleLine.Visible = value; }
            get { return this.PanelSingleLine.Visible; }
        }
    }
}