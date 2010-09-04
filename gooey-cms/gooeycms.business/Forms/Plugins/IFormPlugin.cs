using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Business.Forms.Plugins
{
    public interface IFormPlugin
    {
        /// <summary>
        /// Determines if this plugiin is enbabled or not
        /// </summary>
        /// <returns></returns>
        Boolean IsEnabled();

        /// <summary>
        /// Determines if exceptions are fatal to the entire form post or only this particular plugin.
        /// If this is set to true and this plugin throws an exception, no further plugins will run.
        /// </summary>
        /// <returns></returns>
        Boolean IsExceptionFatal();

        /// <summary>
        /// Sets the form fields that were posted from the form post
        /// </summary>
        Dictionary<String, String> FormFields { set; }

        /// <summary>
        /// Processes the data with the specified set of fields
        /// </summary>
        void Process();
    }
}
