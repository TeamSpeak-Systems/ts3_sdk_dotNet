using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TeamSpeak.Sdk.Client
{
    /// <summary>
    /// Represents a playing wave file
    /// </summary>
    public sealed class WaveHandle
    {
        /// <summary>
        /// ID of the WaveHandle
        /// </summary>
        public ulong ID { get; }
     
        /// <summary>
        /// Server Connection
        /// </summary>
        public Connection Connection { get; }

        private bool _Paused = false;
        /// <summary>
        /// Is the playback of the wave-file paused
        /// </summary>
        public bool Paused
        {
            get { return _Paused; }
            set
            {
                Library.Api.PauseWaveFileHandle(this, value);
                _Paused = value;
            }
        }

        /// <summary>
        /// Creates a new WaveHandle-Object.
        /// </summary>
        /// <remarks>does not spawn a new client on the server. To spawn a new client use another <see cref="Sdk.Client.Connection"/></remarks>
        /// <param name="connection">the server connection</param>
        /// <param name="id">ID of the client</param>
        public WaveHandle(Connection connection, ulong id)
        {
            Require.NotNull(nameof(connection), connection);
            ID = id;
            Connection = connection;
        }

        /// <summary>
        /// Stops the playback of the wave file.
        /// </summary>
        public void Close()
        {
            Library.Api.CloseWaveFileHandle(this);
        }

        /// <summary>
        /// Set 3D position.
        /// </summary>
        /// <param name="position">The 3D position of the sound.</param>
        public void Set3DAttributes(Vector position)
        {
            Library.Api.Set3DWaveAttributes(this, position);
        }

        /// <summary>
        /// Compares two <see cref="WaveHandle"/> for equality. 
        /// </summary>
        /// <param name="a">The first <see cref="WaveHandle"/> structure to compare.</param>
        /// <param name="b">The second <see cref="WaveHandle"/> structure to compare.</param>
        /// <returns>true if the ID and Mode of a and b are equal; otherwise, false.</returns>
        public static bool operator ==(WaveHandle a, WaveHandle b)
        {
            if (object.ReferenceEquals(a, b)) return true;
            if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null)) return false;
            return a.Connection == b.Connection && a.ID == b.ID;
        }

        /// <summary>
        /// Compares two <see cref="WaveHandle"/> for inequality. 
        /// </summary>
        /// <param name="a">The first <see cref="WaveHandle"/> structure to compare.</param>
        /// <param name="b">The second <see cref="WaveHandle"/> structure to compare.</param>
        /// <returns>true if the ID and Mode of a and b are different; otherwise, false.</returns>
        public static bool operator !=(WaveHandle a, WaveHandle b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Indicates whether this instance and a another instance  are equal.
        /// </summary>
        /// <param name="other">Another instance to compare to.</param>
        /// <returns>true if this and  the other instance represent the same value; otherwise, false.</returns>
        public bool Equals(WaveHandle other)
        {
            return this == other;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>true if obj and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return obj is WaveHandle && this == (WaveHandle)obj;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return Connection.ID.GetHashCode() * 7 + ID.GetHashCode();
        }
    }
}