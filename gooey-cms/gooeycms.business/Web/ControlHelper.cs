using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Gooeycms.Business.Web
{
    public static class ControlHelper
    {
        public static Control FindRecursive(Control parent, String id)
        {
            Control temp = null;
            if (parent.ID == id)
            {
                temp = parent;
            }

            foreach (Control c in parent.Controls)
            {
                Control t = FindRecursive(c, id);
                if (t != null)
                {
                    temp = t;
                    break;
                }
            }

            return temp;
        }
    }
}
