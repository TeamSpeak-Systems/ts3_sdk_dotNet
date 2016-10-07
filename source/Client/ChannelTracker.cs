using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Teamspeak.Sdk.Client
{

    internal class ChannelTracker
    {
        private const ulong InvalidParentID = ulong.MaxValue;
        private class Record
        {
            public ulong ParentID;
            public ulong Order;
            public List<Channel> Channels = new List<Channel>();
            public ReaderWriterLockSlim Lock = new ReaderWriterLockSlim();
            public Record(ulong parentID, ulong order)
            {
                ParentID = parentID;
                Order = order;
            }
        }

        private ConcurrentDictionary<ulong, Record> Records;

        public ChannelTracker()
        {
            Reset();
        }

        public ReadonlyChannelCollection GetChannels(ulong id)
        {
            Record tracker;
            if (Records.TryGetValue(id, out tracker))
            {
                tracker.Lock.EnterReadLock();
                Channel[] result = new Channel[tracker.Channels.Count];
                for (int i = 0; i < result.Length; i++)
                    result[i] = tracker.Channels[i];
                tracker.Lock.ExitReadLock();
                return new ReadonlyChannelCollection(result);
            }
            else return new ReadonlyChannelCollection(new Channel[0]);

        }

        public void ChannelAdded(Channel channel, ulong parentID)
        {
            Record new_tracker = new Record(parentID, GetOrder(channel));
            while (true)
            {
                Record tracker = Records.GetOrAdd(channel.ID, new_tracker);
                if (tracker == new_tracker) break;
                Debug.Assert(false);
                ChannelRemoved(channel);
            }
            AddToChannels(parentID, channel, new_tracker.Order);
        }

        public void ChannelRemoved(Channel channel)
        {
            Record tracker;
            if (Records.TryRemove(channel.ID, out tracker))
            {
                tracker.Lock.EnterWriteLock();
                tracker.Channels.Clear();
                tracker.Lock.ExitWriteLock();
            }
        }

        public void ChannelMoved(Channel channel, ulong parentID)
        {
            Record tracker, parentTracker;
            if (Records.TryGetValue(channel.ID, out tracker) == false)
            {
                Debug.Assert(false);
                return;
            }
            if (tracker.ParentID != InvalidParentID)
            {
                if (Records.TryGetValue(tracker.ParentID, out parentTracker))
                {
                    parentTracker.Lock.EnterWriteLock();
                    parentTracker.Channels.Remove(channel);
                    parentTracker.Lock.ExitWriteLock();
                }
                else Debug.Assert(false);
            }
            tracker.ParentID = parentID;
            tracker.Order = GetOrder(channel);
            AddToChannels(parentID, channel, tracker.Order);
        }

        public void CheckOrder(Channel channel)
        {
            Record tracker;
            if (Records.TryGetValue(channel.ID, out tracker) == false)
            {
                Debug.Assert(false);
                return;
            }
            ulong newOrder = GetOrder(channel);
            if (tracker.Order == newOrder)
            {
                return;
            }
            tracker.Order = newOrder;
            Record parentTracker;
            if (Records.TryGetValue(tracker.ParentID, out parentTracker) == false)
            {
                Debug.Assert(false);
                return;
            }
            parentTracker.Lock.EnterWriteLock();
            parentTracker.Channels.Remove(channel);
            AddToChannels(parentTracker, channel, tracker.Order);
            parentTracker.Lock.ExitWriteLock();
        }

        public void Reset()
        {
            ConcurrentDictionary<ulong, Record> records = new ConcurrentDictionary<ulong, Record>();
            records.TryAdd(0, new Record(InvalidParentID, 0));
            Records = records;
        }

        private void AddToChannels(ulong parentID, Channel channel, ulong order)
        {
            Record parentTracker;
            if (Records.TryGetValue(parentID, out parentTracker) == false)
            {
                Debug.Assert(false);
                return;
            }
            parentTracker.Lock.EnterWriteLock();
            AddToChannels(parentTracker, channel, order);
            parentTracker.Lock.ExitWriteLock();
        }

        private void AddToChannels(Record tracker, Channel channel, ulong order)
        {
            int index;
            if (order != 0)
            {
                index = 1 + tracker.Channels.FindIndex(c => c.ID == order);
            }
            else index = 0;
            tracker.Channels.Insert(index, channel);
        }

        private static ulong GetOrder(Channel channel)
        {
            UInt64 order;
            if (Library.Api.TryGetChannelVariableAsUInt64(channel, ChannelProperty.Order, out order) != Error.Ok)
            {
                System.Diagnostics.Debug.Assert(false, "failed to get channel order");
                order = 0;
            }
            return order;
        }
    }
}
