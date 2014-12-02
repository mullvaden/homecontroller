using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using EbaySweden.Trading.DatabaseAccess;
using HomeController.TelldusIntegration;
using HomeController.TelldusIntegration.Dtos;

namespace HomeController.Service
{
    public class Poller
    {
        private readonly IDataAccess _dataAccess;
        private readonly string _smtpserver;
        private Timer _mailingTimer;
        private Timer _statsTimer;
        private IEnumerable<Subscriber> _subscribers;

        public Poller(DataAccess dataAccess, string smtpserver)
        {
            _dataAccess = dataAccess;
            _smtpserver = smtpserver;
            InitSubscribers();
            InitTimers();
            _mailingTimer.Elapsed += (sender, e) => FetchAndMail();
            _statsTimer.Elapsed += (sender, e) => StoreStats();
            // Don't wait around or the service might not start fast enough
            Task.Run(() => StoreStats());
            Task.Run(() => FetchAndMail());
        }

        private void InitTimers()
        {
            _mailingTimer = new Timer(TimeSpan.FromHours(48).TotalMilliseconds) { AutoReset = true };
            _statsTimer = new Timer(TimeSpan.FromMinutes(30).TotalMilliseconds) { AutoReset = true };
        }

        private void InitSubscribers()
        {
            _subscribers = _dataAccess.GetSubscribers();
        }

        private void StoreStats()
        {
            foreach (var subscriber in _subscribers)
                _dataAccess.StoreSensors(subscriber.Id, GetSensors(subscriber));
        }

        public void FetchAndMail()
        {
            foreach (var subscriber in _subscribers)
            {
                var sb = GetSensors(subscriber).ToStringy();

                using (var smtpServer = new SmtpClient(_smtpserver, 25))
                {
                    var mail = new MailMessage
                    {
                        From = new MailAddress("tellstick@numlock.se"),
                        Subject = "Sensor update " + DateTime.Now
                    };
                    foreach (var addr in subscriber.Email.Split(';'))
                        mail.To.Add(addr);
                    mail.Body = sb.ToString();
                    smtpServer.Send(mail);
                }
            }
        }

        private static IEnumerable<TemperatureSensor> GetSensors(Subscriber subscriber)
        {
            var teller = new TelldusIntegrator();
            var sensors = teller.GetTemperatureSensors(subscriber);
            return sensors;
        }

        public void Start()
        {
            _mailingTimer.Start();
            _statsTimer.Start();
        }

        public void Stop()
        {
            _mailingTimer.Stop();
            _statsTimer.Stop();
        }
    }
}