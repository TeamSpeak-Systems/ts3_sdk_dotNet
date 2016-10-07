using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Teamspeak.Sdk.Client
{
    internal class NativeMethods
    {
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error FreeMemoryDelegate(IntPtr pointer);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error InitClientLibDelegate([In]ref ClientUIFunctions functionPointers, IntPtr functionRarePointers, int usedLogTypes, UnmanagedPointer logFileFolder, UnmanagedPointer resourcesFolder);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        internal delegate Error DestroyClientLibDelegate();
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetClientLibVersionDelegate(out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetClientLibVersionNumberDelegate(out ulong result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error SpawnNewServerConnectionHandlerDelegate(int port, out ulong result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error DestroyServerConnectionHandlerDelegate(ulong serverConnectionHandlerID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error CreateIdentityDelegate(out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error IdentityStringToUniqueIdentifierDelegate(UnmanagedPointer identityString, out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetPlaybackDeviceListDelegate(UnmanagedPointer modeID, out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetCaptureDeviceListDelegate(UnmanagedPointer modeID, out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetPlaybackModeListDelegate(out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetCaptureModeListDelegate(out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetDefaultPlaybackDeviceDelegate(UnmanagedPointer modeID, out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetDefaultCaptureDeviceDelegate(UnmanagedPointer modeID, out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetDefaultPlayBackModeDelegate(out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetDefaultCaptureModeDelegate(out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error OpenPlaybackDeviceDelegate(ulong serverConnectionHandlerID, UnmanagedPointer modeID, UnmanagedPointer playbackDevice);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error OpenCaptureDeviceDelegate(ulong serverConnectionHandlerID, UnmanagedPointer modeID, UnmanagedPointer captureDevice);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetCurrentPlaybackDeviceNameDelegate(ulong serverConnectionHandlerID, out IntPtr result, out bool isDefault);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetCurrentPlayBackModeDelegate(ulong serverConnectionHandlerID, out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetCurrentCaptureDeviceNameDelegate(ulong serverConnectionHandlerID, out IntPtr result, out bool isDefault);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetCurrentCaptureModeDelegate(ulong serverConnectionHandlerID, out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error InitiateGracefulPlaybackShutdownDelegate(ulong serverConnectionHandlerID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error ClosePlaybackDeviceDelegate(ulong serverConnectionHandlerID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error CloseCaptureDeviceDelegate(ulong serverConnectionHandlerID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error ActivateCaptureDeviceDelegate(ulong serverConnectionHandlerID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error PlayWaveFileDelegate(ulong serverConnectionHandlerID, UnmanagedPointer path);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error PlayWaveFileHandleDelegate(ulong serverConnectionHandlerID, UnmanagedPointer path, bool loop, out ulong waveHandle);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error PauseWaveFileHandleDelegate(ulong serverConnectionHandlerID, ulong waveHandle, bool pause);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error CloseWaveFileHandleDelegate(ulong serverConnectionHandlerID, ulong waveHandle);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error RegisterCustomDeviceDelegate(UnmanagedPointer deviceID, UnmanagedPointer deviceDisplayName, int capFrequency, int capChannels, int playFrequency, int playChannels);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error UnregisterCustomDeviceDelegate(UnmanagedPointer deviceID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error ProcessCustomCaptureDataDelegate(UnmanagedPointer deviceName, [In]short[] buffer, int samples);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error AcquireCustomPlaybackDataDelegate(UnmanagedPointer deviceName, [Out]short[] buffer, int samples);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error SetLocalTestModeDelegate(ulong serverConnectionHandlerID, int status);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error StartVoiceRecordingDelegate(ulong serverConnectionHandlerID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error StopVoiceRecordingDelegate(ulong serverConnectionHandlerID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error AllowWhispersFromDelegate(ulong serverConnectionHandlerID, ushort clID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error RemoveFromAllowedWhispersFromDelegate(ulong serverConnectionHandlerID, ushort clID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error Systemset3DListenerAttributesDelegate(ulong serverConnectionHandlerID, IntPtr position, IntPtr forward, IntPtr up);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error Set3DWaveAttributesDelegate(ulong serverConnectionHandlerID, ulong waveHandle, [In]ref Vector position);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error Systemset3DSettingsDelegate(ulong serverConnectionHandlerID, float distanceFactor, float rolloffScale);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error Channelset3DAttributesDelegate(ulong serverConnectionHandlerID, ushort clientID, [In]ref Vector position);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetPreProcessorInfoValueFloatDelegate(ulong serverConnectionHandlerID, UnmanagedPointer ident, out float result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetPreProcessorConfigValueDelegate(ulong serverConnectionHandlerID, UnmanagedPointer ident, out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error SetPreProcessorConfigValueDelegate(ulong serverConnectionHandlerID, UnmanagedPointer ident, UnmanagedPointer value);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetEncodeConfigValueDelegate(ulong serverConnectionHandlerID, UnmanagedPointer ident, out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetPlaybackConfigValueAsFloatDelegate(ulong serverConnectionHandlerID, UnmanagedPointer ident, out float result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error SetPlaybackConfigValueDelegate(ulong serverConnectionHandlerID, UnmanagedPointer ident, UnmanagedPointer value);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error SetClientVolumeModifierDelegate(ulong serverConnectionHandlerID, ushort clientID, float value);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error LogMessageDelegate(UnmanagedPointer logMessage, int severity, UnmanagedPointer channel, ulong logID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error SetLogVerbosityDelegate(int logVerbosity);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetErrorMessageDelegate(uint errorCode, out IntPtr error);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error StartConnectionDelegate(ulong serverConnectionHandlerID, UnmanagedPointer identity, UnmanagedPointer ip, uint port, UnmanagedPointer nickname, UnmanagedPointer defaultChannelArray, UnmanagedPointer defaultChannelPassword, UnmanagedPointer serverPassword);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error StopConnectionDelegate(ulong serverConnectionHandlerID, UnmanagedPointer quitMessage);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error RequestClientMoveDelegate(ulong serverConnectionHandlerID, ushort clientID, ulong newChannelID, UnmanagedPointer password, UnmanagedPointer returnCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error RequestClientVariablesDelegate(ulong serverConnectionHandlerID, ushort clientID, UnmanagedPointer returnCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error RequestClientKickFromChannelDelegate(ulong serverConnectionHandlerID, ushort clientID, UnmanagedPointer kickReason, UnmanagedPointer returnCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error RequestClientKickFromServerDelegate(ulong serverConnectionHandlerID, ushort clientID, UnmanagedPointer kickReason, UnmanagedPointer returnCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error RequestChannelDeleteDelegate(ulong serverConnectionHandlerID, ulong channelID, int force, UnmanagedPointer returnCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error RequestChannelMoveDelegate(ulong serverConnectionHandlerID, ulong channelID, ulong newChannelParentID, ulong newChannelOrder, UnmanagedPointer returnCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error RequestSendPrivateTextMsgDelegate(ulong serverConnectionHandlerID, UnmanagedPointer message, ushort targetClientID, UnmanagedPointer returnCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error RequestSendChannelTextMsgDelegate(ulong serverConnectionHandlerID, UnmanagedPointer message, ulong targetChannelID, UnmanagedPointer returnCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error RequestSendServerTextMsgDelegate(ulong serverConnectionHandlerID, UnmanagedPointer message, UnmanagedPointer returnCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error RequestConnectionInfoDelegate(ulong serverConnectionHandlerID, ushort clientID, UnmanagedPointer returnCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error RequestClientSetWhisperListDelegate(ulong serverConnectionHandlerID, ushort clientID, UnmanagedPointer targetChannelIDArray, UnmanagedPointer targetClientIDArray, UnmanagedPointer returnCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error RequestChannelSubscribeDelegate(ulong serverConnectionHandlerID, UnmanagedPointer channelIDArray, UnmanagedPointer returnCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error RequestChannelSubscribeAllDelegate(ulong serverConnectionHandlerID, UnmanagedPointer returnCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error RequestChannelUnsubscribeDelegate(ulong serverConnectionHandlerID, UnmanagedPointer channelIDArray, UnmanagedPointer returnCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error RequestChannelUnsubscribeAllDelegate(ulong serverConnectionHandlerID, UnmanagedPointer returnCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error RequestChannelDescriptionDelegate(ulong serverConnectionHandlerID, ulong channelID, UnmanagedPointer returnCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error RequestMuteClientsDelegate(ulong serverConnectionHandlerID, UnmanagedPointer clientIDArray, UnmanagedPointer returnCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error RequestUnmuteClientsDelegate(ulong serverConnectionHandlerID, UnmanagedPointer clientIDArray, UnmanagedPointer returnCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error RequestClientIDsDelegate(ulong serverConnectionHandlerID, UnmanagedPointer clientUniqueIdentifier, UnmanagedPointer returnCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error RequestSlotsFromProvisioningServerDelegate(UnmanagedPointer ip, ushort port, UnmanagedPointer serverPassword, ushort slots, UnmanagedPointer identity, UnmanagedPointer region, out ulong requestHandle);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error CancelRequestSlotsFromProvisioningServerDelegate(ulong requestHandle);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error StartConnectionWithProvisioningKeyDelegate(ulong serverConnectionHandlerID, UnmanagedPointer identity, UnmanagedPointer nickname, UnmanagedPointer connectionKey, UnmanagedPointer clientMetaData);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetClientIDDelegate(ulong serverConnectionHandlerID, out ushort result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetConnectionStatusDelegate(ulong serverConnectionHandlerID, out int result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetConnectionVariableAsUInt64Delegate(ulong serverConnectionHandlerID, ushort clientID, IntPtr flag, out ulong result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetConnectionVariableAsDoubleDelegate(ulong serverConnectionHandlerID, ushort clientID, IntPtr flag, out double result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetConnectionVariableAsStringDelegate(ulong serverConnectionHandlerID, ushort clientID, IntPtr flag, out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error CleanUpConnectionInfoDelegate(ulong serverConnectionHandlerID, ushort clientID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error RequestServerConnectionInfoDelegate(ulong serverConnectionHandlerID, UnmanagedPointer returnCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetServerConnectionVariableAsUInt64Delegate(ulong serverConnectionHandlerID, IntPtr flag, out ulong result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetServerConnectionVariableAsFloatDelegate(ulong serverConnectionHandlerID, IntPtr flag, out float result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetClientSelfVariableAsIntDelegate(ulong serverConnectionHandlerID, IntPtr flag, out int result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetClientSelfVariableAsStringDelegate(ulong serverConnectionHandlerID, IntPtr flag, out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error SetClientSelfVariableAsIntDelegate(ulong serverConnectionHandlerID, IntPtr flag, int value);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error SetClientSelfVariableAsStringDelegate(ulong serverConnectionHandlerID, IntPtr flag, UnmanagedPointer value);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error FlushClientSelfUpdatesDelegate(ulong serverConnectionHandlerID, UnmanagedPointer returnCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetClientVariableAsIntDelegate(ulong serverConnectionHandlerID, ushort clientID, IntPtr flag, out int result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetClientVariableAsUInt64Delegate(ulong serverConnectionHandlerID, ushort clientID, IntPtr flag, out ulong result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetClientVariableAsStringDelegate(ulong serverConnectionHandlerID, ushort clientID, IntPtr flag, out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetClientListDelegate(ulong serverConnectionHandlerID, out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetChannelOfClientDelegate(ulong serverConnectionHandlerID, ushort clientID, out ulong result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetChannelVariableAsIntDelegate(ulong serverConnectionHandlerID, ulong channelID, IntPtr flag, out int result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetChannelVariableAsUInt64Delegate(ulong serverConnectionHandlerID, ulong channelID, IntPtr flag, out ulong result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetChannelVariableAsStringDelegate(ulong serverConnectionHandlerID, ulong channelID, IntPtr flag, out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetChannelIDFromChannelNamesDelegate(ulong serverConnectionHandlerID, UnmanagedPointer channelNameArray, out ulong result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error SetChannelVariableAsIntDelegate(ulong serverConnectionHandlerID, ulong channelID, IntPtr flag, int value);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error SetChannelVariableAsUInt64Delegate(ulong serverConnectionHandlerID, ulong channelID, IntPtr flag, ulong value);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error SetChannelVariableAsStringDelegate(ulong serverConnectionHandlerID, ulong channelID, IntPtr flag, UnmanagedPointer value);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error FlushChannelUpdatesDelegate(ulong serverConnectionHandlerID, ulong channelID, UnmanagedPointer returnCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error FlushChannelCreationDelegate(ulong serverConnectionHandlerID, ulong channelParentID, UnmanagedPointer returnCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetChannelListDelegate(ulong serverConnectionHandlerID, out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetChannelClientListDelegate(ulong serverConnectionHandlerID, ulong channelID, out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetParentChannelOfChannelDelegate(ulong serverConnectionHandlerID, ulong channelID, out ulong result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetChannelEmptySecsDelegate(ulong serverConnectionHandlerID, ulong channelID, out int result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetServerConnectionHandlerListDelegate(out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetServerVariableAsIntDelegate(ulong serverConnectionHandlerID, IntPtr flag, out int result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetServerVariableAsUInt64Delegate(ulong serverConnectionHandlerID, IntPtr flag, out ulong result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetServerVariableAsStringDelegate(ulong serverConnectionHandlerID, IntPtr flag, out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error RequestServerVariablesDelegate(ulong serverConnectionHandlerID);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetTransferFileNameDelegate(ushort transferID, out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetTransferFilePathDelegate(ushort transferID, out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetTransferFileRemotePathDelegate(ushort transferID, out IntPtr result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetTransferFileSizeDelegate(ushort transferID, out ulong result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetTransferFileSizeDoneDelegate(ushort transferID, out ulong result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error IsTransferSenderDelegate(ushort transferID, out int result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetTransferStatusDelegate(ushort transferID, out int result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetCurrentTransferSpeedDelegate(ushort transferID, out float result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetAverageTransferSpeedDelegate(ushort transferID, out float result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetTransferRunTimeDelegate(ushort transferID, out ulong result);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error SendFileDelegate(ulong serverConnectionHandlerID, ulong channelID, UnmanagedPointer channelPW, UnmanagedPointer file, int overwrite, int resume, UnmanagedPointer sourceDirectory, out ushort result, UnmanagedPointer returnCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error RequestFileDelegate(ulong serverConnectionHandlerID, ulong channelID, UnmanagedPointer channelPW, UnmanagedPointer file, int overwrite, int resume, UnmanagedPointer destinationDirectory, out ushort result, UnmanagedPointer returnCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error HaltTransferDelegate(ulong serverConnectionHandlerID, ushort transferID, int deleteUnfinishedFile, UnmanagedPointer returnCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error RequestFileListDelegate(ulong serverConnectionHandlerID, ulong channelID, UnmanagedPointer channelPW, UnmanagedPointer path, UnmanagedPointer returnCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error RequestFileInfoDelegate(ulong serverConnectionHandlerID, ulong channelID, UnmanagedPointer channelPW, UnmanagedPointer file, UnmanagedPointer returnCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error RequestDeleteFileDelegate(ulong serverConnectionHandlerID, ulong channelID, UnmanagedPointer channelPW, UnmanagedPointer file, UnmanagedPointer returnCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error RequestCreateDirectoryDelegate(ulong serverConnectionHandlerID, ulong channelID, UnmanagedPointer channelPW, UnmanagedPointer directoryPath, UnmanagedPointer returnCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error RequestRenameFileDelegate(ulong serverConnectionHandlerID, ulong fromChannelID, UnmanagedPointer fromChannelPW, ulong toChannelID, UnmanagedPointer toChannelPW, UnmanagedPointer oldFile, UnmanagedPointer newFile, UnmanagedPointer returnCode);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetInstanceSpeedLimitUpDelegate(out ulong limit);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetInstanceSpeedLimitDownDelegate(out ulong limit);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetServerConnectionHandlerSpeedLimitUpDelegate(ulong serverConnectionHandlerID, out ulong limit);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetServerConnectionHandlerSpeedLimitDownDelegate(ulong serverConnectionHandlerID, out ulong limit);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error GetTransferSpeedLimitDelegate(ushort transferID, out ulong limit);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error SetInstanceSpeedLimitUpDelegate(ulong newLimit);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error SetInstanceSpeedLimitDownDelegate(ulong newLimit);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error SetServerConnectionHandlerSpeedLimitUpDelegate(ulong serverConnectionHandlerID, ulong newLimit);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error SetServerConnectionHandlerSpeedLimitDownDelegate(ulong serverConnectionHandlerID, ulong newLimit);
        [UnmanagedFunctionPointer(Native.CallingConvention)]
        delegate Error SetTransferSpeedLimitDelegate(ushort transferID, ulong newLimit);

        readonly FreeMemoryDelegate _FreeMemory;
        readonly InitClientLibDelegate _InitClientLib;
        readonly GetClientLibVersionDelegate _GetClientLibVersion;
        readonly GetClientLibVersionNumberDelegate _GetClientLibVersionNumber;
        readonly SpawnNewServerConnectionHandlerDelegate _SpawnNewServerConnectionHandler;
        readonly DestroyServerConnectionHandlerDelegate _DestroyServerConnectionHandler;
        readonly CreateIdentityDelegate _CreateIdentity;
        readonly IdentityStringToUniqueIdentifierDelegate _IdentityStringToUniqueIdentifier;
        readonly GetPlaybackDeviceListDelegate _GetPlaybackDeviceList;
        readonly GetCaptureDeviceListDelegate _GetCaptureDeviceList;
        readonly GetPlaybackModeListDelegate _GetPlaybackModeList;
        readonly GetCaptureModeListDelegate _GetCaptureModeList;
        //readonly GetDefaultPlaybackDeviceDelegate _GetDefaultPlaybackDevice;
        //readonly GetDefaultCaptureDeviceDelegate _GetDefaultCaptureDevice;
        //readonly GetDefaultPlayBackModeDelegate _GetDefaultPlayBackMode;
        //readonly GetDefaultCaptureModeDelegate _GetDefaultCaptureMode;
        readonly OpenPlaybackDeviceDelegate _OpenPlaybackDevice;
        readonly OpenCaptureDeviceDelegate _OpenCaptureDevice;
        readonly GetCurrentPlaybackDeviceNameDelegate _GetCurrentPlaybackDeviceName;
        readonly GetCurrentPlayBackModeDelegate _GetCurrentPlayBackMode;
        readonly GetCurrentCaptureDeviceNameDelegate _GetCurrentCaptureDeviceName;
        readonly GetCurrentCaptureModeDelegate _GetCurrentCaptureMode;
        readonly InitiateGracefulPlaybackShutdownDelegate _InitiateGracefulPlaybackShutdown;
        readonly ClosePlaybackDeviceDelegate _ClosePlaybackDevice;
        readonly CloseCaptureDeviceDelegate _CloseCaptureDevice;
        readonly ActivateCaptureDeviceDelegate _ActivateCaptureDevice;
        //readonly PlayWaveFileDelegate _PlayWaveFile;
        readonly PlayWaveFileHandleDelegate _PlayWaveFileHandle;
        readonly PauseWaveFileHandleDelegate _PauseWaveFileHandle;
        readonly CloseWaveFileHandleDelegate _CloseWaveFileHandle;
        readonly RegisterCustomDeviceDelegate _RegisterCustomDevice;
        readonly UnregisterCustomDeviceDelegate _UnregisterCustomDevice;
        readonly ProcessCustomCaptureDataDelegate _ProcessCustomCaptureData;
        readonly AcquireCustomPlaybackDataDelegate _AcquireCustomPlaybackData;
        readonly SetLocalTestModeDelegate _SetLocalTestMode;
        readonly StartVoiceRecordingDelegate _StartVoiceRecording;
        readonly StopVoiceRecordingDelegate _StopVoiceRecording;
        readonly AllowWhispersFromDelegate _AllowWhispersFrom;
        readonly RemoveFromAllowedWhispersFromDelegate _RemoveFromAllowedWhispersFrom;
        readonly Systemset3DListenerAttributesDelegate _Systemset3DListenerAttributes;
        readonly Set3DWaveAttributesDelegate _Set3DWaveAttributes;
        readonly Systemset3DSettingsDelegate _Systemset3DSettings;
        readonly Channelset3DAttributesDelegate _Channelset3DAttributes;
        readonly GetPreProcessorInfoValueFloatDelegate _GetPreProcessorInfoValueFloat;
        readonly GetPreProcessorConfigValueDelegate _GetPreProcessorConfigValue;
        readonly SetPreProcessorConfigValueDelegate _SetPreProcessorConfigValue;
        //readonly GetEncodeConfigValueDelegate _GetEncodeConfigValue;
        readonly GetPlaybackConfigValueAsFloatDelegate _GetPlaybackConfigValueAsFloat;
        readonly SetPlaybackConfigValueDelegate _SetPlaybackConfigValue;
        readonly SetClientVolumeModifierDelegate _SetClientVolumeModifier;
        readonly LogMessageDelegate _LogMessage;
        readonly SetLogVerbosityDelegate _SetLogVerbosity;
        readonly GetErrorMessageDelegate _GetErrorMessage;
        readonly StartConnectionDelegate _StartConnection;
        readonly StopConnectionDelegate _StopConnection;
        readonly RequestClientMoveDelegate _RequestClientMove;
        readonly RequestClientVariablesDelegate _RequestClientVariables;
        readonly RequestClientKickFromChannelDelegate _RequestClientKickFromChannel;
        readonly RequestClientKickFromServerDelegate _RequestClientKickFromServer;
        readonly RequestChannelDeleteDelegate _RequestChannelDelete;
        readonly RequestChannelMoveDelegate _RequestChannelMove;
        readonly RequestSendPrivateTextMsgDelegate _RequestSendPrivateTextMsg;
        readonly RequestSendChannelTextMsgDelegate _RequestSendChannelTextMsg;
        readonly RequestSendServerTextMsgDelegate _RequestSendServerTextMsg;
        readonly RequestConnectionInfoDelegate _RequestConnectionInfo;
        readonly RequestClientSetWhisperListDelegate _RequestClientSetWhisperList;
        readonly RequestChannelSubscribeDelegate _RequestChannelSubscribe;
        readonly RequestChannelSubscribeAllDelegate _RequestChannelSubscribeAll;
        readonly RequestChannelUnsubscribeDelegate _RequestChannelUnsubscribe;
        readonly RequestChannelUnsubscribeAllDelegate _RequestChannelUnsubscribeAll;
        readonly RequestChannelDescriptionDelegate _RequestChannelDescription;
        readonly RequestMuteClientsDelegate _RequestMuteClients;
        readonly RequestUnmuteClientsDelegate _RequestUnmuteClients;
        readonly RequestClientIDsDelegate _RequestClientIDs;
        //readonly RequestSlotsFromProvisioningServerDelegate _RequestSlotsFromProvisioningServer;
        //readonly CancelRequestSlotsFromProvisioningServerDelegate _CancelRequestSlotsFromProvisioningServer;
        //readonly StartConnectionWithProvisioningKeyDelegate _StartConnectionWithProvisioningKey;
        readonly GetClientIDDelegate _GetClientID;
        readonly GetConnectionStatusDelegate _GetConnectionStatus;
        readonly GetConnectionVariableAsUInt64Delegate _GetConnectionVariableAsUInt64;
        readonly GetConnectionVariableAsDoubleDelegate _GetConnectionVariableAsDouble;
        readonly GetConnectionVariableAsStringDelegate _GetConnectionVariableAsString;
        readonly CleanUpConnectionInfoDelegate _CleanUpConnectionInfo;
        //readonly RequestServerConnectionInfoDelegate _RequestServerConnectionInfo;
        //readonly GetServerConnectionVariableAsUInt64Delegate _GetServerConnectionVariableAsUInt64;
        //readonly GetServerConnectionVariableAsFloatDelegate _GetServerConnectionVariableAsFloat;
        //readonly GetClientSelfVariableAsIntDelegate _GetClientSelfVariableAsInt;
        //readonly GetClientSelfVariableAsStringDelegate _GetClientSelfVariableAsString;
        readonly SetClientSelfVariableAsIntDelegate _SetClientSelfVariableAsInt;
        readonly SetClientSelfVariableAsStringDelegate _SetClientSelfVariableAsString;
        readonly FlushClientSelfUpdatesDelegate _FlushClientSelfUpdates;
        readonly GetClientVariableAsIntDelegate _GetClientVariableAsInt;
        readonly GetClientVariableAsUInt64Delegate _GetClientVariableAsUInt64;
        readonly GetClientVariableAsStringDelegate _GetClientVariableAsString;
        readonly GetClientListDelegate _GetClientList;
        readonly GetChannelOfClientDelegate _GetChannelOfClient;
        readonly GetChannelVariableAsIntDelegate _GetChannelVariableAsInt;
        readonly GetChannelVariableAsUInt64Delegate _GetChannelVariableAsUInt64;
        readonly GetChannelVariableAsStringDelegate _GetChannelVariableAsString;
        //readonly GetChannelIDFromChannelNamesDelegate _GetChannelIDFromChannelNames; // beware array is terminated by empty string
        readonly SetChannelVariableAsIntDelegate _SetChannelVariableAsInt;
        readonly SetChannelVariableAsUInt64Delegate _SetChannelVariableAsUInt64;
        readonly SetChannelVariableAsStringDelegate _SetChannelVariableAsString;
        readonly FlushChannelUpdatesDelegate _FlushChannelUpdates;
        readonly FlushChannelCreationDelegate _FlushChannelCreation;
        readonly GetChannelListDelegate _GetChannelList;
        readonly GetChannelClientListDelegate _GetChannelClientList;
        readonly GetParentChannelOfChannelDelegate _GetParentChannelOfChannel;
        readonly GetChannelEmptySecsDelegate _GetChannelEmptySecs;
        readonly GetServerConnectionHandlerListDelegate _GetServerConnectionHandlerList;
        readonly GetServerVariableAsIntDelegate _GetServerVariableAsInt;
        readonly GetServerVariableAsUInt64Delegate _GetServerVariableAsUInt64;
        readonly GetServerVariableAsStringDelegate _GetServerVariableAsString;
        readonly RequestServerVariablesDelegate _RequestServerVariables;
        readonly GetTransferFileNameDelegate _GetTransferFileName;
        readonly GetTransferFilePathDelegate _GetTransferFilePath;
        readonly GetTransferFileRemotePathDelegate _GetTransferFileRemotePath;
        readonly GetTransferFileSizeDelegate _GetTransferFileSize;
        readonly GetTransferFileSizeDoneDelegate _GetTransferFileSizeDone;
        readonly IsTransferSenderDelegate _IsTransferSender;
        readonly GetTransferStatusDelegate _GetTransferStatus;
        readonly GetCurrentTransferSpeedDelegate _GetCurrentTransferSpeed;
        readonly GetAverageTransferSpeedDelegate _GetAverageTransferSpeed;
        readonly GetTransferRunTimeDelegate _GetTransferRunTime;
        readonly SendFileDelegate _SendFile;
        readonly RequestFileDelegate _RequestFile;
        readonly HaltTransferDelegate _HaltTransfer;
        readonly RequestFileListDelegate _RequestFileList;
        readonly RequestFileInfoDelegate _RequestFileInfo;
        readonly RequestDeleteFileDelegate _RequestDeleteFile;
        readonly RequestCreateDirectoryDelegate _RequestCreateDirectory;
        readonly RequestRenameFileDelegate _RequestRenameFile;
        readonly GetInstanceSpeedLimitUpDelegate _GetInstanceSpeedLimitUp;
        readonly GetInstanceSpeedLimitDownDelegate _GetInstanceSpeedLimitDown;
        readonly GetServerConnectionHandlerSpeedLimitUpDelegate _GetServerConnectionHandlerSpeedLimitUp;
        readonly GetServerConnectionHandlerSpeedLimitDownDelegate _GetServerConnectionHandlerSpeedLimitDown;
        readonly GetTransferSpeedLimitDelegate _GetTransferSpeedLimit;
        readonly SetInstanceSpeedLimitUpDelegate _SetInstanceSpeedLimitUp;
        readonly SetInstanceSpeedLimitDownDelegate _SetInstanceSpeedLimitDown;
        readonly SetServerConnectionHandlerSpeedLimitUpDelegate _SetServerConnectionHandlerSpeedLimitUp;
        readonly SetServerConnectionHandlerSpeedLimitDownDelegate _SetServerConnectionHandlerSpeedLimitDown;
        readonly SetTransferSpeedLimitDelegate _SetTransferSpeedLimit;

        readonly SdkHandle Handle;

        public NativeMethods(SdkHandle handle)
        {
            Handle = handle;
            handle.GetLibraryMethod("ts3client_freeMemory", out _FreeMemory);
            handle.GetLibraryMethod("ts3client_initClientLib", out _InitClientLib);
            handle.GetLibraryMethod("ts3client_getClientLibVersion", out _GetClientLibVersion);
            handle.GetLibraryMethod("ts3client_getClientLibVersionNumber", out _GetClientLibVersionNumber);
            handle.GetLibraryMethod("ts3client_spawnNewServerConnectionHandler", out _SpawnNewServerConnectionHandler);
            handle.GetLibraryMethod("ts3client_destroyServerConnectionHandler", out _DestroyServerConnectionHandler);
            handle.GetLibraryMethod("ts3client_createIdentity", out _CreateIdentity);
            handle.GetLibraryMethod("ts3client_identityStringToUniqueIdentifier", out _IdentityStringToUniqueIdentifier);
            handle.GetLibraryMethod("ts3client_getPlaybackDeviceList", out _GetPlaybackDeviceList);
            handle.GetLibraryMethod("ts3client_getCaptureDeviceList", out _GetCaptureDeviceList);
            handle.GetLibraryMethod("ts3client_getPlaybackModeList", out _GetPlaybackModeList);
            handle.GetLibraryMethod("ts3client_getCaptureModeList", out _GetCaptureModeList);
            //handle.GetLibraryMethod("ts3client_getDefaultPlaybackDevice", out _GetDefaultPlaybackDevice);
            //handle.GetLibraryMethod("ts3client_getDefaultCaptureDevice", out _GetDefaultCaptureDevice);
            //handle.GetLibraryMethod("ts3client_getDefaultPlayBackMode", out _GetDefaultPlayBackMode);
            //handle.GetLibraryMethod("ts3client_getDefaultCaptureMode", out _GetDefaultCaptureMode);
            handle.GetLibraryMethod("ts3client_openPlaybackDevice", out _OpenPlaybackDevice);
            handle.GetLibraryMethod("ts3client_openCaptureDevice", out _OpenCaptureDevice);
            handle.GetLibraryMethod("ts3client_getCurrentPlaybackDeviceName", out _GetCurrentPlaybackDeviceName);
            handle.GetLibraryMethod("ts3client_getCurrentPlayBackMode", out _GetCurrentPlayBackMode);
            handle.GetLibraryMethod("ts3client_getCurrentCaptureDeviceName", out _GetCurrentCaptureDeviceName);
            handle.GetLibraryMethod("ts3client_getCurrentCaptureMode", out _GetCurrentCaptureMode);
            handle.GetLibraryMethod("ts3client_initiateGracefulPlaybackShutdown", out _InitiateGracefulPlaybackShutdown);
            handle.GetLibraryMethod("ts3client_closePlaybackDevice", out _ClosePlaybackDevice);
            handle.GetLibraryMethod("ts3client_closeCaptureDevice", out _CloseCaptureDevice);
            handle.GetLibraryMethod("ts3client_activateCaptureDevice", out _ActivateCaptureDevice);
            //handle.GetLibraryMethod("ts3client_playWaveFile", out _PlayWaveFile);
            handle.GetLibraryMethod("ts3client_playWaveFileHandle", out _PlayWaveFileHandle);
            handle.GetLibraryMethod("ts3client_pauseWaveFileHandle", out _PauseWaveFileHandle);
            handle.GetLibraryMethod("ts3client_closeWaveFileHandle", out _CloseWaveFileHandle);
            handle.GetLibraryMethod("ts3client_registerCustomDevice", out _RegisterCustomDevice);
            handle.GetLibraryMethod("ts3client_unregisterCustomDevice", out _UnregisterCustomDevice);
            handle.GetLibraryMethod("ts3client_processCustomCaptureData", out _ProcessCustomCaptureData);
            handle.GetLibraryMethod("ts3client_acquireCustomPlaybackData", out _AcquireCustomPlaybackData);
            handle.GetLibraryMethod("ts3client_setLocalTestMode", out _SetLocalTestMode);
            handle.GetLibraryMethod("ts3client_startVoiceRecording", out _StartVoiceRecording);
            handle.GetLibraryMethod("ts3client_stopVoiceRecording", out _StopVoiceRecording);
            handle.GetLibraryMethod("ts3client_allowWhispersFrom", out _AllowWhispersFrom);
            handle.GetLibraryMethod("ts3client_removeFromAllowedWhispersFrom", out _RemoveFromAllowedWhispersFrom);
            handle.GetLibraryMethod("ts3client_systemset3DListenerAttributes", out _Systemset3DListenerAttributes);
            handle.GetLibraryMethod("ts3client_set3DWaveAttributes", out _Set3DWaveAttributes);
            handle.GetLibraryMethod("ts3client_systemset3DSettings", out _Systemset3DSettings);
            handle.GetLibraryMethod("ts3client_channelset3DAttributes", out _Channelset3DAttributes);
            handle.GetLibraryMethod("ts3client_getPreProcessorInfoValueFloat", out _GetPreProcessorInfoValueFloat);
            handle.GetLibraryMethod("ts3client_getPreProcessorConfigValue", out _GetPreProcessorConfigValue);
            handle.GetLibraryMethod("ts3client_setPreProcessorConfigValue", out _SetPreProcessorConfigValue);
            //handle.GetLibraryMethod("ts3client_getEncodeConfigValue", out _GetEncodeConfigValue);
            handle.GetLibraryMethod("ts3client_getPlaybackConfigValueAsFloat", out _GetPlaybackConfigValueAsFloat);
            handle.GetLibraryMethod("ts3client_setPlaybackConfigValue", out _SetPlaybackConfigValue);
            handle.GetLibraryMethod("ts3client_setClientVolumeModifier", out _SetClientVolumeModifier);
            handle.GetLibraryMethod("ts3client_logMessage", out _LogMessage);
            handle.GetLibraryMethod("ts3client_setLogVerbosity", out _SetLogVerbosity);
            handle.GetLibraryMethod("ts3client_getErrorMessage", out _GetErrorMessage);
            handle.GetLibraryMethod("ts3client_startConnection", out _StartConnection);
            handle.GetLibraryMethod("ts3client_stopConnection", out _StopConnection);
            handle.GetLibraryMethod("ts3client_requestClientMove", out _RequestClientMove);
            handle.GetLibraryMethod("ts3client_requestClientVariables", out _RequestClientVariables);
            handle.GetLibraryMethod("ts3client_requestClientKickFromChannel", out _RequestClientKickFromChannel);
            handle.GetLibraryMethod("ts3client_requestClientKickFromServer", out _RequestClientKickFromServer);
            handle.GetLibraryMethod("ts3client_requestChannelDelete", out _RequestChannelDelete);
            handle.GetLibraryMethod("ts3client_requestChannelMove", out _RequestChannelMove);
            handle.GetLibraryMethod("ts3client_requestSendPrivateTextMsg", out _RequestSendPrivateTextMsg);
            handle.GetLibraryMethod("ts3client_requestSendChannelTextMsg", out _RequestSendChannelTextMsg);
            handle.GetLibraryMethod("ts3client_requestSendServerTextMsg", out _RequestSendServerTextMsg);
            handle.GetLibraryMethod("ts3client_requestConnectionInfo", out _RequestConnectionInfo);
            handle.GetLibraryMethod("ts3client_requestClientSetWhisperList", out _RequestClientSetWhisperList);
            handle.GetLibraryMethod("ts3client_requestChannelSubscribe", out _RequestChannelSubscribe);
            handle.GetLibraryMethod("ts3client_requestChannelSubscribeAll", out _RequestChannelSubscribeAll);
            handle.GetLibraryMethod("ts3client_requestChannelUnsubscribe", out _RequestChannelUnsubscribe);
            handle.GetLibraryMethod("ts3client_requestChannelUnsubscribeAll", out _RequestChannelUnsubscribeAll);
            handle.GetLibraryMethod("ts3client_requestChannelDescription", out _RequestChannelDescription);
            handle.GetLibraryMethod("ts3client_requestMuteClients", out _RequestMuteClients);
            handle.GetLibraryMethod("ts3client_requestUnmuteClients", out _RequestUnmuteClients);
            handle.GetLibraryMethod("ts3client_requestClientIDs", out _RequestClientIDs);
            //handle.GetLibraryMethod("ts3client_requestSlotsFromProvisioningServer", out _RequestSlotsFromProvisioningServer);
            //handle.GetLibraryMethod("ts3client_cancelRequestSlotsFromProvisioningServer", out _CancelRequestSlotsFromProvisioningServer);
            //handle.GetLibraryMethod("ts3client_startConnectionWithProvisioningKey", out _StartConnectionWithProvisioningKey);
            handle.GetLibraryMethod("ts3client_getClientID", out _GetClientID);
            handle.GetLibraryMethod("ts3client_getConnectionStatus", out _GetConnectionStatus);
            handle.GetLibraryMethod("ts3client_getConnectionVariableAsUInt64", out _GetConnectionVariableAsUInt64);
            handle.GetLibraryMethod("ts3client_getConnectionVariableAsDouble", out _GetConnectionVariableAsDouble);
            handle.GetLibraryMethod("ts3client_getConnectionVariableAsString", out _GetConnectionVariableAsString);
            handle.GetLibraryMethod("ts3client_cleanUpConnectionInfo", out _CleanUpConnectionInfo);
            //handle.GetLibraryMethod("ts3client_requestServerConnectionInfo", out _RequestServerConnectionInfo);
            //handle.GetLibraryMethod("ts3client_getServerConnectionVariableAsUInt64", out _GetServerConnectionVariableAsUInt64);
            //handle.GetLibraryMethod("ts3client_getServerConnectionVariableAsFloat", out _GetServerConnectionVariableAsFloat);
            //handle.GetLibraryMethod("ts3client_getClientSelfVariableAsInt", out _GetClientSelfVariableAsInt);
            //handle.GetLibraryMethod("ts3client_getClientSelfVariableAsString", out _GetClientSelfVariableAsString);
            handle.GetLibraryMethod("ts3client_setClientSelfVariableAsInt", out _SetClientSelfVariableAsInt);
            handle.GetLibraryMethod("ts3client_setClientSelfVariableAsString", out _SetClientSelfVariableAsString);
            handle.GetLibraryMethod("ts3client_flushClientSelfUpdates", out _FlushClientSelfUpdates);
            handle.GetLibraryMethod("ts3client_getClientVariableAsInt", out _GetClientVariableAsInt);
            handle.GetLibraryMethod("ts3client_getClientVariableAsUInt64", out _GetClientVariableAsUInt64);
            handle.GetLibraryMethod("ts3client_getClientVariableAsString", out _GetClientVariableAsString);
            handle.GetLibraryMethod("ts3client_getClientList", out _GetClientList);
            handle.GetLibraryMethod("ts3client_getChannelOfClient", out _GetChannelOfClient);
            handle.GetLibraryMethod("ts3client_getChannelVariableAsInt", out _GetChannelVariableAsInt);
            handle.GetLibraryMethod("ts3client_getChannelVariableAsUInt64", out _GetChannelVariableAsUInt64);
            handle.GetLibraryMethod("ts3client_getChannelVariableAsString", out _GetChannelVariableAsString);
            //handle.GetLibraryMethod("ts3client_getChannelIDFromChannelNames", out _GetChannelIDFromChannelNames);
            handle.GetLibraryMethod("ts3client_setChannelVariableAsInt", out _SetChannelVariableAsInt);
            handle.GetLibraryMethod("ts3client_setChannelVariableAsUInt64", out _SetChannelVariableAsUInt64);
            handle.GetLibraryMethod("ts3client_setChannelVariableAsString", out _SetChannelVariableAsString);
            handle.GetLibraryMethod("ts3client_flushChannelUpdates", out _FlushChannelUpdates);
            handle.GetLibraryMethod("ts3client_flushChannelCreation", out _FlushChannelCreation);
            handle.GetLibraryMethod("ts3client_getChannelList", out _GetChannelList);
            handle.GetLibraryMethod("ts3client_getChannelClientList", out _GetChannelClientList);
            handle.GetLibraryMethod("ts3client_getParentChannelOfChannel", out _GetParentChannelOfChannel);
            handle.GetLibraryMethod("ts3client_getChannelEmptySecs", out _GetChannelEmptySecs);
            handle.GetLibraryMethod("ts3client_getServerConnectionHandlerList", out _GetServerConnectionHandlerList);
            handle.GetLibraryMethod("ts3client_getServerVariableAsInt", out _GetServerVariableAsInt);
            handle.GetLibraryMethod("ts3client_getServerVariableAsUInt64", out _GetServerVariableAsUInt64);
            handle.GetLibraryMethod("ts3client_getServerVariableAsString", out _GetServerVariableAsString);
            handle.GetLibraryMethod("ts3client_requestServerVariables", out _RequestServerVariables);
            handle.GetLibraryMethod("ts3client_getTransferFileName", out _GetTransferFileName);
            handle.GetLibraryMethod("ts3client_getTransferFilePath", out _GetTransferFilePath);
            handle.GetLibraryMethod("ts3client_getTransferFileRemotePath", out _GetTransferFileRemotePath);
            handle.GetLibraryMethod("ts3client_getTransferFileSize", out _GetTransferFileSize);
            handle.GetLibraryMethod("ts3client_getTransferFileSizeDone", out _GetTransferFileSizeDone);
            handle.GetLibraryMethod("ts3client_isTransferSender", out _IsTransferSender);
            handle.GetLibraryMethod("ts3client_getTransferStatus", out _GetTransferStatus);
            handle.GetLibraryMethod("ts3client_getCurrentTransferSpeed", out _GetCurrentTransferSpeed);
            handle.GetLibraryMethod("ts3client_getAverageTransferSpeed", out _GetAverageTransferSpeed);
            handle.GetLibraryMethod("ts3client_getTransferRunTime", out _GetTransferRunTime);
            handle.GetLibraryMethod("ts3client_sendFile", out _SendFile);
            handle.GetLibraryMethod("ts3client_requestFile", out _RequestFile);
            handle.GetLibraryMethod("ts3client_haltTransfer", out _HaltTransfer);
            handle.GetLibraryMethod("ts3client_requestFileList", out _RequestFileList);
            handle.GetLibraryMethod("ts3client_requestFileInfo", out _RequestFileInfo);
            handle.GetLibraryMethod("ts3client_requestDeleteFile", out _RequestDeleteFile);
            handle.GetLibraryMethod("ts3client_requestCreateDirectory", out _RequestCreateDirectory);
            handle.GetLibraryMethod("ts3client_requestRenameFile", out _RequestRenameFile);
            handle.GetLibraryMethod("ts3client_getInstanceSpeedLimitUp", out _GetInstanceSpeedLimitUp);
            handle.GetLibraryMethod("ts3client_getInstanceSpeedLimitDown", out _GetInstanceSpeedLimitDown);
            handle.GetLibraryMethod("ts3client_getServerConnectionHandlerSpeedLimitUp", out _GetServerConnectionHandlerSpeedLimitUp);
            handle.GetLibraryMethod("ts3client_getServerConnectionHandlerSpeedLimitDown", out _GetServerConnectionHandlerSpeedLimitDown);
            handle.GetLibraryMethod("ts3client_getTransferSpeedLimit", out _GetTransferSpeedLimit);
            handle.GetLibraryMethod("ts3client_setInstanceSpeedLimitUp", out _SetInstanceSpeedLimitUp);
            handle.GetLibraryMethod("ts3client_setInstanceSpeedLimitDown", out _SetInstanceSpeedLimitDown);
            handle.GetLibraryMethod("ts3client_setServerConnectionHandlerSpeedLimitUp", out _SetServerConnectionHandlerSpeedLimitUp);
            handle.GetLibraryMethod("ts3client_setServerConnectionHandlerSpeedLimitDown", out _SetServerConnectionHandlerSpeedLimitDown);
            handle.GetLibraryMethod("ts3client_setTransferSpeedLimit", out _SetTransferSpeedLimit);
        }

        private struct LockObject : IDisposable
        {
            private SdkHandle Handle;

            public LockObject(SdkHandle handle)
            {
                Handle = handle;
                bool success = false;
                Handle.DangerousAddRef(ref success);
                if (success == false)
                    throw new ObjectDisposedException(typeof(Library).FullName);
            }
            void IDisposable.Dispose()
            {
                Handle.DangerousRelease();
            }
        }

        private LockObject Lock
        {
            get { return new LockObject(Handle); }
        }

        private void Check(Error error)
        {
            if (error != Error.Ok && error != Error.OkNoUpdate)
                throw Library.CreateException(error);
        }
        private void Check(Error error, Connection connection, string returnCode)
        {
            if (error != Error.Ok)
            {
                if (returnCode != null)
                    connection.SetReturnCodeResult(returnCode, error, null);
                if (error != Error.OkNoUpdate)
                    throw Library.CreateException(error);
            }
        }
        private void CheckNoThrow(Error error, Connection connection, string returnCode)
        {
            if (error != Error.Ok && returnCode != null)
            {
                connection.SetReturnCodeResult(returnCode, error, null);
            }
        }

        public bool FreeMemory(IntPtr pointer)
        {
            using (Lock) return _FreeMemory(pointer) == Error.Ok;
        }

        public void InitClientLib(ClientUIFunctions functionPointer, IntPtr functionRarePointers, LogTypes usedLogTypes, string logFileFolder, string resourcesFolder)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_logFileFolder = Native.WriteString(logFileFolder))
            using (UnmanagedPointer unmanaged_resourcesFolder = Native.WriteString(resourcesFolder))
            {
                Error error = _InitClientLib(ref functionPointer, functionRarePointers, (int)usedLogTypes, unmanaged_logFileFolder, unmanaged_resourcesFolder);
                Check(error);
            }
        }

        public string GetClientLibVersion()
        {
            using (Lock)
            {
                IntPtr unmanaged_value = IntPtr.Zero;
                Error error = _GetClientLibVersion(out unmanaged_value);
                string result = Native.ReadAndFreeString(unmanaged_value);
                Check(error);
                return result;
            }
        }

        public ulong GetClientLibVersionNumber()
        {
            using (Lock)
            {
                ulong result;
                Check(_GetClientLibVersionNumber(out result));
                return result;
            }
        }

        public Connection SpawnNewServerConnectionHandler(int port)
        {
            return Library.GetServer(RawSpawnNewServerConnectionHandler(port));
        }

        public ulong RawSpawnNewServerConnectionHandler(int port)
        {
            using (Lock)
            {
                ulong result;
                Check(_SpawnNewServerConnectionHandler(port, out result));
                return result;
            }
        }

        public Error TryDestroyServerConnectionHandler(Connection connection)
        {
            using (Lock) return _DestroyServerConnectionHandler(connection.ID);
        }

        public string CreateIdentity()
        {
            using (Lock)
            {
                IntPtr unmanaged_result;
                Error error = _CreateIdentity(out unmanaged_result);
                string result = Native.ReadAndFreeString(unmanaged_result);
                Check(error);
                return result;
            }
        }

        public string IdentityStringToUniqueIdentifier(string identityString)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_identityString = Native.WriteString(identityString))
            {
                IntPtr unmanaged_result;
                Error error = _IdentityStringToUniqueIdentifier(unmanaged_identityString, out unmanaged_result);
                string result = Native.ReadAndFreeString(unmanaged_result);
                Check(error);
                return result;
            }
        }

        public List<SoundDevice> GetPlaybackDeviceList(string modeID)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_modeID = Native.WriteString(modeID))
            {
                IntPtr unmanged_result;
                Error error = _GetPlaybackDeviceList(unmanaged_modeID, out unmanged_result);
                List<SoundDevice> result = Native.ReadAndFreePointerList(unmanged_result, Native.ReadAndFreeSoundDevice(modeID));
                Check(error);
                return result;
            }
        }

        public List<SoundDevice> GetCaptureDeviceList(string modeID)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_modeID = Native.WriteString(modeID))
            {
                IntPtr unmanaged_result;
                Error error = _GetCaptureDeviceList(unmanaged_modeID, out unmanaged_result);
                List<SoundDevice> result = Native.ReadAndFreePointerList(unmanaged_result, Native.ReadAndFreeSoundDevice(modeID));
                Check(error);
                return result;
            }
        }

        public List<string> GetPlaybackModeList()
        {
            using (Lock)
            {
                IntPtr unmanaged_result;
                Error error = _GetPlaybackModeList(out unmanaged_result);
                List<string> result = Native.ReadAndFreePointerList(unmanaged_result, Native.ReadAndFreeString);
                Check(error);
                return result;
            }
        }

        public List<string> GetCaptureModeList()
        {
            using (Lock)
            {
                IntPtr unmanaged_result;
                Error error = _GetCaptureModeList(out unmanaged_result);
                List<string> result = Native.ReadAndFreePointerList(unmanaged_result, Native.ReadAndFreeString);
                Check(error);
                return result;

            }
        }

        public void OpenPlaybackDevice(Connection connection, string modeID, string playbackDevice)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_modeID = Native.WriteString(modeID))
            using (UnmanagedPointer unmanaged_playbackDecive = Native.WriteString(playbackDevice))
            {
                Error error = _OpenPlaybackDevice(connection.ID, unmanaged_modeID, unmanaged_playbackDecive);
                Check(error);
            }
        }

        public void OpenCaptureDevice(Connection connection, string modeID, string captureDevice)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_modeID = Native.WriteString(modeID))
            using (UnmanagedPointer unmanaged_captureDevice = Native.WriteString(captureDevice))
            {
                Error error = _OpenCaptureDevice(connection.ID, unmanaged_modeID, unmanaged_captureDevice);
                Check(error);
            }
        }

        public void GetCurrentPlaybackDeviceName(Connection connection, out string result, out bool isDefault)
        {
            using (Lock)
            {
                IntPtr unmanaged_result;
                Error error = _GetCurrentPlaybackDeviceName(connection.ID, out unmanaged_result, out isDefault);
                result = Native.ReadString(unmanaged_result);
                Check(error);
            }
        }

        public string GetCurrentPlayBackMode(Connection connection)
        {
            using (Lock)
            {
                IntPtr unmanaged_result;
                Error error = _GetCurrentPlayBackMode(connection.ID, out unmanaged_result);
                string result = Native.ReadAndFreeString(unmanaged_result);
                Check(error);
                return result;
            }
        }

        public void GetCurrentCaptureDeviceName(Connection connection, out string result, out bool isDefault)
        {
            using (Lock)
            {
                IntPtr unmanaged_result;
                Error error = _GetCurrentCaptureDeviceName(connection.ID, out unmanaged_result, out isDefault);
                result = Native.ReadAndFreeString(unmanaged_result);
                Check(error);
            }
        }

        public string GetCurrentCaptureMode(Connection connection)
        {
            using (Lock)
            {
                IntPtr unmanaged_result;
                Error error = _GetCurrentCaptureMode(connection.ID, out unmanaged_result);
                string result = Native.ReadAndFreeString(unmanaged_result);
                Check(error);
                return result;
            }
        }

        public Error TryInitiateGracefulPlaybackShutdown(Connection connection)
        {
            using (Lock)
            {
                return _InitiateGracefulPlaybackShutdown(connection.ID);
            }
        }

        public void ClosePlaybackDevice(Connection connection)
        {
            using (Lock)
            {
                Check(_ClosePlaybackDevice(connection.ID));
            }
        }

        public void CloseCaptureDevice(Connection connection)
        {
            using (Lock)
            {
                Check(_CloseCaptureDevice(connection.ID));
            }
        }

        public void ActivateCaptureDevice(Connection connection)
        {
            using (Lock)
            {
                Check(_ActivateCaptureDevice(connection.ID));
            }
        }

        public WaveHandle PlayWaveFileHandle(Connection connection, string path, bool loop)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_path = Native.WriteString(path))
            {
                ulong waveHandleValue;
                Error error = _PlayWaveFileHandle(connection.ID, unmanaged_path, loop, out waveHandleValue);
                Check(error);
                return connection.Cache.GetWaveHandle(waveHandleValue);
            }
        }

        public void PauseWaveFileHandle(WaveHandle waveHandle, bool pause)
        {
            using (Lock)
            {
                Check(_PauseWaveFileHandle(waveHandle.Connection.ID, waveHandle.ID, pause));
            }
        }

        public void CloseWaveFileHandle(WaveHandle waveHandle)
        {
            using (Lock)
            {
                Check(_CloseWaveFileHandle(waveHandle.Connection.ID, waveHandle.ID));
            }
        }

        public void RegisterCustomDevice(string deviceID, string deviceDisplayName, int capFrequency, int capChannels, int playFrequency, int playChannels)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_deviceID = Native.WriteString(deviceID))
            using (UnmanagedPointer unmanaged_deviceDisplayName = Native.WriteString(deviceDisplayName ?? string.Empty))
            {
                Error error = _RegisterCustomDevice(unmanaged_deviceID, unmanaged_deviceDisplayName, capFrequency, capChannels, playFrequency, playChannels);
                Check(error);
            }
        }

        public bool TryUnregisterCustomDevice(string deviceID)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_deviceID = Native.WriteString(deviceID))
            {
                Error error = _UnregisterCustomDevice(unmanaged_deviceID);
                switch (error)
                {
                    case Error.Ok: return true;
                    case Error.SoundUnknownDevice: return false;
                    default: throw Library.CreateException(error);
                }
            }
        }

        public void ProcessCustomCaptureData(string deviceName, short[] buffer, int samples)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_deviceName = Native.WriteString(deviceName))
            {
                Error error = _ProcessCustomCaptureData(unmanaged_deviceName, buffer, samples);
                Check(error);
            }
        }

        public bool AcquireCustomPlaybackData(string deviceName, short[] buffer, int samples)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_deviceName = Native.WriteString(deviceName))
            {
                Error error = _AcquireCustomPlaybackData(unmanaged_deviceName, buffer, samples);
                if (error == Error.SoundNoData) return false;
                Check(error);
                return true;
            }
        }

        public void SetLocalTestMode(Connection connection, bool enabled)
        {
            using (Lock)
            {
                Check(_SetLocalTestMode(connection.ID, enabled ? 1 : 0));
            }
        }

        public void StartVoiceRecording(Connection connection)
        {
            using (Lock)
            {
                Check(_StartVoiceRecording(connection.ID));
            }
        }

        public void StopVoiceRecording(Connection connection)
        {
            using (Lock)
            {
                Check(_StopVoiceRecording(connection.ID));
            }
        }

        public void AllowWhispersFrom(Client client)
        {
            using (Lock)
            {
                Check(_AllowWhispersFrom(client.Connection.ID, client.ID));
            }
        }

        public void RemoveFromAllowedWhispersFrom(Client client)
        {
            using (Lock)
            {
                Check(_RemoveFromAllowedWhispersFrom(client.Connection.ID, client.ID));
            }
        }

        public void Systemset3DListenerAttributes(Connection connection, Vector? position, Vector? forward, Vector? up)
        {
            using (Lock)
            {
                Vector?[] vectors = new Vector?[] { position, forward, up };
                int sizeOfVector = Marshal.SizeOf(typeof(Vector));
                IntPtr unmanaged_vectors = IntPtr.Zero;
                try
                {
                    unmanaged_vectors = Marshal.AllocHGlobal(sizeOfVector * vectors.Length);
                    IntPtr[] pointers = new IntPtr[vectors.Length];
                    for (int i = 0; i < vectors.Length; i++)
                    {
                        if (vectors[i].HasValue)
                        {
                            pointers[i] = unmanaged_vectors + (i * sizeOfVector);
                            Marshal.StructureToPtr(vectors[i].Value, pointers[i], false);
                        }
                        else pointers[i] = IntPtr.Zero;
                    }
                    Error error = _Systemset3DListenerAttributes(connection.ID, pointers[0], pointers[1], pointers[2]);
                    Check(error);

                }
                finally
                {
                    if (unmanaged_vectors != IntPtr.Zero)
                        Marshal.FreeHGlobal(unmanaged_vectors);
                }
            }
        }

        public void Set3DWaveAttributes(WaveHandle waveHandle, Vector position)
        {
            using (Lock)
            {
                Error error = _Set3DWaveAttributes(waveHandle.Connection.ID, waveHandle.ID, ref position);
                Check(error);
            }
        }

        public void Systemset3DSettings(Connection connection, float distanceFactor, float rolloffScale)
        {
            using (Lock)
            {
                Check(_Systemset3DSettings(connection.ID, distanceFactor, rolloffScale));
            }
        }

        public void Channelset3DAttributes(Client client, Vector position)
        {
            using (Lock)
            {
                Check(_Channelset3DAttributes(client.Connection.ID, client.ID, ref position));
            }
        }

        public float GetPreProcessorInfoValueFloat(Connection connection, string ident)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_ident = Native.WriteString(ident))
            {
                float result;
                Check(_GetPreProcessorInfoValueFloat(connection.ID, unmanaged_ident, out result));
                return result;
            }
        }

        public string GetPreProcessorConfigValue(Connection connection, string ident)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_ident = Native.WriteString(ident))
            {
                IntPtr unmanaged_result;
                Error error = _GetPreProcessorConfigValue(connection.ID, unmanaged_ident, out unmanaged_result);
                string result = Native.ReadAndFreeString(unmanaged_result);
                Check(error);
                return result;
            }
        }

        public void SetPreProcessorConfigValue(Connection connection, string ident, string value)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_ident = Native.WriteString(ident))
            using (UnmanagedPointer unmanaged_value = Native.WriteString(value))
            {
                Error error = _SetPreProcessorConfigValue(connection.ID, unmanaged_ident, unmanaged_value);
                Check(error);
            }
        }

        public float GetPlaybackConfigValueAsFloat(Connection connection, string ident)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_ident = Native.WriteString(ident))
            {
                float result;
                Error error = _GetPlaybackConfigValueAsFloat(connection.ID, unmanaged_ident, out result);
                Check(error);
                return result;
            }
        }

        public void SetPlaybackConfigValue(Connection connection, string ident, string value)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_ident = Native.WriteString(ident))
            using (UnmanagedPointer unmanaged_value = Native.WriteString(value))
            {
                Check(_SetPlaybackConfigValue(connection.ID, unmanaged_ident, unmanaged_value));
            }
        }

        public void SetClientVolumeModifier(Connection connection, ushort clientID, float value)
        {
            using (Lock)
            {
                Check(_SetClientVolumeModifier(connection.ID, clientID, value));
            }
        }

        public void LogMessage(string logMessage, LogLevel severity, string channel, ulong logID)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_logMessage = Native.WriteString(logMessage))
            using (UnmanagedPointer unmanaged_channel = Native.WriteString(channel))
            {
                Check(_LogMessage(unmanaged_logMessage, (int)severity, unmanaged_channel, logID));
            }
        }

        public void SetLogVerbosity(LogLevel logVerbosity)
        {
            using (Lock)
            {
                Check(_SetLogVerbosity((int)logVerbosity));
            }
        }

        public Error TryGetErrorMessage(Error error, out string message)
        {
            using (Lock)
            {
                IntPtr result;
                error = _GetErrorMessage((uint)error, out result);
                message = Native.ReadAndFreeString(result);
                return error;
            }
        }

        public Error TryStartConnection(Connection connection, string identity, string ip, uint port, string nickname, string[] defaultChannelArray, string defaultChannelPassword, string serverPassword)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_identity = Native.WriteString(identity))
            using (UnmanagedPointer unmanaged_ip = Native.WriteString(ip))
            using (UnmanagedPointer unmanaged_nickname = Native.WriteString(nickname))
            using (UnmanagedPointer unmanaged_defaultChannelArray = Native.WriteStringArray(defaultChannelArray))
            using (UnmanagedPointer unmanaged_defaultChannelPassword = Native.WriteString(defaultChannelPassword ?? string.Empty))
            using (UnmanagedPointer unmanaged_serverPassword = Native.WriteString(serverPassword ?? string.Empty))
            {
                return _StartConnection(connection.ID, unmanaged_identity, unmanaged_ip, port, unmanaged_nickname, unmanaged_defaultChannelArray, unmanaged_defaultChannelPassword, unmanaged_serverPassword);
            }
        }

        public Error TryStopConnection(Connection connection, string quitMessage)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_quitMessage = Native.WriteString(quitMessage ?? string.Empty))
            {
                return _StopConnection(connection.ID, unmanaged_quitMessage);
            }
        }

        public void RequestClientMove(Client client, Channel channel, string password, string returnCode)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_password = Native.WriteString(password ?? string.Empty))
            using (UnmanagedPointer unmanaged_returnCode = Native.WriteString(returnCode))
            {
                Error error = _RequestClientMove(client.Connection.ID, client.ID, channel.ID, unmanaged_password, unmanaged_returnCode);
                Check(error, client.Connection, returnCode);
            }
        }

        public void RequestClientVariables(Client client, string returnCode)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_returnCode = Native.WriteString(returnCode))
            {
                Error error = _RequestClientVariables(client.Connection.ID, client.ID, unmanaged_returnCode);
                Check(error, client.Connection, returnCode);
            }
        }

        public void RequestClientKickFromChannel(Client client, string kickReason, string returnCode)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_kickReason = Native.WriteString(kickReason ?? string.Empty))
            using (UnmanagedPointer unmanaged_returnCode = Native.WriteString(returnCode))
            {
                Error error = _RequestClientKickFromChannel(client.Connection.ID, client.ID, unmanaged_kickReason, unmanaged_returnCode);
                Check(error, client.Connection, returnCode);
            }
        }

        public void RequestClientKickFromServer(Client client, string kickReason, string returnCode)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_kickReason = Native.WriteString(kickReason ?? string.Empty))
            using (UnmanagedPointer unmanaged_returnCode = Native.WriteString(returnCode))
            {
                Error error = _RequestClientKickFromServer(client.Connection.ID, client.ID, unmanaged_kickReason, unmanaged_returnCode);
                Check(error, client.Connection, returnCode);
            }
        }

        public void RequestChannelDelete(Channel channel, bool force, string returnCode)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_returnCode = Native.WriteString(returnCode))
            {
                Error error = _RequestChannelDelete(channel.Connection.ID, channel.ID, force ? 1 : 0, unmanaged_returnCode);
                Check(error, channel.Connection, returnCode);
            }
        }

        public void RequestChannelMove(Channel channel, Channel newChannelParent, Channel newChannelOrder, string returnCode)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_returnCode = Native.WriteString(returnCode))
            {
                Error error = _RequestChannelMove(channel.Connection.ID, channel.ID, newChannelParent.ID, newChannelOrder?.ID ?? 0, unmanaged_returnCode);
                Check(error, channel.Connection, returnCode);
            }
        }

        public void RequestSendPrivateTextMsg(Client targetClient, string message, string returnCode)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_message = Native.WriteString(message))
            using (UnmanagedPointer unmanaged_returnCode = Native.WriteString(returnCode))
            {
                Error error = _RequestSendPrivateTextMsg(targetClient.Connection.ID, unmanaged_message, targetClient.ID, unmanaged_returnCode);
                Check(error, targetClient.Connection, returnCode);
            }
        }

        public void RequestSendChannelTextMsg(Channel targetChannel, string message, string returnCode)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_message = Native.WriteString(message))
            using (UnmanagedPointer unmanaged_returnCode = Native.WriteString(returnCode))
            {
                Error error = _RequestSendChannelTextMsg(targetChannel.Connection.ID, unmanaged_message, targetChannel.ID, unmanaged_returnCode);
                Check(error, targetChannel.Connection, returnCode);
            }
        }

        public void RequestSendServerTextMsg(Connection connection, string message, string returnCode)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_message = Native.WriteString(message))
            using (UnmanagedPointer unmanaged_returnCode = Native.WriteString(returnCode))
            {
                Error error = _RequestSendServerTextMsg(connection.ID, unmanaged_message, unmanaged_returnCode);
                Check(error, connection, returnCode);
            }
        }

        public void RequestConnectionInfo(Client client, string returnCode)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_returnCode = Native.WriteString(returnCode))
            {
                Error error = _RequestConnectionInfo(client.Connection.ID, client.ID, unmanaged_returnCode);
                Check(error, client.Connection, returnCode);
            }
        }

        public void RequestClientSetWhisperList(Client client, Channel[] targetChannelArray, Client[] targetClientArray, string returnCode)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_targetChannelIDArray = Native.WriteLongIDArray(targetChannelArray, GetID))
            using (UnmanagedPointer unmanaged_targetClientIDArray = Native.WriteShortIDArray(targetClientArray, GetID))
            using (UnmanagedPointer unmanaged_returnCode = Native.WriteString(returnCode))
            {
                Error error = _RequestClientSetWhisperList(client.Connection.ID, client.ID, unmanaged_targetChannelIDArray, unmanaged_targetClientIDArray, unmanaged_returnCode);
                Check(error, client.Connection, returnCode);
            }
        }

        public void RequestChannelSubscribe(Channel[] channelArray, string returnCode)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_channelIDArray = Native.WriteLongIDArray(channelArray, GetID))
            using (UnmanagedPointer unmanaged_returnCode = Native.WriteString(returnCode))
            {
                Connection connection = channelArray[0].Connection;
                Error error = _RequestChannelSubscribe(connection.ID, unmanaged_channelIDArray, unmanaged_returnCode);
                Check(error, connection, returnCode);
            }
        }

        public void RequestChannelSubscribeAll(Connection connection, string returnCode)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_returnCode = Native.WriteString(returnCode))
            {
                Error error = _RequestChannelSubscribeAll(connection.ID, unmanaged_returnCode);
                Check(error, connection, returnCode);
            }
        }

        public void RequestChannelUnsubscribe(Channel[] channelArray, string returnCode)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_channelIDArray = Native.WriteLongIDArray(channelArray, GetID))
            using (UnmanagedPointer unmanaged_returnCode = Native.WriteString(returnCode))
            {
                Connection connection = channelArray[0].Connection;
                Error error = _RequestChannelUnsubscribe(connection.ID, unmanaged_channelIDArray, unmanaged_returnCode);
                Check(error, connection, returnCode);
            }
        }

        public void RequestChannelUnsubscribeAll(Connection connection, string returnCode)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_returnCode = Native.WriteString(returnCode))
            {
                Error error = _RequestChannelUnsubscribeAll(connection.ID, unmanaged_returnCode);
                Check(error, connection, returnCode);
            }
        }

        public void RequestChannelDescription(Channel channel, string returnCode)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_returnCode = Native.WriteString(returnCode))
            {
                Error error = _RequestChannelDescription(channel.Connection.ID, channel.ID, unmanaged_returnCode);
                Check(error, channel.Connection, returnCode);
            }
        }

        public void RequestMuteClients(Client[] clientArray, string returnCode)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_clientIDArray = Native.WriteShortIDArray(clientArray, GetID))
            using (UnmanagedPointer unmanaged_returnCode = Native.WriteString(returnCode))
            {
                Connection connection = clientArray[0].Connection;
                Error error = _RequestMuteClients(connection.ID, unmanaged_clientIDArray, unmanaged_returnCode);
                Check(error, connection, returnCode);
            }
        }

        public void RequestUnmuteClients(Client[] clientArray, string returnCode)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_clientIDArray = Native.WriteShortIDArray(clientArray, GetID))
            using (UnmanagedPointer unmanaged_returnCode = Native.WriteString(returnCode))
            {
                Connection connection = clientArray[0].Connection;
                Error error = _RequestUnmuteClients(connection.ID, unmanaged_clientIDArray, unmanaged_returnCode);
                Check(error, connection, returnCode);
            }
        }

        public Error TryRequestClientIDs(Connection connection, string uniqueIdentifier, string returnCode)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_uniqueIdentifier = Native.WriteString(uniqueIdentifier))
            using (UnmanagedPointer unmanaged_returnCode = Native.WriteString(returnCode))
            {
                Error error = _RequestClientIDs(connection.ID, unmanaged_uniqueIdentifier, unmanaged_returnCode);
                CheckNoThrow(error, connection, returnCode);
                return error;
            }
        }

        public Error TryGetClientID(Connection connection, out Client result)
        {
            using (Lock)
            {
                ushort unmanaged_result;
                Error error = _GetClientID(connection.ID, out unmanaged_result);
                if (error == Error.Ok)
                    result = connection.Cache.GetClient(unmanaged_result);
                else result = null;
                return error;
            }
        }

        public ConnectStatus GetConnectionStatus(Connection connection)
        {
            using (Lock)
            {
                int unmanaged_result;
                Check(_GetConnectionStatus(connection.ID, out unmanaged_result));
                return (ConnectStatus)unmanaged_result;
            }
        }

        public ulong GetConnectionVariableAsUInt64(Client client, ConnectionProperty flag)
        {
            ulong result;
            Check(TryGetConnectionVariableAsUInt64(client, flag, out result));
            return result;
        }

        public Error TryGetConnectionVariableAsUInt64(Client client, ConnectionProperty flag, out ulong result)
        {
            using (Lock)
            {
                return _GetConnectionVariableAsUInt64(client.Connection.ID, client.ID, (IntPtr)flag, out result);
            }
        }

        public double GetConnectionVariableAsDouble(Client client, ConnectionProperty flag)
        {
            using (Lock)
            {
                double result;
                Check(_GetConnectionVariableAsDouble(client.Connection.ID, client.ID, (IntPtr)flag, out result));
                return result;
            }
        }

        public string GetConnectionVariableAsString(Client client, ConnectionProperty flag)
        {
            using (Lock)
            {
                IntPtr unmanaged_result;
                Error error = _GetConnectionVariableAsString(client.Connection.ID, client.ID, (IntPtr)flag, out unmanaged_result);
                string result = Native.ReadAndFreeString(unmanaged_result);
                Check(error);
                return result;
            }
        }

        public void CleanUpConnectionInfo(Client client)
        {
            using (Lock)
            {
                Check(_CleanUpConnectionInfo(client.Connection.ID, client.ID));
            }
        }

        public void SetClientSelfVariableAsInt(Connection connection, ClientProperty flag, int value)
        {
            using (Lock)
            {
                Check(_SetClientSelfVariableAsInt(connection.ID, (IntPtr)flag, value));
            }
        }

        public void SetClientSelfVariableAsString(Connection connection, ClientProperty flag, string value)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_value = Native.WriteString(value))
            {
                Check(_SetClientSelfVariableAsString(connection.ID, (IntPtr)flag, unmanaged_value));
            }
        }

        public void FlushClientSelfUpdates(Connection connection, string returnCode)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_returnCode = Native.WriteString(returnCode))
            {
                Error error = _FlushClientSelfUpdates(connection.ID, unmanaged_returnCode);
                Check(error, connection, returnCode);
            }
        }

        public int GetClientVariableAsInt(Client client, ClientProperty flag)
        {
            using (Lock)
            {
                int result;
                Check(_GetClientVariableAsInt(client.Connection.ID, client.ID, (IntPtr)flag, out result));
                return result;
            }
        }

        public ulong GetClientVariableAsUInt64(Client client, ClientProperty flag)
        {
            using (Lock)
            {
                ulong result;
                Check(_GetClientVariableAsUInt64(client.Connection.ID, client.ID, (IntPtr)flag, out result));
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
                Error error = _GetClientVariableAsString(client.Connection.ID, client.ID, (IntPtr)flag, out unmanaged_result);
                result = Native.ReadAndFreeString(unmanaged_result);
                return error;
            }
        }

        public List<Client> GetClientList(Connection connection)
        {
            using (Lock)
            {
                IntPtr unmanaged_result;
                Error error = _GetClientList(connection.ID, out unmanaged_result);
                List<Client> result = Native.ReadAndFreeShortIDList(unmanaged_result, connection.Cache.GetClient);
                Check(error);
                return result;
            }
        }

        public Channel GetChannelOfClient(Client client)
        {
            using (Lock)
            {
                ulong id;
                Error error = _GetChannelOfClient(client.Connection.ID, client.ID, out id);
                Check(error);
                return client.Connection.Cache.GetChannel(id);
            }
        }

        public int GetChannelVariableAsInt(Channel channel, ChannelProperty flag)
        {
            using (Lock)
            {
                int result;
                Check(_GetChannelVariableAsInt(channel.Connection.ID, channel.ID, (IntPtr)flag, out result));
                return result;
            }
        }

        public ulong GetChannelVariableAsUInt64(Channel channel, ChannelProperty flag)
        {
            using (Lock)
            {
                ulong result;
                Check(TryGetChannelVariableAsUInt64(channel, flag, out result));
                return result;
            }
        }

        public Error TryGetChannelVariableAsUInt64(Channel channel, ChannelProperty flag, out ulong result)
        {
            using (Lock)
            {
                return _GetChannelVariableAsUInt64(channel.Connection.ID, channel.ID, (IntPtr)flag, out result);
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
                Error error = _GetChannelVariableAsString(channel.Connection.ID, channel.ID, (IntPtr)flag, out unmanaged_result);
                result = Native.ReadAndFreeString(unmanaged_result);
                return error;
            }
        }

        public void SetChannelVariableAsInt(Channel channel, ChannelProperty flag, int value)
        {
            using (Lock)
            {
                Check(_SetChannelVariableAsInt(channel.Connection.ID, channel.ID, (IntPtr)flag, value));
            }
        }

        public void SetChannelVariableAsUInt64(Channel channel, ChannelProperty flag, ulong value)
        {
            using (Lock)
            {
                Check(_SetChannelVariableAsUInt64(channel.Connection.ID, channel.ID, (IntPtr)flag, value));
            }
        }

        public void SetChannelVariableAsString(Channel channel, ChannelProperty flag, string value)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_value = Native.WriteString(value))
            {
                Error error = _SetChannelVariableAsString(channel.Connection.ID, channel.ID, (IntPtr)flag, unmanaged_value);
                Check(error);
            }
        }

        public void FlushChannelUpdates(Channel channel, string returnCode)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_returnCode = Native.WriteString(returnCode))
            {
                Error error = _FlushChannelUpdates(channel.Connection.ID, channel.ID, unmanaged_returnCode);
                Check(error, channel.Connection, returnCode);
            }
        }

        public Error TryFlushChannelCreation(Connection connection, Channel parentChannel, string returnCode)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_returnCode = Native.WriteString(returnCode))
            {
                Error error = _FlushChannelCreation(connection.ID, parentChannel?.ID ?? 0, unmanaged_returnCode);
                CheckNoThrow(error, connection, returnCode);
                return error;
            }
        }

        public List<Channel> GetChannelList(Connection connection)
        {
            using (Lock)
            {
                IntPtr unmanaged_result;
                Error error = _GetChannelList(connection.ID, out unmanaged_result);
                List<Channel> result = Native.ReadAndFreeLongIDList(unmanaged_result, connection.Cache.GetChannel);
                Check(error);
                return result;
            }
        }

        public List<Client> GetChannelClientList(Channel channel)
        {
            using (Lock)
            {
                IntPtr unmanaged_result;
                Error error = _GetChannelClientList(channel.Connection.ID, channel.ID, out unmanaged_result);
                List<Client> result = Native.ReadAndFreeShortIDList(unmanaged_result, channel.Connection.Cache.GetClient);
                Check(error);
                return result;
            }
        }

        public Channel GetParentChannelOfChannel(Channel channel)
        {
            using (Lock)
            {
                ulong id;
                Error error = TryGetParentChannelOfChannel(channel.Connection, channel.ID, out id);
                Check(error);
                return channel.Connection.Cache.GetChannel(id);
            }
        }

        internal Error TryGetParentChannelOfChannel(Connection connection, ulong channelID, out ulong result)
        {
            using (Lock)
            {
                return _GetParentChannelOfChannel(connection.ID, channelID, out result);
            }
        }

        public Error TryGetChannelEmptySecs(Channel channel, out int result)
        {
            using (Lock)
            {
                return _GetChannelEmptySecs(channel.Connection.ID, channel.ID, out result);
            }
        }

        public List<Connection> GetServerConnectionHandlerList()
        {
            using (Lock)
            {
                IntPtr unmanaged_result;
                Error error = _GetServerConnectionHandlerList(out unmanaged_result);
                List<Connection> result = Native.ReadAndFreeLongIDList(unmanaged_result, Library.GetServer);
                Check(error);
                return result;
            }
        }

        public int GetServerVariableAsInt(Connection connection, VirtualServerProperty flag)
        {
            using (Lock)
            {
                int result;
                Check(_GetServerVariableAsInt(connection.ID, (IntPtr)flag, out result));
                return result;
            }
        }

        public ulong GetServerVariableAsUInt64(Connection connection, VirtualServerProperty flag)
        {
            using (Lock)
            {
                ulong result;
                Check(_GetServerVariableAsUInt64(connection.ID, (IntPtr)flag, out result));
                return result;
            }
        }

        public string GetServerVariableAsString(Connection connection, VirtualServerProperty flag)
        {
            using (Lock)
            {
                IntPtr unmanaged_result;
                Error error = _GetServerVariableAsString(connection.ID, (IntPtr)flag, out unmanaged_result);
                string result = Native.ReadAndFreeString(unmanaged_result);
                Check(error);
                return result;
            }
        }

        public void RequestServerVariables(Connection connection)
        {
            using (Lock)
            {
                Check(_RequestServerVariables(connection.ID));
            }
        }

        public string GetTransferFileName(FileTransfer transfer)
        {
            using (Lock)
            {
                IntPtr unmanaged_result;
                Error error = _GetTransferFileName(transfer.ID, out unmanaged_result);
                string result = Native.ReadAndFreeString(unmanaged_result);
                Check(error);
                return result;
            }
        }

        public string GetTransferFilePath(FileTransfer transfer)
        {
            using (Lock)
            {
                IntPtr unmanaged_result;
                Error error = _GetTransferFilePath(transfer.ID, out unmanaged_result);
                string result = Native.ReadAndFreeString(unmanaged_result);
                Check(error);
                return result;
            }
        }

        public string GetTransferFileRemotePath(FileTransfer transfer)
        {
            using (Lock)
            {
                IntPtr unmanaged_result;
                Error error = _GetTransferFileRemotePath(transfer.ID, out unmanaged_result);
                string result = Native.ReadAndFreeString(unmanaged_result);
                Check(error);
                return result;
            }
        }

        public ulong GetTransferFileSize(FileTransfer transfer)
        {
            using (Lock)
            {
                ulong result;
                Check(_GetTransferFileSize(transfer.ID, out result));
                return result;
            }
        }

        public ulong GetTransferFileSizeDone(FileTransfer transfer)
        {
            using (Lock)
            {
                ulong result;
                Check(_GetTransferFileSizeDone(transfer.ID, out result));
                return result;
            }
        }

        public bool IsTransferSender(FileTransfer transfer)
        {
            using (Lock)
            {
                int unmanaged_result;
                Check(_IsTransferSender(transfer.ID, out unmanaged_result));
                return unmanaged_result != 0;
            }
        }

        public FileTransferState GetTransferStatus(FileTransfer transfer)
        {
            using (Lock)
            {
                int unmanaged_result;
                Check(_GetTransferStatus(transfer.ID, out unmanaged_result));
                return (FileTransferState)unmanaged_result;
            }
        }

        public float GetCurrentTransferSpeed(FileTransfer transfer)
        {
            using (Lock)
            {
                float result;
                Check(_GetCurrentTransferSpeed(transfer.ID, out result));
                return result;
            }
        }

        public float GetAverageTransferSpeed(FileTransfer transfer)
        {
            using (Lock)
            {
                float result;
                Check(_GetAverageTransferSpeed(transfer.ID, out result));
                return result;
            }
        }

        public ulong GetTransferRunTime(FileTransfer transfer)
        {
            using (Lock)
            {
                ulong result;
                Check(_GetTransferRunTime(transfer.ID, out result));
                return result;
            }
        }

        public FileTransfer SendFile(Channel channel, string channelPW, string file, bool overwrite, bool resume, string sourceDirectory, string returnCode)
        {
            FileTransfer result;
            Check(TrySendFile(channel, channelPW, file, overwrite, resume, sourceDirectory, returnCode, out result));
            return result;
        }
        public Error TrySendFile(Channel channel, string channelPW, string file, bool overwrite, bool resume, string sourceDirectory, string returnCode, out FileTransfer result)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_channelPW = Native.WriteString(channelPW ?? string.Empty))
            using (UnmanagedPointer unmanaged_file = Native.WriteString(file))
            using (UnmanagedPointer unmanaged_sourceDirectory = Native.WriteString(sourceDirectory))
            using (UnmanagedPointer unmanaged_returnCode = Native.WriteString(returnCode))
            {
                ushort id;
                Error error = _SendFile(channel.Connection.ID, channel.ID, unmanaged_channelPW, unmanaged_file, overwrite ? 1 : 0, resume ? 1 : 0, unmanaged_sourceDirectory, out id, unmanaged_returnCode);
                CheckNoThrow(error, channel.Connection, returnCode);
                result = error == Error.Ok ? channel.Connection.Cache.GetTransfer(id) : null;
                return error;
            }
        }

        public FileTransfer RequestFile(Channel channel, string channelPW, string file, bool overwrite, bool resume, string destinationDirectory, string returnCode)
        {
            FileTransfer result;
            Check(TryRequestFile(channel, channelPW, file, overwrite, resume, destinationDirectory, returnCode, out result));
            return result;
        }
        public Error TryRequestFile(Channel channel, string channelPW, string file, bool overwrite, bool resume, string destinationDirectory, string returnCode, out FileTransfer result)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_channelPW = Native.WriteString(channelPW ?? string.Empty))
            using (UnmanagedPointer unmanaged_file = Native.WriteString(file))
            using (UnmanagedPointer unmanaged_destinationDirectory = Native.WriteString(destinationDirectory))
            using (UnmanagedPointer unmanaged_returnCode = Native.WriteString(returnCode))
            {
                ushort id;
                Error error = _RequestFile(channel.Connection.ID, channel.ID, unmanaged_channelPW, unmanaged_file, overwrite ? 1 : 0, resume ? 1 : 0, unmanaged_destinationDirectory, out id, unmanaged_returnCode);
                CheckNoThrow(error, channel.Connection, returnCode);
                result = error == Error.Ok ? channel.Connection.Cache.GetTransfer(id) : null;
                return error;
            }
        }

        public void HaltTransfer(FileTransfer transfer, bool deleteUnfinishedFile, string returnCode)
        {
            Check(TryHaltTransfer(transfer, deleteUnfinishedFile, returnCode));
        }
        public Error TryHaltTransfer(FileTransfer transfer, bool deleteUnfinishedFile, string returnCode)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_returnCode = Native.WriteString(returnCode))
            {
                Error error = _HaltTransfer(transfer.Connection.ID, transfer.ID, deleteUnfinishedFile ? 1 : 0, unmanaged_returnCode);
                CheckNoThrow(error, transfer.Connection, returnCode);
                return error;
            }
        }

        public void RequestFileList(Channel channel, string channelPW, string path, string returnCode)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_channelPW = Native.WriteString(channelPW ?? string.Empty))
            using (UnmanagedPointer unmanaged_path = Native.WriteString(path ?? string.Empty))
            using (UnmanagedPointer unmanaged_returnCode = Native.WriteString(returnCode))
            {
                Error error = _RequestFileList(channel.Connection.ID, channel.ID, unmanaged_channelPW, unmanaged_path, unmanaged_returnCode);
                Check(error, channel.Connection, returnCode);
            }
        }

        public void RequestFileInfo(Channel channel, string channelPW, string file, string returnCode)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_channelPW = Native.WriteString(channelPW ?? string.Empty))
            using (UnmanagedPointer unmanaged_file = Native.WriteString(file ?? string.Empty))
            using (UnmanagedPointer unmanaged_returnCode = Native.WriteString(returnCode))
            {
                Error error = _RequestFileInfo(channel.Connection.ID, channel.ID, unmanaged_channelPW, unmanaged_file, unmanaged_returnCode);
                Check(error, channel.Connection, returnCode);
            }
        }

        public void RequestDeleteFile(Channel channel, string channelPW, string[] file, string returnCode)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_channelPW = Native.WriteString(channelPW ?? string.Empty))
            using (UnmanagedPointer unmanaged_file = Native.WriteStringArray(file))
            using (UnmanagedPointer unmanaged_returnCode = Native.WriteString(returnCode))
            {
                Error error = _RequestDeleteFile(channel.Connection.ID, channel.ID, unmanaged_channelPW, unmanaged_file, unmanaged_returnCode);
                Check(error, channel.Connection, returnCode);
            }
        }

        public void RequestCreateDirectory(Channel channel, string channelPW, string directoryPath, string returnCode)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_channelPW = Native.WriteString(channelPW ?? string.Empty))
            using (UnmanagedPointer unmanaged_directoryPath = Native.WriteString(directoryPath ?? string.Empty))
            using (UnmanagedPointer unmanaged_returnCode = Native.WriteString(returnCode))
            {
                Error error = _RequestCreateDirectory(channel.Connection.ID, channel.ID, unmanaged_channelPW, unmanaged_directoryPath, unmanaged_returnCode);
                Check(error, channel.Connection, returnCode);
            }
        }

        public void RequestRenameFile(Channel fromChannel, string fromChannelPW, Channel toChannel, string toChannelPW, string oldFile, string newFile, string returnCode)
        {
            using (Lock)
            using (UnmanagedPointer unmanaged_fromChannelPW = Native.WriteString(fromChannelPW ?? string.Empty))
            using (UnmanagedPointer unmanaged_toChannelPW = Native.WriteString(toChannelPW ?? string.Empty))
            using (UnmanagedPointer unmanaged_oldFile = Native.WriteString(oldFile ?? string.Empty))
            using (UnmanagedPointer unmanaged_newFile = Native.WriteString(newFile ?? string.Empty))
            using (UnmanagedPointer unmanaged_returnCode = Native.WriteString(returnCode))
            {
                Error error = _RequestRenameFile(fromChannel.Connection.ID, fromChannel.ID, unmanaged_fromChannelPW, toChannel.ID, unmanaged_toChannelPW, unmanaged_oldFile, unmanaged_newFile, unmanaged_returnCode);
                Check(error, fromChannel.Connection, returnCode);
            }
        }

        public ulong GetInstanceSpeedLimitUp()
        {
            using (Lock)
            {
                ulong limit;
                Check(_GetInstanceSpeedLimitUp(out limit));
                return limit;
            }
        }

        public ulong GetInstanceSpeedLimitDown()
        {
            using (Lock)
            {
                ulong limit;
                Check(_GetInstanceSpeedLimitDown(out limit));
                return limit;
            }
        }

        public ulong GetServerConnectionHandlerSpeedLimitUp(Connection connection)
        {
            using (Lock)
            {
                ulong limit;
                Check(_GetServerConnectionHandlerSpeedLimitUp(connection.ID, out limit));
                return limit;
            }
        }

        public ulong GetServerConnectionHandlerSpeedLimitDown(Connection connection)
        {
            using (Lock)
            {
                ulong limit;
                Check(_GetServerConnectionHandlerSpeedLimitDown(connection.ID, out limit));
                return limit;
            }
        }

        public ulong GetTransferSpeedLimit(FileTransfer transfer)
        {
            using (Lock)
            {
                ulong limit;
                Check(_GetTransferSpeedLimit(transfer.ID, out limit));
                return limit;
            }
        }

        public void SetInstanceSpeedLimitUp(ulong newLimit)
        {
            using (Lock)
            {
                Check(_SetInstanceSpeedLimitUp(newLimit));
            }
        }

        public void SetInstanceSpeedLimitDown(ulong newLimit)
        {
            using (Lock)
            {
                Check(_SetInstanceSpeedLimitDown(newLimit));
            }
        }

        public void SetServerConnectionHandlerSpeedLimitUp(Connection connection, ulong newLimit)
        {
            using (Lock)
            {
                Check(_SetServerConnectionHandlerSpeedLimitUp(connection.ID, newLimit));
            }
        }

        public void SetServerConnectionHandlerSpeedLimitDown(Connection connection, ulong newLimit)
        {
            using (Lock)
            {
                Check(_SetServerConnectionHandlerSpeedLimitDown(connection.ID, newLimit));
            }
        }

        public void SetTransferSpeedLimit(FileTransfer transfer, ulong newLimit)
        {
            using (Lock)
            {
                Check(_SetTransferSpeedLimit(transfer.ID, newLimit));
            }
        }

        private static ushort GetID(Client client) { return client.ID; }
        private static ulong GetID(Channel channel) { return channel.ID; }
    }
}
