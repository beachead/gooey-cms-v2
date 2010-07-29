using System;

namespace Gooeycms.Business.Javascript
{
    public class PageNotFoundException : Exception
    {
        public PageNotFoundException()
        {
        }

        public PageNotFoundException(String page) :
            base("The following page does not exist or has not been approved:" + page)
        {
        }
    }
}
