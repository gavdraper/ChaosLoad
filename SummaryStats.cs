using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ChaosLoad
{
    public class SummaryStats : IDisposable
    {
        private readonly int maxAgeMs = 10000;
        private object objLock = new object();
        private List<DateTime> TimeStamps { get; set; } = new List<DateTime>();
        private Thread PruneThread;
        private bool Stopping = false;

        public SummaryStats(int maxAgeMs)
        {
            this.maxAgeMs = maxAgeMs;

            PruneThread = new Thread(() =>
            {
                while (!Stopping)
                {
                    lock (objLock)
                    {
                        TimeStamps.RemoveAll(x => x < DateTime.UtcNow.AddMilliseconds(-maxAgeMs));
                    }
                    Thread.Sleep(5000);
                }
            });
            PruneThread.Start();
        }

        public int Count()
        {
            lock (objLock)
            {
                return TimeStamps.Count(x => x >= DateTime.UtcNow.AddMilliseconds(-maxAgeMs));
            }
        }

        public void Log()
        {
            lock (objLock)
            {
                TimeStamps.Add(DateTime.UtcNow);
            }
        }

        public void Dispose()
        {
            Stopping = true;
            PruneThread.Join();
        }
    }
}