using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encore.PayPal.Nvp;

namespace Gooeycms.Business.Paypal
{
    public class PaypalProfileInfo
    {
        public enum ProfileStatusEnum
        {
            Cancelled,
            Active
        }

        private NvpGetRecurringPaymentsProfileDetails action;

        public PaypalProfileInfo(Encore.PayPal.Nvp.NvpGetRecurringPaymentsProfileDetails action)
        {
            this.action = action;
        }

        public String ProfileId
        {
            get { return action.Get(NvpGetRecurringPaymentsProfileDetails.Response.PROFILEID); }
        }

        public DateTime Created
        {
            get { return DateTime.Parse(action.Get(NvpGetRecurringPaymentsProfileDetails.Response.PROFILESTARTDATE)); }
        }

        public String Status
        {
            get { return action.Get(NvpGetRecurringPaymentsProfileDetails.Response.STATUS); }
        }

        public ProfileStatusEnum ProfileStatus
        {
            get { return (ProfileStatusEnum)Enum.Parse(typeof(ProfileStatusEnum), Status, true); }
        }

        public DateTime? LastBillDate
        {
            get
            {
                DateTime? result = null;

                DateTime item = DateTime.MinValue;
                String temp = action.Get(NvpGetRecurringPaymentsProfileDetails.Response.LASTPAYMENTDATE); 
                DateTime.TryParse(temp,out item);

                if (item != DateTime.MinValue)
                    result = new DateTime?(item);

                return result;
            }
        }

        public Double? LastBillAmount
        {
            get
            {
                Double? amount = null;
                
                Double item = Double.NaN;
                String temp = action.Get(NvpGetRecurringPaymentsProfileDetails.Response.LASTPAYMENTAMT);
                Double.TryParse(temp, out item);

                if (item != Double.NaN)
                    amount = new Double?(item);

                return amount;
            }
        }

        public DateTime? NextBillDate
        {
            get
            {
                DateTime? result = null;

                DateTime item = DateTime.MinValue;
                String temp = action.Get(NvpGetRecurringPaymentsProfileDetails.Response.NEXTBILLINGDATE); 
                DateTime.TryParse(temp,out item);

                if (item != DateTime.MinValue)
                    result = new DateTime?(item);

                return result;
            }
        }

        public Double? NextBillAmount
        {
            get
            {
                Double? amount = null;

                Double item = Double.NaN;
                String temp;
                if (IsTrialPeriod)
                    temp = action.Get("TRIALAMT");
                else
                    temp = action.Get(NvpGetRecurringPaymentsProfileDetails.Response.AMT);
                Double.TryParse(temp, out item);

                if (item != Double.NaN)
                    amount = new Double?(item);

                return amount;
            }
        }

        public Boolean IsTrialPeriod
        {
            get
            {
                Boolean result = false;
                String temp = action.Get(NvpGetRecurringPaymentsProfileDetails.Response.NUMCYCLESREMAINING);

                //check the LENGTH of the STRING instead of risking overflowing with a huge number
                if (temp.Length < 3) 
                    result = true;

                return result;
            }
        }

        public int TrialCyclesRemaining
        {
            get
            {
                long cycles = 0;
                String temp = action.Get(NvpGetRecurringPaymentsProfileDetails.Response.NUMCYCLESREMAINING);
                long.TryParse(temp, out cycles);

                int result = (int)cycles;
                return result;
            }
        }

        public Double? NormalPaymentAmt
        {
            get
            {
                Double? amount = null;

                Double item = Double.NaN;
                String temp = action.Get("REGULARAMT");
                Double.TryParse(temp, out item);

                if (item != Double.NaN)
                    amount = new Double?(item);

                return amount;
            }
        }

        public Boolean IsSuspended
        {
            get { return Status.Equals("Suspended"); }
        }

        public Boolean IsCancelled
        {
            get { return Status.Equals("Cancelled"); }
        }

        public Boolean IsActive
        {
            get { return Status.Equals("Active"); }
        }
    }
}
