using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Handler;
using Gooeycms.Business.Campaigns;
using Gooeycms.Data.Model.Content;
using Microsoft.Security.Application;
using System.Web.UI;
using System.Web;
using Gooeycms.Business.Util;
using Gooeycms.Business.Storage;

namespace Gooeycms.Business.Content
{
    public class FileDownloadHandler : BaseHttpHandler
    {
        protected override void Process(System.Web.HttpContext context)
        {
            Boolean skipRedirect = false;
            String filename = null;
            String campaignQuerystring = "";

            String encryptedDownload = context.Request.QueryString["d"];
            if (encryptedDownload == null)
            {
                String campaignString = context.Request.QueryString["qs"];
                filename = context.Request.QueryString["filename"];

                //Convert the campaign string to the actual qs that will be appended to the redirect
                campaignQuerystring = CampaignManager.Instance.ConvertTrackingLink(campaignString);
            }
            else
            {
                try
                {
                    filename = ContentManager.DecryptFilename(encryptedDownload);
                    skipRedirect = true;
                }
                catch (Exception)
                {
                    ReturnMissingFileError(context);
                }
            }

            //Get the cms content type for this file
            CmsContent content = ContentManager.Instance.GetFile(filename);
            if (content != null)
            {
                if (!content.RequiresRegistration)
                    skipRedirect = true;
            }
            else
                ReturnMissingFileError(context);

            if (!skipRedirect)
            {
                Control  resolver = new Control();
                String redirect = "~" + content.RegistrationPage + "?" + campaignQuerystring + "&fget=" + AntiXss.UrlEncode(filename);
                redirect = resolver.ResolveUrl(redirect);

                context.Response.Redirect(redirect, true);
            }
            else
            {
                DownloadFile(context, content, filename);
            }
        }

        private void DownloadFile(System.Web.HttpContext context, CmsContent content, String filename)
        {
            CmsContentField mimeTypeField = content.FindField("mimetype");
            string mimeType = "application/octet-stream";
            if (mimeTypeField != null)
                mimeType = mimeTypeField.Value;

            Data.Guid siteGuid = CurrentSite.Guid;
            ContentFileUploadImpl filehandler = new ContentFileUploadImpl();
            StorageFile fileinfo = filehandler.GetInfo(siteGuid, filename);

            context.Response.Clear();
            context.Response.ClearHeaders();
            context.Response.ClearContent();
            context.Response.ContentType = mimeType;
            context.Response.AppendHeader("Content-Disposition", "attachment;filename=\"" + filename + "\"");
            context.Response.AppendHeader("Content-Length", fileinfo.Size.ToString());
            context.Response.AppendHeader("Content-Transfer-Encoding", "binary");
            context.Response.Expires = 60;
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.Cache.SetMaxAge(TimeSpan.FromMinutes(0));
            filehandler.Read(siteGuid,filename,context.Response.OutputStream);

            context.Response.Flush();
            context.Response.Close();
            context.Response.End();              
        }

        private void ReturnMissingFileError(System.Web.HttpContext context)
        {
            context.Response.Clear();
            context.Response.StatusCode = 404;
            context.Response.Status = "404 File Not Found";
            context.Response.StatusDescription = "404 File Not Found";
            context.Response.Flush();
            context.Response.End();
        }
    }
}
