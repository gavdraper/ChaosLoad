using System;

namespace ChaosLoad.PlatformLoaders
{
    public class ParamReplacer
    {
        public string Replace(string command)
        {
            command = command.Replace("@DateTime", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss:ms"));
            command = command.Replace("@Date", DateTime.Now.ToString("dd/MM/yyyy"));
            return command;
        }
    }
}