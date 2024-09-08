using Microsoft.EntityFrameworkCore;
using Service.ApplicationDbContext;
using Service.Passports;
using Service.Users;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace TelegramBot;

public class Program
{
    public async static Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        await StartBotAsync(host);
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(context.Configuration.GetConnectionString("DefaultConnection")));

                services.AddSingleton<ITelegramBotClient>(options => 
                    
                    new TelegramBotClient(context.Configuration.GetSection("TelegramSecrets")["Token"]!));

                services.AddScoped<PassportsRepository>();
                services.AddScoped<UsersRepository>();
                services.AddScoped<TelegramBot>();
            });
    
    private async static Task StartBotAsync(IHost host)
    {
        var telegramBot = host.Services.CreateScope().ServiceProvider.GetRequiredService<TelegramBot>();
        var botClient = host.Services.GetRequiredService<ITelegramBotClient>();
        var logger = host.Services.GetRequiredService<ILogger<Program>>();
        
        botClient.StartReceiving(new DefaultUpdateHandler(telegramBot.HandleUpdateAsync, 
            TelegramBot.HandleErrorAsync));
        logger.LogInformation("Telegram bot started.");
        
        await Task.Delay(Timeout.Infinite);
    }
}