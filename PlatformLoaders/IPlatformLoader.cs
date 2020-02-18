using System;
using ChaosLoad.Models;

namespace ChaosLoad.PlatformLoaders
{
    public interface IPlatformLoader
    {
        void RunTask(string connection, string command, int repeat, Action onComplete);
        TemplateType HandlesType { get; }
    }
}