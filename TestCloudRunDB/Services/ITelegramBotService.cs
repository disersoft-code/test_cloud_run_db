using Telegram.Bot;

namespace TestCloudRunDB.Services
{
    internal interface ITelegramBotService
    {
        TelegramBotClient Bot { get; }

        Task Run();
    }
}
