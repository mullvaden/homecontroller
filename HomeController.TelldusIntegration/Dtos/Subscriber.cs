namespace HomeController.TelldusIntegration.Dtos
{
    public class Subscriber
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public string Token { get; set; }
        public string TokenSecret { get; set; }
        public int PollingIntervalMinutes { get; set; }
    }
}