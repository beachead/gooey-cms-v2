using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Handler;
using Gooeycms.Business.Storage;
using System.IO;
using Gooeycms.Business.Web;
using Gooeycms.Business.Util;
using System.Web;

namespace Gooeycms.Business.Content
{
    public class DirectFileDownloadHandler : BaseHttpHandler
    {
        protected override void Process(System.Web.HttpContext context)
        {
            String container = CurrentSite.GetContainerUrl(SiteHelper.UploadContainerKey);
            String directory = StorageClientConst.RootFolder;
            String filename = context.Request.QueryString["filename"];

            StorageFile file = FindFile(container, directory, filename);
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
                        IStorageClient client = StorageHelper.GetStorageClient();
                        context.Response.Clear();
                        context.Response.ClearHeaders();
                        context.Response.ClearContent();
                        context.Response.Cache.SetLastModified(file.LastModified);
                        context.Response.ContentType = GetContentType(filename);
                        context.Response.AddHeader("Content-Length", file.Size.ToString());
                        context.Response.Cache.SetCacheability(HttpCacheability.Public);
                        client.DownloadToStream(context.Response.OutputStream, container, directory, file.Filename);
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

        private void CacheValidResponse(System.Web.HttpContext context, StorageFile file)
        {
            context.Response.Clear();
            context.Response.ClearHeaders();
            context.Response.ClearContent();

            context.Response.StatusCode = 304;
            context.Response.SuppressContent = true;
        }

        private StorageFile FindFile(string container, string directory, string filename)
        {
            //First check the actual directory that the file is supposedly in
            IStorageClient client = StorageHelper.GetStorageClient();
            StorageFile file = client.GetInfo(container, directory, filename);

            //If it wasn't in the actual directory, check the root directory instead
            if (!file.Exists())
            {

                int pos = filename.LastIndexOf("/");
                if (pos > -1)
                    filename = filename.Substring(pos + 1);

                file = client.GetInfo(container, directory, filename);
            }

            return file;
        }

        private static String GetContentType(String filename)
        {
            String result;
            FileInfo info = new FileInfo(filename);
            switch (info.Extension)
            {
                case ".pdf":
                    result = "application/pdf";
                    break;
                case ".doc":
                case ".docx":
                    result = "application/msword";
                    break;
                case ".xls":
                    result = "application/vnd.ms-excel";
                    break;
                case ".txt":
                    result = "text/plain";
                    break;
                default:
                    result = "application/octet-stream";
                    break;
            }

            return result;
        }

        private void ServerErrorResponse(System.Web.HttpContext context, Exception ex)
        {
            Logging.Database.Write("DirectFileDownloadException", "There was an unexpected exception handling file content:" + ex.Message + ", stack:" + ex.StackTrace);

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
