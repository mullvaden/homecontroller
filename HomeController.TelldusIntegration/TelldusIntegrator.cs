using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HomeController.TelldusIntegration.Dtos;
using HomeController.TelldusIntegration.Dtos.TelldusDtos;
using RestSharp;
using RestSharp.Authenticators;

namespace HomeController.TelldusIntegration
{
    public class TelldusIntegrator
    {
        private const string TelldusBaseUrl = "https://api.telldus.com/json";
        private string _publickey = "FEHUVEW84RAFR5SP22RABURUPHAFRUNU";
        private string _privateKey = "ZUXEVEGA9USTAZEWRETHAQUBUR69U6EF";
        private string _token = "408b287b4c4cca887c20f4bf6179a3b70525acb9f";
        private string _tokenSecret = "b6f0efcf16e880a962dcb16ccf5b06f5";

        public List<TemperatureSensor> GetTemperatureSensors()
        {
            var client = new RestClient(TelldusBaseUrl);
            client.Authenticator = OAuth1Authenticator.ForProtectedResource(_publickey, _privateKey, _token, _tokenSecret);
            var request = new RestRequest("sensors/list");
            var response = client.Execute<SensorList>(request);
            var tempSensors = new List<TemperatureSensor>();
            foreach (var sensor in response.Data.sensor)
            {
                request = new RestRequest("sensor/info");
                request.Parameters.Add(new Parameter { Name = "id", Value = sensor.id, Type = ParameterType.GetOrPost });
                var sensorresponse = client.Execute<SensorDetail>(request);
                var sensorDetail = sensorresponse.Data;
                if (sensorDetail == null)
                    continue;
                var temp = sensorDetail.data.FirstOrDefault(d => d.name == "temp");
                if (temp != null)
                {
                    var numberFormatInfo = new NumberFormatInfo { NumberDecimalSeparator = "." };
                    tempSensors.Add(new TemperatureSensor { Id = sensor.id, Name = sensor.name, LastUpdated = sensor.LastUpdated, Temperature = decimal.Parse(temp.value, numberFormatInfo) });
                }
            }

            return tempSensors;
            //return response.Data.sensor;

        }
    }
}
