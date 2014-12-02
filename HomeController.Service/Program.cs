using System.Configuration;
using Topshelf;

namespace HomeController.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<Poller>(s =>
                {
                    s.ConstructUsing(name => new Poller(new DataAccess(ConfigurationManager.ConnectionStrings["StatsConnectionString"].ConnectionString)));
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                    
                });
                x.RunAsLocalSystem();

                x.SetDescription("Numlock HomeController Telldus Poller");
                x.SetDisplayName("Numlock HomeController Telldus Poller");
                x.SetServiceName("Numlock.HomeController.Telldus.Poller");
                x.StartAutomatically();
                
            });
        }
    }
}
