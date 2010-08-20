using System;
using System.Text;
using System.Web;
using Gooeycms.Business.Handler;
using Gooeycms.Business.Pages;
using Gooeycms.Business.Util;
using Gooeycms.Data.Model.Theme;

namespace Gooeycms.Business.Css
{
    public class CssHandler : BaseHttpHandler
    {
        protected override void Process(System.Web.HttpContext context)
        {
            CssFile file = null;
            HttpResponse response = context.Response;
            Exception ex = null;

            HttpRequest request = context.Request;
            String type = request.QueryString["type"];
            String key = request.QueryString["key"];
            String filename = request.QueryString["file"];
            if (filename != null)
            {
                try
                {
                    file = CssManager.Instance.Get(key, filename);
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
                context.Response.Status = "Unexpected exception occurred while retrieving stylesheet content.";
                context.Response.StatusDescription = ex.Message;
                context.Response.Flush();
                context.Response.End();
            }
            else if (file != null)
            {
                String directory = null;
                if ("themes".EqualsCaseInsensitive(type))
                    directory = CurrentSite.GetCurrentTheme().ThemeGuid;

                String content = CssManager.Resolve(file.Content, directory);
                byte[] bytes = Encoding.UTF8.GetBytes(content);
                context.Response.BufferOutput = true;
                context.Response.Clear();
                context.Response.ClearHeaders();
                context.Response.ClearContent();
                context.Response.ContentType = "text/css";
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
