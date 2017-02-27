using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;

namespace HeatMapMonitor_Windows.API
{
    public sealed class HeatMapAPI
    {
        private readonly Config config;

        public HeatMapAPI(Config _config)
        {
            config = _config;
        }

        public T Execute<T>(RestRequest request) where T : new()
        {
            if (string.IsNullOrWhiteSpace(config.APIBaseURL))
            {
                Logger.Log("API URL not configured - skipping API request.");
                return default(T);
            }

            var client = new RestClient();
            client.BaseUrl = new System.Uri(config.APIBaseURL);
            if (!string.IsNullOrWhiteSpace(config.APIAccountId))
            {
                client.Authenticator = new HttpBasicAuthenticator(config.APIAccountId, config.APIAccountSecretKey);
                //TODO: Do we need this? request.AddParameter("AccountSid", config.APIAccountId, ParameterType.UrlSegment); // used on every request
            }
            else
            {
                client.Authenticator = null;
            }
            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var apiException = new ApplicationException(message, response.ErrorException);
                Logger.Log(apiException);
                throw apiException;
            }
            return response.Data;
        }

        public string GetId()
        {
            var request = new RestRequest();
            request.Resource = "device";
            request.Method = Method.POST;
            request.AddJsonBody(new object());
            
            return (Execute<Response_Id>(request) ?? new Response_Id()).device_id;
        }
        public uint GetInterval(string DeviceId)
        {
            var request = new RestRequest();
            request.Resource = "device/{id}";
            request.AddParameter("id", DeviceId);
            request.Method = Method.GET;

            Response_Device resp = (Execute<Response_Device>(request) ?? new Response_Device());
            return resp.refresh_time;
        }

        public void SendReading(string DeviceId, Dictionary<string, HardwareInfo> HardwareInfos)
        {
            Request_SendReading requestData = new Request_SendReading();
            foreach (string HWId in HardwareInfos.Keys)
            {
                HardwareInfo HWInfo = HardwareInfos[HWId];
                ReadingData hwdata = new ReadingData()
                {
                    hardware_id = HWId
                };

                foreach (string SNId in HWInfo.Sensors.Keys)
                {
                    foreach (KeyValuePair<string, float> value in HWInfo.Sensors[SNId].Values)
                    {
                        hwdata.sensor_info.Add(new SensorData()
                        {
                            tag = SNId + "_" + value.Key,
                            value = value.Value
                        });
                    }
                }

                if (hwdata.sensor_info.Count > 0)
                {
                    requestData.data.Add(hwdata);
                }
            }

            var request = new RestRequest();
            request.Resource = "reading/{id}";
            request.AddParameter("id", DeviceId);
            request.Method = Method.POST;
            request.AddJsonBody(requestData);

            Execute<Object>(request);
        }
    }
}
