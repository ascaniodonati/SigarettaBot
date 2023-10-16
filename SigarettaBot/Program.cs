using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SigarettaBot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TelegramBotClient botClient = new TelegramBotClient(Settings.API_TOKEN);
            ReceiverOptions receiverOptions = new ReceiverOptions() { AllowedUpdates = { } };
            using CancellationTokenSource cts = new CancellationTokenSource();

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            Console.WriteLine("Ciao, bot!");
            Console.ReadLine();
        }

        private static Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        static async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
        {
            //Se non è un messaggio ignoriamo, per ora
            if (update.Message == null) { return; }

            if (update.Message.Chat.Type != ChatType.Group)
            {
                await client.SendTextMessageAsync(update.Message.Chat.Id, "Ciao!\nAggiungimi a un gruppo per iniziare a giocare 😊");
            }
        }


    }
}