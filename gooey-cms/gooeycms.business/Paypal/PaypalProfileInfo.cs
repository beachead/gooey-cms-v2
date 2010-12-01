using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encore.PayPal.Nvp;

namespace Gooeycms.Business.Paypal
{
    public class PaypalProfileInfo
    {
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

                int result = 0;
                if (cycles < 6)
                    result = (int)cycles;

                return result;
            }
        }

        public Double? NormalPaymentAmt
        {
            get
            {
                Double? amount = null;

                Double item = Double.NaN;
                String temp = action.Get(NvpGetRecurringPaymentsProfileDetails.Response.AMT);
                Double.TryParse(temp, out item);

                if (item != Double.NaN)
                    amount = new Double?(item);

                return amount;
            }
        }
    }
}
