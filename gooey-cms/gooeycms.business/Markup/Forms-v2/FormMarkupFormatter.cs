using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beachead.Core.Markup;
using System.Text.RegularExpressions;
using Gooeycms.Business.Web;
using Gooeycms.Business.Crypto;
using Microsoft.Security.Application;
using Gooeycms.Business.Util;
using Gooeycms.Business.Content;

namespace Gooeycms.Business.Markup.Forms_v2
{
    public class FormMarkupFormatter : BaseFormatter
    {
        public static Regex Form = new Regex(@"<form\s*(.*?)>(.*?)</form>", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
        private static Regex FormTextField = new Regex(@"<textbox\s+(\w+)\s*([*]|required)?\s*/?>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex FormTextBoxField = new Regex(@"<textarea\s+(\w+)\s*(\d+\s*,\s*\d+)?\s*([*]|required)?\s*/?>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex FormSelectField = new Regex(@"\<select\s*(\w+)\s*([*]|required)?\s*>\s*(\#.*?)\<\s*/select\s*>", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex FormRadioField = new Regex(@"\<select\s*(\w+)\s*([*]|required)?\s*>\s*(\*.*?)\<\s*/select\s*>", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex FormCheckField = new Regex(@"<checkbox\s+(\w+)\s*([*]|required)?\s*/?>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex SubmitButtonField = new Regex(@"<submit\s+([\w|\s]+)\s*(onclick=.*?)?\s*/?>", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static Regex RedirectTo = new Regex(@"redirectto=""(.*?)""", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex EmailTo = new Regex(@"emailto=""(.*?)""", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public override StringBuilder Convert(StringBuilder markup)
        {
            String text = markup.ToString();

            text = FormTextField.Replace(text, new MatchEvaluator(TextFieldEvaluator));
            text = FormTextBoxField.Replace(text, new MatchEvaluator(TextBoxFieldEvaluator));
            text = FormSelectField.Replace(text, new MatchEvaluator(SelectFieldEvaluator));
            text = FormRadioField.Replace(text, new MatchEvaluator(RadioFieldEvaluator));
            text = FormCheckField.Replace(text, new MatchEvaluator(CheckboxFieldEvaluator));
            text = SubmitButtonField.Replace(text, new MatchEvaluator(SubmitButtonEvaluator));
            text = Form.Replace(text, new MatchEvaluator(FormEvaluator));

            return new StringBuilder(text);
        }

        private String FormEvaluator(Match match)
        {
            WebRequestContext context = new WebRequestContext();
            CmsUrl url = new CmsUrl(context.Request.RawUrl);
            String currentUrl = url.RelativePath;

            String formMetaInfo = match.Groups[1].Value;
            String content = match.Groups[2].Value;

            StringBuilder metainfo = new StringBuilder();
            String error = GetHiddenFields(currentUrl, formMetaInfo, metainfo);

            String pagename = url.PathWithoutExtension;
            if (error == null)
            {
                StringBuilder formhtml = new StringBuilder();
                formhtml.Append(@"<form class=""gooeycms-form"" action=""/gooeyforms/formprocess.handler?&pagename=" + AntiXss.UrlEncode(pagename) + @""" method=""post"">").AppendLine();
                formhtml.AppendLine(metainfo.ToString());
                formhtml.Append(content);
                formhtml.Append(@"</form>").AppendLine();
                formhtml.Append(@"<span id=""form_error""></span>");
                return formhtml.Replace("{form_id}", System.Guid.NewGuid().ToString()).ToString();
            }
            else
                return error;
        }

        private static String GetHiddenFields(String currentUrl, String formMetaInfo, StringBuilder metainfo)
        {
            String error = null;

            //Find the meta data
            Match redirectMatch = RedirectTo.Match(formMetaInfo);
            if (redirectMatch.Success)
                metainfo.AppendFormat(@"<input type=""hidden"" name=""{{form_id}}_redirect"" value=""{0}"" />", AntiXss.HtmlEncode(redirectMatch.Groups[1].Value)).AppendLine();
            else
                error = "<b>ERROR-IN-FORM-MARKUP: Form is missing required 'redirectTo' attribute";

            Match emailToMatch = EmailTo.Match(formMetaInfo);
            if (emailToMatch.Success)
                metainfo.AppendFormat(@"<input type=""hidden"" name=""{{form_id}}_submit-email"" value=""{0}"" />", AntiXss.HtmlEncode(emailToMatch.Groups[1].Value)).AppendLine();
            else
                metainfo.AppendLine(@"<input type=""hidden"" name=""{{form_id}}_submit-email"" value="""" />");

            metainfo.AppendFormat(@"<input type=""hidden"" name=""{{form_id}}_culture"" value=""{0}"" />", AntiXss.HtmlEncode(CurrentSite.Culture)).AppendLine();
            metainfo.AppendFormat(@"<input type=""hidden"" name=""{{form_id}}_resource"" value=""{0}"" />", AntiXss.HtmlEncode(currentUrl)).AppendLine();

            //Check if there's a file we need to download after the form is submitted
            String filename = WebRequestContext.Instance.Request.QueryString["fget"];
            String encryptedFilename = "";
            if (!String.IsNullOrWhiteSpace(filename))
                encryptedFilename = ContentManager.EncryptFilename(filename);

            metainfo.AppendFormat(@"<input type=""hidden"" name=""{{form_id}}_gooey-filename"" value=""{0}"" />", AntiXss.HtmlEncode(encryptedFilename)).AppendLine();

            return error;
        }

        private String SubmitButtonEvaluator(Match match)
        {
            String id = "{form_id}_submit";
            String value = AntiXss.HtmlEncode(match.Groups[1].Value);
            String args = match.Groups[2].Value;

            String textbox = String.Format(@"<input type=""submit"" id=""{0}"" name=""{0}"" {2} value=""{1}"" />", id, value.Trim(), args.Trim());

            return textbox;
        }

        private String TextFieldEvaluator(Match match)
        {
            String name = match.Groups[1].Value;
            String id = "{form_id}_" + name;
            String isrequired = (String.IsNullOrWhiteSpace(match.Groups[2].Value)) ? "" : "required";
            
            String emailFieldCss = "";
            if (name.StartsWith("email", StringComparison.InvariantCultureIgnoreCase))
                emailFieldCss = " email";

            String textbox = String.Format(@"<input type=""text"" id=""{0}"" name=""{0}"" class=""form-textbox persistable {1}{2}"" />", id, isrequired,emailFieldCss);

            return textbox;
        }

        private String CheckboxFieldEvaluator(Match match)
        {
            String id = "{form_id}_" + match.Groups[1].Value;
            String isrequired = (String.IsNullOrWhiteSpace(match.Groups[2].Value)) ? "" : "required";
            String textbox = String.Format(@"<input type=""checkbox"" id=""{0}"" name=""{0}"" class=""form-checkbox {1}"" value=""true"" />", id, isrequired);

            return textbox;
        }

        private String SelectFieldEvaluator(Match match)
        {
            String id = "{form_id}_" + match.Groups[1].Value;
            String isrequired = (String.IsNullOrWhiteSpace(match.Groups[2].Value)) ? "" : "required";
            String content = match.Groups[3].Value;

            String isMultiple = "";
            String identifier = "#";
            String[] items = content.Split('\n');
            if (items[0].Trim().StartsWith("##"))
            {
                isMultiple = "multiple=multiple";
                identifier = "##";
            }

            StringBuilder output = new StringBuilder();
            output.AppendFormat(@"<select id=""{0}"" name=""{0}"" class=""form-select persistable {1}"" {2}>", id, isrequired, isMultiple).AppendLine();

            if (!String.IsNullOrEmpty(isrequired) && (String.IsNullOrEmpty(isMultiple)))
                output.AppendLine(@"<option value="""">&nbsp;</option>");

            foreach (String item in items)
            {
                String temp = item.Trim();
                if (temp.Length > 0)
                {
                    String value = temp.Substring(identifier.Length).Trim();
                    output.AppendFormat(@"<option value=""{0}"">{0}</option>", AntiXss.HtmlEncode(value)).AppendLine();
                }
            }

            output.AppendLine("</select>");
            return output.ToString();
        }

        private String RadioFieldEvaluator(Match match)
        {
            String id = "{form_id}_" + match.Groups[1].Value;
            Boolean isrequired = (String.IsNullOrWhiteSpace(match.Groups[2].Value)) ? false : true;
            String content = match.Groups[3].Value;

            String[] items = content.Split('*');

            int count = 0;
            StringBuilder output = new StringBuilder();

            output.AppendLine(@"<div class=""form-radiolist"">");
            foreach (String item in items)
            {
                String value = item.Trim();
                if (value.Length > 0)
                {
                    String selected = "";
                    if ((count == 0) && (!isrequired))
                        selected = "selected=selected";

                    output.AppendFormat(@"<input type=""radio"" id=""{0}_{1}"" name=""{0}"" value=""{2}"" {3} />&nbsp;{2}<br />", id, count++, AntiXss.HtmlEncode(value),selected).AppendLine();
                }
            }
            output.AppendLine("</div>");
            return output.ToString();
        }

        private String TextBoxFieldEvaluator(Match match)
        {
            String id = "{form_id}_" + match.Groups[1].Value;
            String rowcols = match.Groups[2].Value;
            String isrequired = (String.IsNullOrWhiteSpace(match.Groups[3].Value)) ? "" : "required";

            String rows = "15";
            String cols = "50";

            if (!String.IsNullOrWhiteSpace(rowcols))
            {
                String[] temp = rowcols.Split(',');
                rows = temp[0];
                cols = temp[1];
            }

            String textbox = String.Format(@"<textarea class=""form-textarea persistable {0}"" id=""{1}"" name=""{1}"" cols=""{2}"" rows=""{3}""></textarea>", isrequired, id, cols, rows);
            return textbox;
        }


    }
}
