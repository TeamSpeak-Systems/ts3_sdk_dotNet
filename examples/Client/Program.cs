using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TeamSpeak.Sdk;
using TeamSpeak.Sdk.Client;

namespace TeamSpeak.Sdk.Client.Example
{
    static class Program
    {
        private class MenuItem
        {
            public char Key { get; }
            public string Title { get; }
            public Action Method { get; }

            public MenuItem(char key, string title, Action method)
            {
                Key = key;
                Title = title;
                Method = method;
            }
        }

        static byte[] WaveHeader = new byte[]
        {
            0x52, 0x49, 0x46, 0x46, 0x00, 0x00, 0x00, 0x00, 0x57, 0x41, 0x56, 0x45, 0x66, 0x6D, 0x74, 0x20,
            0x10, 0x00, 0x00, 0x00, 0x01, 0x00, 0x02, 0x00, 0x80, 0xBB, 0x00, 0x00, 0x00, 0xB8, 0x0B, 0x00,
            0x10, 0x00, 0x10, 0x00, 0x64, 0x61, 0x74, 0x61, 0x00, 0x00, 0x00, 0x00
        };

        static Connection Connection;
        static bool Abort = false;
        const string IdentityFile = "identity.txt";
        static FileStream WaveFile;
        static MenuItem[] Menu = new MenuItem[]
        {
             new MenuItem('q', "Disconnect from server", Disconnect),
             new MenuItem('h', "Show this help", ShowHelp),
             new MenuItem('c', "Show channels", ShowChannels),
             new MenuItem('s', "Switch to specified channel", SwitchChannel),
             new MenuItem('l', "Show all visible clients", ShowClients),
             new MenuItem('L', "Show all clients in specific channel", ShowClientsInChannel),
             new MenuItem('n', "Create new channel", CreateChannel),
             new MenuItem('N', "Create new channel with password", CreateChannelPassword),
             new MenuItem('d', "Delete channel", DeleteChannel),
             new MenuItem('r', "Rename channel", RenameChannel),
             new MenuItem('R', "Record sound to wav", RecordWav),
             new MenuItem('v', "Toggle Voice Activity Detection / Continuous transmission", ToggleVoiceDetection),
             new MenuItem('V', "Set Voice Activity Detection level", ConfigureVoiceDetection),
             new MenuItem('w', "Set whisper list", SetWhisperList),
             new MenuItem('W', "Clear whisper list", ClearWhisperList),
             new MenuItem('m', "Configure microphone", ConfigureMicrophone),
        };

        static void Main(string[] args)
        {
            Initialize();

            Connection.OpenPlayback();
            Console.WriteLine($"Playback: {Connection.PlaybackDevice}");
            Connection.OpenCapture();
            Console.WriteLine($"Capture: {Connection.CaptureDevice}");

            string identity = ReadIdentity() ?? Library.CreateIdentity();
            Task starting = Connection.Start(identity, "localhost", 9987, "client", serverPassword: "secret");

            Console.WriteLine("Client lib initialized and running");
            Console.WriteLine($"Client lib version: {Library.Version}({Library.VersionNumber})");

            try
            {
                starting.Wait();
            }
            catch (AggregateException e)
            {
                if (e.InnerException is TeamSpeakException)
                {
                    Error errorCode = ((TeamSpeakException)e.InnerException).ErrorCode;
                    Console.WriteLine("Failed to connect to server: {0}", errorCode);
                    return;
                }
                else
                {
                    throw;
                }
            }
            Console.WriteLine();
            Console.WriteLine("TeamSpeak 3 client commandline interface");
            ShowHelp();

            do
            {
                string line = Console.ReadLine();
                if (line.Length != 1) continue;
                char c = line[0];
                MenuItem item = Menu.FirstOrDefault(i => i.Key == c);
                if (item != null)
                {
                    try
                    {
                        item.Method();
                    }
                    catch (TeamSpeakException e)
                    {
                        Console.WriteLine($"Error in {item.Method.Method.Name}: {e.Message} ({e.ErrorCode})");
                    }
                }
            }
            while (Abort == false);

            Connection.IsVoiceRecording = false;
            Speakers channelFillMask = (Speakers)0;
            Connection_EditMixedPlaybackVoiceData(Connection, null, 0, null, ref channelFillMask);
            Connection.Stop();
        }

        private static void Initialize()
        {
            LibraryParameters parameters = new LibraryParameters("../../../../ts3_sdk_3.0.4/bin/");
            parameters.UsedLogTypes = LogTypes.File | LogTypes.Console | LogTypes.Userlogging;
            Library.Initialize(parameters);

            Connection = Library.SpawnNewConnection();
            Connection.StatusChanged += Connection_StatusChanged;
            Connection.NewChannel += Connection_NewChannel;
            Connection.NewChannelCreated += Connection_NewChannelCreated;
            Connection.ChannelDeleted += Connection_ChannelDeleted;
            Connection.ClientMoved += Connection_ClientMoved;
            Connection.ClientMovedSubscription += Connection_SubscriptionClientMoved;
            Connection.ClientTimeout += Connection_ClientTimeout;
            Connection.TalkStatusChanged += Connection_TalkStatusChanged;
            Connection.WhisperIgnored += Connection_WhisperIgnored;
            Connection.ServerError += Connection_ServerError;
            Connection.EditMixedPlaybackVoiceData += Connection_EditMixedPlaybackVoiceData;
        }

        private static void Connection_SubscriptionClientMoved(Client client, Channel oldChannel, Channel newChannel, Visibility visibility)
        {
            Console.WriteLine($"New client: {client.Nickname}");
        }

        private static void Connection_ClientTimeout(Client client, Channel oldChannel, Channel newChannel, Visibility visibility, string message)
        {
            Console.WriteLine($"{client.Nickname} timeouts with message {message}");
        }

        private static void Connection_ServerError(Connection connection, Error error, string returnCode, string extraMessage)
        {
            Console.WriteLine($"Error for server {connection.ID}: {Library.GetErrorMessage(error)} {extraMessage}");
        }

        private static void Connection_TalkStatusChanged(Client client, TalkStatus status, bool isReceivedWhisper)
        {
            string verb = status != TalkStatus.NotTalking ? "starts" : "stops";
            Console.WriteLine($"Client {client.Nickname} {verb} talking.");
        }

        private static void Connection_ClientMoved(Client client, Channel oldChannel, Channel newChannel, Visibility visibility, Client invoker, string message)
        {
            Console.WriteLine($"{client.Nickname} moves from channel {oldChannel?.Name ?? "nowhere"} to {newChannel?.Name ?? "nowhere"} with message {message}");
        }

        private static void Connection_ChannelDeleted(Channel channel, Client invoker)
        {
            Console.WriteLine($"Channel {channel.Name} deleted by {invoker?.Nickname ?? "<server>"}");
        }

        private static void Connection_NewChannelCreated(Channel channel, Client invoker)
        {
            Console.WriteLine($"New Channel {channel.Name} created by {invoker?.Nickname ?? "<server>"}");
        }

        private static void Connection_NewChannel(Channel channel)
        {
            Console.WriteLine($"New Channel: {channel.Name}{( channel.Parent != null ? $", in {channel.Parent.Name}" : "" )}");
        }

        private static void Connection_WhisperIgnored(Client client)
        {
            Connection.AllowWhispersFrom(client);
            Console.WriteLine($"Added client {client.Nickname} to whisper allow list");
        }

        private static void Connection_StatusChanged(Connection connection, ConnectStatus newStatus, Error error)
        {
            Console.WriteLine($"Connect status changed: {connection.ID} {newStatus} {error}");
            if (newStatus == ConnectStatus.Disconnected && error == Error.FailedConnectionInitialisation)
                Console.WriteLine("Looks like there is no server running.");
        }

        private static void Connection_EditMixedPlaybackVoiceData(Connection connection, short[] samples, int channels, Speakers[] channelSpeakers, ref Speakers channelFillMask)
        {
            if (Connection.IsVoiceRecording && WaveFile == null)
            {
                try
                {
                    WaveFile = File.Create("recordedvoices.wav");
                    WaveFile.Seek(WaveHeader.Length, SeekOrigin.Begin);
                }
                catch
                {
                    WaveFile = null;
                }
            }
            else if (Connection.IsVoiceRecording == false && WaveFile != null)
            {
                Array.Copy(BitConverter.GetBytes((int)WaveFile.Length - 8), 0, WaveHeader, 4, 4);
                Array.Copy(BitConverter.GetBytes((int)WaveFile.Length - WaveHeader.Length), 0, WaveHeader, 40, 4);
                WaveFile.Seek(0, SeekOrigin.Begin);
                WaveFile.Write(WaveHeader, 0, WaveHeader.Length);
                WaveFile.Close();
                WaveFile = null;
            }

            /*save samples, when available and requested*/
            if (WaveFile != null && samples.Length > 0 && channels > 0)
            {
                short[] outputSamples = MixSpeackers(samples, channels, channelSpeakers, channelFillMask);
                byte[] buffer = new byte[outputSamples.Length * sizeof(short)];
                Buffer.BlockCopy(outputSamples, 0, buffer, 0, buffer.Length);
                WaveFile.Write(buffer, 0, buffer.Length);
            }
        }

        private static short[] MixSpeackers(short[] samples, int channels, Speakers[] channelSpeakers, Speakers channelFillMask)
        {
            /*get the outbut buffer*/
            int samplesCount = samples.Length / channels;
            short[] result = new short[samplesCount * 2];

            if (channelFillMask == 0) return result;

            int[] currentSampleMix = new int[2]; /*a per channel/sample mix buffer*/
            int[] channelCount = new int[2] { 0, 0 }; /*how many input channels does the output channel contain */

            const Speakers leftChannelMask = Speakers.FrontLeft | Speakers.FrontCenter | Speakers.BackLeft | Speakers.FrontLeftOfCenter | Speakers.BackCenter | Speakers.SideLeft | Speakers.TopCenter | Speakers.TopFrontLeft | Speakers.TopFrontCenter | Speakers.TopBackLeft | Speakers.TopBackCenter;
            const Speakers rightChannelMask = Speakers.FrontRight | Speakers.FrontCenter | Speakers.BackRight | Speakers.FrontRightOfCenter | Speakers.BackCenter | Speakers.SideRight | Speakers.TopCenter | Speakers.TopFrontRight | Speakers.TopFrontCenter | Speakers.TopBackRight | Speakers.TopBackCenter;

            /*loop over all possible speakers*/
            for (int currentInChannel = 0; currentInChannel < channels; currentInChannel++)
            {
                Speakers speaker = channelSpeakers[currentInChannel];
                /*if the speaker has actual data*/
                if ((speaker & channelFillMask) != 0)
                {
                    /*add to the outChannelSpeakerSet*/
                    if ((speaker & leftChannelMask) != 0) channelCount[0] += 1;
                    if ((speaker & rightChannelMask) != 0) channelCount[1] += 1;
                }
            }

            /* hint: if channelCount is 0 for all channels, we could write a zero buffer and quit here */

            /*mix the samples*/
            for (int currentSample = 0; currentSample < samplesCount; currentSample++)
            {
                currentSampleMix[0] = currentSampleMix[1] = 0;

                /*loop over all channels in this frame */
                for (int currentInChannel = 0; currentInChannel < channels; currentInChannel++)
                {
                    Speakers speaker = channelSpeakers[currentInChannel];
                    if ((speaker & leftChannelMask) != 0) currentSampleMix[0] += samples[(currentSample * channels) + currentInChannel];
                    if ((speaker & rightChannelMask) != 0) currentSampleMix[1] += samples[(currentSample * channels) + currentInChannel];
                }

                /*collected all samples, store mixed sample */
                for (int currentOutChannel = 0; currentOutChannel < 2; currentOutChannel++)
                {
                    short value = 0;
                    if (channelCount[currentOutChannel] != 0)
                    {
                        /*clip*/
                        int intValue = currentSampleMix[currentOutChannel] / channelCount[currentOutChannel];
                        if (intValue >= short.MaxValue) value = short.MaxValue;
                        else if (intValue <= short.MinValue) value = short.MinValue;
                        else value = (short)intValue;
                    }
                    /*store*/
                    result[(currentSample * 2) + currentOutChannel] = value;
                }
            }

            return result;
        }

        static string ReadIdentity()
        {
            try
            {
                return File.ReadAllText(IdentityFile);
            }
            catch
            {
                Console.WriteLine($"Could not read file '{IdentityFile}'.");
                return null;
            }
        }

        private static string AskForName()
        {
            Console.WriteLine();
            Console.Write("Enter name: ");
            return Console.ReadLine();
        }

        private static string AskForPassword()
        {
            Console.WriteLine();
            Console.Write("Enter password: ");
            return Console.ReadLine();
        }

        public static int AskForVadLevel()
        {
            Console.WriteLine();
            while (true)
            {
                Console.Write($"Enter VAD level (currently: {Connection.Preprocessor.VadLevel}): ");
                string line = Console.ReadLine();
                int value;
                if (int.TryParse(line, out value) == false)
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    Console.WriteLine();
                    continue;
                }
                else return value;
            }
        }

        public static Client AskForWhisperClient()
        {
            return SelectClient("Select the client whose whisper list should be modified (0 for self):") ?? Connection.Self;
        }

        static Channel SelectChannel()
        {
            foreach (Channel channel in Connection.AllChannels)
            {
                for (Channel c = channel; c != null; c = c.Parent)
                    Console.Write("  ");
                Console.WriteLine($"{channel.ID} {channel.Name}");
            }
            Console.Write("Select Channel:");
            ulong result;
            if (ulong.TryParse(Console.ReadLine(), out result) && result != 0)
                return new Channel(Connection, result);
            else return null;
        }

        static Client SelectClient(string question)
        {
            foreach (Channel channel in Connection.AllChannels)
            {
                StringBuilder ident = new StringBuilder();
                for (Channel c = channel; c != null; c = c.Parent)
                    ident.Append("  ");
                Console.WriteLine($"{ident}{channel.Name}");
                foreach (Client client in channel.Clients)
                    Console.WriteLine($"{ident}  {client.ID} {client.Nickname}");
            }
            Console.Write(question);
            ushort result;
            if (ushort.TryParse(Console.ReadLine(), out result) && result != 0)
                return new Client(Connection, result);
            else return null;
        }

        static string CreateIdentity()
        {
            string identity = Library.CreateIdentity();
            WriteIdentity(identity);
            return identity;
        }

        static bool WriteIdentity(string identity)
        {
            try
            {
                File.WriteAllText(IdentityFile, identity);
                return true;
            }
            catch
            {
                Console.WriteLine($"Error writing identity to file '{IdentityFile}'.");
                return false;
            }
        }

        private static void Disconnect()
        {
            Console.WriteLine();
            Console.WriteLine("Disconnecting from server...");
            Abort = true;
        }

        static void ShowHelp()
        {
            Console.WriteLine();
            foreach (MenuItem item in Menu)
            {
                Console.WriteLine($"[{item.Key}] - {item.Title}");
            }
            Console.WriteLine();
        }

        private static void ShowChannels()
        {
            Console.WriteLine();
            Console.WriteLine($"List of channels on virtual server {Connection.ID}:");
            if (Connection.AllChannels.Any() == false)
            {
                Console.WriteLine("No channels");
                Console.WriteLine();
            }
            else
            {
                foreach (Channel channel in Connection.Channels)
                    Console.WriteLine($"{channel.ID} - {channel.Name}");
            }
            Console.WriteLine();
        }

        private static void SwitchChannel()
        {
            Channel channel = SelectChannel();
            if (channel == null) return;
            string password = channel.IsPasswordProtected ? AskForPassword() : null;
            try
            {
                Connection.Self.MoveTo(channel, password);
                Console.WriteLine($"Switching into channel {channel.Name}({channel.ID})");
            }
            catch (TeamSpeakException e)
            {
                Console.WriteLine($"Error moving client into channel channel: {e.ErrorCode}");
            }
        }

        private static void ShowClients()
        {
            Console.WriteLine();
            Console.WriteLine($"List of all visible clients on virtual server {Connection.ID}:");
            if (Connection.AllClients.Any() == false)
            {
                Console.WriteLine("No clients");
                Console.WriteLine();
            }
            else
            {
                foreach (Client client in Connection.AllClients)
                    Console.WriteLine("{0} - {1} ({2}talking)", client.ID, client.Nickname, client.IsTalking ? "" : "not ");
            }
            Console.WriteLine();
        }

        private static void ShowClientsInChannel()
        {
            Channel channel = SelectChannel();
            if (channel == null) return;

            Console.WriteLine();
            Console.WriteLine($"List of clients in channel {channel.Name} on virtual server {Connection.ID}:");
            if (channel.Clients.Any() == false)
            {
                Console.WriteLine("No clients");
                Console.WriteLine();
            }
            else
            {
                foreach (Client client in channel.Clients)
                {
                    Console.WriteLine($"{client.ID} - {client.Nickname}");
                }
            }
        }

        private static void CreateChannel()
        {
            string name = AskForName();
            CreateChannel(name, null);
        }

        private static void CreateChannelPassword()
        {
            string name = AskForName();
            string password = AskForPassword();
            CreateChannel(name, password == string.Empty ? null : password);
        }

        private static void CreateChannel(string name, string password)
        {
            try
            {
                Task<Channel> task = Connection.CreateChannel(name, null,
                    topic: "Test channel topic",
                    description: "Test channel description",
                    isPermanent: true,
                    isSemiPermanent: false,
                    password: password);
                task.Wait();
                Console.WriteLine();
                Console.WriteLine("Created channel");
                Console.WriteLine();
            }
            catch (TeamSpeakException e)
            {
                Console.WriteLine();
                Console.WriteLine($"Error creating channel: {e.ErrorCode}");
                Console.WriteLine();
            }
        }

        private static void DeleteChannel()
        {
            Channel channel = SelectChannel();
            if (channel == null) return;
            try
            {
                string name = channel.Name;
                channel.Delete().Wait();
                Console.WriteLine($"Deleted channel {name}");
            }
            catch (TeamSpeakException e)
            {
                Console.WriteLine($"Error deleting channel: {e.ErrorCode}");
            }
        }

        private static void RenameChannel()
        {
            Channel channel = SelectChannel();
            if (channel == null) return;
            string name = AskForName();
            try
            {
                string oldName = channel.Name;
                channel.Name = name;
                Console.WriteLine($"Renamed channel {oldName} to {name}");
            }
            catch (TeamSpeakException e)
            {
                Console.WriteLine($"Error renaming channel: {e.ErrorCode}");
            }
        }

        private static void RecordWav()
        {
            Connection.IsVoiceRecording = !Connection.IsVoiceRecording;
            if (Connection.IsVoiceRecording)
                Console.WriteLine("Started recording sound to wav");
            else
                Console.WriteLine("Stopped recording sound to wav");
        }

        private static void ToggleVoiceDetection()
        {
            Connection.Preprocessor.Vad = !Connection.Preprocessor.Vad;
            string status = Connection.Preprocessor.Vad ? "on" : "off";
            Console.WriteLine($"Toggled VAD {status}.");
        }

        private static void ConfigureVoiceDetection()
        {
            int level = AskForVadLevel();
            Connection.Preprocessor.VadLevel = level;
            Console.WriteLine($"Set VAD level to {Connection.Preprocessor.VadLevel}.");
        }

        private static void SetWhisperList()
        {
            Client client = AskForWhisperClient() ?? Connection.Self;
            Console.WriteLine();
            Console.Write("Enter target channel ID: ");
            ulong channelID;
            if (ulong.TryParse(Console.ReadLine(), out channelID) == false)
                channelID = 0;
            Channel channel = new Channel(Connection, channelID);
            client.SetWhisperList(new Channel[] { channel }, null);
            Console.WriteLine($"Whisper list requested for client {client.Nickname} in channel {channel.Name}");
        }

        private static void ClearWhisperList()
        {
            Client client = AskForWhisperClient() ?? Connection.Self;
            if (client != null)
            {
                client.SetWhisperList((Channel[])null, (Client[])null);
                Console.WriteLine($"Whisper list cleared for client {client.Nickname}");
            }
        }

        private static void ConfigureMicrophone()
        {
            bool isTalking = false;
            Connection testConnection = null;
            try
            {
                testConnection = CreateLocalTestMode();
                testConnection.TalkStatusChanged += (client, status, isReceivedWhisper) =>
                {
                    isTalking = status == TalkStatus.Talking;
                };
                Console.WriteLine();
                Console.WriteLine("**********************************");
                Console.WriteLine("Entering configure microphone mode");
                Console.WriteLine("[v] - set VAD level");
                Console.WriteLine("[q] - quit microphone configuration");
                Console.WriteLine();

                for (int counter = 0; ; counter++)
                {
                    if (Console.KeyAvailable)
                    {
                        switch (Console.ReadKey().KeyChar)
                        {
                            case 'v':
                                Console.WriteLine("Insert value to change voice activations level");
                                int value;
                                if (int.TryParse(Console.ReadLine(), out value) == false)
                                {
                                    Console.WriteLine("Invalid input. Please enter a number.");
                                    Console.WriteLine();
                                }
                                testConnection.Preprocessor.VadLevel = value;
                                Console.WriteLine($"new vad level: {testConnection.Preprocessor.VadLevel}");
                                continue;
                            case 'q':
                                testConnection.Close();
                                Console.WriteLine();
                                Console.WriteLine("**********************************");
                                Console.WriteLine("Left configure microphone mode");
                                Console.WriteLine();
                                return;
                        }
                    }
                    if (counter == 9)
                    {
                        Console.WriteLine("{0:0.00} - {1}", testConnection.Preprocessor.DecibelLastPeriod, isTalking ? "talking" : "not talking");
                        counter = 0;
                    }
                    Thread.Sleep(100);
                }
            }
            finally
            {
                testConnection?.Close();
                ShowHelp();
            }
        }

        private static Connection CreateLocalTestMode()
        {
            Connection result = Library.SpawnNewConnection();
            result.OpenCapture();
            result.OpenPlayback();
            result.LocalTestMode = LocalTestMode.LocalVoice;
            return result;
        }
    }
}
