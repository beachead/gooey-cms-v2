using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Data.Model.Store;
using Gooeycms.Business.Store;
using Gooeycms.Business;

namespace Gooeycms.Webrole.Control.auth.Developer
{
    public partial class Edit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadExistingData();
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            String guid = Request.QueryString["e"];
            Package package = SitePackageManager.NewInstance.GetPackage(guid);
            if (package != null)
            {
                package.Title = this.TxtTitle.Text;
                package.Price = Double.Parse(this.TxtPrice.Text, System.Globalization.NumberStyles.Any);
                package.Features = this.TxtFeatures.Text;

                SitePackageManager.NewInstance.Save(package);
            }
            Response.Redirect("./default.aspx?msg=Successfully+updated+package+info", true);
        }

        private void LoadExistingData()
        {
            IList<String> categories = GooeyConfigManager.StorePackageCategories;
            foreach (String category in categories)
            {
                ListItem item = new ListItem(category, category);
                this.LstCategory.Items.Add(item);
            }

            String guid = Request.QueryString["e"];
            Package package = SitePackageManager.NewInstance.GetPackage(guid);
            if (package == null)
                Response.Redirect("./Default.aspx", true);

            this.TxtTitle.Text = package.Title;
            this.TxtPrice.Text = String.Format("{0:c}",package.Price);
            this.TxtFeatures.Text = package.Features;
            this.LstCategory.SelectedValue = package.Category;
        }
    }
}