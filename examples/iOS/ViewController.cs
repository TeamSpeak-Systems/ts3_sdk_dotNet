using System;
using System.Collections.Generic;
using Foundation;
using TeamSpeak.Sdk;
using TeamSpeak.Sdk.Client;
using UIKit;

namespace iOS2
{
	public partial class ViewController : UIViewController
	{
		private List<string> LogItems;
		private Connection Connection;
		private string Identity;

		protected ViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			LogItems = new List<string>();
			LogView.Source = new LogSource(LogItems);
			LoadPreferences();

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
		}

		private void LoadPreferences()
		{
			var plist = NSUserDefaults.StandardUserDefaults;
			Identity = plist.StringForKey("Identity");
			if (string.IsNullOrEmpty(Identity))
			{
				Identity = Library.CreateIdentity();
				plist.SetString(Identity, "Identity");
				plist.Synchronize();
			}
		}

		partial void Button_TouchUpInside(UIButton sender)
		{
			string host = EditHost.Text;
			if (Connection.Status == ConnectStatus.Disconnected)
			{
				Connection.Start(Identity, host, 9987, "User", serverPassword: "secret");
			}
			else
			{
				Connection.Stop();
			}
		}

		private void AppendToLog(string message)
		{
			InvokeOnMainThread(() =>
			{
				LogItems.Add(message);
				LogView.ReloadData();
				NSIndexPath indexPath = NSIndexPath.Create(0, LogItems.Count - 1);
				LogView.ScrollToRow(indexPath, UITableViewScrollPosition.Bottom, true);
			});
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
			InvokeOnMainThread(() =>
			{
				Button.SetTitle(newStatus == ConnectStatus.Disconnected ? "Login" : "Logout", UIControlState.Normal);
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
