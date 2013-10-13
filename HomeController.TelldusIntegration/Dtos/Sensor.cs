using System;
using System.Collections.Generic;

namespace HomeController.TelldusIntegration.Dtos
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

    public class RootObject
    {
        public List<Sensor> sensor { get; set; }
    }
}