﻿using System;

namespace HomeController.TelldusIntegration.Dtos
{
    public class TempSensor
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime LastUpdated { get; set; }
        public decimal Temperature { get; set; }
    }
}