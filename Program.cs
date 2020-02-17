using System;

namespace ChaosDemo
{
    public class Program
    {
        static void Main(string[] args)
        {
            var path = "Scripts/MongoLoad/MongoLoad.json";
            // if (args.Length != 1)
            // {
            //     Console.WriteLine("Must pass in script path");
            //     return;
            // }

            //var chaosLoader = new ChaosLoader(args[0]);
            var chaosLoader = new ChaosLoader(path);
            chaosLoader.RunLoad();

            while (!chaosLoader.Finished)
            {
                System.Threading.Thread.Sleep(1000);
            }

            Console.WriteLine("Finished Executing");
            Console.ReadLine();
        }
    }
}
