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

            Manager manager = new Manager();
            bool OK = true;

            if (OK)
            {
                try
                {
                    if (!(OK = manager.Init()))
                    {
                        Console.WriteLine("Manager failed to initialise!");
                    }
                }
                catch (Exception ex)
                {
                    OK = false;

                    Console.WriteLine("Caught exception during intialisation: ");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
            }
            
            WaitShutdown();

            //Bypass: if (OK)
            {
                try
                {
                    if (!(OK = manager.Shutdown()))
                    {
                        Console.WriteLine("Manager failed to shutdown!");
                    }
                }
                catch (Exception ex)
                {
                    OK = false;

                    Console.WriteLine("Caught exception during shutdown: ");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
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
            Console.WriteLine("Press any key to shutdown...");
            Console.ReadKey();
        }
        static void WaitExit()
        {
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
