using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Campaigns;
using Gooeycms.Data.Model.Campaign;
using Gooeycms.Business.Util;
using Gooeycms.Business.Twilio;
using Gooeycms.Business;
using Gooeycms.Data.Model.Subscription;
using Gooeycms.Business.Subscription;

namespace Gooeycms.Webrole.Control.auth.Campaigns
{
    public partial class ActivatePhone : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadPage();
            }
        }

        private void LoadPage()
        {
            if (CurrentSite.Configuration.PhoneSettings.NumberType != CurrentSite.Configuration.PhoneSettings.PhoneNumberType.NotSet)
            {
                String guid = Request.QueryString["id"];
                CmsCampaign campaign = CampaignManager.Instance.GetCampaign(guid);
                if (campaign == null)
                {
                    this.LblStatus.Text = "The campaign guid is not valid and may not be used";
                }
                else
                {
                    if (String.IsNullOrWhiteSpace(campaign.PhoneNumber))
                    {
                        if (CurrentSite.Configuration.PhoneSettings.RemainingPhoneNumbers > 0)
                        {
                            //Find the available phone numbers
                            IList<AvailablePhoneNumber> numbers;

                            TwilioClient client = CurrentSite.Configuration.PhoneSettings.GetLocalTwilioClient();
                            if (CurrentSite.Configuration.PhoneSettings.NumberType == CurrentSite.Configuration.PhoneSettings.PhoneNumberType.Local)
                                numbers = client.SearchAvailableLocalNumbers(CurrentSite.Configuration.PhoneSettings.DefaultAreaCode);
                            else
                                numbers = client.SearchAvailableTollFreeNumbers();

                            foreach (AvailablePhoneNumber number in numbers)
                            {
                                ListItem item = new ListItem(number.FriendlyName, number.PhoneNumber);
                                this.LstAvailablePhoneNumbers.Items.Add(item);
                            }
                            PhoneNumberViews.SetActiveView(ConfigureView);
                        }
                        else
                        {
                            PhoneNumberViews.SetActiveView(MaxAllowedView);
                        }
                    }
                    else
                    {
                        this.LblActivePhone.Text = campaign.PhoneNumber;
                        PhoneNumberViews.SetActiveView(ExistingView);
                    }
                }
            }
            else
            {
                LblStatus.Text = "You must first configure your phone settings prior to activating numbers.";
                LblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void BtnDeactivate_Click(Object sender, EventArgs e)
        {
            String guid = Request.QueryString["id"];
            CmsCampaign campaign = CampaignManager.Instance.GetCampaign(guid);
            CmsSubscriptionPhoneNumber phone = SubscriptionManager.GetPhoneNumber(campaign.PhoneNumber);

            if (phone != null)
            {
                CurrentSite.Configuration.PhoneSettings.GetLocalTwilioClient().ReleasePhoneNumber(phone.Sid, AvailablePhoneNumber.Parse(campaign.PhoneNumber));

                //Delete it from the subscription itself
                SubscriptionManager.RemovePhoneFromSubscription(CurrentSite.Guid, campaign.PhoneNumber);

                //Remove the number from the campaign
                campaign.PhoneNumber = "";
                CampaignManager.Instance.Add(campaign);

                this.LblStatus.Text = "Phone number was successfully released and removed from your subscription.";
                this.LoadPage();
            }
        }

        protected void BtnSave_Click(Object sender, EventArgs e)
        {
            try
            {
                String number = this.LstAvailablePhoneNumbers.SelectedValue;
                String forward = this.TxtForwardingNumber.Text;

                //Activate the phone number
                TwilioClient client = CurrentSite.Configuration.PhoneSettings.GetLocalTwilioClient();
                AssignedPhoneNumber assigned = client.PurchasePhoneNumber(AvailablePhoneNumber.Parse(number), GooeyConfigManager.TwilioPhoneHandlerUrl);

                //Save the phone number with the subscription
                CmsSubscriptionPhoneNumber phone = assigned.AsCmsSubscriptionPhoneNumber();
                phone.SubscriptionId = CurrentSite.Guid.Value;
                if (!String.IsNullOrWhiteSpace(forward))
                {
                    AvailablePhoneNumber temp = AvailablePhoneNumber.Parse(forward);
                    phone.ForwardNumber = temp.PhoneNumber;
                }

                SubscriptionManager.AddPhoneToSubscription(phone);

                //Associate the phone with the campaign itself
                String guid = Request.QueryString["id"];
                
                CmsCampaign campaign = CampaignManager.Instance.GetCampaign(guid);
                campaign.PhoneNumber = phone.PhoneNumber;
                CampaignManager.Instance.Add(campaign);

                this.LoadPage();
                this.LblStatus.Text = "Successfully activated campaign phone number!";
                this.PhoneNumberViews.SetActiveView(ExistingView);
            }
            catch (Exception ex)
            {
                this.LblStatus.Text = "There was a problem activating the selected number: " + ex.Message;
                this.LblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}