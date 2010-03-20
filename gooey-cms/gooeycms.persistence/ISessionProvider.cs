using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beachead.Persistence
{
    /// <summary>
    /// Interface that all of the persistence session providers should implement.
    /// <br /><br />
    /// Create and provides access to the persistence frameworks session provider.
    /// </summary>
    /// <remarks>
    /// In theory, this is an unnecessary abstraction since the session providers can't
    /// really be abstracted away --- however, by using an interface we will ensure that
    /// there are no method specific calls, and so a simple search-replace for the 
    /// SessionProvider's concrete class name will suffice to update the entire baseline.
    /// </remarks>
    public interface ISessionProvider
    {
        /// <summary>
        /// Returns an already opened session or creates a new one
        /// </summary>
        /// <returns>NHibernate Session</returns>
        NHibernate.ISession GetOpenSession();

        /// <summary>
        /// Closes the provider and flushes everything to disk
        /// </summary>
        void Close();

        /// <summary>
        /// Returns whether the session provider's connection is open
        /// </summary>
        bool IsOpen { get; }

        /// <summary>
        /// Forces the provider to save all of the changes to the datastore.
        /// </summary>
        void Flush();
    }
}
