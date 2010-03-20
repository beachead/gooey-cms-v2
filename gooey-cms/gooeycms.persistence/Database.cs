using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Gooeycms.Persistence
{
    public static class Database
    {
        /// <summary>
        /// Gets a new database connection based upon the web.config.
        /// The connection will automatically be opened, but must be explicitly closed.
        /// </summary>
        /// <returns></returns>
        public static SqlConnection GetNewConnectionFromConfig()
        {
            String mscs = ConfigurationManager.ConnectionStrings["AzureGooeyCmsDb"].ConnectionString;
            SqlConnection connection = new SqlConnection(mscs);
            connection.Open();

            return connection;
        }
    }
}
