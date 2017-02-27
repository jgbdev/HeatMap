using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeatMapMonitor_Windows.API
{
    public sealed class Response_Device
    {
        public uint device_id { get; set; }
        public Coordinates coordinates { get; set; }
        public uint refresh_time { get; set; }
        public uint[] hardware_ids { get; set; }
    }
}
