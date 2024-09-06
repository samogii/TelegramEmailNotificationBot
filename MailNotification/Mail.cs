using System;
using System.Threading;
using System.Threading.Tasks;
using MailKit.Net.Imap;
using MailKit.Search;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace MailNotification
{
    public class Mail
    {

        IConfiguration config = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .Build();
        private readonly TimeSpan _pollInterval = TimeSpan.FromMinutes(1); // Adjust the poll interval as needed
        private readonly TimeSpan _noopInterval = TimeSpan.FromMinutes(9); // NoOp every 9 minutes (adjust based on server timeout)

        public async Task MonitorEmailsAsync()
        {
            while (true)
            {
                try
                {
                    using var client = new ImapClient();
                    await client.ConnectAsync(config["EmailSettings:IMAPServer"], int.Parse(config["EmailSettings:Port"]), true);
                    await client.AuthenticateAsync(config["EmailSettings:Email"], config["EmailSettings:Password"]);

                    var inbox = client.Inbox;
                    await inbox.OpenAsync(MailKit.FolderAccess.ReadWrite);

                    MailKit.UniqueId? lastSeenUid = null;
                    var cancelTokenSource = new CancellationTokenSource();
                    
                    // Start a background task to send NOOP commands periodically
                    var noopTask = Task.Run(async () =>
                    {
                        while (!cancelTokenSource.Token.IsCancellationRequested)
                        {
                            await Task.Delay(_noopInterval);
                            if (!client.IsConnected)
                                break;

                            await client.NoOpAsync(cancelTokenSource.Token);
                        }
                    }, cancelTokenSource.Token);

                    while (true)
                    {
                        try
                        {
                            // Fetch messages that are recent
                            var query = SearchQuery.NotSeen;
                            var uids = await inbox.SearchAsync(query);

                            foreach (var uid in uids)
                            {
                                if (lastSeenUid == null || uid > lastSeenUid)
                                {
                                    var message = await inbox.GetMessageAsync(uid);
                                    Console.WriteLine($"New message from: {message.From} with subject: {message.Subject}");
                                    await SendTelegramNotification($"New message from: {message.From} with subject: {message.Subject}");

                                    // Update last seen UID
                                    lastSeenUid = uid;
                                }
                            }

                            // Wait for a specific interval before checking for new messages again
                            await Task.Delay(_pollInterval); // Poll interval
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"An error occurred while polling: {ex.Message}");
                            // Optionally log or handle polling errors
                        }
                    }

                    // If you ever exit the loop, cancel the NoOp task
                    cancelTokenSource.Cancel();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while connecting: {ex.Message}");
                    // Optionally log the exception and wait before attempting to reconnect
                    await Task.Delay(TimeSpan.FromMinutes(5)); // Wait before reconnecting
                }
            }
        }

        private async Task SendTelegramNotification(string message)
        {
            TelegramNotification tel = new TelegramNotification(config["TelegramBotToken"]);
            await tel.SendNotificationAsync(long.Parse(config["TelegramChatId"]), message);
        }
    }
}
