using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TeamSpeak.Sdk.Server
{
    [StructLayout(LayoutKind.Sequential)]
    struct ServerLibFunctions
    {
        public IntPtr OnVoiceDataEvent;
        public IntPtr OnClientStartTalkingEvent;
        public IntPtr OnClientStopTalkingEvent;
        public IntPtr OnClientConnected;
        public IntPtr OnClientDisconnected;
        public IntPtr OnClientMoved;
        public IntPtr OnChannelCreated;
        public IntPtr OnChannelEdited;
        public IntPtr OnChannelDeleted;
        public IntPtr OnServerTextMessageEvent;
        public IntPtr OnChannelTextMessageEvent;
        public IntPtr OnUserLoggingMessageEvent;
        public IntPtr OnAccountingErrorEvent;
        public IntPtr OnCustomPacketEncrypt;
        public IntPtr OnCustomPacketDecrypt;
        public IntPtr OnFileTransferEvent;
        public IntPtr PermClientCanConnect;
        public IntPtr PermClientCanGetChannelDescription;
        public IntPtr PermClientUpdate;
        public IntPtr PermClientKickFromChannel;
        public IntPtr PermClientKickFromServer;
        public IntPtr PermClientMove;
        public IntPtr PermChannelMove;
        public IntPtr PermSendTextMessage;
        public IntPtr PermServerRequestConnectionInfo;
        public IntPtr PermSendConnectionInfo;
        public IntPtr PermChannelCreate;
        public IntPtr PermChannelEdit;
        public IntPtr PermChannelDelete;
        public IntPtr PermChannelSubscribe;
        public IntPtr PermFileTransferInitUpload;
        public IntPtr PermFileTransferInitDownload;
        public IntPtr PermFileTransferGetFileInfo;
        public IntPtr PermFileTransferGetFileList;
        public IntPtr PermFileTransferDeleteFile;
        public IntPtr PermFileTransferCreateDirectory;
        public IntPtr PermFileTransferRenameFile;
        public IntPtr OnClientPasswordEncrypt;
        public IntPtr OnTransformFilePath;
        public IntPtr OnCustomServerPasswordCheck;
        public IntPtr OnCustomChannelPasswordCheck;
    }

    class NativeEvents
    {
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct FileTransferCallbackExport
        {
            public ushort clientID;
            public ushort transferID;
            public ushort remoteTransferID;
            public uint status;
            public IntPtr statusMessage;
            public ulong remotefileSize;
            public ulong bytes;
            public int isSender;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct ClientMiniExport
        {
            public ushort ID;
            public ulong channel;
            public IntPtr ident;
            public IntPtr nickname;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct VariablesExport
        {
            public struct Item
            {
                public byte itemIsValid;    /* This item has valid values. ignore this item if 0 */
                public byte proposedIsSet;  /* The value in proposed is set. if 0 ignore proposed */
                public IntPtr current;      /* current value (stored in memory) */
                public IntPtr proposed;     /* New value to change to (const, so no updates please) */
            }
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public Item[] items;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct TransformFilePathExport
        {
            public ulong channel;
            public IntPtr filename;
            public TransformAction action;
            public int transformedFileNameMaxSize;
            public int channelPathMaxSize;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct TransformFilePathExportReturns
        {
            public IntPtr transformedFileName;
            public IntPtr channelPath;
            public int logFileAction;
        };

        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnVoiceDataEventHandler(ulong serverID, ushort clientID, IntPtr voiceData, uint voiceDataSize, uint frequency);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnClientStartTalkingEventHandler(ulong serverID, ushort clientID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnClientStopTalkingEventHandler(ulong serverID, ushort clientID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnClientConnectedEventHandler(ulong serverID, ushort clientID, ulong channelID, ref uint removeClientError);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnClientDisconnectedEventHandler(ulong serverID, ushort clientID, ulong channelID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnClientMovedEventHandler(ulong serverID, ushort clientID, ulong oldChannelID, ulong newChannelID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnChannelCreatedEventHandler(ulong serverID, ushort invokerClientID, ulong channelID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnChannelEditedEventHandler(ulong serverID, ushort invokerClientID, ulong channelID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnChannelDeletedEventHandler(ulong serverID, ushort invokerClientID, ulong channelID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnServerTextMessageEventHandler(ulong serverID, ushort invokerClientID, IntPtr textMessage);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnChannelTextMessageEventHandler(ulong serverID, ushort invokerClientID, ulong targetChannelID, IntPtr textMessage);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnUserLoggingMessageEventHandler(IntPtr logmessage, int logLevel, IntPtr logChannel, ulong logID, IntPtr logTime, IntPtr completeLogString);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnAccountingErrorEventHandler(ulong serverID, Error errorCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnCustomPacketEncryptHandler(ref IntPtr dataToSend, ref uint sizeOfData);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnCustomPacketDecryptHandler(ref IntPtr dataReceived, ref uint dataReceivedSize);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnFileTransferEventEventHandler([In]ref FileTransferCallbackExport data);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error PermClientCanConnectEventHandler(ulong serverID, [In] ref ClientMiniExport client);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error PermClientCanGetChannelDescriptionEventHandler(ulong serverID, [In] ref ClientMiniExport client);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error PermClientUpdateEventHandler(ulong serverID, ushort clientID, [In] ref VariablesExport variables);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error PermClientKickFromChannelEventHandler(ulong serverID, [In] ref ClientMiniExport client, int toKickCount, [In] IntPtr toKickClients, IntPtr reasonText);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error PermClientKickFromServerEventHandler(ulong serverID, [In] ref ClientMiniExport client, int toKickCount, IntPtr toKickClients, IntPtr reasonText);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error PermClientMoveEventHandler(ulong serverID, [In] ref ClientMiniExport client, int toMoveCount, IntPtr toMoveClients, ulong newChannel, IntPtr reasonText);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error PermChannelMoveEventHandler(ulong serverID, [In] ref ClientMiniExport client, ulong channelID, ulong newParentChannelID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error PermSendTextMessageEventHandler(ulong serverID, [In] ref ClientMiniExport client, TargetMode targetMode, ulong targetClientOrChannel, IntPtr textMessage);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error PermServerRequestConnectionInfoEventHandler(ulong serverID, [In] ref ClientMiniExport client);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error PermSendConnectionInfoEventHandler(ulong serverID, [In] ref ClientMiniExport client, ref int mayViewIpPort, [In] ref ClientMiniExport targetClient);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error PermChannelCreateEventHandler(ulong serverID, [In] ref ClientMiniExport client, ulong parentChannelID, [In] ref VariablesExport variables);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error PermChannelEditEventHandler(ulong serverID, [In] ref ClientMiniExport client, ulong channelID, [In] ref VariablesExport variables);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error PermChannelDeleteEventHandler(ulong serverID, [In] ref ClientMiniExport client, ulong channelID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error PermChannelSubscribeEventHandler(ulong serverID, [In] ref ClientMiniExport client, ulong channelID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error PermFileTransferInitUploadEventHandler(ulong serverID, [In] ref ClientMiniExport client, IntPtr @params);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error PermFileTransferInitDownloadEventHandler(ulong serverID, [In] ref ClientMiniExport client, IntPtr @params);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error PermFileTransferGetFileInfoEventHandler(ulong serverID, [In] ref ClientMiniExport client, IntPtr @params);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error PermFileTransferGetFileListEventHandler(ulong serverID, [In] ref ClientMiniExport client, IntPtr @params);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error PermFileTransferDeleteFileEventHandler(ulong serverID, [In] ref ClientMiniExport client, IntPtr @params);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error PermFileTransferCreateDirectoryEventHandler(ulong serverID, [In] ref ClientMiniExport client, IntPtr @params);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error PermFileTransferRenameFileEventHandler(ulong serverID, [In] ref ClientMiniExport client, IntPtr @params);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnClientPasswordEncryptHandler(ulong serverID, IntPtr plaintext, IntPtr encryptedText, int encryptedTextByteSize);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error OnTransformFilePathEventHandler(ulong serverID, ushort invokerClientID, [In] ref TransformFilePathExport original, ref TransformFilePathExportReturns result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error OnCustomServerPasswordCheckEventHandler(ulong serverID, [In] ref ClientMiniExport client, IntPtr password);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error OnCustomChannelPasswordCheckEventHandler(ulong serverID, [In] ref ClientMiniExport client, ulong channelID, IntPtr password);

        public readonly ServerLibFunctions ServerLibFunctions;

        /// <summary>
        /// This list ensures that the native event handlers are not garbage collected
        /// </summary>
        private List<Delegate> DelegateReferences = new List<Delegate>();
        public NativeEvents(bool useClientPasswordEncrypt, bool useCustomPacketEncrypt, bool useCustomPacketDecrypt, bool useCustomServerPasswordCheck = false, bool useCustomChannelPasswordCheck = false)
        {
            SetNativeCallback(out ServerLibFunctions.OnVoiceDataEvent, new OnVoiceDataEventHandler(OnVoiceDataEventEventWrapper));
            SetNativeCallback(out ServerLibFunctions.OnClientStartTalkingEvent, new OnClientStartTalkingEventHandler(OnClientStartTalkingEventEventWrapper));
            SetNativeCallback(out ServerLibFunctions.OnClientStopTalkingEvent, new OnClientStopTalkingEventHandler(OnClientStopTalkingEventEventWrapper));
            SetNativeCallback(out ServerLibFunctions.OnClientConnected, new OnClientConnectedEventHandler(OnClientConnectedEventWrapper));
            SetNativeCallback(out ServerLibFunctions.OnClientDisconnected, new OnClientDisconnectedEventHandler(OnClientDisconnectedEventWrapper));
            SetNativeCallback(out ServerLibFunctions.OnClientMoved, new OnClientMovedEventHandler(OnClientMovedEventWrapper));
            SetNativeCallback(out ServerLibFunctions.OnChannelCreated, new OnChannelCreatedEventHandler(OnChannelCreatedEventWrapper));
            SetNativeCallback(out ServerLibFunctions.OnChannelEdited, new OnChannelEditedEventHandler(OnChannelEditedEventWrapper));
            SetNativeCallback(out ServerLibFunctions.OnChannelDeleted, new OnChannelDeletedEventHandler(OnChannelDeletedEventWrapper));
            SetNativeCallback(out ServerLibFunctions.OnServerTextMessageEvent, new OnServerTextMessageEventHandler(OnServerTextMessageEventEventWrapper));
            SetNativeCallback(out ServerLibFunctions.OnChannelTextMessageEvent, new OnChannelTextMessageEventHandler(OnChannelTextMessageEventEventWrapper));
            SetNativeCallback(out ServerLibFunctions.OnUserLoggingMessageEvent, new OnUserLoggingMessageEventHandler(OnUserLoggingMessageEventWrapper));
            SetNativeCallback(out ServerLibFunctions.OnAccountingErrorEvent, new OnAccountingErrorEventHandler(OnAccountingErrorEventEventWrapper));
            if (useCustomPacketEncrypt)
                SetNativeCallback(out ServerLibFunctions.OnCustomPacketEncrypt, new OnCustomPacketEncryptHandler(OnCustomPacketEncryptEventWrapper));
            else ServerLibFunctions.OnCustomPacketEncrypt = IntPtr.Zero;
            if (useCustomPacketDecrypt)
                SetNativeCallback(out ServerLibFunctions.OnCustomPacketDecrypt, new OnCustomPacketDecryptHandler(OnCustomPacketDecryptEventWrapper));
            else ServerLibFunctions.OnCustomPacketDecrypt = IntPtr.Zero;
            SetNativeCallback(out ServerLibFunctions.OnFileTransferEvent, new OnFileTransferEventEventHandler(OnFileTransferEventEventWrapper));
            SetNativeCallback(out ServerLibFunctions.PermClientCanConnect, new PermClientCanConnectEventHandler(PermClientCanConnectEventWrapper));
            SetNativeCallback(out ServerLibFunctions.PermClientCanGetChannelDescription, new PermClientCanGetChannelDescriptionEventHandler(PermClientCanGetChannelDescriptionEventWrapper));
            SetNativeCallback(out ServerLibFunctions.PermClientUpdate, new PermClientUpdateEventHandler(PermClientUpdateEventWrapper));
            SetNativeCallback(out ServerLibFunctions.PermClientKickFromChannel, new PermClientKickFromChannelEventHandler(PermClientKickFromChannelEventWrapper));
            SetNativeCallback(out ServerLibFunctions.PermClientKickFromServer, new PermClientKickFromServerEventHandler(PermClientKickFromServerEventWrapper));
            SetNativeCallback(out ServerLibFunctions.PermClientMove, new PermClientMoveEventHandler(PermClientMoveEventWrapper));
            SetNativeCallback(out ServerLibFunctions.PermChannelMove, new PermChannelMoveEventHandler(PermChannelMoveEventWrapper));
            SetNativeCallback(out ServerLibFunctions.PermSendTextMessage, new PermSendTextMessageEventHandler(PermSendTextMessageEventWrapper));
            SetNativeCallback(out ServerLibFunctions.PermServerRequestConnectionInfo, new PermServerRequestConnectionInfoEventHandler(PermServerRequestConnectionInfoEventWrapper));
            SetNativeCallback(out ServerLibFunctions.PermSendConnectionInfo, new PermSendConnectionInfoEventHandler(PermSendConnectionInfoEventWrapper));
            SetNativeCallback(out ServerLibFunctions.PermChannelCreate, new PermChannelCreateEventHandler(PermChannelCreateEventWrapper));
            SetNativeCallback(out ServerLibFunctions.PermChannelEdit, new PermChannelEditEventHandler(PermChannelEditEventWrapper));
            SetNativeCallback(out ServerLibFunctions.PermChannelDelete, new PermChannelDeleteEventHandler(PermChannelDeleteEventWrapper));
            SetNativeCallback(out ServerLibFunctions.PermChannelSubscribe, new PermChannelSubscribeEventHandler(PermChannelSubscribeEventWrapper));
            SetNativeCallback(out ServerLibFunctions.PermFileTransferInitUpload, new PermFileTransferInitUploadEventHandler(PermFileTransferInitUploadEventWrapper));
            SetNativeCallback(out ServerLibFunctions.PermFileTransferInitDownload, new PermFileTransferInitDownloadEventHandler(PermFileTransferInitDownloadEventWrapper));
            SetNativeCallback(out ServerLibFunctions.PermFileTransferGetFileInfo, new PermFileTransferGetFileInfoEventHandler(PermFileTransferGetFileInfoEventWrapper));
            SetNativeCallback(out ServerLibFunctions.PermFileTransferGetFileList, new PermFileTransferGetFileListEventHandler(PermFileTransferGetFileListEventWrapper));
            SetNativeCallback(out ServerLibFunctions.PermFileTransferDeleteFile, new PermFileTransferDeleteFileEventHandler(PermFileTransferDeleteFileEventWrapper));
            SetNativeCallback(out ServerLibFunctions.PermFileTransferCreateDirectory, new PermFileTransferCreateDirectoryEventHandler(PermFileTransferCreateDirectoryEventWrapper));
            SetNativeCallback(out ServerLibFunctions.PermFileTransferRenameFile, new PermFileTransferRenameFileEventHandler(PermFileTransferRenameFileEventWrapper));
            if (useClientPasswordEncrypt)
                SetNativeCallback(out ServerLibFunctions.OnClientPasswordEncrypt, new OnClientPasswordEncryptHandler(OnClientPasswordEncryptWrapper));
            else ServerLibFunctions.OnClientPasswordEncrypt = IntPtr.Zero;
            SetNativeCallback(out ServerLibFunctions.OnTransformFilePath, new OnTransformFilePathEventHandler(OnTransformFilePathEventWrapper));
            if (useCustomServerPasswordCheck)
                SetNativeCallback(out ServerLibFunctions.OnCustomServerPasswordCheck, new OnCustomServerPasswordCheckEventHandler(OnCustomServerPasswordCheckEventWrapper));
            else ServerLibFunctions.OnCustomServerPasswordCheck = IntPtr.Zero;
            if (useCustomChannelPasswordCheck)
                SetNativeCallback(out ServerLibFunctions.OnCustomChannelPasswordCheck, new OnCustomChannelPasswordCheckEventHandler(OnCustomChannelPasswordCheckEventWrapper));
            else ServerLibFunctions.OnCustomChannelPasswordCheck = IntPtr.Zero;
        }

        private void SetNativeCallback(out IntPtr field, Delegate @delegate)
        {
            DelegateReferences.Add(@delegate);
            field = Marshal.GetFunctionPointerForDelegate(@delegate);
        }

        private bool HandleException(Exception e)
        {
            if (e is System.Threading.ThreadAbortException || e is System.Threading.ThreadInterruptedException)
                return false;
            Debug.Assert(false, "exception on callback thread");
            return true;
        }

        private void OnVoiceDataEventEventWrapper(ulong serverID, ushort clientID, IntPtr voiceData, uint voiceDataSize, uint frequency)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            int size = (int)voiceDataSize;
            byte[] voiceDataBytes = new byte[size];
            Marshal.Copy(voiceData, voiceDataBytes, 0, size);
            virtualServer.OnVoiceData(virtualServer.Cache.GetClient(clientID), voiceDataBytes, frequency);
        }

        private void OnClientStartTalkingEventEventWrapper(ulong serverID, ushort clientID)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            virtualServer.OnClientStartTalking(virtualServer.Cache.GetClient(clientID));
        }

        private void OnClientStopTalkingEventEventWrapper(ulong serverID, ushort clientID)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            Library.GetOrAddVirtualServer(serverID).OnClientStopTalking(virtualServer.Cache.GetClient(clientID));
        }

        private void OnClientConnectedEventWrapper(ulong serverID, ushort clientID, ulong channelID, ref uint removeClientError)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            bool _removeClientError = removeClientError != 0;
            virtualServer.OnClientConnected(virtualServer.Cache.GetClient(clientID), virtualServer.Cache.GetChannel(channelID), ref _removeClientError);
            removeClientError = _removeClientError ? 1u : 0u;
        }

        private void OnClientDisconnectedEventWrapper(ulong serverID, ushort clientID, ulong channelID)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            virtualServer.OnClientDisconnected(virtualServer.Cache.GetClient(clientID), virtualServer.Cache.GetChannel(channelID));
        }

        private void OnClientMovedEventWrapper(ulong serverID, ushort clientID, ulong oldChannelID, ulong newChannelID)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            virtualServer.OnClientMoved(virtualServer.Cache.GetClient(clientID), virtualServer.Cache.GetChannel(oldChannelID), virtualServer.Cache.GetChannel(newChannelID));
        }

        private void OnChannelCreatedEventWrapper(ulong serverID, ushort invokerClientID, ulong channelID)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            virtualServer.OnChannelCreated(virtualServer.Cache.GetClient(invokerClientID), virtualServer.Cache.AddChannel(channelID));
        }

        private void OnChannelEditedEventWrapper(ulong serverID, ushort invokerClientID, ulong channelID)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            virtualServer.OnChannelEdited(virtualServer.Cache.GetClient(invokerClientID), virtualServer.Cache.GetChannel(channelID));
        }

        private void OnChannelDeletedEventWrapper(ulong serverID, ushort invokerClientID, ulong channelID)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            virtualServer.OnChannelDeleted(virtualServer.Cache.GetClient(invokerClientID), virtualServer.Cache.RemoveChannel(channelID));
        }

        private void OnServerTextMessageEventEventWrapper(ulong serverID, ushort invokerClientID, IntPtr textMessage)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            virtualServer.OnServerTextMessage(virtualServer.Cache.GetClient(invokerClientID), Native.ReadString(textMessage));
        }

        private void OnChannelTextMessageEventEventWrapper(ulong serverID, ushort invokerClientID, ulong targetChannelID, IntPtr textMessage)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            virtualServer.OnChannelTextMessage(virtualServer.Cache.GetClient(invokerClientID), virtualServer.Cache.GetChannel(targetChannelID), Native.ReadString(textMessage));
        }

        private void OnUserLoggingMessageEventWrapper(IntPtr logmessage, int logLevel, IntPtr logChannel, ulong logID, IntPtr logTime, IntPtr completeLogString)
        {
            try
            {
                var virtualServer = Library.GetOrAddVirtualServer(logID);
                Library.OnUserLoggingMessage(Native.ReadString(logmessage), (LogLevel)logLevel, Native.ReadString(logChannel), virtualServer, Native.ReadString(logTime), Native.ReadString(completeLogString));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnAccountingErrorEventEventWrapper(ulong serverID, Error errorCode)
        {
            Library.GetOrAddVirtualServer(serverID).OnAccountingError(errorCode);
        }

        private void OnCustomPacketEncryptEventWrapper(ref IntPtr dataToSend, ref uint sizeOfData)
        {
            throw new NotImplementedException();
        }

        private void OnCustomPacketDecryptEventWrapper(ref IntPtr dataReceived, ref uint dataReceivedSize)
        {
            throw new NotImplementedException();
        }

        private void OnFileTransferEventEventWrapper(ref FileTransferCallbackExport data)
        {
            throw new NotImplementedException();
        }

        private Error PermClientCanConnectEventWrapper(ulong serverID, ref ClientMiniExport client)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            Client invoker = ReadClient(virtualServer, client, true);
            return virtualServer.PermissionChecks.OnCanConnect(invoker);
        }

        private Error PermClientCanGetChannelDescriptionEventWrapper(ulong serverID, ref ClientMiniExport client)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            Client invoker = ReadClient(virtualServer, client);
            return virtualServer.PermissionChecks.OnCanGetChannelDescription(invoker);
        }

        private Error PermClientUpdateEventWrapper(ulong serverID, ushort clientID, ref VariablesExport variables)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            Client invoker = virtualServer.Cache.GetClient(clientID);
            ReadOnlyCollection<ExportedVariable<ClientProperty>> properties = ReadExportedVariables<ClientProperty>(variables);
            return virtualServer.PermissionChecks.OnCanUpdateClient(invoker, properties);
        }

        private Error PermClientKickFromChannelEventWrapper(ulong serverID, ref ClientMiniExport client, int toKickCount, IntPtr toKickClients, IntPtr reasonText)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            Client invoker = ReadClient(virtualServer, client);
            ReadOnlyCollection<Client> clients = ReadClientList(virtualServer, toKickCount, toKickClients);
            string reason = Native.ReadString(reasonText);
            return virtualServer.PermissionChecks.OnCanKickFromChannel(invoker, clients, reason);
        }

        private Error PermClientKickFromServerEventWrapper(ulong serverID, ref ClientMiniExport client, int toKickCount, IntPtr toKickClients, IntPtr reasonText)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            Client invoker = ReadClient(virtualServer, client);
            ReadOnlyCollection<Client> clients = ReadClientList(virtualServer, toKickCount, toKickClients);
            string reason = Native.ReadString(reasonText);
            return virtualServer.PermissionChecks.OnCanKickFromServer(invoker, clients, reason);
        }

        private Error PermClientMoveEventWrapper(ulong serverID, ref ClientMiniExport client, int toMoveCount, IntPtr toMoveClients, ulong newChannel, IntPtr reasonText)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            Client invoker = ReadClient(virtualServer, client);
            ReadOnlyCollection<Client> clients = ReadClientList(virtualServer, toMoveCount, toMoveClients);
            Channel channel = virtualServer.Cache.GetChannel(newChannel);
            string reason = Native.ReadString(reasonText);
            return virtualServer.PermissionChecks.OnCanMoveClient(invoker, clients, channel, reason);
        }

        private Error PermChannelMoveEventWrapper(ulong serverID, ref ClientMiniExport client, ulong channelID, ulong newParentChannelID)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            Client invoker = ReadClient(virtualServer, client);
            Channel channel = virtualServer.Cache.GetChannel(channelID);
            Channel newParent = virtualServer.Cache.GetChannel(newParentChannelID);
            return virtualServer.PermissionChecks.OnCanMoveChannel(invoker, channel, newParent);
        }

        private Error PermSendTextMessageEventWrapper(ulong serverID, ref ClientMiniExport client, TargetMode targetMode, ulong targetClientOrChannel, IntPtr textMessage)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            Client invoker = ReadClient(virtualServer, client);
            string message = Native.ReadString(textMessage);
            switch (targetMode)
            {
                case TargetMode.Server: return virtualServer.PermissionChecks.OnCanSendServerTextMessage(invoker, message);
                case TargetMode.Channel: return virtualServer.PermissionChecks.OnCanSendChannelTextMessage(invoker, virtualServer.Cache.GetChannel(targetClientOrChannel), message);
                case TargetMode.Client: return virtualServer.PermissionChecks.OnCanSendClientTextMessage(invoker, virtualServer.Cache.GetClient((ushort)targetClientOrChannel), message);
                default: Debug.Assert(false); return Error.Undefined;
            }
        }

        private Error PermServerRequestConnectionInfoEventWrapper(ulong serverID, ref ClientMiniExport client)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            Client invoker = ReadClient(virtualServer, client);
            return virtualServer.PermissionChecks.OnCanGetServerConnectionInfo(invoker);
        }

        private Error PermSendConnectionInfoEventWrapper(ulong serverID, ref ClientMiniExport client, ref int mayViewIpPort, ref ClientMiniExport targetClient)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            Client invoker = ReadClient(virtualServer, client);
            bool _MayViewIpPort = mayViewIpPort != 0;
            Client _TargetClient = ReadClient(virtualServer, targetClient);
            Error result = virtualServer.PermissionChecks.OnCanGetClientConnectionInfo(invoker, ref _MayViewIpPort, _TargetClient);
            mayViewIpPort = _MayViewIpPort ? 1 : 0;
            return result;
        }

        private Error PermChannelCreateEventWrapper(ulong serverID, ref ClientMiniExport client, ulong parentChannelID, ref VariablesExport variables)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            Client invoker = ReadClient(virtualServer, client);
            Channel parent = virtualServer.Cache.GetChannel(parentChannelID);
            ReadOnlyCollection<ExportedVariable<ChannelProperty>> properties = ReadExportedVariables<ChannelProperty>(variables);
            return virtualServer.PermissionChecks.OnCanCreateChannel(invoker, parent, properties);
        }

        private Error PermChannelEditEventWrapper(ulong serverID, ref ClientMiniExport client, ulong channelID, ref VariablesExport variables)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            Client invoker = ReadClient(virtualServer, client);
            Channel channel = virtualServer.Cache.GetChannel(channelID);
            ReadOnlyCollection<ExportedVariable<ChannelProperty>> properties = ReadExportedVariables<ChannelProperty>(variables);
            return virtualServer.PermissionChecks.OnCanEditChannel(invoker, channel, properties);
        }

        private Error PermChannelDeleteEventWrapper(ulong serverID, ref ClientMiniExport client, ulong channelID)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            Client invoker = ReadClient(virtualServer, client);
            Channel channel = virtualServer.Cache.GetChannel(channelID);
            return virtualServer.PermissionChecks.OnCanDeleteChannel(invoker, channel);
        }

        private Error PermChannelSubscribeEventWrapper(ulong serverID, ref ClientMiniExport client, ulong channelID)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            Client invoker = ReadClient(virtualServer, client);
            Channel channel = virtualServer.Cache.GetChannel(channelID);
            return virtualServer.PermissionChecks.OnCanSubscribeChannel(invoker, channel);
        }

        private Error PermFileTransferInitUploadEventWrapper(ulong serverID, ref ClientMiniExport client, IntPtr @params)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            Client invoker = ReadClient(virtualServer, client);
            @params += 4; // RESERVED
            string fileName = Native.ReadString(@params);
            @params += Native.SizeOfPointer;
            ulong fileSize = (ulong)Marshal.ReadInt64(@params, 0);
            Channel channel = virtualServer.Cache.GetChannel((ulong)Marshal.ReadInt64(@params, 8));
            bool overwrite = Marshal.ReadInt32(@params, 16) != 0;
            bool resume = Marshal.ReadInt32(@params, 20) != 0;
            return virtualServer.PermissionChecks.OnCanInitUpload(invoker, fileName, fileSize, channel, overwrite, resume);
        }

        private Error PermFileTransferInitDownloadEventWrapper(ulong serverID, ref ClientMiniExport client, IntPtr @params)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            Client invoker = ReadClient(virtualServer, client);
            @params += 4; // RESERVED
            string fileName = Native.ReadString(@params);
            @params += Native.SizeOfPointer;
            Channel channel = virtualServer.Cache.GetChannel((ulong)Marshal.ReadInt64(@params));
            return virtualServer.PermissionChecks.OnCanInitDownload(invoker, fileName, channel);
        }

        private Error PermFileTransferGetFileInfoEventWrapper(ulong serverID, ref ClientMiniExport client, IntPtr @params)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            Client invoker = ReadClient(virtualServer, client);
            int size = Marshal.ReadInt32(@params, 8);
            @params += 12;
            List<GetFileInfoPermissionArgs.Request> requests = new List<GetFileInfoPermissionArgs.Request>();
            for (int i = 0; i < size; i++)
            {
                Channel channel = virtualServer.Cache.GetChannel((ulong)Marshal.ReadInt64(@params));
                @params += 8;
                string fileName = Native.ReadString(@params);
                @params += Native.SizeOfPointer;
                requests.Add(new GetFileInfoPermissionArgs.Request(fileName, channel));
            }
            return virtualServer.PermissionChecks.OnCanGetFileInfo(invoker, requests.AsReadOnly());
        }

        private Error PermFileTransferGetFileListEventWrapper(ulong serverID, ref ClientMiniExport client, IntPtr @params)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            Client invoker = ReadClient(virtualServer, client);
            Channel channel = virtualServer.Cache.GetChannel((ulong)Marshal.ReadInt64(@params, 4));
            string path = Native.ReadString(@params + 12);
            return virtualServer.PermissionChecks.OnCanGetFileList(invoker, channel, path);
        }

        private Error PermFileTransferDeleteFileEventWrapper(ulong serverID, ref ClientMiniExport client, IntPtr @params)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            Client invoker = ReadClient(virtualServer, client);
            Channel channel = virtualServer.Cache.GetChannel((ulong)Marshal.ReadInt64(@params, 4));
            int size = Marshal.ReadInt32(@params, 12);
            @params += 16;
            List<string> fileNames = new List<string>();
            for (int i = 0; i < size; i++)
            {
                string fileName = Native.ReadString(@params);
                @params += Native.SizeOfPointer;
                fileNames.Add(fileName);
            }
            return virtualServer.PermissionChecks.OnCanDeleteFile(invoker, channel, fileNames.AsReadOnly());
        }

        private Error PermFileTransferCreateDirectoryEventWrapper(ulong serverID, ref ClientMiniExport client, IntPtr @params)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            Client invoker = ReadClient(virtualServer, client);
            Channel channel = virtualServer.Cache.GetChannel((ulong)Marshal.ReadInt64(@params, 4));
            string directoryName = Native.ReadString(@params + 12);
            return virtualServer.PermissionChecks.OnCanCreateDirectory(invoker, channel, directoryName);
        }

        private Error PermFileTransferRenameFileEventWrapper(ulong serverID, ref ClientMiniExport client, IntPtr @params)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            Client invoker = ReadClient(virtualServer, client);
            bool has_toChannelID = Marshal.ReadInt32(@params) != 0;
            Channel fromChannel = virtualServer.Cache.GetChannel((ulong)Marshal.ReadInt64(@params, 4));
            Channel toChannel;
            if (has_toChannelID)
                toChannel = virtualServer.Cache.GetChannel((ulong)Marshal.ReadInt64(@params, 12));
            else
                toChannel = fromChannel;
            @params += 20;
            string oldFileName = Native.ReadString(@params);
            @params += Native.SizeOfPointer;
            string newFileName = Native.ReadString(@params);
            return virtualServer.PermissionChecks.OnCanRenameFile(invoker, fromChannel, toChannel, oldFileName, newFileName);
        }

        private void OnClientPasswordEncryptWrapper(ulong serverID, IntPtr plaintext, IntPtr encryptedText, int encryptedTextByteSize)
        {
            throw new NotImplementedException();
        }

        private Error OnTransformFilePathEventWrapper(ulong serverID, ushort invokerClientID, ref TransformFilePathExport original, ref TransformFilePathExportReturns result)
        {
            VirtualServer virtualServer = Library.GetOrAddVirtualServer(serverID);
            Client invoker = virtualServer.Cache.GetClient(invokerClientID);
            Channel channel = virtualServer.Cache.GetChannel(original.channel);
            string fileName = Native.ReadString(original.filename);
            string channelPath = Native.ReadString(result.channelPath);
            TransformFilePathEventArgs args = new TransformFilePathEventArgs(invoker, channel, original.action, fileName, original.transformedFileNameMaxSize, channelPath, original.channelPathMaxSize);
            virtualServer.OnTransformFilePath(args);
            if (args.Result == Error.Ok)
            {
                Native.WriteString(args.FileName ?? string.Empty, result.transformedFileName, original.transformedFileNameMaxSize);
                Native.WriteString(args.ChannelPath?? string.Empty, result.channelPath, original.channelPathMaxSize);
                result.logFileAction = args.LogFileAction ? 1 : 0;
            }
            return args.Result;
        }

        private Error OnCustomServerPasswordCheckEventWrapper(ulong serverID, ref ClientMiniExport client, IntPtr password)
        {
            throw new NotImplementedException();
        }

        private Error OnCustomChannelPasswordCheckEventWrapper(ulong serverID, ref ClientMiniExport client, ulong channelID, IntPtr password)
        {
            throw new NotImplementedException();
        }

        private Client ReadClient(VirtualServer virtualServer, ClientMiniExport clientMiniExport, bool createIfNotExists = false)
        {
            Client client;
            if (createIfNotExists)
                client = virtualServer.Cache.AddorGetClient(clientMiniExport.ID);
            else
                client = virtualServer.Cache.GetClient(clientMiniExport.ID);

            client?.Hint(Native.ReadString(clientMiniExport.nickname), Native.ReadString(clientMiniExport.ident));
            Debug.Assert((client.Channel?.ID ?? 0) == clientMiniExport.channel);
            return client;
        }

        private ReadOnlyCollection<Client> ReadClientList(VirtualServer virtualServer, int count, IntPtr clients)
        {
            List<Client> result = new List<Client>(count);
            for (int i = 0; i < count; i++)
            {
                ClientMiniExport client = new ClientMiniExport();
                Marshal.PtrToStructure(clients, client);
                clients += Marshal.SizeOf(client);
                result[i] = ReadClient(virtualServer, client);
            }
            return result.AsReadOnly();
        }
        private ReadOnlyCollection<ExportedVariable<TProperty>> ReadExportedVariables<TProperty>(VariablesExport export)
        {
            List<ExportedVariable<TProperty>> result = new List<ExportedVariable<TProperty>>();
            for (int i = 0; i < export.items.Length; i++)
            {
                VariablesExport.Item item = export.items[i];
                if (item.itemIsValid == 0)
                    continue;
                string current = Native.ReadString(item.current);
                string proposed;
                if (item.proposedIsSet != 0)
                    proposed = Native.ReadString(item.proposed);
                else proposed = null;
                result.Add(new ExportedVariable<TProperty>((TProperty)(object)i, current, proposed));
            }
            return result.AsReadOnly();
        }
    }
}
