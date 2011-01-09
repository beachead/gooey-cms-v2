using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Gooeycms.Webrole.Control
{
    public partial class PopupWindow : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void RadScriptManager_OnError(object sender, AsyncPostBackErrorEventArgs e)
        {
            this.RadScriptManager.AsyncPostBackErrorMessage = e.Exception.Message;
        }

        public RadScriptManager AjaxScriptManager
        {
            get { return this.RadScriptManager; }
        }
    }
}