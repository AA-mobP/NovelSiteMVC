using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace CLTelegramBot
{
    public class TelegramUtilities
    {
        private static readonly ITelegramBotClient botClient = new TelegramBotClient("7163218073:AAHn-T1YmtEYQRQAGf3vHTl_hhSpb35lee0");
        public static async Task SendTextMessage(long chatId, string message)
        {
            await botClient.SendMessage(
            chatId: chatId,
            text: message,
            parseMode: ParseMode.MarkdownV2  // تحديد أن النص يحتوي على تنسيق Markdown V2
            );
        }
    }
}
