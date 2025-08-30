using Microsoft.Extensions.DependencyInjection;
using NovelSiteMVC.BussinessLogic;
using NovelSiteMVC.Models;
using Polly;
using System.Net;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CLTelegramBot
{
    public class TelegramUtilities
    {
        private static readonly ITelegramBotClient botClient = new TelegramBotClient("7163218073:AAHn-T1YmtEYQRQAGf3vHTl_hhSpb35lee0");
        public static void ConfigBot()
        {
            //configure telegram bot
            ReceiverOptions receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new UpdateType[]
                {
                    UpdateType.Message,
                    UpdateType.EditedMessage,
                    UpdateType.CallbackQuery,
                    UpdateType.InlineQuery,
                    UpdateType.ChosenInlineResult
                },
                DropPendingUpdates = false,
            };

            CancellationTokenSource cts = new CancellationTokenSource();

            //add listener
            botClient.StartReceiving(UpdateHandlerAsync, ErrorHandlerAsync, receiverOptions, cancellationToken: cts.Token);
        }

        public static async Task SendTextMessage(long chatId, string message)
        {
            await botClient.SendMessage(
            chatId: chatId,
            text: message,
            parseMode: ParseMode.MarkdownV2  // تحديد أن النص يحتوي على تنسيق Markdown V2
            );
            ONUtility.LogNotificationSent();
        }

        internal static async Task ErrorHandlerAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            await botClient.SendMessage(ONUtility.MeId, $"دالة ErrorHandlerAsync قد دُخلت، والخطأ هو:\n{exception.Message}");
        }

        internal static async Task UpdateHandlerAsync(ITelegramBotClient client, Update update, CancellationToken token)
        {
            if (update.Type == UpdateType.Message)
            {
                if (update?.Message?.Type == MessageType.Text && (update.Message.Chat.Id == ONUtility.MeenaId || update.Message.Chat.Id == ONUtility.MeId))
                {
                    if (update.Message.Text is null)
                        return;
                    
                    AppDbContext dbcontext = new AppDbContext();

                    if (update.Message.Text.StartsWith("/start"))
                    {
                        IONUtility on = new ONUtility(dbcontext);
                        await on.SendDailyTaskList();
                    }
                }
            }
        }
    }
}
