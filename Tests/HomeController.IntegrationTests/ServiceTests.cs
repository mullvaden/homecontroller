﻿using HomeController.Service;
using NUnit.Framework;

namespace HomeController.IntegrationTests
{
    [TestFixture]
    public class ServiceTests
    {
        [Test]
        public void test_service()
        {
            // Arrange
            var p = new Poller(new DataAccess(""));
            // Act
            p.FetchAndSend();
            // Assert

        }
    }
}