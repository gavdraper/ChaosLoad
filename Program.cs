using System;
using ChaosLoad.Models;
using ChaosLoad.PlatformLoaders;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.IO;
using ChaosLoad.Utils;
using System.Linq;

namespace ChaosLoad
{
    public class Program
    {
        static ServiceProvider services;

        static void Main(string[] args)
        {
            SetupDI();

            if (args.Length != 1)
            {
                args = new string[] { "scripts/dockersql/demo.json" };
                //Console.WriteLine("Must pass in script path");
                //return;
            }

            var chaosScript = JsonConvert.DeserializeObject<ChaosScript>(File.ReadAllText(args[0]));

            var loader = services.GetService<ChaosScriptRunner>();
            loader.Run(chaosScript);

            while (!loader.Finished)
            {

                foreach (var l in services.GetServices<IPlatformLoader>().Where(x => x.Active))
                {
                    var stats = l.Stats();
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write(stats + "      ");
                }
                System.Threading.Thread.Sleep(1000);
            }

            Console.WriteLine("Finished Executing");
            Console.ReadLine();
        }

        static void SetupDI()
        {
            services = new ServiceCollection()
                .AddSingleton<IPlatformLoader, MongoDBLoader>()
                .AddSingleton<IPlatformLoader, SqlServerLoader>()
                .AddSingleton<IPlatformLoader, RabbitPublisherLoader>()
                .AddSingleton<ChaosScriptRunner, ChaosScriptRunner>()
                .AddSingleton<ParamReplacer, ParamReplacer>()
                .BuildServiceProvider();
        }

    }
}
