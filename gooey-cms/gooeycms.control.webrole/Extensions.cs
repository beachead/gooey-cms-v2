using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gooeycms.Webrole.Control
{
    public static class Extensions
    {
        public static System.Web.UI.Control FindControlRecursive(this System.Web.UI.Control control, String id)
        {
            if ((control.ID != null) && (control.ID.Equals(id)))
                return control;

            foreach (System.Web.UI.Control inner in control.Controls)
            {
                System.Web.UI.Control found = FindControlRecursive(inner,id);
                if (found != null)
                    return found;
            }

            //Did not find the control, return null
            return null;
        }
    }
}