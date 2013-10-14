using System;
using System.Collections.Generic;

namespace HomeController.TelldusIntegration.Dtos.TelldusDtos
{
    public class Sensor
    {
        public string id { get; set; }
        public string name { get; set; }
        public int lastUpdated { get; set; }
        public int ignored { get; set; }
        public string client { get; set; }
        public string clientName { get; set; }
        public string online { get; set; }
        public int editable { get; set; }
        public DateTime LastUpdated { get { return new DateTime(1970, 1, 1).AddSeconds(lastUpdated); } }
    }

    public class SensorList
    {
        public List<Sensor> sensor { get; set; }
    }

    public class SensorData
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class SensorDetail
    {
        public string id { get; set; }
        public string clientName { get; set; }
        public string name { get; set; }
        public int lastUpdated { get; set; }
        public int ignored { get; set; }
        public int editable { get; set; }
        public List<SensorData> data { get; set; }
        public string protocol { get; set; }
        public string sensorId { get; set; }
        public int timezoneoffset { get; set; }
    }
}