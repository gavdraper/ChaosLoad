using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ChaosLoad;

namespace ChaosLoad.PlatformLoaders
{
    public abstract class Loader
    {
        public abstract string Name { get; set; }
        public bool Active { get; private set; }
        public Dictionary<string, SummaryStats> StatCollection = new Dictionary<string, SummaryStats>();

        public Loader()
        {
            StatCollection.Add("Executions", new SummaryStats(2000));
            StatCollection.Add("Exceptions", new SummaryStats(2000));
        }

        public string Stats()
        {
            var executions = StatCollection["Executions"].Count();
            var exceptions = StatCollection["Exceptions"].Count();
            var statsMessage = $@"{Name} : {executions} Executions, {exceptions} Exceptions";
            return statsMessage;
        }

        public T RunCommand<T>(Func<T> command)
        {
            this.Active = true;
            try
            {
                var result = command();
                StatCollection["Executions"].Log();
                return result;
            }
            catch
            {
                StatCollection["Exceptions"].Log();
                return default(T);
            }
        }
    }
}