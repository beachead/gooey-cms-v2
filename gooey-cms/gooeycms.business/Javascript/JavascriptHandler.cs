using System;
using System.Text;
using System.Web;
using Gooeycms.Business.Handler;
using Gooeycms.Business.Pages;
using Gooeycms.Business.Util;
using Gooeycms.Data.Model.Theme;

namespace Gooeycms.Business.Javascript
{
    public class JavascriptHandler : BaseHttpHandler
    {
        //TODO Implement server-side caching
        //TODO Implement client-side caching (if-modified-since)
        protected override void Process(System.Web.HttpContext context)
        {
            JavascriptFile file = null;
            HttpResponse response = context.Response;
            Exception ex = null;

            if (context.Request.QueryString["file"] != null)
            {
                String filename = context.Request.QueryString["file"];
                CmsTheme theme = CurrentSite.GetCurrentTheme();

                try
                {
                    file = JavascriptManager.Instance.Get(theme, filename);
                }
                catch (PageNotFoundException) { }
                catch (Exception e)
                {
                    ex = e;
                }
            }

            if (ex != null)
            {
                context.Response.Clear();
                context.Response.StatusCode = 500;
                context.Response.Status = "Unexpected exception occurred while retrieving javascript content.";
                context.Response.StatusDescription = ex.Message;
                context.Response.Flush();
                context.Response.End();
            }
            else if (file != null)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(file.Content);
                context.Response.BufferOutput = true;
                context.Response.Clear();
                context.Response.ClearHeaders();
                context.Response.ClearContent();
                context.Response.ContentType = "text/javascript";
                context.Response.Expires = (int)TimeSpan.FromDays(1).TotalMinutes;
                context.Response.Cache.SetCacheability(HttpCacheability.Public);
                context.Response.Cache.SetMaxAge(TimeSpan.FromDays(1));
                context.Response.OutputStream.Write(bytes, 0, bytes.Length);
                context.Response.End();
            }
            else
            {
                context.Response.Clear();
                context.Response.StatusCode = 404;
                context.Response.Flush();
                context.Response.SuppressContent = true;
                context.Response.End();
            }
        }
    }
}
