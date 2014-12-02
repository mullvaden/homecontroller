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
        private readonly Timer _mailingTimer;
        private readonly Timer _statsTimer;
        private IEnumerable<Subscriber> _subscribers;

        public Poller(DataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            InitSubscribers();
            _mailingTimer = new Timer(1000 * 60 * 30) { AutoReset = true };
            _statsTimer = new Timer(1000) { AutoReset = true };
            //_mailingTimer.Elapsed += MailingTimerElapsed;
            _statsTimer.Elapsed += StatsTimerElapsed;

        }

        private void InitSubscribers()
        {
            _subscribers = _dataAccess.GetSubscribers();
        }

        private void StatsTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Task.Run(() => StoreStats());
            _statsTimer.Stop();
        }

        private void StoreStats()
        {
            foreach (var subscriber in _subscribers)
                _dataAccess.StoreSensors(subscriber.Id, GetSensors(subscriber));
        }

        private void MailingTimerElapsed(object sender, ElapsedEventArgs e)
        {
            FetchAndSend();
        }

        public void FetchAndSend()
        {
            foreach (var subscriber in _subscribers)
            {
                var sb = GetSensors(subscriber).ToStringy();

                using (var smtpServer = new SmtpClient("test07", 25))
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