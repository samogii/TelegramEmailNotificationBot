using Microsoft.Extensions.Configuration;

namespace MailNotification
{
     class Program
    {
        static async Task Main(string[] args)
        {

            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration config = builder.Build();
            // Access the IMAPServer value
            var emailListener = new Mail();
            await emailListener.MonitorEmailsAsync();
        }
    }
}
