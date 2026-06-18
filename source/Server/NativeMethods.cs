using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TeamSpeak.Sdk.Server
{
    internal class NativeMethods
    {
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error FreeMemoryDelegate(IntPtr pointer);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error InitServerLibDelegate([In]ref ServerLibFunctions functionPointers, int usedLogTypes, UnmanagedPointer logFileFolder, int argc, IntPtr argv);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error EnableFileManagerDelegate(UnmanagedPointer filebase, UnmanagedPointer ips, int port, ulong downloadBandwitdh, ulong uploadBandwidth);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error DestroyServerLibDelegate();
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error DisableClientCommandDelegate(ClientCommand clientCommand);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetServerLibVersionDelegate(out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetServerLibVersionNumberDelegate(out ulong result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error SetLogVerbosityDelegate(int logVerbosity);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetGlobalErrorMessageDelegate(Error globalErrorCode, out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetClientVariableAsIntDelegate(ulong serverID, ushort clientID, IntPtr flag, out int result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetClientVariableAsStringDelegate(ulong serverID, ushort clientID, IntPtr flag, out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error SetClientVariableAsIntDelegate(ulong serverID, ushort clientID, IntPtr flag, int value);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error SetClientVariableAsStringDelegate(ulong serverID, ushort clientID, IntPtr flag, UnmanagedPointer value);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error FlushClientVariableDelegate(ulong serverID, ushort clientID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error SetClientWhisperListDelegate(ulong serverID, ushort clID, UnmanagedPointer channelID, UnmanagedPointer clientID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetClientListDelegate(ulong serverID, out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetChannelOfClientDelegate(ulong serverID, ushort clientID, out ulong result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error ClientMoveDelegate(ulong serverID, ulong newChannelID, UnmanagedPointer clientIDArray);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error ClientsKickFromServerDelegate(ulong serverID, UnmanagedPointer clientIDArray, UnmanagedPointer kickReason, int failOnClientError);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetClientIDSfromUIDSDelegate(ulong serverID, UnmanagedPointer clientUIDs, out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetChannelVariableAsIntDelegate(ulong serverID, ulong channelID, IntPtr flag, out int result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetChannelVariableAsStringDelegate(ulong serverID, ulong channelID, IntPtr flag, out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error SetChannelVariableAsIntDelegate(ulong serverID, ulong channelID, IntPtr flag, int value);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error SetChannelVariableAsStringDelegate(ulong serverID, ulong channelID, IntPtr flag, UnmanagedPointer value);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error FlushChannelVariableDelegate(ulong serverID, ulong channelID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error FlushChannelCreationDelegate(ulong serverID, ulong channelParentID, out ulong result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetChannelListDelegate(ulong serverID, out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetChannelClientListDelegate(ulong serverID, ulong channelID, out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetParentChannelOfChannelDelegate(ulong serverID, ulong channelID, out ulong result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error ChannelDeleteDelegate(ulong serverID, ulong channelID, int force);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error ChannelMoveDelegate(ulong serverID, ulong channelID, ulong newChannelParentID, ulong newOrder);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetVirtualServerVariableAsIntDelegate(ulong serverID, IntPtr flag, out int result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetVirtualServerVariableAsUInt64Delegate(ulong serverID, IntPtr flag, out ulong result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetVirtualServerVariableAsStringDelegate(ulong serverID, IntPtr flag, out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error SetVirtualServerVariableAsIntDelegate(ulong serverID, IntPtr flag, int value);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error SetVirtualServerVariableAsUInt64Delegate(ulong serverID, IntPtr flag, ulong value);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error SetVirtualServerVariableAsStringDelegate(ulong serverID, IntPtr flag, UnmanagedPointer value);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error FlushVirtualServerVariableDelegate(ulong serverID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetVirtualServerConnectionVariableAsUInt64Delegate(ulong serverID, IntPtr flag, out ulong result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetVirtualServerConnectionVariableAsDoubleDelegate(ulong serverID, IntPtr flag, out double result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetVirtualServerListDelegate(out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error StopVirtualServerDelegate(ulong serverID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error CreateVirtualServerDelegate(uint serverPort, UnmanagedPointer serverIp, UnmanagedPointer serverName, UnmanagedPointer serverKeyPair, uint serverMaxClients, out ulong result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetVirtualServerKeyPairDelegate(ulong serverID, out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error CreateSecuritySaltDelegate(int options, UnmanagedPointer salt, int saltByteSize, out IntPtr securitySalt);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error CalculateSecurityHashDelegate(UnmanagedPointer securitySalt, UnmanagedPointer clientUniqueIdentifier, UnmanagedPointer clientNickName, UnmanagedPointer clientMetaData, out IntPtr securityHash);

        readonly FreeMemoryDelegate _FreeMemory;
        readonly InitServerLibDelegate _InitServerLib;
        readonly EnableFileManagerDelegate _EnableFileManager;
        //readonly DestroyServerLibDelegate _DestroyServerLib;
        readonly DisableClientCommandDelegate _DisableClientCommand;
        readonly GetServerLibVersionDelegate _GetServerLibVersion;
        readonly GetServerLibVersionNumberDelegate _GetServerLibVersionNumber;
        readonly SetLogVerbosityDelegate _SetLogVerbosity;
        readonly GetGlobalErrorMessageDelegate _GetGlobalErrorMessage;
        readonly GetClientVariableAsIntDelegate _GetClientVariableAsInt;
        readonly GetClientVariableAsStringDelegate _GetClientVariableAsString;
        readonly SetClientVariableAsIntDelegate _SetClientVariableAsInt;
        readonly SetClientVariableAsStringDelegate _SetClientVariableAsString;
        readonly FlushClientVariableDelegate _FlushClientVariable;
        readonly SetClientWhisperListDelegate _SetClientWhisperList;
        readonly GetClientListDelegate _GetClientList;
        readonly GetChannelOfClientDelegate _GetChannelOfClient;
        readonly ClientMoveDelegate _ClientMove;
        readonly ClientsKickFromServerDelegate _ClientsKickFromServer;
        readonly GetClientIDSfromUIDSDelegate _GetClientIDSfromUIDS;
        readonly GetChannelVariableAsIntDelegate _GetChannelVariableAsInt;
        readonly GetChannelVariableAsStringDelegate _GetChannelVariableAsString;
        readonly SetChannelVariableAsIntDelegate _SetChannelVariableAsInt;
        readonly SetChannelVariableAsStringDelegate _SetChannelVariableAsString;
        readonly FlushChannelVariableDelegate _FlushChannelVariable;
        readonly FlushChannelCreationDelegate _FlushChannelCreation;
        readonly GetChannelListDelegate _GetChannelList;
        readonly GetChannelClientListDelegate _GetChannelClientList;
        readonly GetParentChannelOfChannelDelegate _GetParentChannelOfChannel;
        readonly ChannelDeleteDelegate _ChannelDelete;
        readonly ChannelMoveDelegate _ChannelMove;
        readonly GetVirtualServerVariableAsIntDelegate _GetVirtualServerVariableAsInt;
        readonly GetVirtualServerVariableAsUInt64Delegate _GetVirtualServerVariableAsUInt64;
        readonly GetVirtualServerVariableAsStringDelegate _GetVirtualServerVariableAsString;
        readonly SetVirtualServerVariableAsIntDelegate _SetVirtualServerVariableAsInt;
        readonly SetVirtualServerVariableAsUInt64Delegate _SetVirtualServerVariableAsUInt64;
        readonly SetVirtualServerVariableAsStringDelegate _SetVirtualServerVariableAsString;
        readonly FlushVirtualServerVariableDelegate _FlushVirtualServerVariable;
        readonly GetVirtualServerConnectionVariableAsUInt64Delegate _GetVirtualServerConnectionVariableAsUInt64;
        readonly GetVirtualServerConnectionVariableAsDoubleDelegate _GetVirtualServerConnectionVariableAsDouble;
        readonly GetVirtualServerListDelegate _GetVirtualServerList;
        readonly StopVirtualServerDelegate _StopVirtualServer;
        readonly CreateVirtualServerDelegate _CreateVirtualServer;
        readonly GetVirtualServerKeyPairDelegate _GetVirtualServerKeyPair;
        //readonly CreateSecuritySaltDelegate _CreateSecuritySalt;
        //readonly CalculateSecurityHashDelegate _CalculateSecurityHash;

        readonly SdkHandle Handle;

        public NativeMethods(SdkHandle handle)
        {
            Handle = handle;
            handle.GetLibraryMethod("ts3server_freeMemory", out _FreeMemory);
            handle.GetLibraryMethod("ts3server_initServerLib", out _InitServerLib);
            handle.GetLibraryMethod("ts3server_enableFileManager", out _EnableFileManager);
            //handle.GetLibraryMethod("ts3server_destroyServerLib", out _DestroyServerLib);
            handle.GetLibraryMethod("ts3server_disableClientCommand", out _DisableClientCommand);
            handle.GetLibraryMethod("ts3server_getServerLibVersion", out _GetServerLibVersion);
            handle.GetLibraryMethod("ts3server_getServerLibVersionNumber", out _GetServerLibVersionNumber);
            handle.GetLibraryMethod("ts3server_setLogVerbosity", out _SetLogVerbosity);
            handle.GetLibraryMethod("ts3server_getGlobalErrorMessage", out _GetGlobalErrorMessage);
            handle.GetLibraryMethod("ts3server_getClientVariableAsInt", out _GetClientVariableAsInt);
            handle.GetLibraryMethod("ts3server_getClientVariableAsString", out _GetClientVariableAsString);
            handle.GetLibraryMethod("ts3server_setClientVariableAsInt", out _SetClientVariableAsInt);
            handle.GetLibraryMethod("ts3server_setClientVariableAsString", out _SetClientVariableAsString);
            handle.GetLibraryMethod("ts3server_flushClientVariable", out _FlushClientVariable);
            handle.GetLibraryMethod("ts3server_setClientWhisperList", out _SetClientWhisperList);
            handle.GetLibraryMethod("ts3server_getClientList", out _GetClientList);
            handle.GetLibraryMethod("ts3server_getChannelOfClient", out _GetChannelOfClient);
            handle.GetLibraryMethod("ts3server_clientMove", out _ClientMove);
            handle.GetLibraryMethod("ts3server_clientsKickFromServer", out _ClientsKickFromServer);
            handle.GetLibraryMethod("ts3server_getClientIDSfromUIDS", out _GetClientIDSfromUIDS);
            handle.GetLibraryMethod("ts3server_getChannelVariableAsInt", out _GetChannelVariableAsInt);
            handle.GetLibraryMethod("ts3server_getChannelVariableAsString", out _GetChannelVariableAsString);
            handle.GetLibraryMethod("ts3server_setChannelVariableAsInt", out _SetChannelVariableAsInt);
            handle.GetLibraryMethod("ts3server_setChannelVariableAsString", out _SetChannelVariableAsString);
            handle.GetLibraryMethod("ts3server_flushChannelVariable", out _FlushChannelVariable);
            handle.GetLibraryMethod("ts3server_flushChannelCreation", out _FlushChannelCreation);
            handle.GetLibraryMethod("ts3server_getChannelList", out _GetChannelList);
            handle.GetLibraryMethod("ts3server_getChannelClientList", out _GetChannelClientList);
            handle.GetLibraryMethod("ts3server_getParentChannelOfChannel", out _GetParentChannelOfChannel);
            handle.GetLibraryMethod("ts3server_channelDelete", out _ChannelDelete);
            handle.GetLibraryMethod("ts3server_channelMove", out _ChannelMove);
            handle.GetLibraryMethod("ts3server_getVirtualServerVariableAsInt", out _GetVirtualServerVariableAsInt);
            handle.GetLibraryMethod("ts3server_getVirtualServerVariableAsUInt64", out _GetVirtualServerVariableAsUInt64);
            handle.GetLibraryMethod("ts3server_getVirtualServerVariableAsString", out _GetVirtualServerVariableAsString);
            handle.GetLibraryMethod("ts3server_setVirtualServerVariableAsInt", out _SetVirtualServerVariableAsInt);
            handle.GetLibraryMethod("ts3server_setVirtualServerVariableAsUInt64", out _SetVirtualServerVariableAsUInt64);
            handle.GetLibraryMethod("ts3server_setVirtualServerVariableAsString", out _SetVirtualServerVariableAsString);
            handle.GetLibraryMethod("ts3server_flushVirtualServerVariable", out _FlushVirtualServerVariable);
            handle.GetLibraryMethod("ts3server_getVirtualServerConnectionVariableAsUInt64", out _GetVirtualServerConnectionVariableAsUInt64);
            handle.GetLibraryMethod("ts3server_getVirtualServerConnectionVariableAsDouble", out _GetVirtualServerConnectionVariableAsDouble);
            handle.GetLibraryMethod("ts3server_getVirtualServerList", out _GetVirtualServerList);
            handle.GetLibraryMethod("ts3server_stopVirtualServer", out _StopVirtualServer);
            handle.GetLibraryMethod("ts3server_createVirtualServer", out _CreateVirtualServer);
            handle.GetLibraryMethod("ts3server_getVirtualServerKeyPair", out _GetVirtualServerKeyPair);
            //handle.GetLibraryMethod("ts3server_createSecuritySalt", out _CreateSecuritySalt);
            //handle.GetLibraryMethod("ts3server_calculateSecurityHash", out _CalculateSecurityHash);
        }

        private SdkLock Lock
        {
            get { return new SdkLock(Handle); }
        }

        private void Check(Error error)
        {
            if (error != Error.Ok && error != Error.OkNoUpdate)
                throw Library.CreateException(error);
        }

        private bool FreeMemory(IntPtr pointer)
        {
            using (Lock) return _FreeMemory(pointer) == Error.Ok;
        }

        private List<T> ReadAndFreeShortIDList<T>(IntPtr pointer, Func<ushort, T> reader)
        {
            if (pointer == IntPtr.Zero) return null;
            List<T> result = Native.ReadShortIDList<T>(pointer, reader);
            FreeMemory(pointer);
            return result;
        }

        private List<T> ReadAndFreeLongIDList<T>(IntPtr pointer, Func<ulong, T> reader)
        {
            if (pointer == IntPtr.Zero) return null;
            List<T> result = Native.ReadLongIDList<T>(pointer, reader);
            FreeMemory(pointer);
            return result;
        }

        private List<T> ReadAndFreePointerList<T>(IntPtr pointer, Func<IntPtr, T> reader)
        {
            if (pointer == IntPtr.Zero) return null;
            List<T> result = Native.ReadPointerList<T>(pointer, reader);
            FreeMemory(pointer);
            return result;
        }

        public static string ReadAndFreeString(IntPtr pointer)
        {
            if (pointer == IntPtr.Zero) return null;
            string result = Native.ReadString(pointer);
            Library.Api.FreeMemory(pointer);
            return result;
        }

        public void InitServerLib(ServerLibFunctions functionPointers, LogTypes usedLogTypes, string logFileFolder)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_logFileFolder = Native.WriteString(logFileFolder))
            {
                // The native server library requires a non-null argv during init, so pass a
                // minimal valid argv containing only the program name.
                string[] args = { "teamspeak_sdk_server" };
                IntPtr[] argv = new IntPtr[args.Length + 1];
                GCHandle pin = default(GCHandle);
                try
                {
                    for (int i = 0; i < args.Length; i++)
                        argv[i] = Marshal.StringToHGlobalAnsi(args[i]);
                    argv[args.Length] = IntPtr.Zero;
                    pin = GCHandle.Alloc(argv, GCHandleType.Pinned);
                    Error error = _InitServerLib(ref functionPointers, (int)usedLogTypes, unmanaged_logFileFolder, args.Length, pin.AddrOfPinnedObject());
                    Check(error);
                }
                finally
                {
                    if (pin.IsAllocated) pin.Free();
                    for (int i = 0; i < args.Length; i++)
                        if (argv[i] != IntPtr.Zero) Marshal.FreeHGlobal(argv[i]);
                }
            }
        }

        public void EnableFileManager(string filebase, string[] ips, int port, ulong downloadBandwitdh, ulong uploadBandwidth)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_filebase = Native.WriteString(filebase))
            using (UnmanagedPointer unmanaged_ips = Native.WriteStringArray(ips))
            {
                Error error = _EnableFileManager(unmanaged_filebase, unmanaged_ips, port, downloadBandwitdh, uploadBandwidth);
                Check(error);
            }
        }

        public void DisableClientCommand(ClientCommand clientCommand)
        {
            using (Lock)
            {
                Check(_DisableClientCommand(clientCommand));
            }
        }

        public string GetServerLibVersion()
        {
            using (Lock)
            {
                IntPtr unmanaged_result;
                Error error = _GetServerLibVersion(out unmanaged_result);
                string result = ReadAndFreeString(unmanaged_result);
                Check(error);
                return result;
            }
        }

        public ulong GetServerLibVersionNumber()
        {
            using (Lock)
            {
                ulong result;
                Check(_GetServerLibVersionNumber(out result));
                return result;
            }
        }

        public void SetLogVerbosity(LogLevel logVerbosity)
        {
            using (Lock)
            {
                Check(_SetLogVerbosity((int)logVerbosity));
            }
        }


        public Error TryGetGlobalErrorMessage(Error globalErrorCode, out string result)
        {
            using (Lock)
            {
                IntPtr unmanaged_result;
                Error error = _GetGlobalErrorMessage(globalErrorCode, out unmanaged_result);
                result = ReadAndFreeString(unmanaged_result);
                return error;
            }
        }

        public int GetClientVariableAsInt(Client client, ClientProperty flag)
        {
            using (Lock)
            {
                int result;
                Check(_GetClientVariableAsInt(client.VirtualServer.ID, client.ID, (IntPtr)flag, out result));
                return result;
            }
        }

        public string GetClientVariableAsString(Client client, ClientProperty flag)
        {
            string result;
            Check(TryGetClientVariableAsString(client, flag, out result));
            return result;
        }

        public Error TryGetClientVariableAsString(Client client, ClientProperty flag, out string result)
        {
            using (Lock)
            {
                IntPtr unmanaged_result;
                Error error = _GetClientVariableAsString(client.VirtualServer.ID, client.ID, (IntPtr)flag, out unmanaged_result);
                result = ReadAndFreeString(unmanaged_result);
                return error;
            }
        }

        public void SetClientVariableAsInt(Client client, ClientProperty flag, int value)
        {
            using (Lock)
            {
                Check(_SetClientVariableAsInt(client.VirtualServer.ID, client.ID, (IntPtr)flag, value));
            }
        }

        public void SetClientVariableAsString(Client client, ClientProperty flag, string value)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_value = Native.WriteString(value))
            {
                Check(_SetClientVariableAsString(client.VirtualServer.ID, client.ID, (IntPtr)flag, unmanaged_value));
            }
        }

        public void FlushClientVariable(Client client)
        {
            using (Lock)
            {
                Check(_FlushClientVariable(client.VirtualServer.ID, client.ID));
            }
        }

        public void SetClientWhisperList(Client client, Channel[] channelID, Client[] clientID)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_channelID = Native.WriteLongIDArray(channelID, GetID))
            using (UnmanagedPointer unmanaged_clientID = Native.WriteShortIDArray(clientID, GetID))
            {
                Check(_SetClientWhisperList(client.VirtualServer.ID, client.ID, unmanaged_channelID, unmanaged_clientID));
            }
        }

        public List<Client> GetClientList(VirtualServer server)
        {
            using (Lock)
            {
                IntPtr unmanaged_result;
                Error error = _GetClientList(server.ID, out unmanaged_result);
                List<Client> result = ReadAndFreeShortIDList(unmanaged_result, server.Cache.GetClient);
                Check(error);
                return result;
            }
        }

        public Channel GetChannelOfClient(Client client)
        {
            using (Lock)
            {
                ulong result;
                Check(_GetChannelOfClient(client.VirtualServer.ID, client.ID, out result));
                return client.VirtualServer.Cache.GetChannel(result);
            }
        }

        public void ClientMove(Channel newChannel, Client[] clientIDArray)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_clientIDArray = Native.WriteShortIDArray(clientIDArray, GetID))
            {
                Check(_ClientMove(newChannel.VirtualServer.ID, newChannel.ID, unmanaged_clientIDArray));
            }
        }

        /// <remarks>
        /// It is assumed all clients are on the same virtual server
        /// </remarks>
        public void ClientsKickFromServer(Client[] clients, string kickReason, bool failOnClientError)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_clientIDArray = Native.WriteShortIDArray(clients, GetID))
            using (UnmanagedPointer unmanaged_kickReason = Native.WriteString(kickReason ?? string.Empty))
            {
                Check(_ClientsKickFromServer(clients[0].VirtualServer.ID, unmanaged_clientIDArray, unmanaged_kickReason, failOnClientError ? 1 : 0));
            }
        }

        public List<Client> GetClientIDSfromUIDS(VirtualServer server, string[] clientUIDs)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_clientUIDs = Native.WriteStringArray(clientUIDs))
            {
                IntPtr unmanaged_result;
                Error error = _GetClientIDSfromUIDS(server.ID, unmanaged_clientUIDs, out unmanaged_result);
                List<Client> result = ReadAndFreeShortIDList(unmanaged_result, server.Cache.GetClient);
                Check(error);
                return result;
            }
        }

        public int GetChannelVariableAsInt(Channel channel, ChannelProperty flag)
        {
            using (Lock)
            {
                int result;
                Check(_GetChannelVariableAsInt(channel.VirtualServer.ID, channel.ID, (IntPtr)flag, out result));
                return result;
            }
        }

        public string GetChannelVariableAsString(Channel channel, ChannelProperty flag)
        {
            string result;
            Check(TryGetChannelVariableAsString(channel, flag, out result));
            return result;
        }
        public Error TryGetChannelVariableAsString(Channel channel, ChannelProperty flag, out string result)
        {
            using (Lock)
            {
                IntPtr unmanaged_result;
                Error error = _GetChannelVariableAsString(channel.VirtualServer.ID, channel.ID, (IntPtr)flag, out unmanaged_result);
                result = ReadAndFreeString(unmanaged_result);
                return error;
            }
        }

        public void SetChannelVariableAsInt(Channel channel, ChannelProperty flag, int value)
        {
            using (Lock)
            {
                Check(_SetChannelVariableAsInt(channel.VirtualServer.ID, channel.ID, (IntPtr)flag, value));
            }
        }

        public void SetChannelVariableAsString(Channel channel, ChannelProperty flag, string value)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_value = Native.WriteString(value))
            {
                Check(_SetChannelVariableAsString(channel.VirtualServer.ID, channel.ID, (IntPtr)flag, unmanaged_value));
            }
        }

        public void FlushChannelVariable(Channel channel)
        {
            using (Lock)
            {
                Check(_FlushChannelVariable(channel.VirtualServer.ID, channel.ID));
            }
        }

        public Channel FlushChannelCreation(VirtualServer virtualServer, ulong parentChannelId = 0)
        {
            Sdk.Require.NotNull(nameof(virtualServer), virtualServer);
            using (Lock)
            {
                ulong result;
                Check(_FlushChannelCreation(virtualServer.ID, parentChannelId, out result));
                return virtualServer.Cache.GetChannel(result);
            }
        }

        public List<Channel> GetChannelList(VirtualServer server)
        {
            using (Lock)
            {
                IntPtr unmanaged_result;
                Error error = _GetChannelList(server.ID, out unmanaged_result);
                List<Channel> result = ReadAndFreeLongIDList(unmanaged_result, server.Cache.GetChannel);
                Check(error);
                return result;
            }
        }

        public List<Client> GetChannelClientList(Channel channel)
        {
            using (Lock)
            {
                IntPtr unmanaged_result;
                Error error = _GetChannelClientList(channel.VirtualServer.ID, channel.ID, out unmanaged_result);
                List<Client> result = ReadAndFreeShortIDList(unmanaged_result, channel.VirtualServer.Cache.GetClient);
                Check(error);
                return result;
            }
        }

        public Channel GetParentChannelOfChannel(Channel channel)
        {
            using (Lock)
            {
                ulong result;
                Check(_GetParentChannelOfChannel(channel.VirtualServer.ID, channel.ID, out result));
                return channel.VirtualServer.Cache.GetChannel(result);
            }
        }

        public void ChannelDelete(Channel channel, bool force)
        {
            using (Lock)
            {
                Check(_ChannelDelete(channel.VirtualServer.ID, channel.ID, force ? 1 : 0));
            }
        }

        public void ChannelMove(Channel channel, Channel newChannelParentID, Channel newOrder)
        {
            using (Lock)
            {
                Check(_ChannelMove(channel.VirtualServer.ID, channel.ID, newChannelParentID?.ID ?? 0, newOrder?.ID ?? 0));
            }
        }

        public int GetVirtualServerVariableAsInt(VirtualServer server, VirtualServerProperty flag)
        {
            using (Lock)
            {
                int result;
                Check(_GetVirtualServerVariableAsInt(server.ID, (IntPtr)flag, out result));
                return result;
            }
        }

        public ulong GetVirtualServerVariableAsUInt64(VirtualServer server, VirtualServerProperty flag)
        {
            using (Lock)
            {
                ulong result;
                Check(_GetVirtualServerConnectionVariableAsUInt64(server.ID, (IntPtr)flag, out result));
                return result;
            }
        }

        public string GetVirtualServerVariableAsString(VirtualServer server, VirtualServerProperty flag)
        {
            using (Lock)
            {
                IntPtr unmanaged_result;
                Error error = _GetVirtualServerVariableAsString(server.ID, (IntPtr)flag, out unmanaged_result);
                string result = ReadAndFreeString(unmanaged_result);
                Check(error);
                return result;
            }
        }

        public void SetVirtualServerVariableAsInt(VirtualServer server, VirtualServerProperty flag, int value)
        {
            using (Lock)
            {
                Check(_SetVirtualServerVariableAsInt(server.ID, (IntPtr)flag, value));
            }
        }

        public void SetVirtualServerVariableAsUInt64(VirtualServer server, VirtualServerProperty flag, ulong value)
        {
            using (Lock)
            {
                Check(_SetVirtualServerVariableAsUInt64(server.ID, (IntPtr)flag, value));
            }
        }

        public void SetVirtualServerVariableAsString(VirtualServer server, VirtualServerProperty flag, string value)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_value = Native.WriteString(value))
            {
                Check(_SetVirtualServerVariableAsString(server.ID, (IntPtr)flag, unmanaged_value));
            }
        }

        public void FlushVirtualServerVariable(VirtualServer server)
        {
            using (Lock)
            {
                Check(_FlushVirtualServerVariable(server.ID));
            }
        }

        public ulong GetVirtualServerConnectionVariableAsUInt64(VirtualServer server, ConnectionProperty flag)
        {
            using (Lock)
            {
                ulong result;
                Check(_GetVirtualServerVariableAsUInt64(server.ID, (IntPtr)flag, out result));
                return result;
            }
        }

        public double GetVirtualServerConnectionVariableAsDouble(VirtualServer server, ConnectionProperty flag)
        {
            using (Lock)
            {
                double result;
                Check(_GetVirtualServerConnectionVariableAsDouble(server.ID, (IntPtr)flag, out result));
                return result;
            }
        }

        public List<VirtualServer> GetVirtualServerList()
        {
            using (Lock)
            {
                IntPtr unmanaged_result;
                Error error = _GetVirtualServerList(out unmanaged_result);
                List<VirtualServer> result = ReadAndFreeLongIDList(unmanaged_result, Library.GetVirtualServer);
                Check(error);
                return result;
            }
        }

        public void StopVirtualServer(VirtualServer server)
        {
            using (Lock)
            {
                Check(_StopVirtualServer(server.ID));
            }
        }

        public VirtualServer CreateVirtualServer(uint serverPort, string serverIp, string serverName, string serverKeyPair, uint serverMaxClients)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_serverIp = Native.WriteString(serverIp))
            using (UnmanagedPointer unmanaged_serverName = Native.WriteString(serverName))
            using (UnmanagedPointer unmanaged_serverKeyPair = Native.WriteString(serverKeyPair))
            {
                ulong result;
                Check(_CreateVirtualServer(serverPort, unmanaged_serverIp, unmanaged_serverName, unmanaged_serverKeyPair, serverMaxClients, out result));
                return Library.GetOrAddVirtualServer(result);
            }
        }

        public string GetVirtualServerKeyPair(VirtualServer server)
        {
            using (Lock)
            {
                IntPtr unmanaged_result;
                Error error = _GetVirtualServerKeyPair(server.ID, out unmanaged_result);
                string result = ReadAndFreeString(unmanaged_result);
                Check(error);
                return result;
            }
        }

        private static ushort GetID(Client client) { return client.ID; }
        private static ulong GetID(Channel channel) { return channel.ID; }
    }
}
