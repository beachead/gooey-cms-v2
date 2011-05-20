using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beachead.Persistence.Hibernate
{
    /// <summary>
    /// NHibernate transaction object that is responsible for committing or rolling
    /// back transactions from the datastore.
    /// </summary>
    public class Transaction : ITransaction
    {
        NHibernate.ITransaction tx;
        bool isOriginator = true;

        public Transaction()
        {
            NHibernate.ISession session = SessionProvider.Instance.GetOpenSession();
                this.tx = session.Transaction;

            //Check if the transaction is already active, if so, the method that opened it
            //needs to close it.
            if (this.tx.IsActive)
                this.isOriginator = false;
            else
                this.tx.Begin();
        }

        /// <summary>
        /// Attaches to an existing transaction bute doesn't create one otherwise
        /// </summary>
        /// <param name="attachToExistingOnly"></param>
        public Transaction(bool attachToExistingOnly)
        {
            NHibernate.ISession session = SessionProvider.Instance.GetOpenSession();
            this.tx = session.Transaction;

            //Check if the transaction is already active, if so, the method that opened it
            //needs to close it.
            if (this.tx.IsActive)
                this.isOriginator = false;
        }

        /// <summary>
        /// Commits the NHibernate transaction to the datastore
        /// </summary>
        public void Commit()
        {
            if ((this.isOriginator) && (!this.tx.WasCommitted) && (!this.tx.WasRolledBack))
                this.tx.Commit();

            SessionProvider.Instance.Close();
            SessionProvider.Instance.GetOpenSession();
        }

        /// <summary>
        /// Rollsback the transaction and undos any changes to the datastore.
        /// </summary>
        public void Rollback()
        {
            if ((!this.tx.WasCommitted) && (!this.tx.WasRolledBack))
                this.tx.Rollback();
        }

        public bool WasRolledBack()
        {
            return (this.tx.WasRolledBack);
        }

        public bool IsActive()
        {
            return (this.tx.IsActive);
        }

        /// <summary>
        /// If the class goes out of scope before commit was called
        /// rollback the transaction and clean everything up.
        /// </summary>
        void IDisposable.Dispose()
        {
            this.Rollback();
            if (this.tx != null)
                this.tx.Dispose();
        }
    }
}
