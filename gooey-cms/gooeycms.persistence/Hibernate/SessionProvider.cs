using System.Runtime.Remoting.Messaging;

namespace Beachead.Persistence.Hibernate
{
    /// <summary>
    /// Provides access to an opened NHibernate session.
    /// </summary>
    public class SessionProvider : ISessionProvider
    {
        //shared with the Utility class
        private NHibernate.ISessionFactory sessionFactory;
        private NHibernate.Cfg.Configuration cfg;

        #region Thread-safe, lazy Singleton

        /// <summary>
        /// This is a thread-safe, lazy singleton.  See http://www.yoda.arachsys.com/csharp/singleton.html
        /// for more details about its implementation.
        /// </summary>
        public static ISessionProvider Instance
        {
            get
            {
                return Nested.hibernateSessionManager;
            }
        }

        /// <summary>
        /// Initializes the NHibernate session factory upon instantiation.
        /// </summary>
        private SessionProvider()
        {
            InitSessionFactory();
        }

        /// <summary>
        /// Assists with ensuring thread-safe, lazy singleton
        /// </summary>
        private class Nested
        {
            static Nested() { }
            internal static readonly SessionProvider hibernateSessionManager = new SessionProvider();
        }

        private NHibernate.ISession SessionInThread
        {
            get { return (NHibernate.ISession)CallContext.GetData("NHIBERNATE_SESSION"); }
            set { CallContext.SetData("NHIBERNATE_SESSION", value); }
        }

        #endregion

        /// <summary>
        /// Initializes the session factory and ensure that the
        /// configuration file exists.
        /// </summary>
        private void InitSessionFactory()
        {
            //expect the hibernate.cfg.xml file to be in the application folder
            NHibernate.Cfg.Configuration temp = new NHibernate.Cfg.Configuration();

            cfg = temp.Configure();
            cfg.AddAssembly("gooeycms.data");

            //build the session factory based upon the configuration
            sessionFactory = cfg.BuildSessionFactory();
        }

        /// <summary>
        /// Gets the existing NHibernate session or creates a new one if it doesn't exist
        /// </summary>
        /// <returns>NHibernate session</returns>
        public NHibernate.ISession GetOpenSession()
        {
            return GetOpenSession(null);
        }

        /// <summary>
        /// Gets an open session if it exists, or creates a new one with the specified interceptor.
        /// </summary>
        /// <param name="interceptor">The interceptor to associate with the session</param>
        /// <returns>NHibernate session</returns>
        public NHibernate.ISession GetOpenSession(NHibernate.IInterceptor interceptor)
        {
            NHibernate.ISession session = this.SessionInThread;
            if (session == null)
            {
                if (interceptor != null)
                {
                    session = this.sessionFactory.OpenSession(interceptor);
                }
                else
                {
                    session = this.sessionFactory.OpenSession();
                }
                this.SessionInThread = session;
            }

            return session;
        }

        public NHibernate.Cfg.Configuration Configuration
        {
            get { return this.cfg; }
        }

        /// <summary>
        /// Returns whether the session is currently open
        /// </summary>
        public bool IsOpen
        {
            get { return this.SessionInThread.IsOpen; }
        }

        public void Flush()
        {
            if (this.SessionInThread != null)
                this.SessionInThread.Flush();
        }

        /// <summary>
        /// Closes the session provider and cleans everything up
        /// </summary>
        public void Close()
        {
            if (this.SessionInThread != null)
            {
                //flush anything to the datastore
                this.SessionInThread.Flush();

                //if the session is open, close it
                if (this.SessionInThread.IsOpen)
                    this.SessionInThread.Close();
                
                this.SessionInThread.Dispose();
                this.SessionInThread = null;
            }
        }
    }
}
