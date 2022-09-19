namespace CityInfo.Api.Services
{
    public class LocalMailService : IMailService
    {
        private readonly string _mailTo = String.Empty;
        private readonly string _mailFrom = String.Empty;
        public LocalMailService(IConfiguration configuration)
        {
            _mailTo = configuration["mailSettings:mailToAdress"];
            _mailFrom = configuration["mailSettings:mailFromAdress"];
        }

        public void Send(string subject, string message)
        {
            Console.WriteLine($"Mail was sent from {_mailFrom}\nTo {_mailTo}\nwith the {nameof(LocalMailService)}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"{message}");
        }
    }
}
