using System;
using ChaosLoad.Models;
using MongoDB.Driver;

namespace ChaosLoad.PlatformLoaders
{
    public class MongoDbLoader : IPlatformLoader
    {
        private readonly ParamReplacer paramReplacer;

        public TemplateType HandlesType => TemplateType.Mongo;

        public MongoDbLoader(ParamReplacer paramReplacer)
        {
            this.paramReplacer = paramReplacer;
        }

        public void RunTask(string connection, string command, int repeat, Action onComplete, int sleep =0)
        {
            var runCount = 0;
            while (runCount <= repeat)
            {
                var mongoUrl = new MongoUrl(connection);
                var db = new MongoClient(mongoUrl).GetDatabase(mongoUrl.DatabaseName);
                db.RunCommand<dynamic>(paramReplacer.Replace(command));
                if (repeat > 0)
                    runCount++;
                System.Threading.Thread.Sleep(sleep);
            }
            onComplete();
        }
    }
}