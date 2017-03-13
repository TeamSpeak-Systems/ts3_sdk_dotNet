using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TeamSpeak.Sdk.Client
{
    /// <summary>
    /// A device that can either playback or capture sound.
    /// </summary>
    public interface ISoundDevice
    {
        /// <summary>
        /// ID of the device.
        /// </summary>
        string ID { get; }
        /// <summary>
        /// The soundbackend.
        /// </summary>
        string Mode { get; }
        /// <summary>
        /// Human-readable name of the device.
        /// </summary>
        string Name { get; }
    }

    /// <summary>
    /// A device that can either playback or capture sound
    /// </summary>
    public struct SoundDevice: ISoundDevice
    {
        /// <summary>
        /// ID of the device.
        /// </summary>
        public string ID { get; }
        /// <summary>
        /// The soundbackend.
        /// </summary>
        public string Mode { get; }
        /// <summary>
        /// Human-readable name of the device.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Creates a new <see cref="SoundDevice"/>
        /// </summary>
        /// <param name="mode">the soundbackend</param>
        /// <param name="id">id of the device</param>
        /// <param name="name">human-readable name</param>
        /// <remarks>To get available SoundDevices use <see cref="Library.GetPlaybackDevices(string)"/> and <see cref="Library.GetCaptureDevices(string)"/></remarks>
        public SoundDevice(string mode, string id, string name)
        {
            ID = id;
            Mode = mode;
            Name = name;
        }

        /// <summary>
        /// Compares two <see cref="SoundDevice"/> structures for equality. 
        /// </summary>
        /// <param name="a">The first <see cref="SoundDevice"/> structure to compare.</param>
        /// <param name="b">The second <see cref="SoundDevice"/> structure to compare.</param>
        /// <returns>true if the ID and Mode of a and b are equal; otherwise, false.</returns>
        public static bool operator == (SoundDevice a, SoundDevice b)
        {
            return a.ID == b.ID && a.Mode == b.Mode;
        }
        /// <summary>
        /// Compares two <see cref="SoundDevice"/> structures for inequality. 
        /// </summary>
        /// <param name="a">The first <see cref="SoundDevice"/> structure to compare.</param>
        /// <param name="b">The second <see cref="SoundDevice"/> structure to compare.</param>
        /// <returns>true if the ID and Mode of a and b are different; otherwise, false.</returns>
        public static bool operator != (SoundDevice a, SoundDevice b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>true if obj and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return obj is SoundDevice && this == (SoundDevice)obj;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return ID.GetHashCode() * 7 + Mode.GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            string result = Name;
            if (string.IsNullOrEmpty(result) == false)
                return result;
            else return ID;
        }
    }

    /// <summary>
    /// A custom device usable for playback and/or capture
    /// </summary>
    public class CustomDevice: ISoundDevice, IDisposable
    {
        internal const string ModeName = "custom";

        /// <summary>
        /// ID of the device.
        /// </summary>
        public string ID { get; }
        
        /// <summary>
        /// The soundbackend.
        /// </summary>
        public string Mode { get { return ModeName; } }
        
        /// <summary>
        /// Human-readable name of the device.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Frequency of the capture device.
        /// </summary>
        public SamplingRate CaptureRate { get; }

        /// <summary>
        /// Number of channels of the capture device.
        /// </summary>
        public int CaptureChannels { get; }

        /// <summary>
        /// Frequency of the playback device.
        /// </summary>
        public SamplingRate PlaybackRate { get; }

        /// <summary>
        /// Number of channels of the playback device.
        /// </summary>
        public int PlaybackChannels { get; }

        private readonly NativeMethods api;

        /// <summary>
        /// Creates a new CustomDevice
        /// </summary>
        /// <param name="name">human-readable name</param>
        /// <param name="captureRate">sampling rate of the capture</param>
        /// <param name="captureChannels">amount of channels to capture, can be 1 or 2</param>
        /// <param name="playbackRate">sampling rate of the playback</param>
        /// <param name="playbackChannels">amount of channels of the playback, can be 1 or 2</param>
        public CustomDevice(string name, SamplingRate captureRate, int captureChannels, SamplingRate playbackRate, int playbackChannels)
        {
            ID = Guid.NewGuid().ToString("N");
            Name = name;
            PlaybackRate = playbackRate;
            PlaybackChannels = playbackChannels;
            CaptureRate = captureRate;
            CaptureChannels = captureChannels;
            api = Library.Api;
            api.RegisterCustomDevice(ID, Name, (int)CaptureRate, CaptureChannels, (int)PlaybackRate, PlaybackChannels);
        }

        /// <summary>
        /// Retrieve playback data from the Library
        /// </summary>
        /// <param name="buffer">Buffer containing the playback data retrieved from the Library.</param>
        /// <param name="samples">The number of samples to acquire</param>
        /// <returns>true if playback data is available; otherwise, false.</returns>
        public bool AcquireData(short[] buffer, int samples)
        {
            Require.NotNull(nameof(buffer), buffer);
            Require.ValidRange(nameof(buffer), nameof(samples), buffer, samples);
            return Library.Api.AcquireCustomPlaybackData(ID, buffer, samples);
        }

        /// <summary>
        /// Send the samples from your device to the Library
        /// </summary>
        /// <param name="buffer">sample buffer containing the data to be send</param>
        /// <param name="samples">The number of samples to send</param>
        public void ProcessData(short[] buffer, int samples)
        {
            Require.NotNull(nameof(buffer), buffer);
            Require.ValidRange(nameof(buffer), nameof(samples), buffer, samples);
            Library.Api.ProcessCustomCaptureData(ID, buffer, samples);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            string result = Name;
            if (string.IsNullOrEmpty(result) == false)
                return result;
            else return base.ToString();
        }

        /// <summary>
        /// Releases all resources used.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Allows CustomDevice to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.
        /// </summary>
        ~CustomDevice()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the CustomDevice and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposed">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposed)
        {
            try
            {
                api.TryUnregisterCustomDevice(ID);
            }
            catch (ObjectDisposedException)
            {
                // Library might be already be disposed
            }
        }
    }
}