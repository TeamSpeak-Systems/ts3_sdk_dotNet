using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Teamspeak.Sdk.Client
{
    internal class ConnectionCaches
    {
        private static int CleanningInterval = 100; // every 100 gets of a cached object => check that cache and remove dead entries
        private static int CleanningCounter = 0;
        private readonly ConcurrentDictionary<ushort, Client> Clients;
        private readonly ConcurrentDictionary<ulong, Channel> Channels;
        private readonly ConcurrentDictionary<ulong, WeakReference> WaveHandles;
        private readonly ConcurrentDictionary<ushort, WeakReference> FileTransfers;
        private readonly Connection Connection;

        public readonly ReceivedListBuilder<FileInfo> FileListBuilder = new ReceivedListBuilder<FileInfo>();
        public readonly ReceivedListBuilder<Client> ClientIDsBuilder = new ReceivedListBuilder<Client>();

        public ConnectionCaches(Connection connection)
        {
            Connection = connection;
            WaveHandles = new ConcurrentDictionary<ulong, WeakReference>();
            Clients = new ConcurrentDictionary<ushort, Client>();
            Channels = new ConcurrentDictionary<ulong, Channel>();
            FileTransfers = new ConcurrentDictionary<ushort, WeakReference>();
        }

        public Channel GetChannel(ulong channelID)
        {
            if (channelID == 0) return null;
            return Channels.GetOrAdd(channelID, id => new Channel(Connection, id, waitForProperties: true));
        }

        public Channel RemoveChannel(ulong channelID)
        {
            if (channelID == 0) return null;
            Channel result;
            if (Channels.TryRemove(channelID, out result) == false)
                result = new Channel(Connection, channelID, waitForProperties: false);
            return result;
        }

        public Client GetClient(ushort clientID)
        {
            return GetOrRemoveClient(clientID, Visibility.Retain);
        }

        public Client GetOrRemoveClient(ushort clientID, Visibility visibility)
        {
            if (clientID == 0) return null;
            Client result;
            switch (visibility)
            {
                case Visibility.Leave:
                    if (Clients.TryRemove(clientID, out result) == false)
                        result = new Client(Connection, clientID, waitForProperties: false);
                    break;
                case Visibility.Enter:
                    result = Clients.GetOrAdd(clientID, id => new Client(Connection, id, waitForProperties: true));
                    break;
                case Visibility.Retain:
                    result = Clients.GetOrAdd(clientID, id => new Client(Connection, id, waitForProperties: false));
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    goto case Visibility.Retain;
            }
            return result;
        }

        public FileTransfer GetTransfer(ushort transferID)
        {
            return GetOrAdd(FileTransfers, transferID, () => new FileTransfer(Connection, transferID));
        }
        public static void RemoveTransfer(FileTransfer transfer)
        {
            WeakReference reference;
            transfer.Connection.Cache.FileTransfers.TryRemove(transfer.ID, out reference);
        }

        public WaveHandle GetWaveHandle(ulong waveHandle)
        {
            return GetOrAdd(WaveHandles, waveHandle, () => new WaveHandle(Connection, waveHandle));
        }

        private static TItem GetOrAdd<TKey, TItem>(ConcurrentDictionary<TKey, WeakReference> cache, TKey key, Func<TItem> createItem)
            where TItem : class
        {
            WeakReference reference = cache.GetOrAdd(key, _ => new WeakReference(null));
            TItem result = reference.Target as TItem;
            if (result == null)
            {
                lock (reference)
                {
                    result = reference.Target as TItem;
                    if (result == null)
                    {
                        result = createItem();
                        reference.Target = result;
                    }
                }
            }
            if (Interlocked.Increment(ref CleanningCounter) == CleanningInterval)
            {
                Interlocked.Add(ref CleanningCounter, -CleanningInterval);
                CleanCache(cache);
            }
            return result;
        }

        private static void CleanCache<TKey>(ConcurrentDictionary<TKey, WeakReference> cache)
        {
            foreach (KeyValuePair<TKey, WeakReference> item in cache)
            {
                if (item.Value.IsAlive == false)
                {
                    lock (item.Value)
                    {
                        WeakReference reference;
                        if (item.Value.IsAlive == false)
                            cache.TryRemove(item.Key, out reference);
                    }
                }
            }
        }
    }

    internal class ReceivedListBuilder<T>
    {
        private readonly ConcurrentDictionary<string, List<T>> Cache = new ConcurrentDictionary<string, List<T>>();

        public bool Register(string code)
        {
            return Cache.TryAdd(code, new List<T>());
        }

        public void Add(string code, T t)
        {
            List<T> list;
            bool success = Cache.TryGetValue(code, out list);
            System.Diagnostics.Debug.Assert(success);
            if (success)
            {
                list.Add(t);
            }
        }

        public List<T> Collect(string code)
        {
            List<T> result;
            bool success = Cache.TryRemove(code, out result);
            System.Diagnostics.Debug.Assert(success);
            return success ? result : new List<T>(0);
        }
    }
}
