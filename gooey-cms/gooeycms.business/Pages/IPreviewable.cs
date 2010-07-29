using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Business.Pages
{
    public interface IPreviewable
    {
        /// <summary>
        /// Method which is invoked to save the preview page and then 
        /// returns the url which can be used to view the page
        /// </summary>
        /// <returns></returns>
        String Save();
    }
}
