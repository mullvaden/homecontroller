using System;

namespace HomeController.TelldusIntegration.Dtos
{
    public class TemperatureSensor
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime LastUpdated { get; set; }
        public decimal Temperature { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}", Id, Name, Temperature, LastUpdated);
        }

        
    }
}