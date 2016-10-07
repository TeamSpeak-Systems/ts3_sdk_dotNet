using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Teamspeak.Sdk.Client
{
    internal static class EventHelper
    {
        public static Task Run<T>(T t, Action<T> action)
            where T : class
        {
            if (t == null) return null;
            return Task.Factory.StartNew(state => action((T)state), t, CancellationToken.None);
        }
    }
}
