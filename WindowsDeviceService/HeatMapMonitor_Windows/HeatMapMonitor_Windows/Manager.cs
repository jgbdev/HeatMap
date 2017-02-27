using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HeatMapMonitor_Windows
{
    public class Manager
    {
        const string DefaultAPIBaseURL = "http://34.251.68.107:5000/api/";
        const string DefaultAPIAccountId = "";
        const string DefaultAPISecretKey = "";

        string ConfigPath;

        Hardware hardware;

        public bool Init()
        {
            bool OK = true;

            ConfigPath = Path.Combine(Environment.CurrentDirectory, "config.txt");

            Config config;
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
            }
            catch (FileNotFoundException)
            {
                config = new Config(DefaultAPIBaseURL, DefaultAPIAccountId, DefaultAPISecretKey);
            }

            API.HeatMapAPI api = new API.HeatMapAPI(config);

            if (!config.HasDeviceId)
            {
                uint IdResponse = api.GetId();
                config.UpdateDeviceId(IdResponse);
                config.Save(ConfigPath);
            }

            Logger.Log("Device id: " + config.DeviceId);

            hardware = new Hardware();

            return OK;
        }

        public bool Update()
        {
            Logger.Log("Updating...");

            Dictionary<string, HardwareInfo> HardwareInfos = hardware.GetHardwareInfos();
            
            return false;
        }

        public bool Shutdown()
        {
            //TODO
            return false;
        }
    }
}
