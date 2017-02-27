using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeatMapMonitor_Windows.API
{
    public class Request_SendReading
    {
        public List<ReadingData> data = new List<ReadingData>();
    }

    public class ReadingData
    {
        public string hardware_id;
        public List<SensorData> sensor_info = new List<SensorData>();
    }

    public class SensorData
    {
        public string tag;
        public float value;
    }
}
