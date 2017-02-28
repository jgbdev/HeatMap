using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeatMapMonitor_Windows;
using System.Timers;

namespace HeatMap_Console
{
    class Program
    {
        static Manager manager;
        static Timer timer;
        static float[] values;
        static uint time;

        static void Main(string[] args)
        {
            Logger.OnMessageLogged += Logger_OnMessageLogged;
            Logger.OnErrorLogged += Logger_OnMessageLogged;

            manager = new Manager();
            bool OK = true;

            bool DummyMode = false;
            Console.WriteLine("Dummy mode? (YES/no)");
            string DummyModeStr = Console.ReadLine().Trim().ToLower();
            if (string.IsNullOrWhiteSpace(DummyModeStr) || DummyModeStr == "yes")
            {
                DummyMode = true;
            }

            if (OK)
            {
                try
                {
                    if (!(OK = manager.Init(!DummyMode)))
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

            if (DummyMode)
            {
                // Generates some load on the server
                Console.WriteLine("Load mode? (sine,square)");
                string LoadMode = Console.ReadLine().Trim().ToLower();
                values = new float[manager.TheConfig.UpdatePeriod / 1000];
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = 32.15f;
                }

                if (LoadMode == "sine")
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] *= (float)Math.Abs(Math.Sin(i * 2 * Math.PI / values.Length));
                    }
                }
                else if (LoadMode == "square")
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] *= i > (values.Length / 2) ? 1.0f : 0.0f;
                    }
                }

                for (int i = 0; i < values.Length; i++)
                {
                    values[i] += 27.92f;
                }

                time = 0;
                timer = new Timer(manager.TheConfig.UpdatePeriod);
                timer.Elapsed += Timer_Elapsed;
                timer.Start();
            }

            WaitShutdown();

            if (DummyMode)
            {
                timer.Stop();
                timer = null;
            }

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

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            time++;
            if (time >= values.Length)
            {
                time = 0;
            }

            Dictionary<string, HardwareInfo> HardwareInfos = new Dictionary<string, HardwareInfo>();
            HardwareInfo DummyInfo = new HardwareInfo("Dummy_CPU");
            HardwareInfos.Add(DummyInfo.Name, DummyInfo);

            DummyInfo.AddSensor("cpu_0").AddValue("cpu_temperature", values[time]);

            manager.UploadReadings(HardwareInfos);
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
