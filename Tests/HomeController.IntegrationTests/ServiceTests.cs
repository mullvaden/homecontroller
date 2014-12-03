using HomeController.Service;
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
            var p = new Poller(new DataAccess("Data Source=.;Initial Catalog=HomeController;Integrated Security=SSPI;"), "test07");
            // Act
            p.FetchAndMail();
            // Assert

        }
    }
}