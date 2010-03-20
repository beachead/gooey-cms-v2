using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beachead.Persistence
{
    /// <summary>
    /// Interface that all transaction classes should implement.
    /// <br /><br /> 
    /// Handles the starting and stopping of datastore transactions.
    /// </summary>
    public interface ITransaction : IDisposable
    {
        /// <summary>
        /// Commits the transaction to the datastore
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollsback the transaction and does not commit anything to the datastore.
        /// </summary>
        void Rollback();
    }
}
