using System;
using ChaosLoad.Models;
using ChaosLoad.PlatformLoaders;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.IO;
using ChaosDemo.PlatformLoaders.Utils;

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
                Console.WriteLine("Must pass in script path");
                return;
            }

            var chaosScript = JsonConvert.DeserializeObject<ChaosScript>(File.ReadAllText(args[0]));

            var loader = services.GetService<ChaosLoader>();
            loader.Run(chaosScript);

            while (!loader.Finished)
            {
                System.Threading.Thread.Sleep(1000);
            }

            Console.WriteLine("Finished Executing");
            Console.ReadLine();
        }

        static void SetupDI()
        {
            services = new ServiceCollection()
                .AddTransient<IPlatformLoader, MongoDbLoader>()
                .AddTransient<IPlatformLoader, SqlDbLoader>()
                .AddSingleton<ChaosLoader, ChaosLoader>()
                .AddSingleton<ParamReplacer, ParamReplacer>()
                .BuildServiceProvider();
        }

    }
}
