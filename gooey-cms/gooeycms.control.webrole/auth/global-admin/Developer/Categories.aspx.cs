using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business;
using Telerik.Web.UI;

namespace Gooeycms.Webrole.Control.auth.global_admin.Developer
{
    public partial class Categories : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            this.LstCategories.Items.Clear();
            foreach (String category in GooeyConfigManager.StorePackageCategories)
            {
                RadListBoxItem item = new RadListBoxItem(category,category);
                this.LstCategories.Items.Add(item);
            }
        }

        protected void LstCategories_Deleting(object sender, RadListBoxEventArgs e)
        {
            foreach (RadListBoxItem item in this.LstCategories.SelectedItems)
            {
                GooeyConfigManager.RemoveStorePackageCategory(item.Value);
            }
        }

        protected void BtnAddItem_Click(Object sender, EventArgs e)
        {
            String category = this.TxtAddItem.Text.Trim();

            //Save the item to the categories
            GooeyConfigManager.AddStorePackageCategory(category);
            LoadData();
        }
    }
}