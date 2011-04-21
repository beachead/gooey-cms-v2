using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Data.Model.Import;
using Gooeycms.Business.Import;
using Gooeycms.Data;
using Gooeycms.Business.Util;
using Gooeycms.Business.Membership;

namespace Gooeycms.Webrole.Control.auth.Import
{
    public partial class SelectImport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                String hash = Request.QueryString["g"];

                IDictionary<ImportType,IList<ImportedItem>> imports = ImportManager.Instance.GetImportedItems(Hash.New(hash));
                LoadListControl(this.ImportPages, imports[ImportType.Page]);
                LoadListControl(this.ImportCssJs, imports[ImportType.Javascript]);
                LoadListControl(this.ImportCssJs, imports[ImportType.Css]);
                LoadListControl(this.ImportImages, imports[ImportType.Image]);
                LoadListControl(this.LstUnknowns, imports[ImportType.Unknown]);
            }
        }

        protected void BtnImport_Click(Object sender, EventArgs e)
        {
            String hash = Request.QueryString["g"];

            //Find all the items which are not checked
            IList<Data.Guid> removed = new List<Data.Guid>();
            GetUncheckedItems(this.ImportPages, removed);
            GetUncheckedItems(this.ImportCssJs, removed);
            GetUncheckedItems(this.ImportImages, removed);
            GetUncheckedItems(this.LstUnknowns, removed, true);

            ImportManager.Instance.AddToImportQueue(Hash.New(hash), CurrentSite.Guid, LoggedInUser.Email, removed, this.ChkDeleteExisting.Checked);

            Response.Redirect("Status.aspx?g=" + hash);
        }

        private void GetUncheckedItems(ListControl list, IList<Data.Guid> items, bool forceRemove = false)
        {
            foreach (ListItem item in list.Items)
            {
                if ((forceRemove) || (!item.Selected))
                    items.Add(item.Value);
            }
        }

        private void LoadListControl(ListControl list, IList<ImportedItem> imports)
        {
            foreach (ImportedItem import in imports)
            {
                ListItem item = new ListItem(new Uri(import.Uri).PathAndQuery, import.Guid, true);
                item.Selected = true;
                list.Items.Add(item);
            }
        }
    }
}