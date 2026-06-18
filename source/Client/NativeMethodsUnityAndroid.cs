/*
 * Copyright 2020 TeamSpeak Systems, Inc.
/* This is an auto generated file. Do not edit.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace TeamSpeak.Sdk.Client
{
    internal partial class NativeMethods
    {
#if UNITY_ANDROID && !UNITY_EDITOR

        private const string DLL_PATH = "teamspeak_sdk_client";

        [DllImport(DLL_PATH, EntryPoint = "ts3client_freeMemory")]
        public static extern Error _FreeMemory(IntPtr pointer);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_initClientLib")]
        public static extern Error _InitClientLib([In] ref ClientUIFunctions functionPointers, IntPtr functionRarePointers, int usedLogTypes, UnmanagedPointer logFileFolder, UnmanagedPointer resourcesFolder);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_destroyClientLib")]
        public static extern Error _DestroyClientLib();

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getClientLibVersion")]
        public static extern Error _GetClientLibVersion(out IntPtr result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getClientLibVersionNumber")]
        public static extern Error _GetClientLibVersionNumber(out LibraryVersion result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_spawnNewServerConnectionHandler")]
        public static extern Error _SpawnNewServerConnectionHandler(int port, out ulong result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_destroyServerConnectionHandler")]
        public static extern Error _DestroyServerConnectionHandler(ulong serverConnectionHandlerID);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_createIdentity")]
        public static extern Error _CreateIdentity(out IntPtr result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_identityStringToUniqueIdentifier")]
        public static extern Error _IdentityStringToUniqueIdentifier(UnmanagedPointer identityString, out IntPtr result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getPlaybackDeviceList")]
        public static extern Error _GetPlaybackDeviceList(UnmanagedPointer modeID, out IntPtr result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getCaptureDeviceList")]
        public static extern Error _GetCaptureDeviceList(UnmanagedPointer modeID, out IntPtr result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getPlaybackModeList")]
        public static extern Error _GetPlaybackModeList(out IntPtr result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getCaptureModeList")]
        public static extern Error _GetCaptureModeList(out IntPtr result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getDefaultPlaybackDevice")]
        public static extern Error _GetDefaultPlaybackDevice(UnmanagedPointer modeID, out IntPtr result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getDefaultCaptureDevice")]
        public static extern Error _GetDefaultCaptureDevice(UnmanagedPointer modeID, out IntPtr result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getDefaultPlayBackMode")]
        public static extern Error _GetDefaultPlayBackMode(out IntPtr result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getDefaultCaptureMode")]
        public static extern Error _GetDefaultCaptureMode(out IntPtr result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_openPlaybackDevice")]
        public static extern Error _OpenPlaybackDevice(ulong serverConnectionHandlerID, UnmanagedPointer modeID, UnmanagedPointer playbackDevice);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_openCaptureDevice")]
        public static extern Error _OpenCaptureDevice(ulong serverConnectionHandlerID, UnmanagedPointer modeID, UnmanagedPointer captureDevice);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getCurrentPlaybackDeviceName")]
        public static extern Error _GetCurrentPlaybackDeviceName(ulong serverConnectionHandlerID, out IntPtr result, out int isDefault);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getCurrentPlayBackMode")]
        public static extern Error _GetCurrentPlayBackMode(ulong serverConnectionHandlerID, out IntPtr result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getCurrentCaptureDeviceName")]
        public static extern Error _GetCurrentCaptureDeviceName(ulong serverConnectionHandlerID, out IntPtr result, out int isDefault);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getCurrentCaptureMode")]
        public static extern Error _GetCurrentCaptureMode(ulong serverConnectionHandlerID, out IntPtr result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_initiateGracefulPlaybackShutdown")]
        public static extern Error _InitiateGracefulPlaybackShutdown(ulong serverConnectionHandlerID);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_closePlaybackDevice")]
        public static extern Error _ClosePlaybackDevice(ulong serverConnectionHandlerID);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_closeCaptureDevice")]
        public static extern Error _CloseCaptureDevice(ulong serverConnectionHandlerID);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_activateCaptureDevice")]
        public static extern Error _ActivateCaptureDevice(ulong serverConnectionHandlerID);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_playWaveFile")]
        public static extern Error _PlayWaveFile(ulong serverConnectionHandlerID, UnmanagedPointer path);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_playWaveFileHandle")]
        public static extern Error _PlayWaveFileHandle(ulong serverConnectionHandlerID, UnmanagedPointer path, bool loop, out ulong waveHandle);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_pauseWaveFileHandle")]
        public static extern Error _PauseWaveFileHandle(ulong serverConnectionHandlerID, ulong waveHandle, bool pause);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_closeWaveFileHandle")]
        public static extern Error _CloseWaveFileHandle(ulong serverConnectionHandlerID, ulong waveHandle);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_registerCustomDevice")]
        public static extern Error _RegisterCustomDevice(UnmanagedPointer deviceID, UnmanagedPointer deviceDisplayName, int capFrequency, int capChannels, int playFrequency, int playChannels);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_unregisterCustomDevice")]
        public static extern Error _UnregisterCustomDevice(UnmanagedPointer deviceID);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_processCustomCaptureData")]
        public static extern Error _ProcessCustomCaptureData(UnmanagedPointer deviceName, [In]short[] buffer, int samples);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_acquireCustomPlaybackData")]
        public static extern Error _AcquireCustomPlaybackData(UnmanagedPointer deviceName, [Out]short[] buffer, int samples);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_setLocalTestMode")]
        public static extern Error _SetLocalTestMode(ulong serverConnectionHandlerID, LocalTestMode status);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_startVoiceRecording")]
        public static extern Error _StartVoiceRecording(ulong serverConnectionHandlerID);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_stopVoiceRecording")]
        public static extern Error _StopVoiceRecording(ulong serverConnectionHandlerID);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_allowWhispersFrom")]
        public static extern Error _AllowWhispersFrom(ulong serverConnectionHandlerID, ushort clID);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_removeFromAllowedWhispersFrom")]
        public static extern Error _RemoveFromAllowedWhispersFrom(ulong serverConnectionHandlerID, ushort clID);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getWhisperReceiveWhitelist")]
        public static extern Error _GetWhisperReceiveWhitelist(ulong serverConnectionHandlerID, out IntPtr result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_isWhisperReceiveWhitelisted")]
        public static extern Error _IsWhisperReceiveWhitelisted(ulong serverConnectionHandlerID, ushort clientID, out int result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_setWhisperReceiveWhitelist")]
        public static extern Error _SetWhisperReceiveWhitelist(ulong serverConnectionHandlerID, UnmanagedPointer clientIDs);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_systemset3DListenerAttributes")]
        public static extern Error _Systemset3DListenerAttributes(ulong serverConnectionHandlerID, IntPtr position, IntPtr forward, IntPtr up);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_set3DWaveAttributes")]
        public static extern Error _Set3DWaveAttributes(ulong serverConnectionHandlerID, ulong waveHandle, [In]ref Vector position);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_systemset3DSettings")]
        public static extern Error _Systemset3DSettings(ulong serverConnectionHandlerID, float distanceFactor, float rolloffScale);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_channelset3DAttributes")]
        public static extern Error _Channelset3DAttributes(ulong serverConnectionHandlerID, ushort clientID, [In]ref Vector position);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getPreProcessorInfoValueFloat")]
        public static extern Error _GetPreProcessorInfoValueFloat(ulong serverConnectionHandlerID, UnmanagedPointer ident, out float result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getPreProcessorConfigValue")]
        public static extern Error _GetPreProcessorConfigValue(ulong serverConnectionHandlerID, UnmanagedPointer ident, out IntPtr result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_setPreProcessorConfigValue")]
        public static extern Error _SetPreProcessorConfigValue(ulong serverConnectionHandlerID, UnmanagedPointer ident, UnmanagedPointer value);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_setKeyPressedDuringChunk")]
        public static extern Error _SetKeyPressedDuringChunk();

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getGlobalConfigValueAsInt")]
        public static extern Error _GetGlobalConfigValueAsInt(UnmanagedPointer ident, out int result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_setGlobalConfigValue")]
        public static extern Error _SetGlobalConfigValue(UnmanagedPointer ident, UnmanagedPointer value);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getEncodeConfigValue")]
        public static extern Error _GetEncodeConfigValue(ulong serverConnectionHandlerID, UnmanagedPointer ident, out IntPtr result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getPlaybackConfigValueAsFloat")]
        public static extern Error _GetPlaybackConfigValueAsFloat(ulong serverConnectionHandlerID, UnmanagedPointer ident, out float result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_setPlaybackConfigValue")]
        public static extern Error _SetPlaybackConfigValue(ulong serverConnectionHandlerID, UnmanagedPointer ident, UnmanagedPointer value);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_setClientVolumeModifier")]
        public static extern Error _SetClientVolumeModifier(ulong serverConnectionHandlerID, ushort clientID, float value);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_logMessage")]
        public static extern Error _LogMessage(UnmanagedPointer logMessage, LogLevel severity, UnmanagedPointer channel, ulong logID);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_setLogVerbosity")]
        public static extern Error _SetLogVerbosity(LogLevel logVerbosity);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getErrorMessage")]
        public static extern Error _GetErrorMessage(uint errorCode, out IntPtr error);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_startConnection")]
        public static extern Error _StartConnection(ulong serverConnectionHandlerID, UnmanagedPointer identity, UnmanagedPointer ip, uint port, UnmanagedPointer nickname, UnmanagedPointer defaultChannelArray, UnmanagedPointer defaultChannelPassword, UnmanagedPointer serverPassword);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_startConnectionWithChannelID")]
        public static extern Error _StartConnectionWithChannelID(ulong serverConnectionHandlerID, UnmanagedPointer identity, UnmanagedPointer ip, uint port, UnmanagedPointer nickname, ulong defaultChannelId, UnmanagedPointer defaultChannelPassword, UnmanagedPointer serverPassword);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_stopConnection")]
        public static extern Error _StopConnection(ulong serverConnectionHandlerID, UnmanagedPointer quitMessage);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_requestClientMove")]
        public static extern Error _RequestClientMove(ulong serverConnectionHandlerID, UnmanagedPointer clientIDArray, ulong newChannelID, UnmanagedPointer password, UnmanagedPointer returnCode);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_requestClientVariables")]
        public static extern Error _RequestClientVariables(ulong serverConnectionHandlerID, ushort clientID, UnmanagedPointer returnCode);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_requestClientKickFromChannel")]
        public static extern Error _RequestClientKickFromChannel(ulong serverConnectionHandlerID, UnmanagedPointer clientIDArray, UnmanagedPointer kickReason, UnmanagedPointer returnCode);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_requestClientKickFromServer")]
        public static extern Error _RequestClientKickFromServer(ulong serverConnectionHandlerID, UnmanagedPointer clientIDArray, UnmanagedPointer kickReason, UnmanagedPointer returnCode);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_requestChannelDelete")]
        public static extern Error _RequestChannelDelete(ulong serverConnectionHandlerID, ulong channelID, int force, UnmanagedPointer returnCode);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_requestChannelMove")]
        public static extern Error _RequestChannelMove(ulong serverConnectionHandlerID, ulong channelID, ulong newChannelParentID, ulong newChannelOrder, UnmanagedPointer returnCode);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_requestSendPrivateTextMsg")]
        public static extern Error _RequestSendPrivateTextMsg(ulong serverConnectionHandlerID, UnmanagedPointer message, ushort targetClientID, UnmanagedPointer returnCode);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_requestSendChannelTextMsg")]
        public static extern Error _RequestSendChannelTextMsg(ulong serverConnectionHandlerID, UnmanagedPointer message, ulong targetChannelID, UnmanagedPointer returnCode);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_requestSendServerTextMsg")]
        public static extern Error _RequestSendServerTextMsg(ulong serverConnectionHandlerID, UnmanagedPointer message, UnmanagedPointer returnCode);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_requestConnectionInfo")]
        public static extern Error _RequestConnectionInfo(ulong serverConnectionHandlerID, ushort clientID, UnmanagedPointer returnCode);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_requestClientSetWhisperList")]
        public static extern Error _RequestClientSetWhisperList(ulong serverConnectionHandlerID, ushort clientID, UnmanagedPointer targetChannelIDArray, UnmanagedPointer targetClientIDArray, UnmanagedPointer returnCode);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_requestChannelSubscribe")]
        public static extern Error _RequestChannelSubscribe(ulong serverConnectionHandlerID, UnmanagedPointer channelIDArray, UnmanagedPointer returnCode);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_requestChannelSubscribeAll")]
        public static extern Error _RequestChannelSubscribeAll(ulong serverConnectionHandlerID, UnmanagedPointer returnCode);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_requestChannelUnsubscribe")]
        public static extern Error _RequestChannelUnsubscribe(ulong serverConnectionHandlerID, UnmanagedPointer channelIDArray, UnmanagedPointer returnCode);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_requestChannelUnsubscribeAll")]
        public static extern Error _RequestChannelUnsubscribeAll(ulong serverConnectionHandlerID, UnmanagedPointer returnCode);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_requestChannelDescription")]
        public static extern Error _RequestChannelDescription(ulong serverConnectionHandlerID, ulong channelID, UnmanagedPointer returnCode);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_requestMuteClients")]
        public static extern Error _RequestMuteClients(ulong serverConnectionHandlerID, UnmanagedPointer clientIDArray, UnmanagedPointer returnCode);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_requestUnmuteClients")]
        public static extern Error _RequestUnmuteClients(ulong serverConnectionHandlerID, UnmanagedPointer clientIDArray, UnmanagedPointer returnCode);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_requestClientIDs")]
        public static extern Error _RequestClientIDs(ulong serverConnectionHandlerID, UnmanagedPointer clientUniqueIdentifier, UnmanagedPointer returnCode);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_requestSlotsFromProvisioningServer")]
        public static extern Error _RequestSlotsFromProvisioningServer(UnmanagedPointer ip, ushort port, UnmanagedPointer serverPassword, ushort slots, UnmanagedPointer identity, UnmanagedPointer region, out ulong requestHandle);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_cancelRequestSlotsFromProvisioningServer")]
        public static extern Error _CancelRequestSlotsFromProvisioningServer(ulong requestHandle);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_startConnectionWithProvisioningKey")]
        public static extern Error _StartConnectionWithProvisioningKey(ulong serverConnectionHandlerID, UnmanagedPointer identity, UnmanagedPointer nickname, UnmanagedPointer connectionKey, UnmanagedPointer clientMetaData);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getClientID")]
        public static extern Error _GetClientID(ulong serverConnectionHandlerID, out ushort result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getConnectionStatus")]
        public static extern Error _GetConnectionStatus(ulong serverConnectionHandlerID, out int result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getConnectionVariableAsUInt64")]
        public static extern Error _GetConnectionVariableAsUInt64(ulong serverConnectionHandlerID, ushort clientID, IntPtr flag, out ulong result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getConnectionVariableAsDouble")]
        public static extern Error _GetConnectionVariableAsDouble(ulong serverConnectionHandlerID, ushort clientID, IntPtr flag, out double result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getConnectionVariableAsString")]
        public static extern Error _GetConnectionVariableAsString(ulong serverConnectionHandlerID, ushort clientID, IntPtr flag, out IntPtr result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_cleanUpConnectionInfo")]
        public static extern Error _CleanUpConnectionInfo(ulong serverConnectionHandlerID, ushort clientID);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_requestServerConnectionInfo")]
        public static extern Error _RequestServerConnectionInfo(ulong serverConnectionHandlerID, UnmanagedPointer returnCode);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getServerConnectionVariableAsUInt64")]
        public static extern Error _GetServerConnectionVariableAsUInt64(ulong serverConnectionHandlerID, IntPtr flag, out ulong result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getServerConnectionVariableAsFloat")]
        public static extern Error _GetServerConnectionVariableAsFloat(ulong serverConnectionHandlerID, IntPtr flag, out float result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getClientSelfVariableAsInt")]
        public static extern Error _GetClientSelfVariableAsInt(ulong serverConnectionHandlerID, IntPtr flag, out int result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getClientSelfVariableAsString")]
        public static extern Error _GetClientSelfVariableAsString(ulong serverConnectionHandlerID, IntPtr flag, out IntPtr result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_setClientSelfVariableAsInt")]
        public static extern Error _SetClientSelfVariableAsInt(ulong serverConnectionHandlerID, IntPtr flag, int value);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_setClientSelfVariableAsString")]
        public static extern Error _SetClientSelfVariableAsString(ulong serverConnectionHandlerID, IntPtr flag, UnmanagedPointer value);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_flushClientSelfUpdates")]
        public static extern Error _FlushClientSelfUpdates(ulong serverConnectionHandlerID, UnmanagedPointer returnCode);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getClientVariableAsInt")]
        public static extern Error _GetClientVariableAsInt(ulong serverConnectionHandlerID, ushort clientID, IntPtr flag, out int result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getClientVariableAsUInt64")]
        public static extern Error _GetClientVariableAsUInt64(ulong serverConnectionHandlerID, ushort clientID, IntPtr flag, out ulong result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getClientVariableAsString")]
        public static extern Error _GetClientVariableAsString(ulong serverConnectionHandlerID, ushort clientID, IntPtr flag, out IntPtr result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getClientList")]
        public static extern Error _GetClientList(ulong serverConnectionHandlerID, out IntPtr result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getChannelOfClient")]
        public static extern Error _GetChannelOfClient(ulong serverConnectionHandlerID, ushort clientID, out ulong result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getChannelVariableAsInt")]
        public static extern Error _GetChannelVariableAsInt(ulong serverConnectionHandlerID, ulong channelID, IntPtr flag, out int result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getChannelVariableAsUInt64")]
        public static extern Error _GetChannelVariableAsUInt64(ulong serverConnectionHandlerID, ulong channelID, IntPtr flag, out ulong result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getChannelVariableAsString")]
        public static extern Error _GetChannelVariableAsString(ulong serverConnectionHandlerID, ulong channelID, IntPtr flag, out IntPtr result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getChannelIDFromChannelNames")]
        public static extern Error _GetChannelIDFromChannelNames(ulong serverConnectionHandlerID, out IntPtr channelNameArray, out ulong result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_setChannelVariableAsInt")]
        public static extern Error _SetChannelVariableAsInt(ulong serverConnectionHandlerID, ulong channelID, IntPtr flag, int value);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_setChannelVariableAsUInt64")]
        public static extern Error _SetChannelVariableAsUInt64(ulong serverConnectionHandlerID, ulong channelID, IntPtr flag, ulong value);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_setChannelVariableAsString")]
        public static extern Error _SetChannelVariableAsString(ulong serverConnectionHandlerID, ulong channelID, IntPtr flag, UnmanagedPointer value);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_flushChannelUpdates")]
        public static extern Error _FlushChannelUpdates(ulong serverConnectionHandlerID, ulong channelID, UnmanagedPointer returnCode);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_flushChannelCreation")]
        public static extern Error _FlushChannelCreation(ulong serverConnectionHandlerID, ulong channelParentID, UnmanagedPointer returnCode);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getChannelList")]
        public static extern Error _GetChannelList(ulong serverConnectionHandlerID, out IntPtr result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getChannelClientList")]
        public static extern Error _GetChannelClientList(ulong serverConnectionHandlerID, ulong channelID, out IntPtr result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getParentChannelOfChannel")]
        public static extern Error _GetParentChannelOfChannel(ulong serverConnectionHandlerID, ulong channelID, out ulong result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getChannelEmptySecs")]
        public static extern Error _GetChannelEmptySecs(ulong serverConnectionHandlerID, ulong channelID, out int result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getServerConnectionHandlerList")]
        public static extern Error _GetServerConnectionHandlerList(out IntPtr result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getServerVariableAsInt")]
        public static extern Error _GetServerVariableAsInt(ulong serverConnectionHandlerID, IntPtr flag, out int result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getServerVariableAsUInt64")]
        public static extern Error _GetServerVariableAsUInt64(ulong serverConnectionHandlerID, IntPtr flag, out ulong result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getServerVariableAsString")]
        public static extern Error _GetServerVariableAsString(ulong serverConnectionHandlerID, IntPtr flag, out IntPtr result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_requestServerVariables")]
        public static extern Error _RequestServerVariables(ulong serverConnectionHandlerID, UnmanagedPointer returnCode);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getTransferFileName")]
        public static extern Error _GetTransferFileName(ushort transferID, out IntPtr result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getTransferFilePath")]
        public static extern Error _GetTransferFilePath(ushort transferID, out IntPtr result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getTransferFileRemotePath")]
        public static extern Error _GetTransferFileRemotePath(ushort transferID, out IntPtr result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getTransferFileSize")]
        public static extern Error _GetTransferFileSize(ushort transferID, out ulong result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getTransferFileSizeDone")]
        public static extern Error _GetTransferFileSizeDone(ushort transferID, out ulong result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_isTransferSender")]
        public static extern Error _IsTransferSender(ushort transferID, out int result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getTransferStatus")]
        public static extern Error _GetTransferStatus(ushort transferID, out int result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getCurrentTransferSpeed")]
        public static extern Error _GetCurrentTransferSpeed(ushort transferID, out float result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getAverageTransferSpeed")]
        public static extern Error _GetAverageTransferSpeed(ushort transferID, out float result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getTransferRunTime")]
        public static extern Error _GetTransferRunTime(ushort transferID, out ulong result);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_sendFile")]
        public static extern Error _SendFile(ulong serverConnectionHandlerID, ulong channelID, UnmanagedPointer channelPW, UnmanagedPointer file, int overwrite, int resume, UnmanagedPointer sourceDirectory, out ushort result, UnmanagedPointer returnCode);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_requestFile")]
        public static extern Error _RequestFile(ulong serverConnectionHandlerID, ulong channelID, UnmanagedPointer channelPW, UnmanagedPointer file, int overwrite, int resume, UnmanagedPointer destinationDirectory, out ushort result, UnmanagedPointer returnCode);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_haltTransfer")]
        public static extern Error _HaltTransfer(ulong serverConnectionHandlerID, ushort transferID, int deleteUnfinishedFile, UnmanagedPointer returnCode);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_requestFileList")]
        public static extern Error _RequestFileList(ulong serverConnectionHandlerID, ulong channelID, UnmanagedPointer channelPW, UnmanagedPointer path, UnmanagedPointer returnCode);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_requestFileInfo")]
        public static extern Error _RequestFileInfo(ulong serverConnectionHandlerID, ulong channelID, UnmanagedPointer channelPW, UnmanagedPointer file, UnmanagedPointer returnCode);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_requestDeleteFile")]
        public static extern Error _RequestDeleteFile(ulong serverConnectionHandlerID, ulong channelID, UnmanagedPointer channelPW, UnmanagedPointer file, UnmanagedPointer returnCode);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_requestCreateDirectory")]
        public static extern Error _RequestCreateDirectory(ulong serverConnectionHandlerID, ulong channelID, UnmanagedPointer channelPW, UnmanagedPointer directoryPath, UnmanagedPointer returnCode);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_requestRenameFile")]
        public static extern Error _RequestRenameFile(ulong serverConnectionHandlerID, ulong fromChannelID, UnmanagedPointer fromChannelPW, ulong toChannelID, UnmanagedPointer toChannelPW, UnmanagedPointer oldFile, UnmanagedPointer newFile, UnmanagedPointer returnCode);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getInstanceSpeedLimitUp")]
        public static extern Error _GetInstanceSpeedLimitUp(out ulong limit);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getInstanceSpeedLimitDown")]
        public static extern Error _GetInstanceSpeedLimitDown(out ulong limit);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getServerConnectionHandlerSpeedLimitUp")]
        public static extern Error _GetServerConnectionHandlerSpeedLimitUp(ulong serverConnectionHandlerID, out ulong limit);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getServerConnectionHandlerSpeedLimitDown")]
        public static extern Error _GetServerConnectionHandlerSpeedLimitDown(ulong serverConnectionHandlerID, out ulong limit);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_getTransferSpeedLimit")]
        public static extern Error _GetTransferSpeedLimit(ushort transferID, out ulong limit);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_setInstanceSpeedLimitUp")]
        public static extern Error _SetInstanceSpeedLimitUp(ulong newLimit);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_setInstanceSpeedLimitDown")]
        public static extern Error _SetInstanceSpeedLimitDown(ulong newLimit);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_setServerConnectionHandlerSpeedLimitUp")]
        public static extern Error _SetServerConnectionHandlerSpeedLimitUp(ulong serverConnectionHandlerID, ulong newLimit);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_setServerConnectionHandlerSpeedLimitDown")]
        public static extern Error _SetServerConnectionHandlerSpeedLimitDown(ulong serverConnectionHandlerID, ulong newLimit);

        [DllImport(DLL_PATH, EntryPoint = "ts3client_setTransferSpeedLimit")]
        public static extern Error _SetTransferSpeedLimit(ushort transferID, ulong newLimit);
#endif
    }
}
