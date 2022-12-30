using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TestCloudRunDB.Services
{
    internal class TelegramBotService : ITelegramBotService
    {

        public TelegramBotClient Bot { get; private set; }

        private readonly ILogger<TelegramBotService> _log;
        private readonly IConfiguration _config;

        public TelegramBotService(ILogger<TelegramBotService> log, IConfiguration config)
        {
            _log = log;
            _config = config;

            Bot = new TelegramBotClient(_config.GetValue<string>("TelegramBotToken"));

        }

        public async Task Run()
        {
            try
            {
                _log.LogInformation("init telegram bot...");

                using var cts = new CancellationTokenSource();

                // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
                var receiverOptions = new ReceiverOptions
                {
                    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
                };
                Bot.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, cancellationToken: cts.Token);


                var me = await Bot.GetMeAsync();

                _log.LogDebug("Start listening for {Username}", me.Username);

                // Send cancellation request to stop bot
                //cts.Cancel();

            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error Exception");
            }
        }

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Message is not { } message)
                return;
            // Only process text messages
            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;

            _log.LogDebug("Received a '{messageText}' message in chat {chatId}.", messageText, chatId);

            // Echo received message text
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "You said:\n" + messageText,
                cancellationToken: cancellationToken);
        }

        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _log.LogError("Error bot telegram:{err}", ErrorMessage);
            return Task.CompletedTask;
        }

    }
}
