using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Timers;

namespace HeatMapMonitor_Windows
{
    public class Manager
    {
        const string DefaultAPIBaseURL = "http://34.251.68.107:5000/api/";
        const string DefaultAPIAccountId = "";
        const string DefaultAPISecretKey = "";

        string ConfigPath;

        Hardware hardware;
        API.HeatMapAPI api;
        Config config;
        Timer timer;

        public bool Init()
        {
            bool OK = true;

            ConfigPath = Path.Combine(Environment.CurrentDirectory, "heatmap_config.txt");

            try
            {
                config = new Config(ConfigPath);
            }
            catch (FormatException ex)
            {
                Logger.Log(ex);

                throw;
            }
            catch (IndexOutOfRangeException)
            {
                config = new Config(DefaultAPIBaseURL, DefaultAPIAccountId, DefaultAPISecretKey);

                config.UpdateDeviceId("");
                config.Save(ConfigPath);
            }
            catch (FileNotFoundException)
            {
                config = new Config(DefaultAPIBaseURL, DefaultAPIAccountId, DefaultAPISecretKey);

                config.UpdateDeviceId("");
                config.Save(ConfigPath);
            }

            api = new API.HeatMapAPI(config);

            if (!config.HasDeviceId)
            {
                string IdResponse = api.GetId();
                config.UpdateDeviceId(IdResponse);
                config.Save(ConfigPath);
            }

            Logger.Log("Device id: " + config.DeviceId);

            config.UpdatePeriod = api.GetInterval(config.DeviceId);

            Logger.Log("Update period: " + config.UpdatePeriod);
            
            hardware = new Hardware();

            timer = new Timer(config.UpdatePeriod);
            timer.Elapsed += Timer_Elapsed;

            if(!Update())
            {
                throw new Exception("Failed initial update.");
            }

            timer.Start();

            return OK;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            bool Updated = false;
            try
            {
                Updated = Update();
            }
            catch
            {
                Updated = false;
            }

            if (!Updated)
            {
                Logger.Log("Manager failed to update. Stopping interval timer...");
                timer.Stop();
            }
        }

        public bool Update()
        {
            Logger.Log("Updating...");

            Dictionary<string, HardwareInfo> HardwareInfos = hardware.GetHardwareInfos();
            api.SendReading(config.DeviceId, HardwareInfos);

            return true;
        }

        public bool Shutdown()
        {
            if (timer != null)
            {
                timer.Stop();
            }

            return true;
        }
    }
}
