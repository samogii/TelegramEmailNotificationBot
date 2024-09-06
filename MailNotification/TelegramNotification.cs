using Telegram.Bot;
namespace MailNotification
{
  public class TelegramNotification
  {
    private readonly string _botToken;
    private readonly TelegramBotClient _botClient;

    public TelegramNotification(string botToken)
    {
      _botToken = botToken;
      _botClient = new TelegramBotClient(_botToken);
    }

    /// <summary>
    /// Sends a notification message to a specified chat in Telegram.
    /// </summary>
    /// <param name="chatId">The chat ID to send the message to.</param>
    /// <param name="message">The message content to send.</param>
    public async Task SendNotificationAsync(long chatId, string message)
    {
      await _botClient.SendTextMessageAsync(chatId, message);
    }
  }
}
