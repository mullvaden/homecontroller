using System.Collections.Generic;
using HomeController.TelldusIntegration.Dtos;
using RestSharp;
using RestSharp.Authenticators;

namespace HomeController.TelldusIntegration
{
    public class HookUpTelldus
    {
        private const string TelldusBaseUrl = "https://api.telldus.com/json";
        private string _publickey = "FEHUVEW84RAFR5SP22RABURUPHAFRUNU";
        private string _privateKey = "ZUXEVEGA9USTAZEWRETHAQUBUR69U6EF";
        private string _token = "408b287b4c4cca887c20f4bf6179a3b70525acb9f";
        private string _tokenSecret = "b6f0efcf16e880a962dcb16ccf5b06f5";

        public void Hookitup()
        {
            var client = new RestClient(TelldusBaseUrl);
            client.Authenticator = OAuth1Authenticator.ForProtectedResource(_publickey, _privateKey, _token, _tokenSecret);
            var request = new RestRequest("devices/list");
            var response = client.Execute(request);
            //client.ExecuteAsync(request, response =>
            //{
            //    TakeResponse();
            //});
        }

        public List<Sensor> GetSensors()
        {
            var client = new RestClient(TelldusBaseUrl);
            client.Authenticator = OAuth1Authenticator.ForProtectedResource(_publickey, _privateKey, _token, _tokenSecret);
            var request = new RestRequest("sensors/list");
            var response = client.Execute<RootObject>(request);
            return response.Data.sensor;
            
        }
    }
}
