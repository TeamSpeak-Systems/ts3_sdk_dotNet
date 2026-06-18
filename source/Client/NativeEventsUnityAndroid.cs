using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
#endif

namespace TeamSpeak.Sdk.Client
{
    partial class NativeEvents
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        private static char RECORD_SEPARATOR = '\x1e';

        public void ParseEvent(string message)
        {
            try {
                string[] parts = message.Split(RECORD_SEPARATOR);
                switch(parts[0]){
                    case "ConnectStatusChange":
                        OnConnectStatusChangeEventWrapper(ulong.Parse(parts[1]) , int.Parse(parts[2]), uint.Parse(parts[3]));
                        break;
                    case "ServerProtocolVersion":
                        OnServerProtocolVersionEventWrapper(ulong.Parse(parts[1]), int.Parse(parts[2]));
                        break;
                    case "NewChannel":
                        OnNewChannelEventWrapper(ulong.Parse(parts[1]), ulong.Parse(parts[2]), ulong.Parse(parts[3]));
                        break;
                    case "NewChannelCreated":
                        OnNewChannelCreated(ulong.Parse(parts[1]), ulong.Parse(parts[2]), ulong.Parse(parts[3]), ushort.Parse(parts[4]), parts[5], parts[6]);
                        break;
                    case "DelChannel": 
                        OnDelChannel(ulong.Parse(parts[1]), ulong.Parse(parts[2]), ushort.Parse(parts[3]) , parts[4], parts[5]);
                        break;
                    case "ChannelMove":
                        OnChannelMove(ulong.Parse(parts[1]) , ulong.Parse(parts[2]) , ulong.Parse(parts[3]) , ushort.Parse(parts[4]) , parts[5] , parts[6]);
                        break;
                    case "UpdateChannel":
                        OnUpdateChannelEventWrapper(ulong.Parse(parts[1]), ulong.Parse(parts[2]));
                        break;
                    case "UpdateChannelEdited":
                        OnUpdateChannelEdited(ulong.Parse(parts[1]), ulong.Parse(parts[2]), ushort.Parse(parts[3]), parts[4], parts[5]);
                        break;
                    case "UpdateClient":
                        OnUpdateClient(ulong.Parse(parts[1]), ushort.Parse(parts[2]), ushort.Parse(parts[3]), parts[4], parts[5]);
                        break;
                    case "ClientMove":
                        OnClientMove(ulong.Parse(parts[1]), ushort.Parse(parts[2]), ulong.Parse(parts[3]), ulong.Parse(parts[4]), int.Parse(parts[5]), parts[6]);
                        break;
                    case "ClientMoveSubscription":
                        OnClientMoveSubscriptionEventWrapper(ulong.Parse(parts[1]), ushort.Parse(parts[2]), ulong.Parse(parts[3]), ulong.Parse(parts[4]), int.Parse(parts[5]));
                        break;
                    case "ClientMoveTimeout":
                        OnClientMoveTimeout(ulong.Parse(parts[1]), ushort.Parse(parts[2]), ulong.Parse(parts[3]), ulong.Parse(parts[4]), int.Parse(parts[5]), parts[6]);
                        break;
                    case "ClientMoveMoved":
                        OnClientMoveMoved(ulong.Parse(parts[1]), ushort.Parse(parts[2]), ulong.Parse(parts[3]), ulong.Parse(parts[4]), int.Parse(parts[5]), ushort.Parse(parts[6]), parts[7], parts[8], parts[9]);
                        break;
                    case "ClientKickFromChannel":	
                        OnClientKickFromChannel(ulong.Parse(parts[1]), ushort.Parse(parts[2]), ulong.Parse(parts[3]), ulong.Parse(parts[4]), int.Parse(parts[5]), ushort.Parse(parts[6]), parts[7], parts[8], parts[9]);
                        break;
                    case "ClientKickFromServer":
                        OnClientKickFromServer(ulong.Parse(parts[1]), ushort.Parse(parts[2]), ulong.Parse(parts[3]), ulong.Parse(parts[4]), int.Parse(parts[5]), ushort.Parse(parts[6]), parts[7], parts[8], parts[9]);
                        break;
                    case "ClientIDs":
                        OnClientIDs(ulong.Parse(parts[1]), parts[2], ushort.Parse(parts[3]), parts[4]);
                        break;
                    case "ClientIDsFinished":
                        OnClientIDsFinishedEventWrapper(ulong.Parse(parts[1]));
                        break;
                    case "ServerEdited":
                        OnServerEdited(ulong.Parse(parts[1]), ushort.Parse(parts[2]), parts[3], parts[4]);
                        break;
                    case "ServerUpdated": 
                        OnServerUpdatedEventWrapper(ulong.Parse(parts[1]));
                        break;
                    case "ServerError":
                        OnServerError(ulong.Parse(parts[1]), parts[2], uint.Parse(parts[3]), parts[4], parts[5]);
                        break;
                    case "ServerStop":
                        OnServerStop(ulong.Parse(parts[1]), parts[2]);
                        break;
                    case "TextMessage":
                        OnTextMessage(ulong.Parse(parts[1]), ushort.Parse(parts[2]), ushort.Parse(parts[3]), ushort.Parse(parts[4]), parts[5], parts[6], parts[7]);
                        break;
                    case "TalkStatusChange":
                        OnTalkStatusChangeEventWrapper(ulong.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[3]), ushort.Parse(parts[4]));
                        break;
                    case "IgnoredWhisper":
                        OnIgnoredWhisperEventWrapper(ulong.Parse(parts[1]), ushort.Parse(parts[2]));
                        break;
                    case "ConnectionInfo":
                        OnConnectionInfoEventWrapper(ulong.Parse(parts[1]), ushort.Parse(parts[2]));
                        break;
                    case "ServerConnectionInfo":
                        OnServerConnectionInfoEventWrapper(ulong.Parse(parts[1]));
                        break;
                    case "ChannelSubscribe":
                        OnChannelSubscribeEventWrapper(ulong.Parse(parts[1]), ulong.Parse(parts[2]));
                        break;
                    case "ChannelSubscribeFinished":
                        OnChannelSubscribeFinishedEventWrapper(ulong.Parse(parts[1]));
                        break;
                    case "ChannelUnsubscribe":
                        OnChannelUnsubscribeEventWrapper(ulong.Parse(parts[1]), ulong.Parse(parts[2]));
                        break;
                    case "ChannelUnsubscribeFinished":
                        OnChannelUnsubscribeFinishedEventWrapper(ulong.Parse(parts[1]));
                        break;
                    case "ChannelDescriptionUpdate":
                        OnChannelDescriptionUpdateEventWrapper(ulong.Parse(parts[1]), ulong.Parse(parts[2]));
                        break;
                    case "ChannelPasswordChanged":
                        OnChannelPasswordChangedEventWrapper(ulong.Parse(parts[1]), ulong.Parse(parts[2]));
                        break;
                    case "PlaybackShutdownComplete":
                        OnPlaybackShutdownCompleteEventWrapper(ulong.Parse(parts[1]));
                        break;
                    case "SoundDeviceListChanged":
                        OnSoundDeviceListChanged(parts[1], int.Parse(parts[2]));
                        break;
                    case "UserLoggingMessage":
                        OnUserLoggingMessage(parts[1], int.Parse(parts[2]), parts[3], ulong.Parse(parts[4]), parts[5], parts[6]);
                        break;
                    /*case "ProvisioningSlotRequestResult":
                        OnProvisioningSlotRequestResultEventWrapper(uint.Parse(parts[1]), ulong.Parse(parts[2]), parts[3]);
                        break;*/
                    case "FileTransferStatus":
                        OnFileTransferStatus(ushort.Parse(parts[1]), uint.Parse(parts[2]), parts[3], ulong.Parse(parts[4]), ulong.Parse (parts[5]));
                        break;
                    case "FileList":
                        OnFileList(ulong.Parse(parts[1]), ulong.Parse(parts[2]), parts[3], parts[4], ulong.Parse(parts[5]), ulong.Parse (parts[6]),int.Parse(parts[7]),ulong.Parse(parts[8]),parts[9]);
                        break;
                    case "FileListFinished":
                        OnFileListFinished(ulong.Parse(parts[1]), ulong.Parse(parts[2]), parts[3]);
                        break;
                    case "FileInfo":
                        OnFileInfo(ulong.Parse(parts[1]), ulong.Parse(parts[2]), parts[3], ulong.Parse(parts[4]), ulong.Parse(parts[5]));
                        break;
                    case "AndroidServiceBound":
                        ulong connectionId = ulong.Parse(parts[1]);
                        string serverAddress = parts[2];
                        int port = int.Parse(parts[3]);
                        string serverPw = parts[4];
                        string identity = parts[5];
                        string nickname = parts[6];
                        int connectStatus = int.Parse(parts[7]);
                        Debug.Log($"{parts[0]} event: connectionId: {connectionId}, connectStatus: {connectStatus} ");
                        // TODO
                        break;
                    default:
                        Debug.Log("received unhandled message: " + parts[0]);
                        break;
                }
            } catch(Exception exc) {
                Debug.Log("Callback error: " + exc.Message);
            }
        }
#endif
    }
}
