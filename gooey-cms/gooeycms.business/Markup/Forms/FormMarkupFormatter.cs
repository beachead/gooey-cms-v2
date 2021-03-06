﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Gooeycms.Business.Web;
using Gooeycms.Business.Crypto;
using Microsoft.Security.Application;

namespace Beachead.Core.Markup.Forms
{
    /// <summary>
    /// Formats the form markup into standard HTML form code.
    /// </summary>
    public class FormMarkupFormatter : BaseFormatter
    {
        private static Regex Form = new Regex(@"<form>(.*?)</form>", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);

        private const String SubmitFormat = @"<input class=""submit"" id=""{0}_{1}"" name=""{0}_{1}"" type=""submit"" value=""{2}""  />";

        public override StringBuilder Convert(StringBuilder markup)
        {
            Match match = Form.Match(markup.ToString());
            while (match.Success)
            {
                String content = match.Groups[1].Value;
                String id = System.Guid.NewGuid().ToString();

                //Find the form and pull it out of the markup
                StringBuilder html = new StringBuilder();
                FormMetaInfoParser metainfo = new FormMetaInfoParser(id);
                FormFieldParser fields = new FormFieldParser(id,base.FormatEngine);

                WebRequestContext context = new WebRequestContext();
                CmsUrl url = new CmsUrl(context.Request.RawUrl);

                String pagename = url.PathWithoutExtension;
                String token = AntiXss.UrlEncode(TokenManager.Issue(pagename, TimeSpan.FromMinutes(2)));

                html.Append(@"<div class=""webscript-form"">").AppendLine();
                html.Append(@"<form class=""gooeycms-form"" action=""/gooeyforms/formprocess.handler?token=" + token + @"&pagename=" + AntiXss.UrlEncode(pagename) + @""" method=""post"">").AppendLine();
                html = metainfo.Convert(html, content);
                html = fields.Convert(html, content);
                html.AppendFormat(SubmitFormat, id, "submit", metainfo.SubmitButtonText).AppendLine();
                html.Append(@"</form>").AppendLine();
                html.Append(@"<span id=""form_error""></span>");
                html.Append("</div>").AppendLine();

                //Replace the form markup with the html
                markup = new StringBuilder(Form.Replace(markup.ToString(), html.ToString(), 1));

                match = match.NextMatch();
            }

            return markup;
        }
    }
}
