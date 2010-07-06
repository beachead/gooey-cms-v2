using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Beachead.Web.CMS.controls.ResizableTextBox
{
    public partial class ResizableTextBox : System.Web.UI.UserControl
    {
        protected String ScriptPath;
        protected String CssPath;
        protected void Page_Load(object sender, EventArgs e)
        {
            CssPath = Page.ResolveUrl("~/controls/ContentEditor/ResizableTextBox/style.css");
            ScriptPath = Page.ResolveUrl("~/controls/ContentEditor/ResizableTextBox/javascript.js");
        }

        public short TabIndex
        {
            set { this.ResizableTextArea.TabIndex = value; }
            get { return this.ResizableTextArea.TabIndex; }
        }

        public String Text
        {
            get { return this.ResizableTextArea.Text; }
            set { this.ResizableTextArea.Text = value; }
        }

        public String TextboxId
        {
            get { return this.ResizableTextArea.ClientID; }
        }
    }
}