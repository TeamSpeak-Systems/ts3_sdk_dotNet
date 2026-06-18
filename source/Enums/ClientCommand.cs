using System;

namespace TeamSpeak.Sdk
{
    /// <summary>
    /// Commands used by clients.
    /// </summary>
    public enum ClientCommand: int
    {
        /// <summary>
        /// Disables requesting connection information for other clients.
        /// </summary>
        RequestConnectionInfo = 0,
        /// <summary>
        /// Disables any channel switching requested by the client.
        /// </summary>
        /// <remarks>
        /// Server can still move clients.
        /// </remarks>
        RequestClientMove = 1,
        /// <summary>
        /// Disable muting/unmuting clients.
        /// </summary>
        RequestXXMuteClients = 2,
        /// <summary>
        /// Disable kick requests from clients.
        /// </summary>
        /// <remarks>
        /// Server can still kick clients.
        /// </remarks>
        RequestClientKickFromXXX = 3,
        /// <summary>
        /// Disable creating new channels.
        /// </summary>
        FlushChannelCreation = 4,
        /// <summary>
        /// Disables editing/changing channel.
        /// </summary>
        FlushChannelUpdates = 5,
        /// <summary>
        /// Disable moving channels around.
        /// </summary>
        RequestChannelMove = 6,
        /// <summary>
        /// Disable deleting channels.
        /// </summary>
        RequestChannelDelete = 7,
        /// <summary>
        /// Disable getting channel description.
        /// </summary>
        RequestChannelDescription = 8,
        /// <summary>
        /// disable subscriptions.
        /// </summary>
        RequestChannelXXSubscribeXXX =  9,
        /// <summary>
        /// disable server connection information.
        /// </summary>
        RequestServerConnectionInfo  = 10,
        /// <summary>
        /// Disable sending text messages.
        /// </summary>
        RequestSendXXXTextMsg = 11,
        /// <summary>
        /// Disable client filetransfer commands.
        /// </summary>
        Filetransfers = 12,
    }
}