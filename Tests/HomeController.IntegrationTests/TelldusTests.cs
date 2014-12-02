using System;
using HomeController.TelldusIntegration;
using HomeController.TelldusIntegration.Dtos;
using NUnit.Framework;

namespace HomeController.IntegrationTests
{
    [TestFixture]
    public class TelldusTests
    {

        [Test]
        public void GetSensors()
        {
            var teller = new TelldusIntegrator();
            var sensors = teller.GetTemperatureSensors(new Subscriber());
            Assert.That(sensors, Is.Not.Null);
            Assert.That(sensors.Count, Is.EqualTo(4));
            foreach (var sensor in sensors)
            {
                Console.WriteLine(sensor.Id + " " + sensor.Name+ " " + sensor.Temperature + " " + sensor.LastUpdated);
            }

        }

    }
}
