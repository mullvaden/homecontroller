using System;
using HomeController.TelldusIntegration.Dtos;
using NUnit.Framework;

namespace HomeController.Tests
{
    [TestFixture]
    public class TestApi
    {
        [Test]
        public void name()
        {
            // Arrange
            var date = DateTime.Parse("2015-03-01 02:27:00");
            // Act
            Console.WriteLine(date.ToSince());
            // Assert

        }
    }
}
