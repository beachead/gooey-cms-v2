using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Store;
using Gooeycms.Data.Model.Store;
using Gooeycms.Business.Paypal;

namespace Gooeycms.Webrole.Ecommerce.store
{
    public partial class Complete : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                Process();
        }

        private void Process()
        {
            String txid = Request.QueryString["tx"];
            String amt = Request.QueryString["amt"];
            String packageGuid = Request.QueryString["item_number"];
            String receiptGuid = Request.QueryString["cm"];

            //Find the receipt for this purchase
            Receipt receipt = ReceiptManager.Instance.GetByGuid(receiptGuid);
            if (receipt != null)
            {
                Double expectedAmount = receipt.Amount;
                Double amountPaid = Double.Parse(amt);

                //Make sure the txid is valid and compare the amount paid to the expected amount
                Boolean isResponseValid = PaypalManager.Instance.ValidatePDTResponse(txid,expectedAmount);

                Package package = SitePackageManager.NewInstance.GetPackage(packageGuid);
                this.LblPurchaseType.Text = package.PackageTypeString;

                //Place the package into the user's package queue
                SitePackageManager.NewInstance.AddToUser(receipt.UserGuid,package);

                //Update the receipt 
                receipt.IsComplete = true;
                receipt.Processed = DateTime.Now;
                receipt.TransactionId = txid;
                ReceiptManager.Instance.Update(receipt);
            }
        }
    }
}