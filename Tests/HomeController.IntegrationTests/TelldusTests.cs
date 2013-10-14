using System;
using HomeController.TelldusIntegration;
using NUnit.Framework;

namespace HomeController.IntegrationTests
{
    [TestFixture]
    public class TelldusTests
    {
        [Test]
        public void HookTelldus()
        {
            var teller = new HookUpTelldus();
            teller.Hookitup();

        }
        [Test]
        public void GetSensors()
        {
            var teller = new HookUpTelldus();
            var sensors = teller.GetSensors();
            Assert.That(sensors, Is.Not.Null);
            Assert.That(sensors.Count, Is.EqualTo(2));
            foreach (var sensor in sensors)
            {
                Console.WriteLine(sensor.Id + " " + sensor.Name+ " " + sensor.Temperature + " " + sensor.LastUpdated);
            }

        }

    }
}
