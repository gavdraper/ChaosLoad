using System;
using ChaosLoad.PlatformLoaders;
using ChaosLoad.Utils;
using ChaosLoad.Models;
using MongoDB.Driver;

namespace ChaosLoad.PlatformLoaders
{
    public class MongoDBLoader : Loader, IPlatformLoader
    {
        private readonly ParamReplacer paramReplacer;
        public override string Name { get; set; } = "Mongo DB Loader";
        public TemplateType HandlesType => TemplateType.Mongo;

        public MongoDBLoader(ParamReplacer paramReplacer)
        {
            this.paramReplacer = paramReplacer;
        }

        public void RunTask(string connection, string command, int repeat, Action onComplete, int sleep = 0)
        {
            var runCount = 0;

            var mongoUrl = new MongoUrl(connection);
            var db = new MongoClient(mongoUrl).GetDatabase(mongoUrl.DatabaseName);

            while (repeat == 0 || runCount <= repeat)
            {
                var paramCommand = paramReplacer.Replace(command);
                db.RunCommand<dynamic>(paramCommand);

                if (repeat > 0)
                    runCount++;
                System.Threading.Thread.Sleep(sleep);
            }
            onComplete();
        }
    }
}