using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using EbaySweden.Trading.DatabaseAccess;
using GoogleChartSharp;
using HomeController.TelldusIntegration;
using HomeController.TelldusIntegration.Dtos;
using RazorEngine;

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
                _dataAccess.StoreSensors(subscriber.Id, GetTempSensors(subscriber));
        }

        public void FetchAndMail()
        {
            foreach (var subscriber in _subscribers)
            {
                var sensors = GetTempSensors(subscriber);
                var sb = sensors.ToStringy();
                //var body = Razor.Parse(Resources.SensorTemplate, new { Period = DateTime.Now.ToLongDateString(), Sensors = sensors }, "Stats");

                using (var smtpServer = new SmtpClient(_smtpserver, 25))
                {
                    var mail = new MailMessage
                    {
                        From = new MailAddress("tellstick@numlock.se"),
                        Subject = "Sensor update " + DateTime.Now,
                        //Body = body,
                        Body = sb.ToString()
                        //IsBodyHtml = true
                    };
                    foreach (var addr in subscriber.Email.Split(';'))
                        mail.To.Add(addr);

                    smtpServer.Send(mail);
                }
                //if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                //    MailStats(subscriber);
            }
        }

        private void MailStats(Subscriber subscriber)
        {
            var daysBack = 7;

            var datalist = _dataAccess.GetTemperatureSensorsSince(subscriber.Id, daysBack: daysBack);

            var sensors = datalist.GroupBy(c => c.Name).Select(t => new SensorViewModel { Name = t.Key, Image = "" });

            var imageUrl = MakeStatDiagram(sensors, datalist);

            var body = Razor.Parse(Resources.StatsTemplate, new { Period = "7 dagar", Sensors = sensors }, "Stats");

            using (var smtpServer = new SmtpClient(_smtpserver, 25))
            {
                var mail = new MailMessage
                {
                    From = new MailAddress("tellstick@numlock.se"),
                    Subject = "Sensor stats " + DateTime.Now,
                    IsBodyHtml = true,
                    Body = body,
                };
                foreach (var addr in subscriber.Email.Split(';'))
                    mail.To.Add(addr);

                smtpServer.Send(mail);
            }

            // massage to 
        }

        private string MakeStatDiagram(IEnumerable<SensorViewModel> sensors, List<TemperatureSensor> datalist)
        {
            var dataset = new List<decimal[]>();

            foreach (var sensor in sensors)
            {
                dataset.Add(datalist.Where(t => t.Name == sensor.Name).Select(u => u.Temperature).ToArray());
                dataset.Add(datalist.Where(t => t.Name == sensor.Name).Select(u => (decimal)u.LastUpdated.Ticks).ToArray());
            }
            var lineChart = new LineChart(250, 150, LineChartType.MultiDataSet);
            lineChart.SetTitle("Bla bla Per Line", "0000FF", 14);
            lineChart.SetData(dataset);
            lineChart.AddAxis(new ChartAxis(ChartAxisType.Bottom));
            lineChart.AddAxis(new ChartAxis(ChartAxisType.Left));

            return lineChart.GetUrl();
        }

        private static IEnumerable<TemperatureSensor> GetTempSensors(Subscriber subscriber)
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

    public class SensorViewModel
    {
        public string Name { get; set; }

        public string Image { get; set; }
    }
}