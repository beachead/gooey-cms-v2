﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encore.PayPal.Nvp;
using Gooeycms.Data.Model.Subscription;
using Gooeycms.Business.Subscription;

namespace Gooeycms.Business.Paypal
{
    public class PaypalExpressCheckout
    {
        public const String Enabled = "1";
        public const String Disabled = "0";

        private List<NvpBaItem> billingAgreements = new List<NvpBaItem>();
        private string token;

        public struct ProfileResultStatus
        {
            public String ProfileId;
            public String ProfileStatus;
        }

        public PaypalExpressCheckout()
        {
            //NvpConfig.Settings.Version = "54.0";
        }

        public PaypalExpressCheckout(string token)
        {
            this.token = token;
            LoadExistingDetails();
        }

        public String SubscriptionId { get; private set; }
        public String PayerID { get; private set; }

        private void LoadExistingDetails()
        {
            NvpGetExpressCheckoutDetails details = new NvpGetExpressCheckoutDetails();
            
            SetDefaults(details);
            details.Add(NvpGetExpressCheckoutDetails.Request.TOKEN, this.token);
            bool result = details.Post();
            if (!result)
                throw PaypalException.GenerateException(details);

            this.SubscriptionId = details.Get(NvpGetExpressCheckoutDetails.Response.CUSTOM);
            this.PayerID = details.Get(NvpGetExpressCheckoutDetails.Response.PAYERID);
        }

        public String SetExpressCheckout(String email, String subscriptionId)
        {
            return SetExpressCheckout(email, subscriptionId, GooeyConfigManager.PaypalReturnUrl, GooeyConfigManager.PaypalCancelUrl);
        }

        public String SetExpressCheckout(String email, String subscriptionId, String returnUrl, String cancelUrl)
        {
            NvpSetExpressCheckout checkout = new NvpSetExpressCheckout();
            SetDefaults(checkout);

            checkout.Add(NvpSetExpressCheckout.Request._RETURNURL, returnUrl);
            checkout.Add(NvpSetExpressCheckout.Request._CANCELURL, cancelUrl);
            checkout.Add(NvpSetExpressCheckout.Request.NOSHIPPING, Enabled);
            if (!String.IsNullOrWhiteSpace(email))
                checkout.Add(NvpSetExpressCheckout.Request.EMAIL, email);
            checkout.Add(NvpSetExpressCheckout.Request.CUSTOM, subscriptionId);
            checkout.Add(billingAgreements);
            checkout.SkipReview = true;

            Logging.Info("Paypal Request URL:" + checkout.RequestString);

            bool result = checkout.Post();
            if (!result)
                throw PaypalException.GenerateException(checkout);

            //Return the redirect url including the token
            return checkout.RedirectUrl;
        }

        public void AddBillingAgreement(Registration registration)
        {
            NvpBaItem agreement = PaypalExpressCheckout.GetBillingAgreement(registration);
            AddBillingAgreement(agreement);
        }

        public void AddBillingAgreement(NvpBaItem agreement)
        {
            this.billingAgreements.Add(agreement);
        }

        public static NvpBaItem GetBillingAgreement(String subscription, String description)
        {
            NvpBaItem agreement = new NvpBaItem();
            agreement.BillingType = NvpBillingCodeType.RecurringPayments;
            agreement.Custom = subscription;
            agreement.Description = description;
            agreement.PaymentType = NvpMerchantPullPaymentCodeType.Any;

            return agreement;
        }

        private static void SetDefaults(NvpApiBase request)
        {
            request.Credentials.Delegate = GetCredentials;
            request.Environment = (GooeyConfigManager.IsPaypalSandbox) ? NvpEnvironment.Sandbox : NvpEnvironment.Live;
            request.Add(NvpCommonRequest.VERSION, GooeyConfigManager.PaypalApiVersion);
        }

        private static NvpCredentials GetCredentials()
        {
            NvpCredentials creds = new NvpCredentials();
            creds.Username = GooeyConfigManager.PaypalUsername;
            creds.Password = GooeyConfigManager.PaypalPassword;
            creds.Signature = GooeyConfigManager.PaypalSignature;
                
            return creds;
        }

        /// <summary>
        /// Gets all of the billing agreements for the specified registration
        /// </summary>
        /// <param name="registration"></param>
        /// <returns></returns>
        public static NvpBaItem GetBillingAgreement(Registration registration)
        {
            StringBuilder description = new StringBuilder();
            CmsSubscriptionPlan plan = SubscriptionManager.GetSubscriptionPlan(registration);
            Double totalPrice = (Double)plan.Price;

            description.AppendFormat("{0} / {1:c} ", plan.Name, plan.Price);
            if (registration.IsCampaignEnabled)
            {
                description.AppendFormat(" +Campaigns / {0:c} ", GooeyConfigManager.CampaignOptionPrice);
                totalPrice += GooeyConfigManager.CampaignOptionPrice;
            }

            if (registration.IsSalesforceEnabled)
            {
                description.AppendFormat(" +Salesforce / {0:c} ", GooeyConfigManager.SalesForcePrice);
                totalPrice += GooeyConfigManager.SalesForcePrice;
            }
            description.AppendFormat(". Total: {0:c} / month after {1} days free.", totalPrice, GooeyConfigManager.FreeTrialLength);

            return PaypalExpressCheckout.GetBillingAgreement(registration.Guid, description.ToString());
        }

        /// <summary>
        /// Gets all of the billing agreements for the specified registration
        /// </summary>
        /// <param name="registration"></param>
        /// <returns></returns>
        public static NvpBaItem GetBillingAgreement(CmsSubscription subscription, int freeTrialLength = -1)
        {
            if (freeTrialLength == -1)
                freeTrialLength = GooeyConfigManager.FreeTrialLength;

            StringBuilder description = new StringBuilder();
            CmsSubscriptionPlan plan = subscription.SubscriptionPlan;
            Double totalPrice = (Double)plan.Price;

            description.AppendFormat("{0} / {1:c} ", plan.Name, plan.Price);
            if (subscription.IsCampaignEnabled)
            {
                description.AppendFormat(" +Campaigns / {0:c} ", GooeyConfigManager.CampaignOptionPrice);
                totalPrice += GooeyConfigManager.CampaignOptionPrice;
            }

            if (subscription.IsSalesforceEnabled)
            {
                description.AppendFormat(" +Salesforce / {0:c} ", GooeyConfigManager.SalesForcePrice);
                totalPrice += GooeyConfigManager.SalesForcePrice;
            }
            description.AppendFormat(". Total: {0:c} / month",totalPrice);
            
            if (freeTrialLength > 0)
                description.AppendFormat(" after {0} days free.", freeTrialLength);

            return PaypalExpressCheckout.GetBillingAgreement(subscription.Guid, description.ToString());
        }

        public ProfileResultStatus CreateRecurringPayment(Registration registration)
        {
            NvpBaItem agreement = GetBillingAgreement(registration);
            Double total = SubscriptionManager.CalculateCost(registration);

            return CreateRecurringPayment(agreement, total);
        }

        public ProfileResultStatus CreateRecurringPayment(CmsSubscription subscription, int freeTrial = -1)
        {
            //Check if they have an existing billing profile active, if so, cancel it, so we can create the new one
            PaypalProfileInfo existingProfile = GetProfileInfo(subscription.PaypalProfileId);
            if ((existingProfile != null) && (existingProfile.IsActive))
            {
                try
                {
                    this.Cancel(existingProfile.ProfileId);
                }
                catch (Exception ex)
                {
                    Logging.Database.Write("paypal-error", "There was a problem cancelling existing profile. Subscription: " + subscription.Guid + ", Paypal Profile Id:" + existingProfile.ProfileId + ", Error: " + ex.Message);
                    throw new ApplicationException("There was a problem upgrading your account. There seems to be an active billing profile which we were unable to automatically cancel. Please contact custom support.");
                }
            }

            //Create the new agreement
            NvpBaItem agreement = GetBillingAgreement(subscription, freeTrial);
            Double total = SubscriptionManager.CalculateCost(subscription);

            return CreateRecurringPayment(agreement, total, freeTrial);
        }

        public ProfileResultStatus CreateRecurringPayment(NvpBaItem agreement, Double total, int freeTrial = -1)
        {
            if (freeTrial == -1)
                freeTrial = GooeyConfigManager.FreeTrialLength;

            NvpCreateRecurringPaymentsProfile action = new NvpCreateRecurringPaymentsProfile();
            SetDefaults(action);

            action.Add(NvpCreateRecurringPaymentsProfile.Request._TOKEN, token);
            action.Add(NvpCreateRecurringPaymentsProfile.Request._DESC, agreement.Description);
            action.Add(NvpCreateRecurringPaymentsProfile.Request._PROFILESTARTDATE, DateTime.Now.ToUniversalTime().ToString("o"));
            action.Add(NvpCreateRecurringPaymentsProfile.Request._AMT, total.ToString("f"));
            action.Add(NvpCreateRecurringPaymentsProfile.Request._BILLINGFREQUENCY, "1");
            action.Add(NvpCreateRecurringPaymentsProfile.Request._BILLINGPERIOD, NvpBillingPeriodType.Month);

            if (freeTrial > 0)
            {
                action.Add(NvpCreateRecurringPaymentsProfile.Request._TRIALBILLINGPERIOD, NvpBillingPeriodType.Day);
                action.Add(NvpCreateRecurringPaymentsProfile.Request._TRIALBILLINGFREQUENCY, "1"); //bill once a day for "freetriallength" days
                action.Add(NvpCreateRecurringPaymentsProfile.Request._TRIALTOTALBILLINGCYCLES, freeTrial.ToString());
                action.Add(NvpCreateRecurringPaymentsProfile.Request._TRIALAMT, Double.Parse("0").ToString("f"));
            }

            action.Add(NvpCreateRecurringPaymentsProfile.Request.MAXFAILEDPAYMENTS, GooeyConfigManager.PaypalMaxFailedPayments.ToString());
            action.Add(NvpCreateRecurringPaymentsProfile.Request.AUTOBILLOUTAMT, NvpAutoBillType.AddToNextBilling);

            Boolean result = action.Post();
            if (!result)
                throw PaypalException.GenerateException(action);

            String profileId = action.Get(NvpCreateRecurringPaymentsProfile.Response.PROFILEID);
            String profileStatus = action.Get(NvpCreateRecurringPaymentsProfile.Response.PROFILESTATUS);

            ProfileResultStatus status = new ProfileResultStatus();
            status.ProfileId = profileId;
            status.ProfileStatus = profileStatus;

            return status;
        }

        public PaypalProfileInfo GetProfileInfo(string profileId)
        {
            PaypalProfileInfo info = null;
            if (!String.IsNullOrEmpty(profileId))
            {
                NvpGetRecurringPaymentsProfileDetails action = new NvpGetRecurringPaymentsProfileDetails();
                SetDefaults(action);

                action.Add(NvpGetRecurringPaymentsProfileDetails.Request.PROFILEID, profileId);
                Boolean result = action.Post();
                if (!result)
                    throw PaypalException.GenerateException(action);

                info = new PaypalProfileInfo(action);
            }

            return info;
        }

        public Boolean Suspend(string profileId)
        {
            Boolean result = false;
            if (!String.IsNullOrEmpty(profileId))
            {
                NvpManageRecurringPaymentsProfileStatus action = new NvpManageRecurringPaymentsProfileStatus();
                SetDefaults(action);

                action.Add(NvpManageRecurringPaymentsProfileStatus.Request.PROFILEID, profileId);
                action.Add(NvpManageRecurringPaymentsProfileStatus.Request.ACTION, NvpStatusChangeActionType.Suspend);
                result = action.Post();
                if (!result)
                    throw PaypalException.GenerateException(action);
            }

            return result;
        }

        public Boolean Reactivate(string profileId)
        {
            Boolean result = false;
            if (!String.IsNullOrEmpty(profileId))
            {
                NvpManageRecurringPaymentsProfileStatus action = new NvpManageRecurringPaymentsProfileStatus();
                SetDefaults(action);

                action.Add(NvpManageRecurringPaymentsProfileStatus.Request.PROFILEID, profileId);
                action.Add(NvpManageRecurringPaymentsProfileStatus.Request.ACTION, NvpStatusChangeActionType.Reactivate);
                result = action.Post();
                if (!result)
                    throw PaypalException.GenerateException(action);
            }

            return result;
        }

        public Boolean Cancel(string profileId)
        {
            Boolean result = true;
            if (!String.IsNullOrEmpty(profileId))
            {
                //Make sure the profile isn't already cancelled...
                PaypalProfileInfo info = GetProfileInfo(profileId);
                if (!info.IsCancelled)
                {
                    NvpManageRecurringPaymentsProfileStatus action = new NvpManageRecurringPaymentsProfileStatus();
                    SetDefaults(action);

                    action.Add(NvpManageRecurringPaymentsProfileStatus.Request.PROFILEID, profileId);
                    action.Add(NvpManageRecurringPaymentsProfileStatus.Request.ACTION, NvpStatusChangeActionType.Cancel);
                    result = action.Post();
                    if (!result)
                        throw PaypalException.GenerateException(action);
                }
            }

            return result;            
        }

        internal void ExtendTrialPeriod(string profileId, Int32 numberOfCycles)
        {
            if (!String.IsNullOrEmpty(profileId))
            {
                NvpUpdateRecurringPaymentsProfile action = new NvpUpdateRecurringPaymentsProfile();
                SetDefaults(action);

                action.Add(NvpUpdateRecurringPaymentsProfile.Request._PROFILEID, profileId);
                action.Add("TRIALTOTALBILLINGCYCLES", numberOfCycles.ToString());

                Boolean result = action.Post();
                if (!result)
                    throw PaypalException.GenerateException(action);
            }
        }

        internal void UpdateBillingAgreement(string profileId, double newCost, string desc)
        {
            if (!String.IsNullOrEmpty(profileId))
            {
                NvpUpdateRecurringPaymentsProfile action = new NvpUpdateRecurringPaymentsProfile();
                SetDefaults(action);

                action.Add(NvpUpdateRecurringPaymentsProfile.Request._PROFILEID, profileId);
                action.Add(NvpUpdateRecurringPaymentsProfile.Request.DESC, desc);
                action.Add(NvpUpdateRecurringPaymentsProfile.Request.AMT, newCost.ToString("f"));

                Boolean result = action.Post();
                if (!result)
                    throw PaypalException.GenerateException(action);
            }
        }
    }
}
