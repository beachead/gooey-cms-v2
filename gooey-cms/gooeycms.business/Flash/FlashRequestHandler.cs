using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Handler;
using Gooeycms.Business.Storage;
using Gooeycms.Business.Util;
using System.IO;
using System.Web;
using Gooeycms.Business.Web;

namespace Gooeycms.Business.Flash
{
    public class FlashRequestHandler : BaseHttpHandler
    {
        protected override void Process(System.Web.HttpContext context)
        {
            String container = CurrentSite.GetContainerUrl(SiteHelper.ImagesContainerKey);
            String directory = StorageClientConst.RootFolder;
            String filename = context.Request.QueryString["filename"] + ".swf";

            //Find the actual filename, since all flash files are stored without the directory
            int pos = filename.LastIndexOf("/");
            if (pos > -1)
                filename = filename.Substring(pos);

            IStorageClient client = StorageHelper.GetStorageClient();
            StorageFile file = client.GetInfo(container, directory, filename);

            Boolean isCacheValid = WebRequestContext.Instance.IsModifiedSince(file.LastModified);
            if (isCacheValid)
            {
                CacheValidResponse(context, file);
            }
            else
            {
                try
                {
                    context.Response.Clear();
                    context.Response.ClearHeaders();
                    context.Response.ClearContent();
                    context.Response.Cache.SetLastModified(file.LastModified);
                    context.Response.ContentType = "application/x-shockwave-flash";
                    context.Response.AddHeader("Content-Length", file.Size.ToString());
                    context.Response.Cache.SetCacheability(HttpCacheability.Public);
                    client.DownloadToStream(context.Response.OutputStream, container, directory, filename);
                }
                catch (FileNotFoundException ex)
                {
                    FileNotFoundResponse(context, ex.Message);
                }
                catch (DirectoryNotFoundException ex)
                {
                    FileNotFoundResponse(context, ex.Message);
                }
                catch (Exception ex)
                {
                    ServerErrorResponse(context, ex);
                }
            }
        }

        private void CacheValidResponse(System.Web.HttpContext context, StorageFile file)
        {
            context.Response.Clear();
            context.Response.ClearHeaders();
            context.Response.ClearContent();

            context.Response.StatusCode = 304;
            context.Response.SuppressContent = true;
        }

        private void ServerErrorResponse(System.Web.HttpContext context, Exception ex)
        {
            Logging.Database.Write("FlashHandlerError", "There was an unexpected exception handling flash content:" + ex.Message + ", stack:" + ex.StackTrace);

            context.Response.Clear();
            context.Response.StatusCode = 500;
            context.Response.StatusDescription = ex.Message;
            context.Response.Flush();
            context.Response.End();
        }

        private void FileNotFoundResponse(System.Web.HttpContext context, string errorMessage)
        {
            context.Response.Clear();
            context.Response.StatusCode = 404;
            context.Response.Flush();
            context.Response.SuppressContent = true;
            context.Response.End();
        }
    }
}
