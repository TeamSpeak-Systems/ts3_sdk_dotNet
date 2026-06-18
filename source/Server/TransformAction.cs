using System;

namespace TeamSpeak.Sdk.Server
{
    /// <summary>
    /// Action that is related to the <see cref="TransformFilePathEventArgs"/>.
    /// </summary>
    public enum TransformAction: int
    {
        /// <summary>
        /// When the virtual server is being is being created.
        /// </summary>
        /// <remarks>
        /// <see cref="TransformFilePathEventArgs.ChannelPath"/> can be changed to create a different folder for the virtual server.
        /// </remarks>
        InitServer = 0,
        /// <summary>
        /// When a channel is being is being created.
        /// </summary>
        /// <remarks>
        /// <see cref="TransformFilePathEventArgs.ChannelPath"/> can be changed to create a different folder for the channel.
        /// </remarks>
        InitChannel = 1,
        /// <summary>
        /// When a file is being uploaded.
        /// </summary>
        /// <remarks>
        /// <see cref="TransformFilePathEventArgs.ChannelPath"/> and/or <see cref="TransformFilePathEventArgs.TransformedFileName"/> can be changed.
        /// </remarks>
        Upload = 2,
        /// <summary>
        /// When a file is being downloaded.
        /// </summary>
        /// <remarks>
        /// <see cref="TransformFilePathEventArgs.ChannelPath"/> and/or <see cref="TransformFilePathEventArgs.TransformedFileName"/> can be changed.
        /// </remarks>
        Download = 3,
        /// <summary>
        /// When a file is being deleted.
        /// </summary>
        /// <remarks>
        /// <see cref="TransformFilePathEventArgs.ChannelPath"/> and/or <see cref="TransformFilePathEventArgs.TransformedFileName"/> can be changed.
        /// </remarks>
        Delete = 4,
        /// <summary>
        /// When a folder is being created.
        /// </summary>
        /// <remarks>
        /// <see cref="TransformFilePathEventArgs.ChannelPath"/> and/or <see cref="TransformFilePathEventArgs.TransformedFileName"/> can be changed.
        /// </remarks>
        CreateDirectory = 5,
        /// <summary>
        /// When a file/folder is being renamed.
        /// </summary>
        /// <remarks>
        /// Will be called 2 times, first for old, then for new name. Here <see cref="TransformFilePathEventArgs.ChannelPath"/> and/or <see cref="TransformFilePathEventArgs.TransformedFileName"/> can be changed.
        /// </remarks>
        Rename = 6,
        /// <summary>
        /// Called when a listing of a folder is requested.
        /// </summary>
        /// <remarks>
        /// <see cref="TransformFilePathEventArgs.ChannelPath"/> <see cref="TransformFilePathEventArgs.TransformedFileName"/>
        /// </remarks>
        /// Here result->channelPath and/or result->transformedFileName can be changed.
        FileList = 7,
        /// <summary>
        /// Called when file info is requested.
        /// </summary>
        /// <remarks>
        /// Here <see cref="TransformFilePathEventArgs.ChannelPath"/> and/or <see cref="TransformFilePathEventArgs.TransformedFileName"/> can be changed.
        /// </remarks>
        FileInfo = 8,
    }
}