using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Beachead.Persistence.Hibernate;

namespace Beachead.Persistence.Hibernate.Handlers
{
    public class HibernateHttpModule : IHttpModule
    {
        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.EndRequest += new EventHandler(CommitAndCloseSession);
        }

        private void CommitAndCloseSession(object sender, EventArgs e)
        {
            Transaction tx = new Transaction();
            if (tx.IsActive())
                tx.Commit();
            
            SessionProvider.Instance.Close();
        }

        #endregion
    }
}
