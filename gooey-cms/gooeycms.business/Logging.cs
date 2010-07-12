using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Gooeycms.Business
{
    public static class Logging
    {
        public static void Info(String msg)
        {
            if (IsEnabled)
                System.Diagnostics.Trace.TraceInformation(msg);
        }

        public static void Warn(String msg)
        {
            if (IsEnabled)
                System.Diagnostics.Trace.TraceWarning(msg);
        }

        public static void Error(String msg, Exception ex)
        {
            if (IsEnabled)
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(ex);
                System.Diagnostics.Trace.TraceError(msg + "\r\n" + trace.ToString());
            }
        }

        public static bool IsEnabled
        {
            get { return GooeyConfigManager.IsLoggingEnabled; }
        }
    }
}
