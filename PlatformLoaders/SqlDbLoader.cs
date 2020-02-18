using System;
using System.Data.SqlClient;
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

        public void RunTask(string connection, string command, int repeat, Action onComplete)
        {
            var runCount = 0;
            while (runCount <= repeat)
            {
                using (var sqlCon = new SqlConnection(connection))
                {
                    sqlCon.Open();
                    var cmd = new SqlCommand(paramReplacer.Replace(command), sqlCon);
                    cmd.ExecuteNonQuery();
                }
                if (repeat > 0)
                    runCount++;
            }
            onComplete();
        }
    }
}