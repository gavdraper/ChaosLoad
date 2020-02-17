using System;
using System.Data.SqlClient;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace ChaosDemo
{
    public class ChaosLoader
    {
        private readonly string templatePath;
        private ChaosScript chaosScript;
        private int runningThreads;
        private bool allThreadsStarted = false;
        private readonly object lck = new object();

        public bool Finished { get; set; } = false;

        public ChaosLoader(string templatePath)
        {
            this.templatePath = templatePath;
        }

        public void RunLoad()
        {
            var json = System.IO.File.ReadAllText(templatePath);
            chaosScript = JsonConvert.DeserializeObject<ChaosScript>(json);
            foreach (var template in chaosScript.Templates)
            {
                Console.WriteLine(template.ScriptPath);
                for (var i = 0; i < template.Threads; i++)
                {
                    var templateThread = new System.Threading.Thread(() =>
                    {
                        if (chaosScript.Type == TemplateType.Sql)
                            runSqlTask(template, chaosScript.ConnectionString);
                        else if (chaosScript.Type == TemplateType.Mongo)
                            runMongoTask(template, chaosScript.ConnectionString);
                    });
                    templateThread.Start();
                    setRunningThreadCount(1);
                }
            }
            allThreadsStarted = true;
        }

        private void setRunningThreadCount(int threadChange)
        {
            lock (lck)
            {
                runningThreads += threadChange;
                Console.Clear();
                Console.WriteLine("Running Threads " + runningThreads);
                if (allThreadsStarted && runningThreads == 0)
                {
                    Finished = true;
                }
            }
        }

        private string replaceParams(string command)
        {
            command = command.Replace("@DateTime", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss:ms"));
            command = command.Replace("@Date", DateTime.Now.ToString("dd/MM/yyyy"));
            return command;
        }

        private void runSqlTask(ChaosTemplate template, string conStr)
        {
            var runCount = 0;
            var commandText = System.IO.File.ReadAllText(template.ScriptPath);
            while (runCount <= template.RunCount)
            {
                using (var sqlCon = new SqlConnection(conStr))
                {
                    sqlCon.Open();
                    var cmd = new SqlCommand(replaceParams(commandText), sqlCon);
                    cmd.ExecuteNonQuery();
                }
                if (template.RunCount > 0)
                    runCount++;
            }
            setRunningThreadCount(-1);
        }

        private void runMongoTask(ChaosTemplate template, string conStr)
        {
            var runCount = 0;
            var commandText = System.IO.File.ReadAllText(template.ScriptPath);
            while (runCount <= template.RunCount)
            {

                var client = new MongoClient(conStr);
                var db = client.GetDatabase(template.MongoDatabase);
                db.RunCommand<dynamic>(replaceParams(commandText));
                if (template.RunCount > 0)
                    runCount++;
            }
            setRunningThreadCount(-1);
        }

    }


}
