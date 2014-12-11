using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace UtiliCS
{
    public static class Profiler
    {
        public static Dictionary<string, Stopwatch> StopWatches = new Dictionary<string, Stopwatch>();

        public static void Start(string name)
        {
            Stopwatch watch;
            if (!StopWatches.TryGetValue(name, out watch))
            {
                StopWatches.Add(name, watch = new Stopwatch());
                watch.Start();
            }
            else
            {
                watch.Restart();
            }
        }

        public static void End(string name)
        {
            var watch = StopWatches[name];
            Debug.WriteLine("Prf: [{0}] took [{1}]ms.", name, watch.ElapsedMilliseconds);
        }
    }
}
