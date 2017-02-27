using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenHardwareMonitor;
using OpenHardwareMonitor.Hardware;

namespace HeatMapMonitor_Windows
{
    public class Hardware
    {
        Computer computer;
        HardwareVisitor visitor;

        public Hardware()
        {
            try
            {
                computer = new Computer();
                computer.Open();

                computer.HardwareAdded += Computer_HardwareAdded;
                computer.HardwareRemoved += Computer_HardwareRemoved;

                computer.MainboardEnabled = true;
                computer.CPUEnabled = true;
                computer.GPUEnabled = true;
                computer.HDDEnabled = true;
                computer.RAMEnabled = true;
            }
            catch (Exception ex)
            {
                computer = null;

                Logger.Log(ex);
                throw;
            }

            visitor = new HardwareVisitor();
        }

        private void Computer_HardwareAdded(IHardware hardware)
        {
            Logger.Log("Hardware added: " + hardware.Identifier);
            //TODO?
        }
        private void Computer_HardwareRemoved(IHardware hardware)
        {
            Logger.Log("Hardware removed: " + hardware.Identifier);
            //TODO?
        }

        ~Hardware()
        {
            if (computer != null)
            {
                computer.Close();
                computer = null;
            }
        }

        public Dictionary<string, HardwareInfo> GetHardwareInfos()
        {
            Dictionary<string, HardwareInfo> result = new Dictionary<string, HardwareInfo>();

            visitor.Init(result);
            computer.Traverse(visitor);

            return result;
        }

        public class HardwareVisitor : IVisitor
        {
            private Dictionary<string, HardwareInfo> Result;
            public void Init(Dictionary<string, HardwareInfo> result)
            {
                Result = result;
            }

            public void VisitComputer(IComputer computer)
            {
                computer.Traverse(this);
            }

            public void VisitHardware(IHardware hardware)
            {
                string id = hardware.Identifier.ToString();
                Logger.Log("Adding hardware: " + id);
                Result.Add(id, new HardwareInfo(id));

                hardware.Update();

                System.Threading.Thread.Sleep(1000);

                hardware.Update();

                hardware.Traverse(this);
            }

            public void VisitParameter(IParameter parameter)
            {
                //Do nothing
            }

            public void VisitSensor(ISensor sensor)
            {
                string hwid = sensor.Hardware.Identifier.ToString();
                
                HardwareInfo hwinfo = Result[hwid];
                SensorInfo sninfo = hwinfo.AddSensor(sensor.Name);
                switch (sensor.SensorType)
                {
                    case SensorType.Load:
                        AddSensorValues("load", sensor, sninfo);
                        break;
                    case SensorType.Temperature:
                        AddSensorValues("temperature", sensor, sninfo);
                        break;
                }
            }

            private static void AddSensorValues(string Tag, ISensor sensor, SensorInfo sninfo)
            {
                if (sensor.Value.HasValue)
                {
                    sninfo.AddValue(Tag, sensor.Value.Value);
                }
                else if (sensor.Values != null && sensor.Values.Count() > 0)
                {
                    float val_tot = 0.0f;
                    foreach (SensorValue val in sensor.Values)
                    {
                        val_tot += val.Value;
                    }
                    val_tot /= sensor.Values.Count();

                    sninfo.AddValue(Tag + "_multiple_avg", val_tot);
                }
            }
        }
    }
}
