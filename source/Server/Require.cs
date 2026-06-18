using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamSpeak.Sdk.Server
{
    internal static class Require
    {
        public static void SameVirtualServer(string name, VirtualServer server, Channel channel)
        {
            if (channel != null && channel.VirtualServer != server)
                throw new ArgumentException("Channel is bound to another VirtualServer.", name);
        }

        public static void SameVirtualServer(string name, VirtualServer virtualServer, Channel[] channelArray)
        {
            if (channelArray != null)
            {
                for (int i = 0; i < channelArray.Length; i++)
                {
                    if (channelArray[i] != null && channelArray[i].VirtualServer != virtualServer)
                        throw new ArgumentException("Channel is bound to another VirtualServer.", Sdk.Require.GetIndexedName(name, i));
                }
            }
        }

        public static void SameVirtualServer(string name, VirtualServer virtualServer, Client client)
        {
            if (client != null && client.VirtualServer!= virtualServer)
                throw new ArgumentException("Client is bound to another ServerConnection.", name);
        }

        public static void SameVirtualServer(string name, VirtualServer virtualServer, Client[] clientArray)
        {
            if (clientArray != null)
            {
                for (int i = 0; i < clientArray.Length; i++)
                {
                    if (clientArray[i] != null && clientArray[i].VirtualServer!= virtualServer)
                        throw new ArgumentException("Client is bound to another ServerConnection.", Sdk.Require.GetIndexedName(name, i));
                }
            }
        }
    }
}
