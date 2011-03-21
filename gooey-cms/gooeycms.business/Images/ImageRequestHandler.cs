using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Handler;
using Gooeycms.Business.Util;
using Gooeycms.Business.Storage;
using System.IO;
using System.Web;
using Gooeycms.Business.Web;

namespace Gooeycms.Business.Images
{
    public class ImageRequestHandler : BaseHttpHandler
    {
        protected override void Process(System.Web.HttpContext context)
        {
            String container = CurrentSite.GetContainerUrl(SiteHelper.ImagesContainerKey);
            String directory = StorageClientConst.RootFolder;
            String filename = context.Request.QueryString["filename"];

            int pos = filename.LastIndexOf("/");
            if (pos > -1)
                filename = filename.Substring(pos + 1);

            IStorageClient client = StorageHelper.GetStorageClient();
            StorageFile file = client.GetInfo(container, directory, filename);

            if (file.Exists())
            {
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
                        context.Response.ContentType = GetContentType(filename);
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
            else
            {
                FileNotFoundResponse(context, "404:" + filename + " does not exist");
            }
        }

        private static String GetContentType(String filename)
        {
            String result = "image/gif";
            FileInfo info = new FileInfo(filename);
            switch(info.Extension)
            {
                case "jpg":
                case "jpeg":
                    result = "image/jpeg";
                    break;
                case "gif":
                    result = "image/gif";
                    break;
                case "png":
                    result = "image/png";
                    break;
                default:
                    result = "image/gif";
                    break;
            }

            return result;
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
            Logging.Database.Write("ImageHandlerError", "There was an unexpected exception handling flash content:" + ex.Message + ", stack:" + ex.StackTrace);

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
