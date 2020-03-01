using System;

namespace ChaosLoad.Utils
{
    public class ParamReplacer
    {
        public string Replace(string command)
        {
            command = command.Replace("@DateTime", DateTime.Now.ToString("yyyyMMdd hh:mm:ss"));
            command = command.Replace("@Date", DateTime.Now.ToString("dd/MM/yyyy"));
            return command;
        }
    }
}