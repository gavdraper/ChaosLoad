using System;
using System.Data.SqlClient;
using ChaosDemo.PlatformLoaders.Utils;
using ChaosLoad.Models;

namespace ChaosLoad.PlatformLoaders
{
    public class SqlDbLoader : IPlatformLoader
    {
        private readonly ParamReplacer paramReplacer;

        public TemplateType HandlesType => TemplateType.Sql;

        public SqlDbLoader(ParamReplacer paramReplacer)
        {
            this.paramReplacer = paramReplacer;
        }

        public void RunTask(string connection, string command, int repeat, Action onComplete, int sleep = 0)
        {
            var runCount = 0;

            using (var sqlCon = new SqlConnection(connection))
            {
                sqlCon.Open();
                var cmd = new SqlCommand(paramReplacer.Replace(command), sqlCon);

                while (repeat == 0 || runCount < repeat)
                {
                    cmd.ExecuteNonQuery();
                    
                    if (repeat > 0)
                        runCount++;
                    System.Threading.Thread.Sleep(sleep);
                }
            }
            onComplete();
        }
    }
}