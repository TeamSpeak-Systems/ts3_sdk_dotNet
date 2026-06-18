using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamSpeak.Sdk.Client
{
    /// <summary>
    /// Encode Config
    /// </summary>
    public class EncodeConfig
    {
#pragma warning disable 1591
        public string Name
        {
            get { return Library.Api.GetEncodeConfigValue(Connection, "name"); }
        }

        public int Quality
        {
            get { return GetInt("quality"); }
        }

        public int Bitrate
        {
            get { return GetInt("bitrate"); }
        }

        private int GetInt(string ident)
        {
            string value = Library.Api.GetEncodeConfigValue(Connection, ident);
            if (int.TryParse(value, out int result) == false)
                throw new TeamSpeakException(Error.Undefined, null);
            return result;
        }

        public Connection Connection { get; }

        public EncodeConfig(Connection connection)
        {
            Sdk.Require.NotNull(nameof(connection), connection);
            Connection = connection;
        }
#pragma warning restore
    }
}
