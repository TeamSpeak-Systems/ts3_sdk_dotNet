using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

#if UNITY_IOS &&!UNITY_EDITOR
public class MonoPInvokeCallbackAttribute : System.Attribute
{
    private Type type;
    public MonoPInvokeCallbackAttribute( Type t ) { type = t; }
}
#endif

namespace TeamSpeak.Sdk.Client
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ClientUIFunctions
    {
        public IntPtr OnMessage;
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
#if !TEAMSPEAK_SDK_3_0
        public IntPtr OnEditCapturedVoiceDataPreprocessEvent;
#endif
        public IntPtr OnEditCapturedVoiceDataEvent;
        public IntPtr OnCustom3dRolloffCalculationClientEvent;
        public IntPtr OnCustom3dRolloffCalculationWaveEvent;
        public IntPtr OnUserLoggingMessageEvent;
        public IntPtr OnCustomPacketEncryptEvent;
        public IntPtr OnCustomPacketDecryptEvent;
        public IntPtr OnCheckServerUniqueIdentifierEvent;
        public IntPtr OnClientPasswordEncrypt;
        public IntPtr OnFileTransferStatusEvent;
        public IntPtr OnFileListEvent;
        public IntPtr OnFileListFinishedEvent;
        public IntPtr OnFileInfoEvent;
        public IntPtr OnChatLoginTokenEvent;
        public IntPtr OnAuthenticationTokenEvent;
        public IntPtr OnScreenshareSessionEvent;
        public IntPtr OnJsonReply;
        public IntPtr OnSendCallToMatrix;
        public IntPtr OnProtoResponse;
        public IntPtr OnProtoEvent;
    }

    partial class NativeEvents
    {
        #region delegates

#if UNITY_EDITOR || !UNITY_ANDROID
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnMessageHandler(IntPtr msg);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnConnectStatusChangeEventHandler(ulong serverConnectionHandlerID, int newStatus, uint errorNumber);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnServerProtocolVersionEventHandler(ulong serverConnectionHandlerID, int protocolVersion);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnNewChannelEventHandler(ulong serverConnectionHandlerID, ulong channelID, ulong channelParentID);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnNewChannelCreatedEventHandler(ulong serverConnectionHandlerID, ulong channelID, ulong channelParentID, ushort invokerID, IntPtr invokerName, IntPtr invokerUniqueIdentifier);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnDelChannelEventHandler(ulong serverConnectionHandlerID, ulong channelID, ushort invokerID, IntPtr invokerName, IntPtr invokerUniqueIdentifier);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnChannelMoveEventHandler(ulong serverConnectionHandlerID, ulong channelID, ulong newChannelParentID, ushort invokerID, IntPtr invokerName, IntPtr invokerUniqueIdentifier);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnUpdateChannelEventHandler(ulong serverConnectionHandlerID, ulong channelID);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnUpdateChannelEditedEventHandler(ulong serverConnectionHandlerID, ulong channelID, ushort invokerID, IntPtr invokerName, IntPtr invokerUniqueIdentifier);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnUpdateClientEventHandler(ulong serverConnectionHandlerID, ushort clientID, ushort invokerID, IntPtr invokerName, IntPtr invokerUniqueIdentifier);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnClientMoveEventHandler(ulong serverConnectionHandlerID, ushort clientID, ulong oldChannelID, ulong newChannelID, int visibility, IntPtr moveMessage);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnClientMoveSubscriptionEventHandler(ulong serverConnectionHandlerID, ushort clientID, ulong oldChannelID, ulong newChannelID, int visibility);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnClientMoveTimeoutEventHandler(ulong serverConnectionHandlerID, ushort clientID, ulong oldChannelID, ulong newChannelID, int visibility, IntPtr timeoutMessage);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnClientMoveMovedEventHandler(ulong serverConnectionHandlerID, ushort clientID, ulong oldChannelID, ulong newChannelID, int visibility, ushort moverID, IntPtr moverName, IntPtr moverUniqueIdentifier, IntPtr moveMessage);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnClientKickFromChannelEventHandler(ulong serverConnectionHandlerID, ushort clientID, ulong oldChannelID, ulong newChannelID, int visibility, ushort kickerID, IntPtr kickerName, IntPtr kickerUniqueIdentifier, IntPtr kickMessage);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnClientKickFromServerEventHandler(ulong serverConnectionHandlerID, ushort clientID, ulong oldChannelID, ulong newChannelID, int visibility, ushort kickerID, IntPtr kickerName, IntPtr kickerUniqueIdentifier, IntPtr kickMessage);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnClientIDsEventHandler(ulong serverConnectionHandlerID, IntPtr uniqueClientIdentifier, ushort clientID, IntPtr clientName);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnClientIDsFinishedEventHandler(ulong serverConnectionHandlerID);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnServerEditedEventHandler(ulong serverConnectionHandlerID, ushort editerID, IntPtr editerName, IntPtr editerUniqueIdentifier);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnServerUpdatedEventHandler(ulong serverConnectionHandlerID);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnServerErrorEventHandler(ulong serverConnectionHandlerID, IntPtr errorMessage, uint error, IntPtr returnCode, IntPtr extraMessage);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnServerStopEventHandler(ulong serverConnectionHandlerID, IntPtr shutdownMessage);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnTextMessageEventHandler(ulong serverConnectionHandlerID, ushort targetMode, ushort toID, ushort fromID, IntPtr fromName, IntPtr fromUniqueIdentifier, IntPtr message);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnTalkStatusChangeEventHandler(ulong serverConnectionHandlerID, int status, int isReceivedWhisper, ushort clientID);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnIgnoredWhisperEventHandler(ulong serverConnectionHandlerID, ushort clientID);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnConnectionInfoEventHandler(ulong serverConnectionHandlerID, ushort clientID);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnServerConnectionInfoEventHandler(ulong serverConnectionHandlerID);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnChannelSubscribeEventHandler(ulong serverConnectionHandlerID, ulong channelID);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnChannelSubscribeFinishedEventHandler(ulong serverConnectionHandlerID);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnChannelUnsubscribeEventHandler(ulong serverConnectionHandlerID, ulong channelID);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnChannelUnsubscribeFinishedEventHandler(ulong serverConnectionHandlerID);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnChannelDescriptionUpdateEventHandler(ulong serverConnectionHandlerID, ulong channelID);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnChannelPasswordChangedEventHandler(ulong serverConnectionHandlerID, ulong channelID);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnPlaybackShutdownCompleteEventHandler(ulong serverConnectionHandlerID);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnSoundDeviceListChangedEventHandler(IntPtr modeID, int playOrCap);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnEditPlaybackVoiceDataEventHandler(ulong serverConnectionHandlerID, ushort clientID, IntPtr samples, int sampleCount, int channels);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnEditPostProcessVoiceDataEventHandler(ulong serverConnectionHandlerID, ushort clientID, IntPtr samples, int sampleCount, int channels, IntPtr channelSpeakerArray, ref Speakers channelFillMask);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnEditMixedPlaybackVoiceDataEventHandler(ulong serverConnectionHandlerID, IntPtr samples, int sampleCount, int channels, IntPtr channelSpeakerArray, ref Speakers channelFillMask);
#if !TEAMSPEAK_SDK_3_0
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnEditCapturedVoiceDataPreprocessEventHandler(ulong serverConnectionHandlerID, IntPtr samples, int sampleCount, int channels, ref int flags);
#endif
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnEditCapturedVoiceDataEventHandler(ulong serverConnectionHandlerID, IntPtr samples, int sampleCount, int channels, ref int edited);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnCustom3dRolloffCalculationClientEventHandler(ulong serverConnectionHandlerID, ushort clientID, float distance, ref float volume);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnCustom3dRolloffCalculationWaveEventHandler(ulong serverConnectionHandlerID, ulong waveHandle, float distance, ref float volume);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnUserLoggingMessageEventHandler(IntPtr logmessage, int logLevel, IntPtr logChannel, ulong logID, IntPtr logTime, IntPtr completeLogString);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnCustomPacketEncryptEventHandler(ref IntPtr dataToSend, ref uint sizeOfData);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnCustomPacketDecryptEventHandler(ref IntPtr dataReceived, ref uint dataReceivedSize);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnCheckServerUniqueIdentifierEventHandler(ulong serverConnectionHandlerID, IntPtr serverUniqueIdentifier, out bool cancelConnect);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnClientPasswordEncryptEventHandler(ulong serverConnectionHandlerID, IntPtr plaintext, IntPtr encryptedText, int encryptedTextByteSize);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnFileTransferStatusEventHandler(ushort transferID, uint status, IntPtr statusMessage, ulong remotefileSize, ulong serverConnectionHandlerID);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnFileListEventHandler(ulong serverConnectionHandlerID, ulong channelID, IntPtr path, IntPtr name, ulong size, ulong datetime, int type, ulong incompletesize, IntPtr returnCode);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnFileListFinishedEventHandler(ulong serverConnectionHandlerID, ulong channelID, IntPtr path);
#if UNITY_EDITOR || !UNITY_IOS
        [UnmanagedFunctionPointer(Native.CallingConvention)]
#endif
        delegate void OnFileInfoEventHandler(ulong serverConnectionHandlerID, ulong channelID, IntPtr name, ulong size, ulong datetime);
#endif
        #endregion delegates

#if UNITY_EDITOR || !UNITY_ANDROID
        public readonly ClientUIFunctions ClientUIFunctions;
#endif

        /// <summary>
        /// This list ensures that the native event handlers are not garbage collected
        /// </summary>
        private List<Delegate> DelegateReferences = new List<Delegate>();
        public NativeEvents(bool useClientPasswordEncrypt, bool useCustomPacketEncrypt, bool useCustomPacketDecrypt)
        {
#if UNITY_EDITOR || !UNITY_ANDROID
            SetNativeCallback(out ClientUIFunctions.OnMessage, new OnMessageHandler(OnMessageWrapper));
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
            SetNativeCallback(out ClientUIFunctions.OnClientIDsFinishedEvent, new OnClientIDsFinishedEventHandler(OnClientIDsFinishedEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnServerEditedEvent, new OnServerEditedEventHandler(OnServerEditedEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnServerUpdatedEvent, new OnServerUpdatedEventHandler(OnServerUpdatedEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnServerErrorEvent, new OnServerErrorEventHandler(OnServerErrorEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnServerStopEvent, new OnServerStopEventHandler(OnServerStopEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnTextMessageEvent, new OnTextMessageEventHandler(OnTextMessageEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnTalkStatusChangeEvent, new OnTalkStatusChangeEventHandler(OnTalkStatusChangeEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnIgnoredWhisperEvent, new OnIgnoredWhisperEventHandler(OnIgnoredWhisperEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnConnectionInfoEvent, new OnConnectionInfoEventHandler(OnConnectionInfoEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnServerConnectionInfoEvent, new OnServerConnectionInfoEventHandler(OnServerConnectionInfoEventWrapper));
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
#if !TEAMSPEAK_SDK_3_0
            SetNativeCallback(out ClientUIFunctions.OnEditCapturedVoiceDataPreprocessEvent, new OnEditCapturedVoiceDataPreprocessEventHandler(OnEditCapturedVoiceDataPreprocessEventWrapper));
#endif
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
            SetNativeCallback(out ClientUIFunctions.OnCheckServerUniqueIdentifierEvent, new OnCheckServerUniqueIdentifierEventHandler(OnCheckServerUniqueIdentifierEventWrapper));
            if (useClientPasswordEncrypt)
                SetNativeCallback(out ClientUIFunctions.OnClientPasswordEncrypt, new OnClientPasswordEncryptEventHandler(OnClientPasswordEncryptEventWrapper));
            else ClientUIFunctions.OnClientPasswordEncrypt = IntPtr.Zero;
            SetNativeCallback(out ClientUIFunctions.OnFileTransferStatusEvent, new OnFileTransferStatusEventHandler(OnFileTransferStatusEventWrapper));
            SetNativeCallback(out ClientUIFunctions.OnFileListEvent, new OnFileListEventHandler(OnFileListEventWrapper));
            ClientUIFunctions.OnFileListFinishedEvent = IntPtr.Zero;
            SetNativeCallback(out ClientUIFunctions.OnFileInfoEvent, new OnFileInfoEventHandler(OnFileInfoEventWrapper));
            ClientUIFunctions.OnChatLoginTokenEvent = IntPtr.Zero;
            ClientUIFunctions.OnAuthenticationTokenEvent = IntPtr.Zero;
            ClientUIFunctions.OnScreenshareSessionEvent = IntPtr.Zero;
            ClientUIFunctions.OnJsonReply = IntPtr.Zero;
            ClientUIFunctions.OnSendCallToMatrix = IntPtr.Zero;
            ClientUIFunctions.OnProtoResponse = IntPtr.Zero;
            ClientUIFunctions.OnProtoEvent = IntPtr.Zero;
#endif
        }

        private void SetNativeCallback(out IntPtr field, Delegate @delegate)
        {
            DelegateReferences.Add(@delegate);
            field = Marshal.GetFunctionPointerForDelegate(@delegate);
        }

        /* il2cpp Unity ios doesn't support members, hence we're going static. */

        private static bool HandleException(Exception e)
        {
            if (e is System.Threading.ThreadAbortException || e is System.Threading.ThreadInterruptedException)
                return false;
            Debug.Assert(false, "exception on callback thread");
            return true;
        }

        #region event_wrappers

#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnMessageHandler))]
#endif
        private static void OnMessageWrapper(IntPtr msg)
        {
            try
            {
                Debug.WriteLine("[TeamSpeak] " + Native.ReadString(msg));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnConnectStatusChangeEventHandler))]
#endif
        private static void OnConnectStatusChangeEventWrapper(ulong serverConnectionHandlerID, int newStatus, uint errorNumber)
        {
            try
            {
                Library.GetServer(serverConnectionHandlerID).OnStatusChanged((ConnectStatus)newStatus, (Error)errorNumber);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnServerProtocolVersionEventHandler))]
#endif
        private static void OnServerProtocolVersionEventWrapper(ulong serverConnectionHandlerID, int protocolVersion)
        {
            try
            {
                Library.GetServer(serverConnectionHandlerID).OnProtocolVersionReceived(protocolVersion);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnNewChannelEventHandler))]
#endif
        private static void OnNewChannelEventWrapper(ulong serverConnectionHandlerID, ulong channelID, ulong channelParentID)
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

        private static void OnNewChannelCreated(ulong serverConnectionHandlerID, ulong channelID, ulong channelParentID, ushort invokerID, string invokerName, string invokerUniqueIdentifier)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                Channel channel = connection.Cache.GetChannel(channelID);
                connection.ChannelTracker.ChannelAdded(channel, channelParentID);
                Client invoker = connection.Cache.GetClient(invokerID);
                invoker?.Hint(invokerName, invokerUniqueIdentifier);
                connection.OnNewChannelCreated(channel, invoker);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }
#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnNewChannelCreatedEventHandler))]
#endif
        private static void OnNewChannelCreatedEventWrapper(ulong serverConnectionHandlerID, ulong channelID, ulong channelParentID, ushort invokerID, IntPtr invokerName, IntPtr invokerUniqueIdentifier)
        {
            try
            {
                OnNewChannelCreated(serverConnectionHandlerID, channelID, channelParentID, invokerID, Native.ReadString(invokerName), Native.ReadString(invokerUniqueIdentifier));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private static void OnDelChannel(ulong serverConnectionHandlerID, ulong channelID, ushort invokerID, string invokerName, string invokerUniqueIdentifier)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                Channel channel = connection.Cache.RemoveChannel(channelID);
                connection.ChannelTracker.ChannelRemoved(channel);
                Client invoker = connection.Cache.GetClient(invokerID);
                invoker?.Hint(invokerName, invokerUniqueIdentifier);
                connection.OnChannelDeleted(channel, invoker);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }
#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnDelChannelEventHandler))]
#endif
        private static void OnDelChannelEventWrapper(ulong serverConnectionHandlerID, ulong channelID, ushort invokerID, IntPtr invokerName, IntPtr invokerUniqueIdentifier)
        {
            try
            {
                OnDelChannel(serverConnectionHandlerID, channelID, invokerID, Native.ReadString(invokerName), Native.ReadString(invokerUniqueIdentifier));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private static void OnChannelMove(ulong serverConnectionHandlerID, ulong channelID, ulong newChannelParentID, ushort invokerID, string invokerName, string invokerUniqueIdentifier)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                Channel channel = connection.Cache.GetChannel(channelID);
                connection.ChannelTracker.ChannelMoved(channel, newChannelParentID);
                Client invoker = connection.Cache.GetClient(invokerID);
                invoker?.Hint(invokerName, invokerUniqueIdentifier);
                connection.OnChannelMoved(channel, invoker);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }
#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnChannelMoveEventHandler))]
#endif
        private static void OnChannelMoveEventWrapper(ulong serverConnectionHandlerID, ulong channelID, ulong newChannelParentID, ushort invokerID, IntPtr invokerName, IntPtr invokerUniqueIdentifier)
        {
            try
            {
                OnChannelMove(serverConnectionHandlerID, channelID, newChannelParentID, invokerID, Native.ReadString(invokerName), Native.ReadString(invokerUniqueIdentifier));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnUpdateChannelEventHandler))]
#endif
        private static void OnUpdateChannelEventWrapper(ulong serverConnectionHandlerID, ulong channelID)
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

        private static void OnUpdateChannelEdited(ulong serverConnectionHandlerID, ulong channelID, ushort invokerID, string invokerName, string invokerUniqueIdentifier)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                Client invoker = connection.Cache.GetClient(invokerID);
                invoker?.Hint(invokerName, invokerUniqueIdentifier);
                Channel channel = connection.Cache.GetChannel(channelID);
                connection.ChannelTracker.CheckOrder(channel);
                connection.OnChannelChanged(channel, invoker);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }
#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnUpdateChannelEditedEventHandler))]
#endif
        private static void OnUpdateChannelEditedEventWrapper(ulong serverConnectionHandlerID, ulong channelID, ushort invokerID, IntPtr invokerName, IntPtr invokerUniqueIdentifier)
        {
            try
            {
                OnUpdateChannelEdited(serverConnectionHandlerID, channelID, invokerID, Native.ReadString(invokerName), Native.ReadString(invokerUniqueIdentifier));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private static void OnUpdateClient(ulong serverConnectionHandlerID, ushort clientID, ushort invokerID, string invokerName, string invokerUniqueIdentifier)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                Client invoker = connection.Cache.GetClient(invokerID);
                invoker?.Hint(invokerName, invokerUniqueIdentifier);
                connection.OnClientUpdated(connection.Cache.GetClient(clientID), invoker);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }
#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnUpdateClientEventHandler))]
#endif
        private static void OnUpdateClientEventWrapper(ulong serverConnectionHandlerID, ushort clientID, ushort invokerID, IntPtr invokerName, IntPtr invokerUniqueIdentifier)
        {
            try
            {
                OnUpdateClient(serverConnectionHandlerID, clientID, invokerID, Native.ReadString(invokerName), Native.ReadString(invokerUniqueIdentifier));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private static void OnClientMove(ulong serverConnectionHandlerID, ushort clientID, ulong oldChannelID, ulong newChannelID, int visibility, string moveMessage)
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
                    moveMessage);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }
#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnClientMoveEventHandler))]
#endif
        private static void OnClientMoveEventWrapper(ulong serverConnectionHandlerID, ushort clientID, ulong oldChannelID, ulong newChannelID, int visibility, IntPtr moveMessage)
        {
            try
            {
                OnClientMove(serverConnectionHandlerID, clientID, oldChannelID, newChannelID, visibility, Native.ReadString(moveMessage));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnClientMoveSubscriptionEventHandler))]
#endif
        private static void OnClientMoveSubscriptionEventWrapper(ulong serverConnectionHandlerID, ushort clientID, ulong oldChannelID, ulong newChannelID, int visibility)
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

        private static void OnClientMoveTimeout(ulong serverConnectionHandlerID, ushort clientID, ulong oldChannelID, ulong newChannelID, int visibility, string timeoutMessage)
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
                    timeoutMessage);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }
#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnClientMoveTimeoutEventHandler))]
#endif
        private static void OnClientMoveTimeoutEventWrapper(ulong serverConnectionHandlerID, ushort clientID, ulong oldChannelID, ulong newChannelID, int visibility, IntPtr timeoutMessage)
        {
            try
            {
                OnClientMoveTimeout(serverConnectionHandlerID, clientID, oldChannelID, newChannelID, visibility, Native.ReadString(timeoutMessage));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private static void OnClientMoveMoved(ulong serverConnectionHandlerID, ushort clientID, ulong oldChannelID, ulong newChannelID, int visibility, ushort moverID, string moverName, string moverUniqueIdentifier, string moveMessage)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                Visibility managed_Visibility = (Visibility)visibility;
                Client mover = connection.Cache.GetClient(moverID);
                mover?.Hint(moverName, moverUniqueIdentifier);
                connection.OnClientMoved(
                    connection.Cache.GetOrRemoveClient(clientID, managed_Visibility),
                    connection.Cache.GetChannel(oldChannelID),
                    connection.Cache.GetChannel(newChannelID),
                    managed_Visibility,
                    mover,
                    moveMessage);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }
#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnClientMoveMovedEventHandler))]
#endif
        private static void OnClientMoveMovedEventWrapper(ulong serverConnectionHandlerID, ushort clientID, ulong oldChannelID, ulong newChannelID, int visibility, ushort moverID, IntPtr moverName, IntPtr moverUniqueIdentifier, IntPtr moveMessage)
        {
            try
            {
                OnClientMoveMoved(serverConnectionHandlerID, clientID, oldChannelID, newChannelID, visibility, moverID, Native.ReadString(moverName), Native.ReadString(moverUniqueIdentifier), Native.ReadString(moveMessage));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private static void OnClientKickFromChannel(ulong serverConnectionHandlerID, ushort clientID, ulong oldChannelID, ulong newChannelID, int visibility, ushort kickerID, string kickerName, string kickerUniqueIdentifier, string kickMessage)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                Visibility managed_Visibility = (Visibility)visibility;
                Client kicker = connection.Cache.GetClient(kickerID);
                kicker?.Hint(kickerName, kickerUniqueIdentifier);
                connection.OnClientKickedFromChannel(
                    connection.Cache.GetOrRemoveClient(clientID, managed_Visibility),
                    connection.Cache.GetChannel(oldChannelID),
                    connection.Cache.GetChannel(newChannelID),
                    managed_Visibility,
                    kicker,
                    kickMessage);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }
#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnClientKickFromChannelEventHandler))]
#endif
        private static void OnClientKickFromChannelEventWrapper(ulong serverConnectionHandlerID, ushort clientID, ulong oldChannelID, ulong newChannelID, int visibility, ushort kickerID, IntPtr kickerName, IntPtr kickerUniqueIdentifier, IntPtr kickMessage)
        {
            try
            {
                OnClientKickFromChannel(serverConnectionHandlerID, clientID, oldChannelID, newChannelID, visibility, kickerID, Native.ReadString(kickerName), Native.ReadString(kickerUniqueIdentifier), Native.ReadString(kickMessage));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private static void OnClientKickFromServer(ulong serverConnectionHandlerID, ushort clientID, ulong oldChannelID, ulong newChannelID, int visibility, ushort kickerID, string kickerName, string kickerUniqueIdentifier, string kickMessage)
        {
            try
            {
                Connection connetion = Library.GetServer(serverConnectionHandlerID);
                Visibility managed_Visibility = (Visibility)visibility;
                Client kicker = connetion.Cache.GetClient(kickerID);
                kicker?.Hint(kickerName, kickerUniqueIdentifier);
                connetion.OnClientKickedFromServer(
                    connetion.Cache.GetOrRemoveClient(clientID, managed_Visibility),
                    connetion.Cache.GetChannel(oldChannelID),
                    connetion.Cache.GetChannel(newChannelID),
                    managed_Visibility,
                    kicker,
                    kickMessage);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }
#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnClientKickFromServerEventHandler))]
#endif
        private static void OnClientKickFromServerEventWrapper(ulong serverConnectionHandlerID, ushort clientID, ulong oldChannelID, ulong newChannelID, int visibility, ushort kickerID, IntPtr kickerName, IntPtr kickerUniqueIdentifier, IntPtr kickMessage)
        {
            try
            {
                OnClientKickFromServer(serverConnectionHandlerID, clientID, oldChannelID, newChannelID, visibility, kickerID, Native.ReadString(kickerName), Native.ReadString(kickerUniqueIdentifier), Native.ReadString(kickMessage));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private static void OnClientIDs(ulong serverConnectionHandlerID, string uniqueClientIdentifier, ushort clientID, string clientName)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                Client client = connection.Cache.GetClient(clientID);
                client?.Hint(uniqueClientIdentifier, clientName);
                connection.OnClientIDReceived(client);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }
#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnClientIDsEventHandler))]
#endif
        private static void OnClientIDsEventWrapper(ulong serverConnectionHandlerID, IntPtr uniqueClientIdentifier, ushort clientID, IntPtr clientName)
        {
            try
            {
                OnClientIDs(serverConnectionHandlerID, Native.ReadString(uniqueClientIdentifier), clientID, Native.ReadString(clientName));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnClientIDsFinishedEventHandler))]
#endif
        private static void OnClientIDsFinishedEventWrapper(ulong serverConnectionHandlerID)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                // TODO
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private static void OnServerEdited(ulong serverConnectionHandlerID, ushort editerID, string editerName, string editerUniqueIdentifier)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                Client editer = connection.Cache.GetClient(editerID);
                editer?.Hint(editerName, editerUniqueIdentifier);
                connection.OnServerUpdated(editer);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }
#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnServerEditedEventHandler))]
#endif
        private static void OnServerEditedEventWrapper(ulong serverConnectionHandlerID, ushort editerID, IntPtr editerName, IntPtr editerUniqueIdentifier)
        {
            try
            {
                OnServerEdited(serverConnectionHandlerID, editerID, Native.ReadString(editerName), Native.ReadString(editerUniqueIdentifier));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnServerUpdatedEventHandler))]
#endif
        private static void OnServerUpdatedEventWrapper(ulong serverConnectionHandlerID)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnServerUpdated(null);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private static void OnServerError(ulong serverConnectionHandlerID, string errorMessage, uint error, string returnCode, string extraMessage)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnServerError((Error)error, returnCode, extraMessage);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }
#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnServerErrorEventHandler))]
#endif
        private static void OnServerErrorEventWrapper(ulong serverConnectionHandlerID, IntPtr errorMessage, uint error, IntPtr returnCode, IntPtr extraMessage)
        {
            try
            {
                OnServerError(serverConnectionHandlerID, Native.ReadString(errorMessage), error, Native.ReadString(returnCode), Native.ReadString(extraMessage));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private static void OnServerStop(ulong serverConnectionHandlerID, string shutdownMessage)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnServerStop(shutdownMessage);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }
#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnServerStopEventHandler))]
#endif
        private static void OnServerStopEventWrapper(ulong serverConnectionHandlerID, IntPtr shutdownMessage)
        {
            try
            {
                OnServerStop(serverConnectionHandlerID, Native.ReadString(shutdownMessage));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private static void OnTextMessage(ulong serverConnectionHandlerID, ushort targetMode, ushort toID, ushort fromID, string fromName, string fromUniqueIdentifier, string message)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                Client from = connection.Cache.GetClient(fromID);
                from?.Hint(fromName, fromUniqueIdentifier);
                switch ((TargetMode)targetMode)
                {
                    case TargetMode.Client:
                        Client to = connection.Cache.GetClient(toID);
                        connection.OnClientMessage(from, to, message);
                        break;
                    case TargetMode.Channel:
                        connection.OnChannelMessage(from, message);
                        break;
                    case TargetMode.Server:
                        connection.OnServerMessage(from, message);
                        break;
                    default: goto case TargetMode.Server;
                }
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }
#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnTextMessageEventHandler))]
#endif
        private static void OnTextMessageEventWrapper(ulong serverConnectionHandlerID, ushort targetMode, ushort toID, ushort fromID, IntPtr fromName, IntPtr fromUniqueIdentifier, IntPtr message)
        {
            try
            {
                OnTextMessage(serverConnectionHandlerID, targetMode, toID, fromID, Native.ReadString(fromName), Native.ReadString(fromUniqueIdentifier), Native.ReadString(message));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnTalkStatusChangeEventHandler))]
#endif
        private static void OnTalkStatusChangeEventWrapper(ulong serverConnectionHandlerID, int status, int isReceivedWhisper, ushort clientID)
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

#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnIgnoredWhisperEventHandler))]
#endif
        private static void OnIgnoredWhisperEventWrapper(ulong serverConnectionHandlerID, ushort clientID)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnWhisperIgnored(connection.Cache.GetClient(clientID));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnConnectionInfoEventHandler))]
#endif
        private static void OnConnectionInfoEventWrapper(ulong serverConnectionHandlerID, ushort clientID)
        {
            // TODO
        }

#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnServerConnectionInfoEventHandler))]
#endif
        private static void OnServerConnectionInfoEventWrapper(ulong serverConnectionHandlerID)
        {
            // TODO
        }

#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnChannelSubscribeEventHandler))]
#endif
        private static void OnChannelSubscribeEventWrapper(ulong serverConnectionHandlerID, ulong channelID)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnChannelSubscribed(connection.Cache.GetChannel(channelID));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnChannelSubscribeFinishedEventHandler))]
#endif
        private static void OnChannelSubscribeFinishedEventWrapper(ulong serverConnectionHandlerID)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnChannelSubscribesFinished();
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnChannelUnsubscribeEventHandler))]
#endif
        private static void OnChannelUnsubscribeEventWrapper(ulong serverConnectionHandlerID, ulong channelID)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnChannelUnsubscribed(connection.Cache.GetChannel(channelID));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnChannelUnsubscribeFinishedEventHandler))]
#endif
        private static void OnChannelUnsubscribeFinishedEventWrapper(ulong serverConnectionHandlerID)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnChannelUnsubscribesFinished();
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnChannelDescriptionUpdateEventHandler))]
#endif
        private static void OnChannelDescriptionUpdateEventWrapper(ulong serverConnectionHandlerID, ulong channelID)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnChannelDescriptionUpdated(connection.Cache.GetChannel(channelID));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnChannelPasswordChangedEventHandler))]
#endif
        private static void OnChannelPasswordChangedEventWrapper(ulong serverConnectionHandlerID, ulong channelID)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnChannelPasswordChanged(connection.Cache.GetChannel(channelID));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnPlaybackShutdownCompleteEventHandler))]
#endif
        private static void OnPlaybackShutdownCompleteEventWrapper(ulong serverConnectionHandlerID)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnPlaybackShutdownCompleted();
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private static void OnSoundDeviceListChanged(string modeID, int playOrCap)
        {
            try
            {
                Library.OnSoundDeviceListChanged(modeID, playOrCap != 0);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }
#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnSoundDeviceListChangedEventHandler))]
#endif
        private static void OnSoundDeviceListChangedEventWrapper(IntPtr modeID, int playOrCap)
        {
            try
            {
                OnSoundDeviceListChanged(Native.ReadString(modeID), playOrCap);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnEditPlaybackVoiceDataEventHandler))]
#endif
        private static void OnEditPlaybackVoiceDataEventWrapper(ulong serverConnectionHandlerID, ushort clientID, IntPtr samples, int sampleCount, int channels)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnEditPlaybackVoiceData(connection.Cache.GetClient(clientID), samples, sampleCount, channels);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnEditPostProcessVoiceDataEventHandler))]
#endif
        private static void OnEditPostProcessVoiceDataEventWrapper(ulong serverConnectionHandlerID, ushort clientID, IntPtr samples, int sampleCount, int channels, IntPtr channelSpeakerArray, ref Speakers channelFillMask)
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

#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnEditMixedPlaybackVoiceDataEventHandler))]
#endif
        private static void OnEditMixedPlaybackVoiceDataEventWrapper(ulong serverConnectionHandlerID, IntPtr samples, int sampleCount, int channels, IntPtr channelSpeakerArray, ref Speakers channelFillMask)
        {
            try
            {
                Library.GetServer(serverConnectionHandlerID).OnEditMixedPlaybackVoiceData(
                samples, sampleCount, channels, channelSpeakerArray, ref channelFillMask);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        #region sdk_3_1
#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnEditCapturedVoiceDataPreprocessEventHandler))]
#endif
        private static void OnEditCapturedVoiceDataPreprocessEventWrapper(ulong serverConnectionHandlerID, IntPtr samples, int sampleCount, int channels, ref int flags)
        {
            // TODO
            try
            {
                Library.GetServer(serverConnectionHandlerID).OnEditCapturedVoiceDataPreprocess(
                    samples, sampleCount, channels,
                    ref flags);
                /*
                bool managed_edited = (flags & 1) == 1;
                bool managed_cancel = (flags & 2) == 0;
                Library.GetServer(serverConnectionHandlerID).OnEditCapturedVoiceDataPreprocess(
                    samples, sampleCount, channels,
                    ref managed_edited, ref managed_cancel);
                flags = (managed_edited ? 1 : 0) | (managed_cancel ? 0 : 2);
                */
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }
        #endregion sdk_3_1

#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnEditCapturedVoiceDataEventHandler))]
#endif
        private static void OnEditCapturedVoiceDataEventWrapper(ulong serverConnectionHandlerID, IntPtr samples, int sampleCount, int channels, ref int edited)
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

#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnCustom3dRolloffCalculationClientEventHandler))]
#endif
        private static void OnCustom3dRolloffCalculationClientEventWrapper(ulong serverConnectionHandlerID, ushort clientID, float distance, ref float volume)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnCustom3dRolloffCalculationClient(connection.Cache.GetClient(clientID), distance, ref volume);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnCustom3dRolloffCalculationWaveEventHandler))]
#endif
        private static void OnCustom3dRolloffCalculationWaveEventWrapper(ulong serverConnectionHandlerID, ulong waveHandle, float distance, ref float volume)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnCustom3dRolloffCalculationWave(connection.Cache.GetWaveHandle(waveHandle), distance, ref volume);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private static void OnUserLoggingMessage(string logmessage, int logLevel, string logChannel, ulong logID, string logTime, string completeLogString)
        {
            try
            {
                Connection connection = Library.GetServer(logID);
                Library.OnUserLoggingMessage(logmessage, (LogLevel)logLevel, logChannel, connection, logTime, completeLogString);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }
#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnUserLoggingMessageEventHandler))]
#endif
        private static void OnUserLoggingMessageEventWrapper(IntPtr logmessage, int logLevel, IntPtr logChannel, ulong logID, IntPtr logTime, IntPtr completeLogString)
        {
            try
            {
                OnUserLoggingMessage(Native.ReadString(logmessage), logLevel, Native.ReadString(logChannel), logID, Native.ReadString(logTime), Native.ReadString(completeLogString));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnCustomPacketEncryptEventHandler))]
#endif
        private static void OnCustomPacketEncryptEventWrapper(ref IntPtr dataToSend, ref uint sizeOfData)
        {
            try
            {
                Library.OnCustomPacketEncrypt(ref dataToSend, ref sizeOfData);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnCustomPacketDecryptEventHandler))]
#endif
        private static void OnCustomPacketDecryptEventWrapper(ref IntPtr dataReceived, ref uint dataReceivedSize)
        {
            try
            {
                Library.OnCustomPacketDecrypt(ref dataReceived, ref dataReceivedSize);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnCheckServerUniqueIdentifierEventHandler))]
#endif
        private static void OnCheckServerUniqueIdentifierEventWrapper(ulong serverConnectionHandlerID, IntPtr serverUniqueIdentifier, out bool cancelConnect)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnCheckUniqueIdentifier(Native.ReadString(serverUniqueIdentifier), out cancelConnect);
            }
            catch (Exception e) { cancelConnect = true; if (HandleException(e) == false) throw; }
        }

#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnClientPasswordEncryptEventHandler))]
#endif
        private static void OnClientPasswordEncryptEventWrapper(ulong serverConnectionHandlerID, IntPtr plaintext, IntPtr encryptedText, int encryptedTextByteSize)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                string ciphertext = Library.OnClientPasswordEncrypt(connection, Native.ReadString(plaintext), encryptedTextByteSize - 1);
                Native.WriteString(ciphertext ?? string.Empty, encryptedText, encryptedTextByteSize);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private static void OnFileTransferStatus(ushort transferID, uint status, string statusMessage, ulong remotefileSize, ulong serverConnectionHandlerID)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnFileTransferStatusReceived(connection.Cache.GetTransfer(transferID), (Error)status);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }
#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnFileTransferStatusEventHandler))]
#endif
        private static void OnFileTransferStatusEventWrapper(ushort transferID, uint status, IntPtr statusMessage, ulong remotefileSize, ulong serverConnectionHandlerID)
        {
            try
            {
                OnFileTransferStatus(transferID, status, Native.ReadString(statusMessage), remotefileSize, serverConnectionHandlerID);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private static void OnFileList(ulong serverConnectionHandlerID, ulong channelID, string path, string name, ulong size, ulong datetime, int type, ulong incompletesize, string returnCode)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnFileListReceived(
                    connection.Cache.GetChannel(channelID),
                    path,
                    name,
                    size,
                    Native.FromUnixTime(datetime),
                    (FileListType)type,
                    incompletesize,
                    returnCode);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }
#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnFileListEventHandler))]
#endif
        private static void OnFileListEventWrapper(ulong serverConnectionHandlerID, ulong channelID, IntPtr path, IntPtr name, ulong size, ulong datetime, int type, ulong incompletesize, IntPtr returnCode)
        {
            try
            {
                OnFileList(serverConnectionHandlerID, channelID, Native.ReadString(path), Native.ReadString(name), size, datetime, type, incompletesize, Native.ReadString(returnCode));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private static void OnFileListFinished(ulong serverConnectionHandlerID, ulong channelID, string path)
        {
            try
            {
                // TODO
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }
#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnFileListFinishedEventHandler))]
#endif
        private static void OnFileListFinishedEventWrapper(ulong serverConnectionHandlerID, ulong channelID, IntPtr path)
        {
            try
            {
                OnFileListFinished(serverConnectionHandlerID, channelID, Native.ReadString(path));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        private static void OnFileInfo(ulong serverConnectionHandlerID, ulong channelID, string name, ulong size, ulong datetime)
        {
            try
            {
                Connection connection = Library.GetServer(serverConnectionHandlerID);
                connection.OnFileInfoReceived(
                    connection.Cache.GetChannel(channelID),
                    name,
                    size,
                    Native.FromUnixTime(datetime));
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }
#if UNITY_IOS && !UNITY_EDITOR
        [MonoPInvokeCallback (typeof (OnFileInfoEventHandler))]
#endif
        private static void OnFileInfoEventWrapper(ulong serverConnectionHandlerID, ulong channelID, IntPtr name, ulong size, ulong datetime)
        {
            try
            {
                OnFileInfo(serverConnectionHandlerID, channelID, Native.ReadString(name), size, datetime);
            }
            catch (Exception e) { if (HandleException(e) == false) throw; }
        }

        #endregion
    }
}
