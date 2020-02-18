using System;
using System.Collections.Generic;
using ChaosLoad.Models;
using ChaosLoad.PlatformLoaders;
using Newtonsoft.Json;
using System.Threading;
using System.Linq;

namespace ChaosLoad
{
    public class ChaosLoader
    {
        public bool Finished { get; set; } = false;

        private int runningThreadCount;
        private bool allThreadsStarted = false;
        private readonly object lck = new object();
        private readonly IEnumerable<IPlatformLoader> loaders;

        public ChaosLoader(IEnumerable<IPlatformLoader> loaders)
        {
            this.loaders = loaders;
        }

        public void Run(ChaosScript chaosScript)
        {
            foreach (var template in chaosScript.Templates)
            {
                var commandText = System.IO.File.ReadAllText(template.ScriptPath);
                for (var i = 0; i < template.Threads; i++)
                {
                    var templateThread = new Thread(() => RunPlatformLoader(
                        chaosScript.ConnectionString,
                        chaosScript.Type,
                        commandText,
                        template.RunCount)
                    );
                    templateThread.Start();
                    IncrementThreeadCount();
                }
            }
            allThreadsStarted = true;
        }

        private void RunPlatformLoader(string connectionString, TemplateType type, string commandText, int repeat)
        {
            var loader = loaders.Single(x => x.HandlesType == type);
            loader.RunTask(
                connectionString,
                commandText,
                repeat,
                () => DecrementThreeadCount());
        }

        private void IncrementThreeadCount()
        {
            lock (lck)
            {
                runningThreadCount += 1;
            }
        }

        private void DecrementThreeadCount()
        {
            lock (lck)
            {
                runningThreadCount -= 1;
                if (allThreadsStarted && runningThreadCount == 0)
                {
                    Finished = true;
                }
            }
        }
    }
}
