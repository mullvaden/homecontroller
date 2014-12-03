using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using EbaySweden.Trading.DatabaseAccess;
using HomeController.TelldusIntegration.Dtos;

namespace HomeController.Service
{
    public interface IDataAccess
    {
        void StoreSensors(int subscriptionId, IEnumerable<TemperatureSensor> sensors);
        IEnumerable<Subscriber> GetSubscribers();
        List<TemperatureSensor> GetTemperatureSensorsSince(int subscriptionId, int daysBack = 7);
    }

    public class DataAccess : IDataAccess
    {
        private DbAccessor _db;

        public DataAccess(string connectionString)
        {
            _db = new DbAccessor(connectionString);
        }

        public void StoreSensors(int subscriptionId, IEnumerable<TemperatureSensor> sensors)
        {
            foreach (var sensor in sensors)
            {
                var parameters = new SqlParameterHandler(p =>
                {
                    p.AddWithValue("@subscriptionId", subscriptionId);
                    p.AddWithValue("@sensorId", sensor.Id);
                    p.AddWithValue("@name", sensor.Name);
                    p.AddWithValue("@Temperature", sensor.Temperature);
                    p.AddWithValue("@LastUpdated", sensor.LastUpdated);
                });
                _db.PerformSpNonQuery("StoreSensor", parameters);
            }

        }

        public IEnumerable<Subscriber> GetSubscribers()
        {
            var reader = new ParametersAndReader<Subscriber>
            {
                RecordReader = r => new Subscriber
                {
                    Id = r.GetDefault<int>("Id"),
                    Name = r.GetDefault<string>("Name"),
                    Email = r.GetDefault<string>("Email"),
                    PublicKey = r.GetDefault<string>("PublicKey"),
                    PrivateKey = r.GetDefault<string>("PrivateKey"),
                    Token = r.GetDefault<string>("Token"),
                    TokenSecret = r.GetDefault<string>("TokenSecret"),
                    PollingIntervalMinutes = r.GetDefault<int>("PollingIntervalMinutes"),
                }
            };

            return _db.PerformSpRead(reader, "GetSubscribers");
        }

        public List<TemperatureSensor> GetTemperatureSensorsSince(int subscriptionId, int daysBack = 7)
        {
           var reader = new ParametersAndReader<TemperatureSensor>
            {
                Parameters = p =>
                {
                    p.AddWithValue("@subscriptionId", subscriptionId);
                    p.AddWithValue("@daysBack", daysBack);
                },
                RecordReader = r => new TemperatureSensor
                {
                    Id = r.GetDefault<string>("SensorId"),
                    Name = r.GetDefault<string>("Name"),
                    Temperature = r.GetDefault<decimal>("Temperature"),
                    LastUpdated= r.GetDefault<DateTime>("LastUpdated"),
                    
                }
            };

            return _db.PerformSpRead(reader, "GetSensorsSince");  
        }
    }
}
