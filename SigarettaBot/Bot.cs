using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Reflection;
using SigarettaBot.Commands;
using System.Runtime.CompilerServices;

namespace SigarettaBot
{
    public static class Bot
    {
        public static TelegramBotClient Client;
        public static User Me;

        public static void Start()
        {
            Client = new TelegramBotClient(Settings.API_TOKEN);
            ReceiverOptions receiverOptions = new ReceiverOptions() { AllowedUpdates = { } };
            using CancellationTokenSource cts = new CancellationTokenSource();

            Client.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            Me = Client.GetMeAsync().Result;

            Console.WriteLine($"BOT: Inizio a ricevere i messaggi!");
            Console.ReadLine();
        }

        private static Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        static async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
        {
            //Se non è un messaggio ignoriamo, per ora
            if (update.Message == null || update.Message.Type != MessageType.Text) { return; }

            //Se invece è un comando
            if (update.Message.Text.StartsWith("/"))
            {
                await HandleTelegramCommands(update, client);
            }
        }

        public async static Task HandleTelegramCommands(Update update, ITelegramBotClient botClient)
        {
            //Doppio controllo per assicurarsi che sia un messaggio di testo
            string? incomingMessage = update?.Message?.Text;
            if (incomingMessage == null) return;

            //Se nel messaggio è presente cancelliamo la parte @sigarettagiocobot
            string suffix = $"@{Me.Username}";
            if (incomingMessage.Contains(suffix))
                incomingMessage = incomingMessage.Replace(suffix, "");

            string inputCommand = "";
            string[]? parameters = new string[] { };
            string[] messageWithParameters = incomingMessage.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            //Ci sono parametri?
            if (messageWithParameters.Length > 0)
            {
                inputCommand = messageWithParameters[0].Substring(1);
                parameters = messageWithParameters.Length > 1 ? messageWithParameters.Skip(1).ToArray() : new string[] { };
            }

            //Tutta la parte di reflection per semplificarsi la vita con i comandi
            MethodInfo[] commandMethods = typeof(GameCommands).GetMethods(BindingFlags.Static | BindingFlags.Public);

            foreach (var commandMethod in commandMethods)
            {
                Command commandAttribute = Attribute.GetCustomAttributes(commandMethod, typeof(Command)).FirstOrDefault() as Command;

                if (commandAttribute?.Trigger == inputCommand)
                {
                    if (commandAttribute.OnlyGroup && update.Message.Chat.Type == ChatType.Private)
                    {
                        await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Questo comando è solo per i gruppi");
                    }
                    else if (commandAttribute.OnlyAdmins && update.ChatMember.NewChatMember.Status != ChatMemberStatus.Administrator)
                    {
                        await botClient.SendTextMessageAsync(update.Message.Chat.Id, "❌ Questo comando è solo per gli admin");
                    }
                    else
                    {
                        commandMethod?.Invoke(typeof(GameCommands), new object[] {update, botClient, parameters});
                    }
                }
            }
        }
    }
}
