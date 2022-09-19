namespace CityInfo.Api.Services
{
    public class CloudMailService : IMailService
    {
        private readonly string _mailTo = String.Empty;
        private readonly string _mailFrom = String.Empty;
        public CloudMailService(IConfiguration configuration)
        {
            _mailTo = configuration["mailSettings:mailToAdress"];
            _mailFrom = configuration["mailSettings:mailFromAdress"];
        }
        public void Send(string subject, string message)
        {
            Console.WriteLine($"Mail was sent from {_mailFrom}\nTo {_mailTo}\nwith the {nameof(CloudMailService)}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"{message}");
        }
    }
}
