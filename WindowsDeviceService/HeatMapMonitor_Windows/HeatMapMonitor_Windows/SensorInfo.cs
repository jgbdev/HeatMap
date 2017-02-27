using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeatMapMonitor_Windows
{
    public class SensorInfo
    {
        public readonly string Name;
        public Dictionary<string, float> Values = new Dictionary<string, float>();

        public SensorInfo(string _Name)
        {
            Name = _Name;
        }
        public void ClearValues()
        {
            Values.Clear();
        }
        public SensorInfo AddValue(string Name, float Value)
        {
            if (Values.ContainsKey(Name))
            {
                Values[Name] = Value;
                return this;
            }

            Logger.Log("Adding value: " + Name + "=" + Value);

            Values.Add(Name, Value);
            return this;
        }
    }
}
