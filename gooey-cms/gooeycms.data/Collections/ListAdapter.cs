using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Gooeycms.Data.Collections
{
    /// <summary>
    /// The ListAdapter implements the generic IEnumerable interface in order to allow a 
    /// generic list instantiation of a List that implements the IList interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListAdapter<T> : IEnumerable<T>
    {
        private IList nativeList;

        /// <summary>
        /// Initializes a new instance of the ListAdapter class.
        /// </summary>
        /// <param name="nativeListParameter">The native list parameter.</param>
        public ListAdapter(IList nativeListParameter)
        {
            nativeList = nativeListParameter;
        }

        ///<summary>
        ///Returns an enumerator that iterates through the collection.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
        ///</returns>
        ///<filterpriority>1</filterpriority>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new NativeListEnumerator<T>(GetEnumerator());
        }

        ///<summary>
        ///Returns an enumerator that iterates through a collection.
        ///</summary>
        ///
        ///<returns>
        ///An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public IEnumerator GetEnumerator()
        {
            return nativeList.GetEnumerator();
        }
    }
}
