using System.Collections.Concurrent;
using Service.Passports;
using Service.Users;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot;

public class TelegramBot(PassportsRepository passports, UsersRepository users, ITelegramBotClient botClient)
{
    readonly private static ConcurrentDictionary<string, string> SendButtonUser = [];
    readonly private string[] _emojis = ["🥳", "🔥", "🌈", "🥂", "🎂", "🏆", "🎊", "🎉", "❤️‍🔥"];
    readonly private Random _random = new();
    readonly private static PassportsRepository passports;

    public async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.CallbackQuery)
        {
            var data = update.CallbackQuery!.Data;
            if (await passports.CheckPassport(data))
            {
                await botClient.SendChatActionAsync(update.CallbackQuery.Message!.Chat.Id, ChatAction.Typing,
                    cancellationToken: cancellationToken);
                var userId = update.CallbackQuery.From.Id;
                var passport = await passports.GetPassport(data);
                passport!.AuthenticatedTelegramTag = $"@{update.CallbackQuery.From.Username}";
                await passports.UpdatePassport(passport);
                await botClient.SendTextMessageAsync(userId, _emojis[_random.Next(0, _emojis.Length)],
                    cancellationToken: cancellationToken);
                await botClient.EditMessageReplyMarkupAsync(update.CallbackQuery.Message!.Chat.Id, update.CallbackQuery.Message.MessageId,
                    null, cancellationToken);
                SendButtonUser.Remove(userId.ToString(), out _);
            }
        }

        if (update.Type != UpdateType.Message)
            return;
        var message = update.Message;
        try
        {
            if (message!.Text is not null)
            {
                var userTag = $"@{message.Chat.Username!}";
                var userId = message.Chat.Id.ToString();
                switch (message.Text)
                {
                    case "/start":
                    {
                        await botClient.SendChatActionAsync(userId, ChatAction.Typing, cancellationToken: cancellationToken);
                        await users.AddConnectId(userTag, userId);
                        await botClient.SendTextMessageAsync(userId, "Нажмите кнопку проверить личность на сайте",
                            cancellationToken: cancellationToken);
                    }
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public static Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException =>
                $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };
        return Task.CompletedTask;
    }

    public async Task SendButton(string telegramId, string sessionId)
    {
        if (!SendButtonUser.ContainsKey(telegramId))
        {
            await botClient.SendChatActionAsync(telegramId, ChatAction.Typing);
            var button = new InlineKeyboardButton("Подтвердить")
            {
                CallbackData = sessionId
            };
            var keyboard = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    button
                }
            });
            await botClient.SendTextMessageAsync(int.Parse(telegramId), "🤨", replyMarkup: keyboard);
            SendButtonUser[telegramId] = telegramId;
        }
    }
}