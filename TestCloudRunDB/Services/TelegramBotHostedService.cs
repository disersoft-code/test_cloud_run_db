namespace TestCloudRunDB.Services
{
    internal class TelegramBotHostedService : IHostedService
    {
        private readonly ITelegramBotService _telegramBot;

        public TelegramBotHostedService(ITelegramBotService telegramBot)
        {
            _telegramBot = telegramBot;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _telegramBot.Run();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
