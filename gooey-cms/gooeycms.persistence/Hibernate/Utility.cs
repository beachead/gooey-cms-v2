using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Driver;
using System.Reflection;
using System.Data;
using System.Text.RegularExpressions;

namespace Beachead.Persistence.Hibernate
{
    /// <summary>
    /// NHibernate utility class which provides access to different
    /// properties defined within the NHibernate configuration file.
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// Determines if a SQL2000 or greater database is being used
        /// </summary>
        /// <returns></returns>
        public bool IsSql2000Database
        {
            get
            {
                String dialect = this.GetDialect();
                return dialect.Contains("MsSql200");
            }
        }

        /// <summary>
        /// Gets the database name based upon the connection string
        /// </summary>
        public String GetDatabaseName()
        {
            if (!IsSql2000Database)
                throw new PersistenceException("This utility only works with SQL 2000 or greater database instances");

            String result = null;

            String conn = this.GetConnectionString();
            Regex regex = new Regex(@"initial catalog\s*=\s*(\w+);", RegexOptions.IgnoreCase);
            if (regex.IsMatch(conn))
            {
                Match match = regex.Match(conn);
                result = match.Groups[1].Value.Trim();
            }
            return result;
        }

        /// <summary>
        /// Retrieves the default database connection string which can be used to
        /// create a new database within the database instance.
        /// </summary>
        /// <returns>An appropriate connection string</returns>
        public String GetDbDefaultConnectionString()
        {
            String conn = this.GetConnectionString();
            String[] pairs = conn.Split(';');

            StringBuilder output = new StringBuilder();
            Dictionary<String, String> table = new Dictionary<string, string>();
            foreach (String pair in pairs)
            {
                if ( (!pair.ToLower().Contains("initial catalog")) && 
                     (!pair.ToLower().Contains("database")) )
                {
                    output.Append(pair).Append(";");
                }
            }
            output.Append("database=master");

            return output.ToString();
        }

        public IDbConnection GetDbConnection()
        {
            IDriver driver = this.GetDriver();
            IDbConnection conn = driver.CreateConnection();
            if (String.IsNullOrEmpty(this.GetConnectionString()))
                throw new PersistenceException("The connection string has not been configured in the configuration file.");

            conn.ConnectionString = this.GetConnectionString();

            return conn;
        }

        /// <summary>
        /// Returns the dialect currently configured to be used
        /// </summary>
        /// <returns></returns>
        internal String GetDialect()
        {
            return Provider.Configuration.Properties[NHibernate.Cfg.Environment.Dialect];
        }

        /// <summary>
        /// Gets the NHibernate connection string 
        /// </summary>
        /// <returns></returns>
        internal String GetConnectionString()
        {
            return Provider.Configuration.Properties[NHibernate.Cfg.Environment.ConnectionString];
        }

        /// <summary>
        /// Gets the driver which was defined in the NHibernate configuration file
        /// </summary>
        /// <returns>Driver currently being used by NHibernate</returns>
        internal IDriver GetDriver()
        {
            //String driverName = GetPropertyFromSession(typeof(NHibernate.Connection.DriverConnectionProvider), "ConnectionString");
            String driverName = Provider.Configuration.Properties[NHibernate.Cfg.Environment.ConnectionDriver];
            Type driverType = NHibernate.Util.ReflectHelper.ClassForName(driverName);
            return (IDriver)Activator.CreateInstance(driverType);
        }

        SessionProvider Provider
        {
            get { return (SessionProvider)SessionProvider.Instance; }
        }
    }
}
