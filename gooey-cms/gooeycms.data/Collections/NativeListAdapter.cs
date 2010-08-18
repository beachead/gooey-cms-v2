using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Gooeycms.Data.Collections
{
    /// <summary>
    /// The Native List Enumerator offers an enumerator for a class that implements the IEnumerator interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NativeListEnumerator<T> : IEnumerator<T>
    {
        private IEnumerator nativeEnumerator;

        /// <summary>
        /// Initializes a new instance of the NativeListEnumerator class.
        /// </summary>
        /// <param name="nativeEnumeratorParameter">The native enumerator parameter.</param>
        public NativeListEnumerator(IEnumerator nativeEnumeratorParameter)
        {
            nativeEnumerator = nativeEnumeratorParameter;
        }

        ///<summary>
        ///Gets the element in the collection at the current position of the enumerator.
        ///</summary>
        ///
        ///<returns>
        ///The element in the collection at the current position of the enumerator.
        ///</returns>
        ///
        T IEnumerator<T>.Current
        {
            get { return (T)nativeEnumerator.Current; }
        }

        ///<summary>
        ///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        ///</summary>
        ///<filterpriority>2</filterpriority>
        public void Dispose()
        {
            nativeEnumerator = null;
        }

        ///<summary>
        ///Advances the enumerator to the next element of the collection.
        ///</summary>
        ///
        ///<returns>
        ///true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
        ///</returns>
        ///
        ///<exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception><filterpriority>2</filterpriority>
        public bool MoveNext()
        {
            return nativeEnumerator.MoveNext();
        }

        ///<summary>
        ///Sets the enumerator to its initial position, which is before the first element in the collection.
        ///</summary>
        ///
        ///<exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception><filterpriority>2</filterpriority>
        public void Reset()
        {
            nativeEnumerator.Reset();
        }

        /// <summary>
        /// Gets the current element in the collection.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The current element in the collection.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element. </exception>
        public object Current
        {
            get { return nativeEnumerator.Current; }
        }
    }
}
