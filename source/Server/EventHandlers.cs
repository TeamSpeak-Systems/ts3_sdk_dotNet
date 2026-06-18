using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TeamSpeak.Sdk.Server
{
    public class ExportedVariable<TProperty>
    {
        public TProperty Property { get; }
        public string Current { get; }
        public string Proposed { get; }

        public ExportedVariable(TProperty property, string current, string proposed)
        {
            Property = property;
            Current = current;
            Proposed = proposed;
        }
    }

    public class FileTransferCallbackData
    {
        public Client Client { get; }
        public ushort TransferID { get; }
        public ushort RemoteTransferID { get; }
        public FileTransferState State { get; }
        public string StatusMessage { get; }
        public ulong RemoteFileSize { get; }
        public ulong Bytes { get; }
        public bool IsSender { get; }

        public FileTransferCallbackData(Client client, ushort transferID, ushort remoteTransferID, FileTransferState state, string statusMessage, ulong remoteFileSize, ulong bytes, bool isSender)
        {
            Client = client;
            TransferID = transferID;
            RemoteTransferID = remoteTransferID;
            State = state;
            StatusMessage = statusMessage;
            RemoteFileSize = remoteFileSize;
            Bytes = bytes;
            IsSender = isSender;
        }
    }

    public class TransformFilePathEventArgs: EventArgs
    {
        /// <summary>
        /// The <see cref="Client"/> that invoked the <see cref="Action"/>
        /// </summary>
        /// <remarks>Will be <see langword="null"/> when <see cref="Action"/> is <see cref="TransformAction.InitServer"/> or <see cref="TransformAction.InitChannel"/>.</remarks>
        public Client Invoker { get; }
        /// <summary>
        /// Channel where the transform is to be done
        /// </summary>
        /// <remarks>Will be <see langword="null"/> when <see cref="Action"/> is <see cref="TransformAction.InitServer"/>.</remarks>
        public Channel Channel { get; }
        /// <summary>
        /// Original file name
        /// </summary>
        public string OriginalFileName { get; }
        /// <summary>
        /// The action the event was called for.
        /// </summary>
        public TransformAction Action { get; }
        /// <summary>
        /// Maximum size of <see cref="FileName"/>
        /// </summary>
        public int FileNameMaxSize { get; }
        /// <summary>
        /// Maximum size of <see cref="ChannelPath"/> 
        /// </summary>
        public int ChannelPathMaxSize { get; }
        /// <summary>
        /// Name of the file 
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Where the channel path is located.
        /// </summary>
        public string ChannelPath { get; set; }
        /// <summary>
        /// When <see langword="false"/>: no entry into the logfile is made;
        /// Otherwise <see langword="true"/>: the action is being logged. <see cref="LogTypes.File"/> must have been set in <see cref="LibraryParameters.UsedLogTypes"/> for logging to happen.
        /// </summary>
        public bool LogFileAction { get; set; }

        /// <summary>
        /// When set to not <see cref="Error.Ok"/> the transform fails, and the <see cref="Action"/> fails.
        /// </summary>
        public Error Result { get; set; }

        public TransformFilePathEventArgs(Client invoker, Channel channel, TransformAction action, string fileName, int fileNameMaxSize, string channelPath, int channelPathMaxSize)
        {
            Invoker = invoker;
            Channel = channel;
            Action = action;
            OriginalFileName = fileName;
            FileName = FileName;
            FileNameMaxSize = fileNameMaxSize;
            ChannelPath = channelPath;
            ChannelPathMaxSize = channelPathMaxSize;
            Result = Error.Ok;
        }
    };

#pragma warning disable 1591
    public delegate void VoiceDataEventHandler(Client client, byte[] voiceData, uint frequency);
    public delegate void ClientStartTalkingEventHandler(Client client);
    public delegate void ClientStopTalkingEventHandler(Client client);
    public delegate void ClientConnectedEventHandler(Client client, Channel channel, ref bool removeClientError);
    public delegate void ClientDisconnectedEventHandler(Client client, Channel channel);
    public delegate void ClientMovedEventHandler(Client client, Channel oldChannel, Channel newChannel);
    public delegate void ChannelCreatedEventHandler(Client invoker, Channel channel);
    public delegate void ChannelEditedEventHandler(Client invoker, Channel channel);
    public delegate void ChannelDeletedEventHandler(Client invoker, Channel channel);
    public delegate void ServerTextMessageEventHandler(Client invoker, string textMessage);
    public delegate void ChannelTextMessageEventHandler(Client invoker, Channel target, string textMessage);
    public delegate void UserLoggingMessageEventHandler(string message, LogLevel level, string channel, VirtualServer virtualServer, string time, string completeString);
    public delegate void AccountingErrorEventHandler(Error errorCode);
    public delegate void CustomPacketEncryptHandler(ref IntPtr dataToSend, ref uint sizeOfData);
    public delegate void CustomPacketDecryptHandler(ref IntPtr dataReceived, ref uint dataReceivedSize);
    public delegate void FileTransferEventEventHandler(FileTransferCallbackData data);
    public delegate void ClientPasswordEncryptHandler(string plaintext, byte[] encryptedText);
    public delegate void TransformFilePathEventHandler(TransformFilePathEventArgs args);
#pragma warning restore
}
