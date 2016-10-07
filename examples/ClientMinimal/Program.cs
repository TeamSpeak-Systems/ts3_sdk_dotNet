/*
 * TeamSpeak 3 client minimal sample
 *
 * Copyright (c) 2016 TeamSpeak-Systems
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamSpeak.Sdk;
using TeamSpeak.Sdk.Client;

namespace TeamSpeak.Sdk.Client.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a new server connection handler using the default
            using (Connection connection = new Connection())
            {
                // Events, every callback from the library can be subscribed/unsubscribed at any time
                // the only exception are encryption related events, they must be declared before using any
                // functionality of the library.
                connection.StatusChanged += Connection_StatusChanged;
                connection.NewChannel += Connection_NewChannel;
                connection.NewChannelCreated += Connection_NewChannelCreated;
                connection.ChannelDeleted += Connection_ChannelDeleted;
                connection.ClientMoved += Connection_ClientMoved;
                connection.ClientMovedSubscription += Connection_ClientMovedSubscription;
                connection.ClientTimeout += Connection_ClientTimedOut;
                connection.TalkStatusChanged += Connection_TalkStatusChanged;
                connection.ServerError += Connection_ServerError;

                // Create a new client identity
                string identity = Library.CreateIdentity();

                // Connect to server on localhost:9987 with nickname "client", no default channel, no default channel password and server password "secret"
                Task task = connection.Start(identity, "localhost", 9987, "client", serverPassword: "secret");

                Console.WriteLine("Client lib initialized and running");

                // Print client lib version
                Console.WriteLine($"Client lib version: {Library.Version}");

                // Wait for connection to finish connecting
                task.Wait();

                // Wait for user input
                Console.WriteLine();
                Console.WriteLine("--- Press Return to disconnect from server and exit ---");
                Console.ReadLine();

                // Disconnect from server
                connection.Stop().Wait();
            }
        }

        /// <summary>
        /// Callback for connection status change.
        /// Connection status switches through the states Disconnected, Connecting, Connected and ConnectionEstablished.
        /// </summary>
        /// <param name="connection">Server connection handler</param>
        /// <param name="newStatus">New connection status, see <see cref="ConnectStatus"/>.</param>
        /// <param name="error">Error code. Should be <see cref="Error.Ok"/> when connecting or actively disconnection. Contains error state when losing connection.</param>
        private static void Connection_StatusChanged(Connection connection, ConnectStatus newStatus, Error error)
        {
            Console.WriteLine($"Connect status changed: {connection.ID} {newStatus} {error}");
            /* Failed to connect ? */
            if (newStatus == ConnectStatus.Disconnected && error == Error.FailedConnectionInitialisation)
                Console.WriteLine("Looks like there is no server running.");
        }

        /// <summary>
        /// Callback for current channels being announced to the client after connecting to a server.
        /// </summary>
        /// <param name="channel">the announced channel.</param>
        private static void Connection_NewChannel(Channel channel)
        {
            Console.WriteLine($"onNewChannelEvent: {channel.Connection.ID} {channel.Name} {channel.Parent?.Name}");
        }

        /// <summary>
        /// Callback for just created channels.
        /// </summary>
        /// <param name="channel">the announced channel.</param>
        /// <param name="invoker">the client who created the channel.</param>
        private static void Connection_NewChannelCreated(Channel channel, Client invoker)
        {
            Console.WriteLine($"New channel created: {channel.Name}");
        }

        /// <summary>
        /// Callback when a channel was deleted.
        /// </summary>
        /// <param name="channel">the deleted channel.</param>
        /// <param name="invoker">the client who deleted the channel.</param>
        private static void Connection_ChannelDeleted(Channel channel, Client invoker)
        {
            Console.WriteLine($"Channel {channel.Name}({channel.ID}) deleted by {invoker.Nickname}({invoker.ID})");
        }

        /// <summary>
        /// Called when a client joins, leaves or moves to another channel.
        /// </summary>
        /// <param name="client">the moved client.</param>
        /// <param name="oldChannel">the old channel left by the client.</param>
        /// <param name="newChannel">the new channel joined by the client.</param>
        /// <param name="visibility">Visibility of the moved client.</param>
        /// <param name="invoker">the client who moved the user</param>
        /// <param name="message">optional message giving the reason for the move</param>
        private static void Connection_ClientMoved(Client client, Channel oldChannel, Channel newChannel, Visibility visibility, Client invoker, string message)
        {
            Console.WriteLine($"Client {client.Nickname} moves from channel {oldChannel?.Name ?? "nowhere"} to {newChannel?.Name ?? "nowhere"} with message {message}");
        }

        /// <summary>
        /// Callback for other clients in current and subscribed channels being announced to the client.
        /// </summary>
        /// <param name="client">the announced client</param>
        /// <param name="oldChannel">the subscribed channel where the client left visibility</param>
        /// <param name="newChannel">the subscribed channel where the client entered visibility</param>
        /// <param name="visibility">Visibility of the announced client.</param>
        private static void Connection_ClientMovedSubscription(Client client, Channel oldChannel, Channel newChannel, Visibility visibility)
        {
            Console.WriteLine($"New client: {client.Nickname}");
        }

        /// <summary>
        /// Called when a client drops his connection.
        /// </summary>
        /// <param name="client">the moved client</param>
        /// <param name="oldChannel">the channel the leaving client was previously member of</param>
        /// <param name="newChannel">null, as client is leaving</param>
        /// <param name="visibility">Always <see cref="Visibility.Leave"/></param>
        /// <param name="message">Optional message giving the reason for the timeout</param>
        private static void Connection_ClientTimedOut(Client client, Channel oldChannel, Channel newChannel, Visibility visibility, string message)
        {
            Console.WriteLine($"Client {client.Nickname} timeouts with message {message}");
        }

        /// <summary>
        /// This event is called when a client starts or stops talking.
        /// </summary>
        /// <param name="client">the client who announced the talk status change</param>
        /// <param name="status">the state of the voice transmission</param>
        /// <param name="isReceivedWhisper">true if this event was caused by whispering, false if caused by normal talking</param>
        private static void Connection_TalkStatusChanged(Client client, TalkStatus status, bool isReceivedWhisper)
        {
            string verb = client.IsTalking ? "starts" : "stops";
            Console.WriteLine($"Client {client.Nickname} {verb} talking.");
        }
        private static void Connection_ServerError(Connection connection, Error error, string returnCode, string extraMessage)
        {
            Console.WriteLine($"Error: {Library.GetErrorMessage(error)} {extraMessage}");
        }
    }
}
