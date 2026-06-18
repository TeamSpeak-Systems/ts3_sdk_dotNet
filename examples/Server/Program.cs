using System;
using System.IO;
using System.Linq;
using TeamSpeak.Sdk;
using TeamSpeak.Sdk.Server;

namespace Server
{
    class Program
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

        static bool Abort = false;

        private static void CreateChannel(string name)
        {
            try
            {
                var channel = virtualServer.CreateChannel(name);
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

        private static string AskForName()
        {
            Console.WriteLine();
            Console.Write("Enter name: ");
            return Console.ReadLine();
        }

        private static void CreateChannel()
        {
            string name = AskForName();
            CreateChannel(name);
        }

        static MenuItem[] Menu = new MenuItem[]
        {
            new MenuItem('n', "Create new channel", CreateChannel),
            new MenuItem('q', "Stop server", Quit),
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Starting up server!");
            Initialize();
            Console.WriteLine();
            Console.WriteLine("TeamSpeak server commandline interface");
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
        }

        static VirtualServer virtualServer;

        private static void Initialize()
        {
            LibraryParameters parameters = new LibraryParameters(AppContext.BaseDirectory);
            parameters.UsedLogTypes = LogTypes.File | LogTypes.Console | LogTypes.Userlogging;
            Library.Initialize(parameters);
            Library.UserLogMessage += (string message, LogLevel level, string channel, VirtualServer virtualServer, string time, string completeString) =>
            {
                Console.WriteLine(completeString);
            };
            virtualServer = Library.CreateVirtualServer(8, "", 9987, "0.0.0.0", "TeamSpeak SDK Server");
            virtualServer.Password = "secret";
            virtualServer.ClientConnected += (Client client, Channel channel, ref bool removeClientError) =>
            {
                Console.WriteLine("Client connected.");
            };

            virtualServer.ClientDisconnected += (Client client, Channel channel) =>
            {
                Console.WriteLine("Client disconnected.");
            };
            Console.WriteLine("Initialized.");
        }

        private static void Quit()
        {
            Console.WriteLine();
            Console.WriteLine("Quit...");
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
    }
}
