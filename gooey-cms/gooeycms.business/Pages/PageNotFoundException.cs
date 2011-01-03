using System;

namespace Gooeycms.Business.Pages
{
    public class PageNotFoundException : Exception
    {
        public String NotFoundPath { get; set; }
        public PageNotFoundException()
        {
        }

        public PageNotFoundException(String page) :
            base("The following page does not exist or has not been approved:" + page)
        {
            NotFoundPath = page;
        }
    }
}
