using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeatMapMonitor_Windows;

namespace HeatMap_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.OnMessageLogged += Logger_OnMessageLogged;
            Logger.OnErrorLogged += Logger_OnMessageLogged;

            Manager manager = new Manager();
            bool OK = true;

            if (OK)
            {
                try
                {
                    if (!(OK = manager.Init()))
                    {
                        Logger_OnMessageLogged("Manager failed to initialise!");
                    }
                }
                catch (Exception ex)
                {
                    OK = false;

                    Logger.Log(ex);
                }
            }
            
            WaitShutdown();

            //Bypass: if (OK)
            {
                try
                {
                    if (!(OK = manager.Shutdown()))
                    {
                        Logger_OnMessageLogged("Manager failed to shutdown!");
                    }
                }
                catch (Exception ex)
                {
                    OK = false;

                    Logger.Log(ex);
                }
            }

            WaitExit();
        }

        private static void Logger_OnMessageLogged(string message)
        {
            Console.WriteLine(message);
        }

        static void WaitShutdown()
        {
            Logger_OnMessageLogged("Press any key to shutdown...");
            Console.ReadKey();
        }
        static void WaitExit()
        {
            Logger_OnMessageLogged("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
