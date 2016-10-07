using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TeamSpeak.Sdk.Client
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ClientUIFunctions
    {
        public IntPtr OnConnectStatusChangeEvent;
        public IntPtr OnServerProtocolVersionEvent;
        public IntPtr OnNewChannelEvent;
        public IntPtr OnNewChannelCreatedEvent;
        public IntPtr OnDelChannelEvent;
        public IntPtr OnChannelMoveEvent;
        public IntPtr OnUpdateChannelEvent;
        public IntPtr OnUpdateChannelEditedEvent;
        public IntPtr OnUpdateClientEvent;
        public IntPtr OnClientMoveEvent;
        public IntPtr OnClientMoveSubscriptionEvent;
        public IntPtr OnClientMoveTimeoutEvent;
        public IntPtr OnClientMoveMovedEvent;
        public IntPtr OnClientKickFromChannelEvent;
        public IntPtr OnClientKickFromServerEvent;
        public IntPtr OnClientIDsEvent;
        public IntPtr OnClientIDsFinishedEvent;
        public IntPtr OnServerEditedEvent;
        public IntPtr OnServerUpdatedEvent;
        public IntPtr OnServerErrorEvent;
        public IntPtr OnServerStopEvent;
        public IntPtr OnTextMessageEvent;
        public IntPtr OnTalkStatusChangeEvent;
        public IntPtr OnIgnoredWhisperEvent;
        public IntPtr OnConnectionInfoEvent;
        public IntPtr OnServerConnectionInfoEvent;
        public IntPtr OnChannelSubscribeEvent;
        public IntPtr OnChannelSubscribeFinishedEvent;
        public IntPtr OnChannelUnsubscribeEvent;
        public IntPtr OnChannelUnsubscribeFinishedEvent;
        public IntPtr OnChannelDescriptionUpdateEvent;
        public IntPtr OnChannelPasswordChangedEvent;
        public IntPtr OnPlaybackShutdownCompleteEvent;
        public IntPtr OnSoundDeviceListChangedEvent;
        public IntPtr OnEditPlaybackVoiceDataEvent;
        public IntPtr OnEditPostProcessVoiceDataEvent;
        public IntPtr OnEditMixedPlaybackVoiceDataEvent;
        public IntPtr OnEditCapturedVoiceDataEvent;
        public IntPtr OnCustom3dRolloffCalculationClientEvent;
        public IntPtr OnCustom3dRolloffCalculationWaveEvent;
        public IntPtr OnUserLoggingMessageEvent;
        public IntPtr OnCustomPacketEncryptEvent;
        public IntPtr OnCustomPacketDecryptEvent;
        public IntPtr OnProvisioningSlotRequestResultEvent;
        public IntPtr OnCheckServerUniqueIdentifierEvent;
        public IntPtr OnClientPasswordEncrypt;
        public IntPtr OnFileTransferStatusEvent;
        public IntPtr OnFileListEvent;
        public IntPtr OnFileListFinishedEvent;
        public IntPtr OnFileInfoEvent;
    }

    class NativeEvents
    {
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnConnectStatusChangeEventHandler(ulong serverConnectionHandlerID, int newStatus, uint errorNumber);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnServerProtocolVersionEventHandler(ulong serverConnectionHandlerID, int protocolVersion);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnNewChannelEventHandler(ulong serverConnectionHandlerID, ulong channelID, ulong channelParentID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnNewChannelCreatedEventHandler(ulong serverConnectionHandlerID, ulong channelID, ulong channelParentID, ushort invokerID, IntPtr invokerName, IntPtr invokerUniqueIdentifier);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnDelChannelEventHandler(ulong serverConnectionHandlerID, ulong channelID, ushort invokerID, IntPtr invokerName, IntPtr invokerUniqueIdentifier);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnChannelMoveEventHandler(ulong serverConnectionHandlerID, ulong channelID, ulong newChannelParentID, ushort invokerID, IntPtr invokerName, IntPtr invokerUniqueIdentifier);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnUpdateChannelEventHandler(ulong serverConnectionHandlerID, ulong channelID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnUpdateChannelEditedEventHandler(ulong serverConnectionHandlerID, ulong channelID, ushort invokerID, IntPtr invokerName, IntPtr invokerUniqueIdentifier);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnUpdateClientEventHandler(ulong serverConnectionHandlerID, ushort clientID, ushort invokerID, IntPtr invokerName, IntPtr invokerUniqueIdentifier);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnClientMoveEventHandler(ulong serverConnectionHandlerID, ushort clientID, ulong oldChannelID, ulong newChannelID, int visibility, IntPtr moveMessage);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnClientMoveSubscriptionEventHandler(ulong serverConnectionHandlerID, ushort clientID, ulong oldChannelID, ulong newChannelID, int visibility);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnClientMoveTimeoutEventHandler(ulong serverConnectionHandlerID, ushort clientID, ulong oldChannelID, ulong newChannelID, int visibility, IntPtr timeoutMessage);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnClientMoveMovedEventHandler(ulong serverConnectionHandlerID, ushort clientID, ulong oldChannelID, ulong newChannelID, int visibility, ushort moverID, IntPtr moverName, IntPtr moverUniqueIdentifier, IntPtr moveMessage);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnClientKickFromChannelEventHandler(ulong serverConnectionHandlerID, ushort clientID, ulong oldChannelID, ulong newChannelID, int visibility, ushort kickerID, IntPtr kickerName, IntPtr kickerUniqueIdentifier, IntPtr kickMessage);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnClientKickFromServerEventHandler(ulong serverConnectionHandlerID, ushort clientID, ulong oldChannelID, ulong newChannelID, int visibility, ushort kickerID, IntPtr kickerName, IntPtr kickerUniqueIdentifier, IntPtr kickMessage);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnClientIDsEventHandler(ulong serverConnectionHandlerID, IntPtr uniqueClientIdentifier, ushort clientID, IntPtr clientName);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnClientIDsFinishedEventHandler(ulong serverConnectionHandlerID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnServerEditedEventHandler(ulong serverConnectionHandlerID, ushort editerID, IntPtr editerName, IntPtr editerUniqueIdentifier);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnServerUpdatedEventHandler(ulong serverConnectionHandlerID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnServerErrorEventHandler(ulong serverConnectionHandlerID, IntPtr errorMessage, uint error, IntPtr returnCode, IntPtr extraMessage);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnServerStopEventHandler(ulong serverConnectionHandlerID, IntPtr shutdownMessage);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnTextMessageEventHandler(ulong serverConnectionHandlerID, ushort targetMode, ushort toID, ushort fromID, IntPtr fromName, IntPtr fromUniqueIdentifier, IntPtr message);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnTalkStatusChangeEventHandler(ulong serverConnectionHandlerID, int status, int isReceivedWhisper, ushort clientID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnIgnoredWhisperEventHandler(ulong serverConnectionHandlerID, ushort clientID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnConnectionInfoEventHandler(ulong serverConnectionHandlerID, ushort clientID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnServerConnectionInfoEventHandler(ulong serverConnectionHandlerID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnChannelSubscribeEventHandler(ulong serverConnectionHandlerID, ulong channelID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnChannelSubscribeFinishedEventHandler(ulong serverConnectionHandlerID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnChannelUnsubscribeEventHandler(ulong serverConnectionHandlerID, ulong channelID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnChannelUnsubscribeFinishedEventHandler(ulong serverConnectionHandlerID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnChannelDescriptionUpdateEventHandler(ulong serverConnectionHandlerID, ulong channelID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnChannelPasswordChangedEventHandler(ulong serverConnectionHandlerID, ulong channelID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnPlaybackShutdownCompleteEventHandler(ulong serverConnectionHandlerID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnSoundDeviceListChangedEventHandler(IntPtr modeID, int playOrCap);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnEditPlaybackVoiceDataEventHandler(ulong serverConnectionHandlerID, ushort clientID, IntPtr samples, int sampleCount, int channels);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnEditPostProcessVoiceDataEventHandler(ulong serverConnectionHandlerID, ushort clientID, IntPtr samples, int sampleCount, int channels, IntPtr channelSpeakerArray, ref Speakers channelFillMask);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnEditMixedPlaybackVoiceDataEventHandler(ulong serverConnectionHandlerID, IntPtr samples, int sampleCount, int channels, IntPtr channelSpeakerArray, ref Speakers channelFillMask);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnEditCapturedVoiceDataEventHandler(ulong serverConnectionHandlerID, IntPtr samples, int sampleCount, int channels, ref int edited);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnCustom3dRolloffCalculationClientEventHandler(ulong serverConnectionHandlerID, ushort clientID, float distance, ref float volume);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnCustom3dRolloffCalculationWaveEventHandler(ulong serverConnectionHandlerID, ulong waveHandle, float distance, ref float volume);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnUserLoggingMessageEventHandler(IntPtr logmessage, int logLevel, IntPtr logChannel, ulong logID, IntPtr logTime, IntPtr completeLogString);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnCustomPacketEncryptEventHandler(ref IntPtr dataToSend, ref uint sizeOfData);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnCustomPacketDecryptEventHandler(ref IntPtr dataReceived, ref uint dataReceivedSize);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnProvisioningSlotRequestResultEventHandler(uint error, ulong requestHandle, IntPtr connectionKey);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnCheckServerUniqueIdentifierEventHandler(ulong serverConnectionHandlerID, IntPtr serverUniqueIdentifier, out bool cancelConnect);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnClientPasswordEncryptEventHandler(ulong serverConnectionHandlerID, IntPtr plaintext, IntPtr encryptedText, int encryptedTextByteSize);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnFileTransferStatusEventHandler(ushort transferID, uint status, IntPtr statusMessage, ulong remotefileSize, ulong serverConnectionHandlerID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnFileListEventHandler(ulong serverConnectionHandlerID, ulong channelID, IntPtr path, IntPtr name, ulong size, ulong datetime, int type, ulong incompletesize, IntPtr returnCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnFileListFinishedEventHandler(ulong serverConnectionHandlerID, ulong channelID, IntPtr path);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate void OnFileInfoEventHandler(ulong serverConnectionHandlerID, ulong channelID, IntPtr name, ulong size, ulong datetime);

        public readonly ClientUIFunctions ClientUIFunctions;

        /// <summary>
        /// This list ensures that the native event handlers are not garbage collected
        /// </summary>
        private List<Delegate> DelegateReferences = new List<Delegate>();
        public NativeEvents(bool useClientPasswordEncrypt, bool useCustomPacketEncrypt, bool useCustomPacketDecrypt)
        {
            SetNativeCallback(out ClientUIFunctions.OnConnectStatusChangeEvent, new OnConnectStatusChangeEventHandler(OnConnectStatusChangeEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnServerProtocolVersionEvent, new OnServerProtocolVersionEventHandler(OnServerProtocolVersionEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnNewChannelEvent, new OnNewChannelEventHandler(OnNewChannelEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnNewChannelCreatedEvent, new OnNewChannelCreatedEventHandler(OnNewChannelCreatedEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnDelChannelEvent, new OnDelChannelEventHandler(OnDelChannelEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnChannelMoveEvent, new OnChannelMoveEventHandler(OnChannelMoveEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnUpdateChannelEvent, new OnUpdateChannelEventHandler(OnUpdateChannelEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnUpdateChannelEditedEvent, new OnUpdateChannelEditedEventHandler(OnUpdateChannelEditedEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnUpdateClientEvent, new OnUpdateClientEventHandler(OnUpdateClientEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnClientMoveEvent, new OnClientMoveEventHandler(OnClientMoveEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnClientMoveSubscriptionEvent, new OnClientMoveSubscriptionEventHandler(OnClientMoveSubscriptionEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnClientMoveTimeoutEvent, new OnClientMoveTimeoutEventHandler(OnClientMoveTimeoutEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnClientMoveMovedEvent, new OnClientMoveMovedEventHandler(OnClientMoveMovedEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnClientKickFromChannelEvent, new OnClientKickFromChannelEventHandler(OnClientKickFromChannelEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnClientKickFromServerEvent, new OnClientKickFromServerEventHandler(OnClientKickFromServerEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnClientIDsEvent, new OnClientIDsEventHandler(OnClientIDsEventWrapper));
            ClientUIFunctions.OnClientIDsFinishedEvent = IntPtr.Zero;
            SetNativeCallback(out ClientUIFunctions.OnServerEditedEvent, new OnServerEditedEventHandler(OnServerEditedEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnServerUpdatedEvent, new OnServerUpdatedEventHandler(OnServerUpdatedEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnServerErrorEvent, new OnServerErrorEventHandler(OnServerErrorEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnServerStopEvent, new OnServerStopEventHandler(OnServerStopEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnTextMessageEvent, new OnTextMessageEventHandler(OnTextMessageEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnTalkStatusChangeEvent, new OnTalkStatusChangeEventHandler(OnTalkStatusChangeEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnIgnoredWhisperEvent, new OnIgnoredWhisperEventHandler(OnIgnoredWhisperEventWrapper));
            ClientUIFunctions.OnConnectionInfoEvent = IntPtr.Zero;
            ClientUIFunctions.OnServerConnectionInfoEvent = IntPtr.Zero;
            SetNativeCallback(out ClientUIFunctions.OnChannelSubscribeEvent, new OnChannelSubscribeEventHandler(OnChannelSubscribeEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnChannelSubscribeFinishedEvent, new OnChannelSubscribeFinishedEventHandler(OnChannelSubscribeFinishedEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnChannelUnsubscribeEvent, new OnChannelUnsubscribeEventHandler(OnChannelUnsubscribeEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnChannelUnsubscribeFinishedEvent, new OnChannelUnsubscribeFinishedEventHandler(OnChannelUnsubscribeFinishedEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnChannelDescriptionUpdateEvent, new OnChannelDescriptionUpdateEventHandler(OnChannelDescriptionUpdateEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnChannelPasswordChangedEvent, new OnChannelPasswordChangedEventHandler(OnChannelPasswordChangedEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnPlaybackShutdownCompleteEvent, new OnPlaybackShutdownCompleteEventHandler(OnPlaybackShutdownCompleteEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnSoundDeviceListChangedEvent, new OnSoundDeviceListChangedEventHandler(OnSoundDeviceListChangedEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnEditPlaybackVoiceDataEvent, new OnEditPlaybackVoiceDataEventHandler(OnEditPlaybackVoiceDataEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnEditPostProcessVoiceDataEvent, new OnEditPostProcessVoiceDataEventHandler(OnEditPostProcessVoiceDataEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnEditMixedPlaybackVoiceDataEvent, new OnEditMixedPlaybackVoiceDataEventHandler(OnEditMixedPlaybackVoiceDataEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnEditCapturedVoiceDataEvent, new OnEditCapturedVoiceDataEventHandler(OnEditCapturedVoiceDataEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnCustom3dRolloffCalculationClientEvent, new OnCustom3dRolloffCalculationClientEventHandler(OnCustom3dRolloffCalculationClientEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnCustom3dRolloffCalculationWaveEvent, new OnCustom3dRolloffCalculationWaveEventHandler(OnCustom3dRolloffCalculationWaveEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnUserLoggingMessageEvent, new OnUserLoggingMessageEventHandler(OnUserLoggingMessageEventWrapper));
            if (useCustomPacketEncrypt)
                SetNativeCallback(out ClientUIFunctions.OnCustomPacketEncryptEvent, new OnCustomPacketEncryptEventHandler(OnCustomPacketEncryptEventWrapper));
            else ClientUIFunctions.OnCustomPacketEncryptEvent = IntPtr.Zero;
            if (useCustomPacketDecrypt)
                SetNativeCallback(out ClientUIFunctions.OnCustomPacketDecryptEvent, new OnCustomPacketDecryptEventHandler(OnCustomPacketDecryptEventWrapper));
            else ClientUIFunctions.OnCustomPacketDecryptEvent = IntPtr.Zero;
            ClientUIFunctions.OnProvisioningSlotRequestResultEvent = IntPtr.Zero;
            SetNativeCallback(out ClientUIFunctions.OnCheckServerUniqueIdentifierEvent, new OnCheckServerUniqueIdentifierEventHandler(OnCheckServerUniqueIdentifierEventWrapper));
            if (useClientPasswordEncrypt)
                SetNativeCallback(out ClientUIFunctions.OnClientPasswordEncrypt, new OnClientPasswordEncryptEventHandler(OnClientPasswordEncryptEventWrapper));
            else ClientUIFunctions.OnClientPasswordEncrypt = IntPtr.Zero;
            SetNativeCallback(out ClientUIFunctions.OnFileTransferStatusEvent, new OnFileTransferStatusEventHandler(OnFileTransferStatusEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnFileListEvent, new OnFileListEventHandler(OnFileListEventWrapper));
            ClientUIFunctions.OnFileListFinishedEvent = IntPtr.Zero;
            SetNativeCallback(out ClientUIFunctions.OnFileInfoEvent, new OnFileInfoEventHandler(OnFileInfoEventWrapper));

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

        private void OnConnectStatusChangeEventWrapper(ulong serverConnectionHandlerID, int newStatus, uint errorNumber)
        {
            try
            {
                Library.GetServer(serverConnectionHandlerID).OnStatusChanged((ConnectStatus)newStatus, (Error)errorNumber);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnServerProtocolVersionEventWrapper(ulong serverConnectionHandlerID, int protocolVersion)
        {
            try
            {
                Library.GetServer(serverConnectionHandlerID).OnProtocolVersionReceived(protocolVersion);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnNewChannelEventWrapper(ulong serverConnectionHandlerID, ulong channelID, ulong channelParentID)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                Channel channel = connection.Cache.GetChannel(channelID);
                connection.ChannelTracker.ChannelAdded(channel, channelParentID);
                connection.OnNewChannel(channel);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnNewChannelCreatedEventWrapper(ulong serverConnectionHandlerID, ulong channelID, ulong channelParentID, ushort invokerID, IntPtr invokerName, IntPtr invokerUniqueIdentifier)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                Channel channel = connection.Cache.GetChannel(channelID);
                connection.ChannelTracker.ChannelAdded(channel, channelParentID);
                Client invoker = connection.Cache.GetClient(invokerID);
                invoker?.Hint(Native.ReadString(invokerName), Native.ReadString(invokerUniqueIdentifier));
                connection.OnNewChannelCreated(channel, invoker);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnDelChannelEventWrapper(ulong serverConnectionHandlerID, ulong channelID, ushort invokerID, IntPtr invokerName, IntPtr invokerUniqueIdentifier)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                Channel channel = connection.Cache.RemoveChannel(channelID);
                connection.ChannelTracker.ChannelRemoved(channel);
                Client invoker = connection.Cache.GetClient(invokerID);
                invoker?.Hint(Native.ReadString(invokerName), Native.ReadString(invokerUniqueIdentifier));
                connection.OnChannelDeleted(channel, invoker);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnChannelMoveEventWrapper(ulong serverConnectionHandlerID, ulong channelID, ulong newChannelParentID, ushort invokerID, IntPtr invokerName, IntPtr invokerUniqueIdentifier)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                Channel channel = connection.Cache.GetChannel(channelID);
                connection.ChannelTracker.ChannelMoved(channel, newChannelParentID);
                Client invoker = connection.Cache.GetClient(invokerID);
                invoker?.Hint(Native.ReadString(invokerName), Native.ReadString(invokerUniqueIdentifier));
                connection.OnChannelMoved(channel, invoker);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnUpdateChannelEventWrapper(ulong serverConnectionHandlerID, ulong channelID)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                Channel channel = connection.Cache.GetChannel(channelID);
                connection.ChannelTracker.CheckOrder(channel);
                connection.OnChannelChanged(channel, null);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnUpdateChannelEditedEventWrapper(ulong serverConnectionHandlerID, ulong channelID, ushort invokerID, IntPtr invokerName, IntPtr invokerUniqueIdentifier)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                Client invoker = connection.Cache.GetClient(invokerID);
                invoker?.Hint(Native.ReadString(invokerName), Native.ReadString(invokerUniqueIdentifier));
                Channel channel = connection.Cache.GetChannel(channelID);
                connection.ChannelTracker.CheckOrder(channel);
                connection.OnChannelChanged(channel, invoker);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnUpdateClientEventWrapper(ulong serverConnectionHandlerID, ushort clientID, ushort invokerID, IntPtr invokerName, IntPtr invokerUniqueIdentifier)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                Client invoker = connection.Cache.GetClient(invokerID);
                invoker?.Hint(Native.ReadString(invokerName), Native.ReadString(invokerUniqueIdentifier));
                connection.OnClientUpdated(connection.Cache.GetClient(clientID), invoker);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnClientMoveEventWrapper(ulong serverConnectionHandlerID, ushort clientID, ulong oldChannelID, ulong newChannelID, int visibility, IntPtr moveMessage)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                Visibility managed_Visibility = (Visibility)visibility;
                Client client = connection.Cache.GetOrRemoveClient(clientID, managed_Visibility);
                connection.OnClientMoved(
                    client,
                    connection.Cache.GetChannel(oldChannelID),
                    connection.Cache.GetChannel(newChannelID),
                    managed_Visibility,
                    client,
                    Native.ReadString(moveMessage));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnClientMoveSubscriptionEventWrapper(ulong serverConnectionHandlerID, ushort clientID, ulong oldChannelID, ulong newChannelID, int visibility)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                Visibility managed_Visibility = (Visibility)visibility;
                connection.OnSubscriptionClientMoved(
                    connection.Cache.GetOrRemoveClient(clientID, managed_Visibility),
                    connection.Cache.GetChannel(oldChannelID),
                    connection.Cache.GetChannel(newChannelID),
                    managed_Visibility);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnClientMoveTimeoutEventWrapper(ulong serverConnectionHandlerID, ushort clientID, ulong oldChannelID, ulong newChannelID, int visibility, IntPtr timeoutMessage)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                Visibility managed_Visibility = (Visibility)visibility;
                connection.OnClientTimeout(
                    connection.Cache.GetOrRemoveClient(clientID, managed_Visibility),
                    connection.Cache.GetChannel(oldChannelID),
                    connection.Cache.GetChannel(newChannelID),
                    managed_Visibility,
                    Native.ReadString(timeoutMessage));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnClientMoveMovedEventWrapper(ulong serverConnectionHandlerID, ushort clientID, ulong oldChannelID, ulong newChannelID, int visibility, ushort moverID, IntPtr moverName, IntPtr moverUniqueIdentifier, IntPtr moveMessage)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                Visibility managed_Visibility = (Visibility)visibility;
                Client mover = connection.Cache.GetClient(moverID);
                mover?.Hint(Native.ReadString(moverName), Native.ReadString(moverUniqueIdentifier));
                connection.OnClientMoved(
                    connection.Cache.GetOrRemoveClient(clientID, managed_Visibility),
                    connection.Cache.GetChannel(oldChannelID),
                    connection.Cache.GetChannel(newChannelID),
                    managed_Visibility,
                    mover,
                    Native.ReadString(moveMessage));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnClientKickFromChannelEventWrapper(ulong serverConnectionHandlerID, ushort clientID, ulong oldChannelID, ulong newChannelID, int visibility, ushort kickerID, IntPtr kickerName, IntPtr kickerUniqueIdentifier, IntPtr kickMessage)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                Visibility managed_Visibility = (Visibility)visibility;
                Client kicker = connection.Cache.GetClient(kickerID);
                kicker?.Hint(Native.ReadString(kickerName), Native.ReadString(kickerUniqueIdentifier));
                connection.OnClientKickedFromChannel(
                    connection.Cache.GetOrRemoveClient(clientID, managed_Visibility),
                    connection.Cache.GetChannel(oldChannelID),
                    connection.Cache.GetChannel(newChannelID),
                    managed_Visibility,
                    kicker,
                    Native.ReadString(kickMessage));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnClientKickFromServerEventWrapper(ulong serverConnectionHandlerID, ushort clientID, ulong oldChannelID, ulong newChannelID, int visibility, ushort kickerID, IntPtr kickerName, IntPtr kickerUniqueIdentifier, IntPtr kickMessage)
        {
            try
            {
                Connection connetion = Library.GetServer(serverConnectionHandlerID);
                Visibility managed_Visibility = (Visibility)visibility;
                Client kicker = connetion.Cache.GetClient(kickerID);
                kicker?.Hint(Native.ReadString(kickerName), Native.ReadString(kickerUniqueIdentifier));
                connetion.OnClientKickedFromServer(
                    connetion.Cache.GetOrRemoveClient(clientID, managed_Visibility),
                    connetion.Cache.GetChannel(oldChannelID),
                    connetion.Cache.GetChannel(newChannelID),
                    managed_Visibility,
                    kicker,
                    Native.ReadString(kickMessage));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnClientIDsEventWrapper(ulong serverConnectionHandlerID, IntPtr uniqueClientIdentifier, ushort clientID, IntPtr clientName)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                Client client = connection.Cache.GetClient(clientID);
                client?.Hint(Native.ReadString(uniqueClientIdentifier), Native.ReadString(clientName));
                connection.OnClientIDReceived(client);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnServerEditedEventWrapper(ulong serverConnectionHandlerID, ushort editerID, IntPtr editerName, IntPtr editerUniqueIdentifier)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                Client editer = connection.Cache.GetClient(editerID);
                editer?.Hint(Native.ReadString(editerName), Native.ReadString(editerUniqueIdentifier));
                connection.OnServerUpdated(editer);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnServerUpdatedEventWrapper(ulong serverConnectionHandlerID)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnServerUpdated(null);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnServerErrorEventWrapper(ulong serverConnectionHandlerID, IntPtr errorMessage, uint error, IntPtr returnCode, IntPtr extraMessage)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnServerError((Error)error, Native.ReadString(returnCode), Native.ReadString(extraMessage));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnServerStopEventWrapper(ulong serverConnectionHandlerID, IntPtr shutdownMessage)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnServerStop(Native.ReadString(shutdownMessage));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnTextMessageEventWrapper(ulong serverConnectionHandlerID, ushort targetMode, ushort toID, ushort fromID, IntPtr fromName, IntPtr fromUniqueIdentifier, IntPtr message)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                string managed_message = Native.ReadString(message);
                Client from = connection.Cache.GetClient(fromID);
                from?.Hint(Native.ReadString(fromName), Native.ReadString(fromUniqueIdentifier));
                switch ((TargetMode)targetMode)
                {
                    case TargetMode.Client:
                        Client to = connection.Cache.GetClient(toID);
                        connection.OnClientMessage(from, to, managed_message);
                        break;
                    case TargetMode.Channel:
                        connection.OnChannelMessage(from, managed_message);
                        break;
                    case TargetMode.Server:
                        connection.OnServerMessage(from, managed_message);
                        break;
                    default: Debug.Assert(false); goto case TargetMode.Server;
                }
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnTalkStatusChangeEventWrapper(ulong serverConnectionHandlerID, int status, int isReceivedWhisper, ushort clientID)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnTalkStatusChanged(
                    connection.Cache.GetClient(clientID),
                    (TalkStatus)status,
                    isReceivedWhisper != 0);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnIgnoredWhisperEventWrapper(ulong serverConnectionHandlerID, ushort clientID)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnWhisperIgnored(connection.Cache.GetClient(clientID));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnChannelSubscribeEventWrapper(ulong serverConnectionHandlerID, ulong channelID)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnChannelSubscribed(connection.Cache.GetChannel(channelID));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnChannelSubscribeFinishedEventWrapper(ulong serverConnectionHandlerID)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnChannelSubscribesFinished();
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnChannelUnsubscribeEventWrapper(ulong serverConnectionHandlerID, ulong channelID)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnChannelUnsubscribed(connection.Cache.GetChannel(channelID));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnChannelUnsubscribeFinishedEventWrapper(ulong serverConnectionHandlerID)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnChannelUnsubscribesFinished();
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnChannelDescriptionUpdateEventWrapper(ulong serverConnectionHandlerID, ulong channelID)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnChannelDescriptionUpdated(connection.Cache.GetChannel(channelID));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnChannelPasswordChangedEventWrapper(ulong serverConnectionHandlerID, ulong channelID)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnChannelPasswordChanged(connection.Cache.GetChannel(channelID));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnPlaybackShutdownCompleteEventWrapper(ulong serverConnectionHandlerID)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnPlaybackShutdownCompleted();
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnSoundDeviceListChangedEventWrapper(IntPtr modeID, int playOrCap)
        {
            try
            {
                Library.OnSoundDeviceListChanged(Native.ReadString(modeID), playOrCap != 0);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnEditPlaybackVoiceDataEventWrapper(ulong serverConnectionHandlerID, ushort clientID, IntPtr samples, int sampleCount, int channels)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnEditPlaybackVoiceData(connection.Cache.GetClient(clientID), samples, sampleCount, channels);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnEditPostProcessVoiceDataEventWrapper(ulong serverConnectionHandlerID, ushort clientID, IntPtr samples, int sampleCount, int channels, IntPtr channelSpeakerArray, ref Speakers channelFillMask)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnEditPostProcessVoiceData(
                    connection.Cache.GetClient(clientID),
                    samples, sampleCount, channels, channelSpeakerArray, ref channelFillMask);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnEditMixedPlaybackVoiceDataEventWrapper(ulong serverConnectionHandlerID, IntPtr samples, int sampleCount, int channels, IntPtr channelSpeakerArray, ref Speakers channelFillMask)
        {
            try
            {
                Library.GetServer(serverConnectionHandlerID).OnEditMixedPlaybackVoiceData(
                samples, sampleCount, channels, channelSpeakerArray, ref channelFillMask);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnEditCapturedVoiceDataEventWrapper(ulong serverConnectionHandlerID, IntPtr samples, int sampleCount, int channels, ref int edited)
        {
            try
            {
                bool managed_edited = (edited & 1) == 1;
                bool managed_cancel = (edited & 2) == 0;
                Library.GetServer(serverConnectionHandlerID).OnEditCapturedVoiceData(
                    samples, sampleCount, channels,
                    ref managed_edited, ref managed_cancel);
                edited = (managed_edited ? 1 : 0) | (managed_cancel ? 0 : 2);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnCustom3dRolloffCalculationClientEventWrapper(ulong serverConnectionHandlerID, ushort clientID, float distance, ref float volume)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnCustom3dRolloffCalculationClient(connection.Cache.GetClient(clientID), distance, ref volume);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnCustom3dRolloffCalculationWaveEventWrapper(ulong serverConnectionHandlerID, ulong waveHandle, float distance, ref float volume)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnCustom3dRolloffCalculationWave(connection.Cache.GetWaveHandle(waveHandle), distance, ref volume);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnUserLoggingMessageEventWrapper(IntPtr logmessage, int logLevel, IntPtr logChannel, ulong logID, IntPtr logTime, IntPtr completeLogString)
        {
            try
            {
                Connection connection = Library.GetServer(logID);
                Library.OnUserLoggingMessage(Native.ReadString(logmessage), (LogLevel)logLevel, Native.ReadString(logChannel), connection, Native.ReadString(logTime), Native.ReadString(completeLogString));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnCustomPacketEncryptEventWrapper(ref IntPtr dataToSend, ref uint sizeOfData)
        {
            try
            {
                Library.OnCustomPacketEncrypt(ref dataToSend, ref sizeOfData);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnCustomPacketDecryptEventWrapper(ref IntPtr dataReceived, ref uint dataReceivedSize)
        {
            try
            {
                Library.OnCustomPacketDecrypt(ref dataReceived, ref dataReceivedSize);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnCheckServerUniqueIdentifierEventWrapper(ulong serverConnectionHandlerID, IntPtr serverUniqueIdentifier, out bool cancelConnect)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnCheckUniqueIdentifier(Native.ReadString(serverUniqueIdentifier), out cancelConnect);
            }
            catch (Exception e) { cancelConnect = true; if (HandleException(e) == false) throw; }
        }

        private void OnClientPasswordEncryptEventWrapper(ulong serverConnectionHandlerID, IntPtr plaintext, IntPtr encryptedText, int encryptedTextByteSize)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                string ciphertext = Library.OnClientPasswordEncrypt(connection, Native.ReadString(plaintext), encryptedTextByteSize - 1);
                Native.WriteString(ciphertext ?? string.Empty, encryptedText, encryptedTextByteSize);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnFileTransferStatusEventWrapper(ushort transferID, uint status, IntPtr statusMessage, ulong remotefileSize, ulong serverConnectionHandlerID)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnFileTransferStatusReceived(connection.Cache.GetTransfer(transferID), (Error)status);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnFileListEventWrapper(ulong serverConnectionHandlerID, ulong channelID, IntPtr path, IntPtr name, ulong size, ulong datetime, int type, ulong incompletesize, IntPtr returnCode)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnFileListReceived(
                    connection.Cache.GetChannel(channelID),
                    Native.ReadString(path),
                    Native.ReadString(name),
                    size,
                    Native.FromUnixTime(datetime),
                    (FileListType)type,
                    incompletesize,
                    Native.ReadString(returnCode));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private void OnFileInfoEventWrapper(ulong serverConnectionHandlerID, ulong channelID, IntPtr name, ulong size, ulong datetime)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnFileInfoReceived(
                    connection.Cache.GetChannel(channelID),
                    Native.ReadString(name),
                    size,
                    Native.FromUnixTime(datetime));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }
    }
}
