using System;
using System.Net.Mail;
using System.Text;
using System.Timers;
using HomeController.TelldusIntegration;

namespace HomeController.Service
{
    public class Poller
    {
        private readonly Timer _timer;

        public Poller()
        {
            //_timer = new Timer(1000 * 60 * 30) { AutoReset = true };
            _timer = new Timer(1000) { AutoReset = true };
            _timer.Elapsed += _timer_Elapsed;

        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            FetchAndSend();
            _timer.Stop();
        }

        public void FetchAndSend()
        {
            var teller = new TelldusIntegrator();
            var sensors = teller.GetTemperatureSensors();
            var sb = new StringBuilder();
            foreach (var sensor in sensors)
            {
                sb.AppendLine(sensor.Id + " " + sensor.Name + " " + sensor.Temperature + " " + sensor.LastUpdated);
            }
            using (var smtpServer = new SmtpClient("test07", 25))
            {
                var mail = new MailMessage();
                mail.From = new MailAddress("tellstick@numlock.se");
                mail.Subject = "Sensor update " + DateTime.Now;
                mail.To.Add("dbrunteson@ebay.com");
                mail.Body = sb.ToString();
                smtpServer.Send(mail);
            }
        }

        public void Start() { _timer.Start(); }
        public void Stop() { _timer.Stop(); }
    }
}