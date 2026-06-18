using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamSpeak.Sdk.Client
{
    internal static class Require
    {
        public static void SameConnection(string name, Connection connection, Channel channel)
        {
            if (channel != null && channel.Connection != connection)
                throw new ArgumentException("Channel is bound to another ServerConnection.", name);
        }

        public static void SameConnection(string name, Connection connection, Channel[] channelArray)
        {
            if (channelArray != null)
            {
                for (int i = 0; i < channelArray.Length; i++)
                {
                    if (channelArray[i] != null && channelArray[i].Connection != connection)
                        throw new ArgumentException("Channel is bound to another ServerConnection.", Sdk.Require.GetIndexedName(name, i));
                }
            }
        }

        public static void SameConnection(string name, Connection connection, Client client)
        {
            if (client != null && client.Connection != connection)
                throw new ArgumentException("Client is bound to another ServerConnection.", name);
        }

        public static void SameConnection(string name, Connection connection, Client[] clientArray)
        {
            if (clientArray != null)
            {
                for (int i = 0; i < clientArray.Length; i++)
                {
                    if (clientArray[i] != null && clientArray[i].Connection != connection)
                        throw new ArgumentException("Client is bound to another ServerConnection.", Sdk.Require.GetIndexedName(name, i));
                }
            }
        }
    }
}
