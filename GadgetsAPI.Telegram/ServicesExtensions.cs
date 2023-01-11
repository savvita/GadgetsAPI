using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace GadgetsAPI.Tg
{
    public static class ServicesExtensions
    {
        public static void AddTelegram(this IServiceCollection services, string token)
        {
            services.AddSingleton<TelegramBotClient>(services =>
            {
                return new TelegramBotClient(token);
            });
        }
    }
}
