using System;
using System.Text;
using System.Text.RegularExpressions;
using Gooeycms.Business.Web;

namespace Beachead.Core.Markup.Forms
{
    /// <summary>
    /// Parses the meta information from the form markup and formats
    /// it into the appropriate HTML
    /// </summary>
    public class FormMetaInfoParser
    {
        private static Regex Redirect = new Regex(@"redirect:\s*(.*)",RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);
        private static Regex ButtonText = new Regex(@"buttontext:\s*(.*)",RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);
        private static Regex EmailResults = new Regex(@"emailresults:\s*(.*)",RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

        private const String HiddenFieldFormat = @"<input type=""hidden"" id=""{0}_{1}"" name=""{0}_{1}"" value=""{2}"" />";

        private String markup = null;
        private String id;
        private String buttonText = "Submit";

        public FormMetaInfoParser(String formId)
        {
            this.id = formId;
        }

        public String SubmitButtonText
        {
            get { return this.buttonText; }
        }

        /// <summary>
        /// Takes the markup and then starts to build up the HTML form
        /// </summary>
        /// <param name="markup"></param>
        /// <returns></returns>
        public StringBuilder Convert(StringBuilder html,String content)
        {
            this.markup = content.Trim();

            WebRequestContext context = new WebRequestContext();

            //CampaignManager campaignManager = new CampaignManager();
            String culture = "en-us"; //TODO CultureManager.NewInstance().CurrentCulture;

            String redirectTo = GetMetaValue(Redirect).Trim();
            String submitButtonText = GetMetaValue(ButtonText).Trim();
            String emailResultsTo = GetMetaValue(EmailResults).Trim();
            String currentResource = GetCurrentResource();
            String campaigns = ""; //TODO campaignManager.GetInternalCampaignTrail("|");
            String file = context.CurrentHttpContext.Request.QueryString["fget"];
            if (file == null)
                file = "";

            html.Append(HiddenField("culture", culture)).AppendLine();
            html.Append(HiddenField("redirect", CmsUrl.ResolveUrl(redirectTo))).AppendLine();
            html.Append(HiddenField("submit-email", emailResultsTo)).AppendLine();
            html.Append(HiddenField("resource", currentResource)).AppendLine();
            html.Append(HiddenField("campaign", campaigns.Trim())).AppendLine();
            if (!String.IsNullOrEmpty(file))
                html.Append(HiddenField("downloadfile", file.Trim())).AppendLine();

            this.buttonText = submitButtonText;

            return html;
        }

        private String GetCurrentResource()
        {
            WebRequestContext context = new WebRequestContext();
            return new CmsUrl(context.Request.Path).RelativePath;
        }

        private String HiddenField(String key, String value)
        {
            return String.Format(HiddenFieldFormat,this.id,key,value);
        }

        /// <summary>
        /// Parses the meta information from the regular expression
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        private String GetMetaValue(Regex pattern)
        {
            String result = null;

            Match match = pattern.Match(this.markup);
            if (match != null)
            {
                result = match.Groups[1].Value; //ensure we keep the casing
            }

            return result;
        }
    }
}
