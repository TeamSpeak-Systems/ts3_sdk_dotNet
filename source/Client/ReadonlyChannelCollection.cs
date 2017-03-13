using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamSpeak.Sdk.Client
{
    /// <summary>
    /// An immutable collection containing <see cref="Channel"/>
    /// </summary>
    public class ReadonlyChannelCollection: ICollection<Channel>, ICollection, IEnumerable<Channel>, IEnumerable
    {
        private readonly ICollection<Channel> Channels;

        /// <summary>
        /// Creates a new <see cref="ReadonlyChannelCollection"/>-Object
        /// </summary>
        /// <param name="channels">the channels in the new collection</param>
        public ReadonlyChannelCollection(ICollection<Channel> channels)
        {
            Channels = channels;
        }

        /// <summary>
        /// Returns a <see cref="Channel"/> with the same name
        /// </summary>
        /// <param name="name">name of the channel</param>
        /// <returns>a <see cref="Channel"/> with a matching name</returns>
        public Channel this[string name]
        {
            get
            {
                return Channels.First(c => c.Name == name);
            }
        }

        /// <summary>
        /// Gets the number of elements actually contained in the <see cref="ReadonlyChannelCollection"/>
        /// </summary>
        public int Count { get { return Channels.Count; } }
        /// <summary>
        /// Determines whether an <see cref="Channel"/> is in the <see cref="ReadonlyChannelCollection"/>
        /// </summary>
        /// <param name="item">The <see cref="Channel"/> to locate in the <see cref="ReadonlyChannelCollection"/></param>
        /// <returns>true if item is found in the <see cref="ReadonlyChannelCollection"/> otherwise, false.</returns>
        public bool Contains(Channel item) { return Channels.Contains(item); }
        void ICollection.CopyTo(Array array, int index) { ((ICollection)Channels).CopyTo(array, index); }
        /// <summary>
        /// Copies the entire <see cref="ReadonlyChannelCollection"/> to a compatible one-dimensional array, starting at the beginning of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from <see cref="ReadonlyChannelCollection"/>.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(Channel[] array, int arrayIndex) { Channels.CopyTo(array, arrayIndex); }
        bool ICollection.IsSynchronized { get { return ((ICollection)Channels).IsSynchronized; } }
        object ICollection.SyncRoot { get { return ((ICollection)Channels).SyncRoot; } }
        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="ReadonlyChannelCollection"/>
        /// </summary>
        /// <returns>A <see cref="IEnumerable{Channel}"/> for the <see cref="ReadonlyChannelCollection"/></returns>
        public IEnumerator<Channel> GetEnumerator() { return ((IEnumerable<Channel>)Channels).GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return Channels.GetEnumerator(); }
        /// <summary>
        /// Gets a value indicating whether the <see cref="ReadonlyChannelCollection"/> is read-only.
        /// </summary>
        public bool IsReadOnly { get { return true; } }
        void ICollection<Channel>.Add(Channel item) { throw new NotSupportedException(); }
        void ICollection<Channel>.Clear() { throw new NotSupportedException(); }
        bool ICollection<Channel>.Remove(Channel item) { throw new NotSupportedException(); }
    }
}
