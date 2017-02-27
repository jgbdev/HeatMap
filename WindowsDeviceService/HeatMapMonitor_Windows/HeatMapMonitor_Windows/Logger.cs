using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeatMapMonitor_Windows
{
    public delegate void MessageLogged(string message);
    public static class Logger
    {
        public static event MessageLogged OnMessageLogged = (string message) => {
            //Console.WriteLine(message);
            //Console.WriteLine();
        };

        public static void Log(Exception ex)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("Error!");
            message.AppendLine("=======================================");
            message.AppendLine(ex.Message);
            message.AppendLine("---------------------------------------");
            message.AppendLine(ex.StackTrace);
            message.AppendLine("=======================================");
            Log(message.ToString());
        }

        public static void Log(string message)
        {
            OnMessageLogged(message);
        }

    }
}
