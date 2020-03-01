using System;
using System.Data.SqlClient;
using System.Diagnostics;
using ChaosLoad.PlatformLoaders;
using ChaosLoad.Utils;
using ChaosLoad.Models;

namespace ChaosLoad.PlatformLoaders
{

    public class SqlServerLoader : Loader, IPlatformLoader
    {
        private readonly ParamReplacer paramReplacer;

        public TemplateType HandlesType => TemplateType.Sql;

        public override string Name { get; set; } = "SQL Server Loader";

        public SqlServerLoader(ParamReplacer paramReplacer)
        {
            this.paramReplacer = paramReplacer;
        }

        public void RunTask(string connection, string command, int repeat, Action onComplete, int sleep = 0)
        {
            var runCount = 0;

            using (var sqlCon = new SqlConnection(connection))
            {
                sqlCon.Open();
                var cmd = new SqlCommand("", sqlCon);
                while (repeat == 0 || runCount < repeat)
                {
                    cmd.CommandText = paramReplacer.Replace(command);
                    RunCommand(cmd.ExecuteNonQuery);
                    if (repeat > 0)
                        runCount++;
                    System.Threading.Thread.Sleep(sleep);
                }
            }
            onComplete();
        }
    }
}