using System;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace ChaosDemo
{
   public  class Program
    {
        static void Main(string[] args)
        {
            var json = System.IO.File.ReadAllText("Demo.json");
            var tasks = JsonConvert.DeserializeObject<ChaosScript>(json);
            foreach (var t in tasks.Templates)
            {
                Console.WriteLine(t.ScriptPath);
                for (var i = 0; i < t.Threads; i++)
                {
                    System.Threading.Tasks.Task.Run(() => runTask(t,tasks.ConnectionString));
                    SetThreads(1);
                }
            }
            while (true)
            {
                System.Threading.Thread.Sleep(1000);
            }
        }

        private static bool stopping = false;
        public static int threads = 0;

        public static object lck = new object();

        public static void SetThreads(int threadChange)
        {
            lock(lck){
                threads+= threadChange;
                Console.Clear();
                Console.WriteLine("Running threads " + threads);
            }
        }

        private static void runTask(ChaosTemplate template,string conStr)
        {
            var runCount = 0;
            var sql = System.IO.File.ReadAllText(template.ScriptPath);
            while (!stopping && runCount < template.RunCount)
            {
                using(var sqlCon = new SqlConnection(conStr))
                {
                    var cmd = new SqlCommand(sql,sqlCon);
                    sqlCon.Open();
                    cmd.ExecuteNonQuery();
                    sqlCon.Close();
                }
                runCount++;
            }
            SetThreads(-1);
        }
    }

}
