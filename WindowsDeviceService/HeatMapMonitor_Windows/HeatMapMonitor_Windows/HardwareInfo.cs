using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeatMapMonitor_Windows
{
    public class HardwareInfo
    {
        public readonly string Name;
        public Dictionary<string, SensorInfo> Sensors = new Dictionary<string, SensorInfo>();

        public HardwareInfo(string _Name)
        {
            Name = _Name;
        }

        public void ClearSensors()
        {
            Sensors.Clear();
        }
        public SensorInfo AddSensor(string Name)
        {
            if(Sensors.ContainsKey(Name))
            {
                return Sensors[Name];
            }

            Logger.Log("Adding sensor: " + Name);

            SensorInfo NewInfo = new SensorInfo(Name);
            Sensors.Add(Name, NewInfo);
            return NewInfo;
        }
    }
}
