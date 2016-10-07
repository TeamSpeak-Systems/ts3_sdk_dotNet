using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamSpeak.Sdk.Client
{
    /// <summary>
    /// Used after calling <see cref="O:TeamSpeak.Sdk.Client.Connection.Start"/> to inform the <see cref="Client"/> of the connection status changes.
    /// </summary>
    /// <param name="connection"><see cref="Connection"/> whose status changed</param>
    /// <param name="newStatus">the new status of the connection</param>
    /// <param name="error">error that resulted in the change in connection status</param>
    public delegate void ConnectStatusChangeEventHandler(Connection connection, ConnectStatus newStatus, Error error);

    /// <summary>
    /// Used to report the protocol version while connecting to a server.
    /// </summary>
    /// <param name="connection">the <see cref="Connection"/></param>
    /// <param name="protocolVersion">a <see cref="int"/> representing the version of the protocol</param>
    public delegate void ProtocolVersionEventHandler(Connection connection, int protocolVersion);

    /// <summary>
    /// Used to inform about the existence of each <see cref="Channel"/>, after connection has been established.
    /// </summary>
    /// <param name="channel">the <see cref="Channel"/></param>
    public delegate void NewChannelEventHandler(Channel channel);

    /// <summary>
    /// Used after a new <see cref="Channel"/> was created.
    /// </summary>
    /// <param name="channel">the <see cref="Channel"/></param>
    /// <param name="invoker"><see cref="Client"/> who created the <see cref="Channel"/></param>
    public delegate void NewChannelCreatedEventHandler(Channel channel, Client invoker);

    /// <summary>
    /// Used after <see cref="Channel"/> was deleted.
    /// </summary>
    /// <param name="channel">the <see cref="Channel"/></param>
    /// <param name="invoker"><see cref="Client"/> who deleted the <see cref="Channel"/></param>
    public delegate void ChannelDeletedEventHandler(Channel channel, Client invoker);

    /// <summary>
    /// Used when a <see cref="Channel"/> is being moved.
    /// </summary>
    /// <param name="channel">the <see cref="Channel"/></param>
    /// <param name="invoker"><see cref="Client"/> who moved the <see cref="Channel"/></param>
    public delegate void ChannelMoveEventHandler(Channel channel, Client invoker);

    /// <summary>
    /// Used when a <see cref="Channel"/> was changed
    /// </summary>
    /// <param name="channel">the <see cref="Channel"/></param>
    /// <param name="invoker"><see cref="Client"/> who changed the <see cref="Channel"/></param>
    public delegate void ChannelChangedEventHandler(Channel channel, Client invoker);

    /// <summary>
    /// Used when a <see cref="Client"/> was changed
    /// </summary>
    /// <param name="client">the <see cref="Client"/></param>
    /// <param name="invoker">the <see cref="Client"/> who did the changes</param>
    public delegate void UpdateClientEventHandler(Client client, Client invoker);

    /// <summary>
    /// Used when a <see cref="Client"/> is actively switching to a <see cref="Channel"/>.
    /// </summary>
    /// <param name="client">the <see cref="Client"/></param>
    /// <param name="oldChannel"><see cref="Channel"/> from the <see cref="Client"/> came from</param>
    /// <param name="newChannel"><see cref="Channel"/> where the <see cref="Client"/> is going to</param>
    /// <param name="visibility">visibility of the <see cref="Client"/></param>
    /// <param name="invoker">the <see cref="Client"/> who initiated the move</param>
    /// <param name="message">optional message</param>
    public delegate void ClientMoveEventHandler(Client client, Channel oldChannel, Channel newChannel, Visibility visibility, Client invoker, string message);

    /// <summary>
    /// Used when a <see cref="Client"/> becomes visible to the current connection.
    /// </summary>
    /// <param name="client">the <see cref="Client"/></param>
    /// <param name="oldChannel"><see cref="Channel"/> from where the <see cref="Client"/> came from</param>
    /// <param name="newChannel"><see cref="Channel"/> </param>
    /// <param name="visibility"></param>
    public delegate void ClientMoveSubscriptionEventHandler(Client client, Channel oldChannel, Channel newChannel, Visibility visibility);

    /// <summary>
    /// Used when a <see cref="Client"/>s is used because he timed out
    /// </summary>
    /// <param name="client">the <see cref="Client"/></param>
    /// <param name="oldChannel"><see cref="Channel"/> from the <see cref="Client"/> came from</param>
    /// <param name="newChannel"><see cref="Channel"/> where the <see cref="Client"/> is going to</param>
    /// <param name="visibility">visibility of the <see cref="Client"/></param>
    /// <param name="message">optional message</param>
    public delegate void ClientMoveTimeoutEventHandler(Client client, Channel oldChannel, Channel newChannel, Visibility visibility, string message);

    /// <summary>
    /// Used when a <see cref="Client"/> is kicked from a <see cref="Channel"/>
    /// </summary>
    /// <param name="client">the <see cref="Client"/></param>
    /// <param name="oldChannel"><see cref="Channel"/> from the <see cref="Client"/> came from</param>
    /// <param name="newChannel"><see cref="Channel"/> where the <see cref="Client"/> is going to</param>
    /// <param name="visibility">visibility of the <see cref="Client"/></param>
    /// <param name="invoker">the <see cref="Client"/> who initiated the kick</param>
    /// <param name="message">optional message</param>
    public delegate void ClientKickFromChannelEventHandler(Client client, Channel oldChannel, Channel newChannel, Visibility visibility, Client invoker, string message);

    /// <summary>
    /// Used When a <see cref="Client"/> is kicked from the server
    /// </summary>
    /// <param name="client">the <see cref="Client"/></param>
    /// <param name="oldChannel"><see cref="Channel"/> from the <see cref="Client"/> came from</param>
    /// <param name="newChannel"><see cref="Channel"/> where the <see cref="Client"/> is going to</param>
    /// <param name="visibility">visibility of the <see cref="Client"/></param>
    /// <param name="invoker">the <see cref="Client"/> who initiated the kick</param>
    /// <param name="message">optional message</param>
    public delegate void ClientKickFromServerEventHandler(Client client, Channel oldChannel, Channel newChannel, Visibility visibility, Client invoker, string message);

    /// <summary>
    /// Used when the virtual server was changed
    /// </summary>
    /// <param name="connection">the connection</param>
    /// <param name="editor">the <see cref="Client"/> who changed the server</param>
    public delegate void ServerUpdatedEventHandler(Connection connection, Client editor);

    /// <summary>
    /// Error codes sent by the server to the <see cref="Client"/>
    /// </summary>
    /// <param name="connection">the connection</param>
    /// <param name="error">the error code</param>
    /// <param name="returnCode">optional return code</param>
    /// <param name="extraMessage">additional message send by the server</param>
    public delegate void ServerErrorEventHandler(Connection connection, Error error, string returnCode, string extraMessage);

    /// <summary>
    /// Used when a server shutdown has been received
    /// </summary>
    /// <param name="connection">the connection</param>
    /// <param name="message">optional message</param>
    public delegate void ServerStopEventHandler(Connection connection, string message);

    /// <summary>
    /// Used when a server message was received
    /// </summary>
    /// <param name="client">the <see cref="Client"/> that send the message</param>
    /// <param name="message">the message</param>
    public delegate void ServerMessageEventHandler(Client client, string message);
    
    /// <summary>
    /// Used when a channel message was received
    /// </summary>
    /// <param name="client">the <see cref="Client"/> that send the message</param>
    /// <param name="message">the message</param>
    public delegate void ChannelMessageEventHandler(Client client, string message);
    
    /// <summary>
    /// Used when a private text message was received
    /// </summary>
    /// <param name="from">the <see cref="Client"/> that send the message</param>
    /// <param name="to">the <see cref="Client"/> that receives the message</param>
    /// <param name="message">the message</param>
    public delegate void ClientMessageEventHandler(Client from, Client to, string message);

    /// <summary>
    /// Used when a <see cref="Client"/> starts or stops talking
    /// </summary>
    /// <param name="client">the <see cref="Client"/> whose talk status changed</param>
    /// <param name="status">the new <see cref="TalkStatus"/></param>
    /// <param name="isReceivedWhisper">true if the event was caused by private whispering; otherwise false</param>
    public delegate void TalkStatusChangeEventHandler(Client client, TalkStatus status, bool isReceivedWhisper);

    /// <summary>
    /// Used when whisper is received from a <see cref="Client"/> that has not yet been added to the <see cref="Client"/> allow list.
    /// </summary>
    /// <param name="client">the <see cref="Client"/> that is whispering</param>
    public delegate void IgnoredWhisperEventHandler(Client client);

    /// <summary>
    /// Used when <see cref="Channel"/> has been subscribed
    /// </summary>
    /// <param name="channel">the newly subscribed <see cref="Channel"/></param>
    public delegate void ChannelSubscribeEventHandler(Channel channel);

    /// <summary>
    /// Used to marks the end of multiple calls to <see cref="Connection.ChannelSubscribed"/> 
    /// </summary>
    /// <param name="connection">the connection</param>
    public delegate void ChannelSubscribeFinishedEventHandler(Connection connection);

    /// <summary>
    /// Called when <see cref="Channel"/> has been unsubscribed
    /// </summary>
    /// <param name="channel">the newly unsubscribed <see cref="Channel"/></param>
    public delegate void ChannelUnsubscribeEventHandler(Channel channel);

    /// <summary>
    /// Used to mark the end of multiple calls to <see cref="Connection.ChannelUnsubscribed"/> 
    /// </summary>
    /// <param name="connection">ServerConnection connection</param>
    public delegate void ChannelUnsubscribeFinishedEventHandler(Connection connection);

    /// <summary>
    /// Used when the <see cref="Channel.Description"/> was edited
    /// </summary>
    /// <param name="channel">the <see cref="Channel"/></param>
    public delegate void ChannelDescriptionUpdateEventHandler(Channel channel);

    /// <summary>
    /// Used when a <see cref="Channel.Password"/> was modified.
    /// </summary>
    /// <param name="channel">the <see cref="Channel"/></param>
    public delegate void ChannelPasswordChangedEventHandler(Channel channel);

    /// <summary>
    /// Called after <see cref="Connection.InitiateGracefulPlaybackShutdown"/> finished for a device
    /// </summary>
    /// <param name="connection">the <see cref="Connection"/></param>
    public delegate void PlaybackShutdownCompleteEventHandler(Connection connection);

    /// <summary>
    /// Called when the list of <see cref="SoundDevice"/> returned by <see cref="Library.GetCaptureDevices(string)"/> and <see cref="Library.GetPlaybackDevices(string)"/> was changed
    /// </summary>
    /// <param name="mode">The soundbackend affected</param>
    /// <param name="playOrCap">true if the list of playback devices is changed; otherwise the list of capture devices is changed</param>
    public delegate void SoundDeviceListChangedEventHandler(string mode, bool playOrCap);

    /// <summary>
    /// Used when a incoming voice packet from a remote <see cref="Client"/> was decoded and is about to be played
    /// </summary>
    /// <param name="client">the <see cref="Client"/> whose sound data is about to be played</param>
    /// <param name="samples">a <see cref="Array"/> containing the sound date</param>
    /// <param name="channels">the number of channels in the sound data</param>
    public delegate void EditPlaybackVoiceDataEventHandler(Client client, short[] samples, int channels);

    /// <summary>
    /// Used when a incoming voice packet from a remote <see cref="Client"/> was decoded and 3D positioned.
    /// </summary>
    /// <param name="client">the <see cref="Client"/> whose sound data is about to be played</param>
    /// <param name="samples">a <see cref="Array"/> containing the sound date</param>
    /// <param name="channels">the number of channels in the sound data</param>
    /// <param name="channelSpeakers">a <see cref="Array"/> that maps sound channels to speakers</param>
    /// <param name="channelFillMask">a bit-mask of <see cref="Speakers"/> that defines which channels are filled. For efficiency reasons, not all channels need to have actual sound data in it. So before this data is used, use this bit-mask to check if the channel is actually filled. If you decide to add data to a channel that is empty, set the bit for this channel in this mask.</param>
    public delegate void EditPostProcessVoiceDataEventHandler(Client client, short[] samples, int channels, Speakers[] channelSpeakers, ref Speakers channelFillMask);

    /// <summary>
    /// Used when all sounds that are about to be played back for this <see cref="Connection"/> have been mixed. This is the last chance to alter/get sound.
    /// </summary>
    /// <param name="connection">the <see cref="Connection"/></param>
    /// <param name="samples">a <see cref="Array"/> containing the sound date</param>
    /// <param name="channels">the number of channels in the sound data</param>
    /// <param name="channelSpeakers">a <see cref="Array"/> that maps sound channels to speakers</param>
    /// <param name="channelFillMask">a bit-mask of <see cref="Speakers"/> that defines which channels are filled. For efficiency reasons, not all channels need to have actual sound data in it. So before this data is used, use this bit-mask to check if the channel is actually filled. If you decide to add data to a channel that is empty, set the bit for this channel in this mask.</param>
    public delegate void EditMixedPlaybackVoiceDataEventHandler(Connection connection, short[] samples, int channels, Speakers[] channelSpeakers, ref Speakers channelFillMask);

    /// <summary>
    /// Used after sound is recorded from the sound device and is preprocessed. 
    /// </summary>
    /// <param name="connection">the <see cref="Connection"/></param>
    /// <param name="samples">a <see cref="Array"/> containing the sound date</param>
    /// <param name="channels">the number of channels in the sound data</param>
    /// <param name="edited">true if the sound data was changed by the event-handler; otherwise false</param>
    /// <param name="cancel">true if the sound data should be discarded; otherwise false</param>
    public delegate void EditCapturedVoiceDataEventHandler(Connection connection, short[] samples, int channels, ref bool edited, ref bool cancel);

    /// <summary>
    /// Used to calculate volume attenuation for distance in 3D positioning of a <see cref="Client"/>.
    /// </summary>
    /// <param name="client">the <see cref="Client"/></param>
    /// <param name="distance">The distance between the listener and the client.</param>
    /// <param name="volume">The volume which the Client Lib calculated.</param>
    public delegate void Custom3dRolloffCalculationClientEventHandler(Client client, float distance, ref float volume);

    /// <summary>
    /// Used to calculate volume attenuation for distance in 3D positioning of a wave file.
    /// </summary>
    /// <param name="wave">the <see cref="WaveHandle"/> for the playing wave file, returned by <see cref="TeamSpeak.Sdk.Client.Connection.PlayWaveFile(string, bool)"/>.</param>
    /// <param name="distance">The distance between the listener and the client.</param>
    /// <param name="volume">The volume which the Client Lib calculated.</param>
    public delegate void Custom3dRolloffCalculationWaveEventHandler(WaveHandle wave, float distance, ref float volume);

    /// <summary>
    /// Used for user customizable logging and handling
    /// </summary>
    /// <param name="message">Actual log message text.</param>
    /// <param name="level">Severity of log message, defined by the <see cref="LogLevel"/>. Note that only log messages of a level higher or equal to than the one configured in <see cref="Library.LogLevel"/> will appear.</param>
    /// <param name="channel">Optional custom text to categorize the message channel.</param>
    /// <param name="connection">the <see cref="Connection"/></param>
    /// <param name="time">Date and time when the log message occurred.</param>
    /// <param name="completeString">A verbose log message including all other parameters for convenience.</param>
    public delegate void UserLoggingMessageEventHandler(string message, LogLevel level, string channel, Connection connection, string time, string completeString);

    /// <summary>
    /// Encrypts outgoing data
    /// </summary>
    /// <param name="dataToSend">An array with the outgoing data to be encrypted. Apply your custom encryption to the data array. If the encrypted data is smaller than sizeOfData, write your encrypted data into the existing memory of dataToSend. If your encrypted data is larger, you need to allocate memory and change dataToSend. You need to take care of freeing your own allocated memory yourself. The memory allocated by the SDK, to which dataToSend is originally pointing to, must not be freed.</param>
    /// <param name="sizeOfData">The size of the data array.</param>
    public delegate void CustomPacketEncryptHandler(ref IntPtr dataToSend, ref uint sizeOfData);

    /// <summary>
    /// Decrypts incoming data
    /// </summary>
    /// <param name="dataReceived">An array with the received data to be decrypted. Apply your custom decryption to the data array.If the decrypted data is smaller than dataReceivedSize, write your decrypted data into the existing memory of dataReceived. If your decrypted data is larger, you need to allocate memory and change dataReceived. You need to take care of freeing your own allocated memory yourself. The memory allocated by the SDK, to which dataReceived is originally pointing to, must not be freed.</param>
    /// <param name="dataReceivedSize">The size of the data array.</param>
    public delegate void CustomPacketDecryptHandler(ref IntPtr dataReceived, ref uint dataReceivedSize);

    /// <summary>
    /// Used to check if the unique identifier is the correct one
    /// </summary>
    /// <param name="connection">the <see cref="Connection"/></param>
    /// <param name="serverUniqueIdentifier">UniqueIdentifier of the server</param>
    /// <param name="cancelConnect">true if the connection process should be aborted; otherwise false</param>
    public delegate void CheckServerUniqueIdentifierEventHandler(Connection connection, string serverUniqueIdentifier, ref bool cancelConnect);

    /// <summary>
    /// Used to hash the password in the same way it is hashed in the outside data store.
    /// </summary>
    /// <param name="connection">the <see cref="Connection"/> the password is being send to</param>
    /// <param name="plaintext">The plaintext password.</param>
    /// <param name="maxEncryptedTextByteSize">that maximum size of encrypted plaintext password.</param>
    /// <returns>the custom encrypted password. The string encoded in utf8 most not be larger then maxEncryptedTextByteSize</returns>
    public delegate string ClientPasswordEncryptHandler(Connection connection, string plaintext, int maxEncryptedTextByteSize);

    /// <summary>
    /// Used when a file finished being transfered.
    /// </summary>
    /// <param name="transfer">the <see cref="FileTransfer"/> that represents the finished transfer</param>
    /// <param name="status">the result of the transfer</param>
    public delegate void FileTransferStatusEventHandler(FileTransfer transfer, Error status);

    /// <summary>
    /// Used to return the reply of the server for <see cref="Channel.GetFileInfo(string, string)"/>
    /// </summary>
    public delegate void FileInfoEventHandler(Channel channel, string name, ulong size, DateTimeOffset lastModified);

}
