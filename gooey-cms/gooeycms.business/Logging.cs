using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Beachead.Persistence.Hibernate;

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

        public static class Database
        {
            public static void Write(String eventType, String message)
            {
                Gooeycms.Data.Model.Logging log = new Data.Model.Logging();
                log.Inserted = DateTime.Now;
                log.EventType = eventType;
                log.EventMessage = message;

                Gooeycms.Data.Model.LoggingDao dao = new Data.Model.LoggingDao();
                using (Transaction tx = new Transaction())
                {
                    dao.Save(log);
                    tx.Commit();
                }
            }
        }

        internal static string FormatException(Exception ex)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Source: " + ex.Source);
            builder.AppendLine("Message: " + ex.Message);
            builder.AppendLine("Stack: " + ex.StackTrace);

            return builder.ToString();
        }
    }
}
