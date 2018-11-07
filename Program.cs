using System;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace ChaosDemo
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Must pass in script path");
                return;
            }
            var json = System.IO.File.ReadAllText(args[0]);
            var tasks = JsonConvert.DeserializeObject<ChaosScript>(json);
            foreach (var t in tasks.Templates)
            {
                Console.WriteLine(t.ScriptPath);
                for (var i = 0; i < t.Threads; i++)
                {
                    var t2 = new System.Threading.Thread(() => { runTask(t, tasks.ConnectionString); });
                    t2.Start();
                    SetTasks(1);
                }
            }
            while (true)
            {
                System.Threading.Thread.Sleep(1000);
            }
        }

        public static int tasks = 0;

        public static object lck = new object();

        public static void SetTasks(int threadChange)
        {
            lock (lck)
            {
                tasks += threadChange;
                Console.Clear();
                Console.WriteLine("Running Tasks " + tasks);
            }
        }

        private static void runTask(ChaosTemplate template, string conStr)
        {
            var runCount = 0;
            var sql = System.IO.File.ReadAllText(template.ScriptPath);
            while (runCount < template.RunCount)
            {
                using (var sqlCon = new SqlConnection(conStr))
                {
                    var cmd = new SqlCommand(sql, sqlCon);
                    try
                    {
                        sqlCon.Open();
                        cmd.ExecuteNonQuery();
                        sqlCon.Close();
                    }
                    catch { }
                }
                if (template.RunCount > 0)
                    runCount++;
            }
            SetTasks(-1);
        }
    }

}
