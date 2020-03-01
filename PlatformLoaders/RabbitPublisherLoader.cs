using System;
using ChaosLoad.PlatformLoaders;
using ChaosLoad.Utils;
using ChaosLoad.Models;
using EasyNetQ;
using Newtonsoft.Json;

namespace ChaosLoad.PlatformLoaders
{
    public class RabbitPublisherLoader : Loader, IPlatformLoader
    {
        private readonly ParamReplacer paramReplacer;

        public TemplateType HandlesType => TemplateType.RabbitPublisher;

        public override string Name { get; set; } = "Rabbit Publisher";

        public RabbitPublisherLoader(ParamReplacer paramReplacer)
        {
            this.paramReplacer = paramReplacer;
        }

        public void RunTask(string connection, string command, int repeat, Action onComplete, int sleep = 0)
        {
            var runCount = 0;
            var bus = RabbitHutch.CreateBus(connection);

            while (repeat == 0 || runCount < repeat)
            {
                var message = JsonConvert.DeserializeObject<dynamic>(command);
                bus.Publish(message);

                if (repeat > 0)
                    runCount++;
                System.Threading.Thread.Sleep(sleep);
            }
            onComplete();
        }
    }
}