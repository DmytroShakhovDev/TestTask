namespace TestProject.WebAPI.Helpers
{
    public class RabbitMQ
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
    }
    public class Credentials
    {
        public string UserName { get; set; }
        public string  Password { get; set; }
        public string Secret { get; set; }
    }
}