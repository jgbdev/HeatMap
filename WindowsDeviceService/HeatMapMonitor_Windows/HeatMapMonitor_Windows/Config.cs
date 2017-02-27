using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HeatMapMonitor_Windows
{
    public class Config
    {
        public string APIBaseURL
        {
            get; private set;
        }
        public string APIAccountId
        {
            get; private set;
        }
        public string APIAccountSecretKey
        {
            get; private set;
        }
        public string DeviceId
        {
            get; private set;
        }

        public uint UpdatePeriod
        {
            get;
            set;
        }

        public bool HasDeviceId
        {
            get
            {
                return !string.IsNullOrWhiteSpace(DeviceId);
            }
        }

        public bool RequiresSave
        {
            get; private set;
        }

        public static bool MuteLog
        {
            get;
            private set;
        }

        public Config(string _APIBaseURL, string _APIAccountId, string _APISecretKey)
        {
            APIBaseURL = _APIBaseURL;
            APIAccountId = _APIAccountId;
            APIAccountSecretKey = _APISecretKey;
            DeviceId = null;
            RequiresSave = false;
        }

        public Config(string path)
        {
            Load(path);
            RequiresSave = false;
        }

        public void Load(string path)
        {
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);

                if (lines.Length >= 4)
                {
                    // APIBaseURL
                    // APIAccountId
                    // APISecretKey
                    // DeviceId

                    APIBaseURL = lines[0];
                    APIAccountId = lines[1];
                    APIAccountSecretKey = lines[2];
                    DeviceId = lines[3];

                    if (lines.Length >= 5)
                    {
                        MuteLog = bool.Parse(lines[4]);
                    }
                }
                else
                {
                    throw new IndexOutOfRangeException("Config file doesn't have enough lines!");
                }
            }
            else
            {
                throw new FileNotFoundException("Config file does not exist yet!");
            }
        }
        public void Save(string path)
        {
            if (RequiresSave)
            {
                StringBuilder configText = new StringBuilder();
                configText.AppendLine(APIBaseURL ?? "");
                configText.AppendLine(APIAccountId ?? "");
                configText.AppendLine(APIAccountSecretKey ?? "");
                configText.AppendLine(DeviceId ?? "");
                configText.AppendLine(MuteLog.ToString());
                File.WriteAllText(path, configText.ToString());
            }
        }

        public void UpdateDeviceId(string value)
        {
            DeviceId = value;
            RequiresSave = true;
        }
    }
}
