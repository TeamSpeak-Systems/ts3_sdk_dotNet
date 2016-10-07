using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using TeamSpeak.Sdk;
using TeamSpeak.Sdk.Client;

namespace TeamSpeak.Sdk.Client.Example
{
    [Activity(Label = "TeamSpeak SDK", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private ListView Log;
        private LogAdapter LogAdapter;
        private Button Button;
        private Connection Connection;
        private EditText EditHost;
        private string Identity;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Initialize();
            LoadPreferences();
        }

        private void Initialize()
        {

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Setup TeamSpeak Sdk
            if (Library.IsInitialized == false)
            {
                Library.UserLogMessage += Library_UserLogMessage;
                Library.Initialize(LogTypes.Userlogging);
            }

            // Setup a new Connection, but only when necessary
            if (Connection == null)
            {
                Connection = Library.SpawnNewConnection();
                Connection.StatusChanged += Connection_StatusChanged;
                Connection.NewChannel += Connection_NewChannel;
                Connection.NewChannelCreated += Connection_NewChannelCreated;
                Connection.ChannelDeleted += Connection_ChannelDeleted;
                Connection.ClientMoved += Connection_ClientMoved;
                Connection.ClientMovedSubscription += Connection_ClientMovedSubscription;
                Connection.ClientTimeout += Connection_ClientTimedOut;
                Connection.TalkStatusChanged += Connection_TalkStatusChanged;
                Connection.ServerError += Connection_ServerError;
            }

            Button = FindViewById<Button>(Resource.Id.Button);
            Button.Click += Button_Click;
            Log = FindViewById<ListView>(Resource.Id.Log);
            LogAdapter = new LogAdapter(this);
            Log.Adapter = LogAdapter;
            if (Log.Adapter.Count > 0)
                Log.SetSelection(Log.Adapter.Count - 1);
            EditHost = FindViewById<EditText>(Resource.Id.EditHost);
        }

        private void LoadPreferences()
        {
            ISharedPreferences preferences = GetSharedPreferences("TeamSpeak.Sdk.Example", FileCreationMode.Private);
            Identity = preferences.GetString("Identity", null);
            if (string.IsNullOrEmpty(Identity))
            {
                Identity = Library.CreateIdentity();
                ISharedPreferencesEditor editor = preferences.Edit();
                editor.PutString("Identity", Identity);
                editor.Apply();
            }
        }

        private void AppendToLog(string line)
        {
            RunOnUiThread(delegate
            {
                LogAdapter.Append(line);
            });
        }

        private async void Button_Click(object sender, EventArgs e)
        {
            string host = EditHost.Text;
            if (Connection.Status == ConnectStatus.Disconnected)
            {
                try
                {
                    await Connection.Start(Identity, host, 9987, "User", serverPassword: "secret");
                }
                catch (TeamSpeakException exception)
                {
                    AppendToLog($"Connection failed: {exception.Message}");
                }
            }
            else
            {
                try
                {
                    await Connection.Stop();
                }
                catch (TeamSpeakException exception)
                {
                    AppendToLog($"Disconnect failed: {exception.Message}");
                }
            }
        }

        private void Library_UserLogMessage(string message, LogLevel level, string channel, Connection connection, string time, string completeString)
        {
            AppendToLog(completeString);
        }

        /// <summary>
        /// Callback for connection status change.
        /// Connection status switches through the states Disconnected, Connecting, Connected and ConnectionEstablished.
        /// </summary>
        /// <param name="connection">Server connection handler</param>
        /// <param name="newStatus">New connection status, see <see cref="ConnectStatus"/>.</param>
        /// <param name="error">Error code. Should be <see cref="Error.Ok"/> when connecting or actively disconnection. Contains error state when losing connection.</param>
        private void Connection_StatusChanged(Connection connection, ConnectStatus newStatus, Error error)
        {
            RunOnUiThread(delegate
            {
                Button.Text = GetString(newStatus == ConnectStatus.Disconnected ? Resource.String.Login : Resource.String.Logout);
                AppendToLog($"Connect status changed: {connection.ID} {newStatus} {error}");
                /* Failed to connect ? */
                if (newStatus == ConnectStatus.Disconnected && error == Error.FailedConnectionInitialisation)
                    AppendToLog("Looks like there is no server running.");
            });
        }

        /// <summary>
        /// Callback for current channels being announced to the client after connecting to a server.
        /// </summary>
        /// <param name="channel">the announced channel.</param>
        private void Connection_NewChannel(Channel channel)
        {
            AppendToLog($"onNewChannelEvent: {channel.Connection.ID} {channel.Name} {channel.Parent?.Name}");
        }

        /// <summary>
        /// Callback for just created channels.
        /// </summary>
        /// <param name="channel">the announced channel.</param>
        /// <param name="invoker">the client who created the channel.</param>
        private void Connection_NewChannelCreated(Channel channel, Client invoker)
        {
            AppendToLog($"New channel created: {channel.Name}");
        }

        /// <summary>
        /// Callback when a channel was deleted.
        /// </summary>
        /// <param name="channel">the deleted channel.</param>
        /// <param name="invoker">the client who deleted the channel.</param>
        private void Connection_ChannelDeleted(Channel channel, Client invoker)
        {
            AppendToLog($"Channel {channel.Name}({channel.ID}) deleted by {invoker.Nickname}({invoker.ID})");
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
        private void Connection_ClientMoved(Client client, Channel oldChannel, Channel newChannel, Visibility visibility, Client invoker, string message)
        {
            AppendToLog($"Client {client.Nickname} moves from channel {oldChannel?.Name ?? "nowhere"} to {newChannel?.Name ?? "nowhere"} with message {message}");
        }

        /// <summary>
        /// Callback for other clients in current and subscribed channels being announced to the client.
        /// </summary>
        /// <param name="client">the announced client</param>
        /// <param name="oldChannel">the subscribed channel where the client left visibility</param>
        /// <param name="newChannel">the subscribed channel where the client entered visibility</param>
        /// <param name="visibility">Visibility of the announced client.</param>
        private void Connection_ClientMovedSubscription(Client client, Channel oldChannel, Channel newChannel, Visibility visibility)
        {
            AppendToLog($"New client: {client.Nickname}");
        }

        /// <summary>
        /// Called when a client drops his connection.
        /// </summary>
        /// <param name="client">the moved client</param>
        /// <param name="oldChannel">the channel the leaving client was previously member of</param>
        /// <param name="newChannel">null, as client is leaving</param>
        /// <param name="visibility">Always <see cref="Visibility.Leave"/></param>
        /// <param name="message">Optional message giving the reason for the timeout</param>
        private void Connection_ClientTimedOut(Client client, Channel oldChannel, Channel newChannel, Visibility visibility, string message)
        {
            AppendToLog($"Client {client.Nickname} timeouts with message {message}");
        }

        /// <summary>
        /// This event is called when a client starts or stops talking.
        /// </summary>
        /// <param name="client">the client who announced the talk status change</param>
        /// <param name="status">the state of the voice transmission</param>
        /// <param name="isReceivedWhisper">true if this event was caused by whispering, false if caused by normal talking</param>
        private void Connection_TalkStatusChanged(Client client, TalkStatus status, bool isReceivedWhisper)
        {
            string verb = client.IsTalking ? "starts" : "stops";
            AppendToLog($"Client {client.Nickname} {verb} talking.");
        }
        private void Connection_ServerError(Connection connection, Error error, string returnCode, string extraMessage)
        {
            AppendToLog($"Error: {Library.GetErrorMessage(error)} {extraMessage}");
        }
    }
}

