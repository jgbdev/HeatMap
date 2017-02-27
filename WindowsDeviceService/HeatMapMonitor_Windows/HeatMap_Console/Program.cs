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

            if (OK)
            {
                try
                {
                    if (!(OK = manager.Update()))
                    {
                        Console.WriteLine("Manager failed to update!");
                    }
                }
                catch (Exception ex)
                {
                    OK = false;

                    Console.WriteLine("Caught exception during updating: ");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
            }

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

            Exit();
        }

        private static void Logger_OnMessageLogged(string message)
        {
            Console.WriteLine(message);
        }

        static void Exit()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
