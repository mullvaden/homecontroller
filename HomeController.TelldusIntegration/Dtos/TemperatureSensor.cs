﻿using System;

namespace HomeController.TelldusIntegration.Dtos
{
    public class TemperatureSensor
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime LastUpdated { get; set; }
        public decimal Temperature { get; set; }
        public string Image { get; set; }
        public override string ToString()
        {
            return string.Format("{0} {1} °C {2} ", Name, Temperature, LastUpdated.ToSince());
        }

        
    }
}