using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gooeycms.Business.Web.Microsoft
{
    /// <summary>
    /// Helper class for the GridView control
    /// </summary>
    public static class GridViewHelper
    {
        /// <summary>
        /// Finds a control within the given gridview or gridview row
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gridview"></param>
        /// <param name="controlToFind"></param>
        /// <returns></returns>
        public static T FindControl<T>(Object sender, String controlToFind) where T : class
        {
            Control control = (Control)sender;
            GridViewRow row = (GridViewRow)control.NamingContainer;

            return row.FindControl(controlToFind) as T;
        }
    }
}
