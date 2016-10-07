using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TeamSpeak.Sdk.Client
{
    /// <summary>
    /// Parameter of the microphone preprocessor.
    /// </summary>
    public class Preprocessor
    {
        /// <summary>
        /// Enable or disable noise suppression. Enabled by default.
        /// </summary>
        public bool Denoise
        {
            get { return GetBool("denoise"); }
            set { SetBool("denoise", value); }
        }

        /// <summary>
        /// Enable or disable Voice Activity Detection. Enabled by default.
        /// </summary>
        public bool Vad
        {
            get { return GetBool("vad"); }
            set { SetBool("vad", value); }
        }

        /// <summary>
        /// Voice Activity Detection level in decibel. A high voice activation level 
        /// means you have to speak louder into the microphone in order to start transmitting.
        /// Reasonable values range from -50 to 50. Default is 0.
        /// To adjust the VAD level in your client, you can query <see cref="DecibelLastPeriod"/> over a period of time to query.
        /// </summary>
        public float VadLevel
        {
            get { return GetFloat("voiceactivation_level"); }
            set { SetFloat("voiceactivation_level", value); }
        }

        /// <summary>
        /// Voice Activity Detection extra buffer size. Should be 0 to 8, defaults to 2. Lower value means faster transmission, higher value means better VAD quality but higher latency.
        /// </summary>
        public int VadExtraBufferSize
        {
            get { return GetInt("vad_extrabuffersize"); }
            set { SetInt("vad_extrabuffersize", value); }
        }

        /// <summary>
        /// Enable or disable Automatic Gain Control. Enabled by default.
        /// </summary>
        public bool Agc
        {
            get { return GetBool("agc"); }
            set { SetBool("agc", value); }
        }

        /// <summary>
        /// AGC level. Default is 16000.
        /// </summary>
        public float AgcLevel
        {
            get { return GetFloat("agc_level"); }
            set { SetFloat("agc_level", value); }
        }

        /// <summary>
        /// AGC max gain. Default is 30
        /// </summary>
        public int AgcMaxGain
        {
            get { return GetInt("agc_max_gain"); }
            set { SetInt("agc_max_gain", value); }
        }

        /// <summary>
        /// Checks if echo canceling is enabled
        /// </summary>
        public bool EchoCanceling
        {
            get { return GetBool("echo_canceling"); }
            set { SetBool("echo_canceling", value); }
        }

        /// <summary>
        /// the current voice input level
        /// </summary>
        public float DecibelLastPeriod
        {
            get { return Library.Api.GetPreProcessorInfoValueFloat(Connection, "decibel_last_period"); }
        }

        /// <summary>
        /// Server Connection
        /// </summary>
        public Connection Connection { get; }

        /// <summary>
        /// Create a new Preprocessor-Object
        /// </summary>
        /// <param name="connection">the server connection</param>
        public Preprocessor(Connection connection)
        {
            Require.NotNull(nameof(connection), connection);
            Connection = connection;
        }

        private bool GetBool(string ident)
        {
            switch(Library.Api.GetPreProcessorConfigValue(Connection, ident))
            {
                case "true": return true;
                case "false": return false;
                default: throw new TeamSpeakException(Error.Undefined, null);
            }
        }
        private int GetInt(string ident)
        {
            string value = Library.Api.GetPreProcessorConfigValue(Connection, ident);
            int result;
            if (int.TryParse(value, out result) == false)
                throw new TeamSpeakException(Error.Undefined, null);
            return result;
        }
        private float GetFloat(string ident)
        {
            const NumberStyles FloatStyle = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint 
                                          | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingWhite;
            string value = Library.Api.GetPreProcessorConfigValue(Connection, ident);
            float result;
            if (float.TryParse(value, FloatStyle, CultureInfo.InvariantCulture, out result) == false)
                throw new TeamSpeakException(Error.Undefined, null);
            return result;
        }

        private void SetBool(string ident, bool value)
        {
            Library.Api.SetPreProcessorConfigValue(Connection, ident, value ? "true" : "false");
        }
        private void SetInt(string ident, int value)
        {
            Library.Api.SetPreProcessorConfigValue(Connection, ident, value.ToString());
        }
        private void SetFloat(string ident, float value)
        {
            Library.Api.SetPreProcessorConfigValue(Connection, ident, value.ToString("0.0", CultureInfo.InvariantCulture));
        }
    }
}
