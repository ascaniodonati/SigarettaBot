using SigarettaBot.Manager;
using SigarettaBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SigarettaBot.Commands
{
    public static partial class GameCommands
    {
        [Command(Trigger = "nuovapartita", OnlyGroup = true)]
        public static void CreateGame(Update update, ITelegramBotClient botClient, string[] parameters)
        {
            //Controlla che non sia già stata cominciata una partita
            if (GameManager.GetSigarette.Any(x => x.chatId == update.Message.Chat.Id))
            {
                return;
            }

            Sigaretta sigaretta = new Sigaretta(update.Message.Chat.Id);
            GameManager.AddGame(sigaretta);
        }

        [Command(Trigger = "start", OnlyGroup = false)]
        public static void Start(Update update, ITelegramBotClient botClient, string[] parameters)
        {
            //Il messaggio non viene considerato se viene da un gruppo
            if (update.Message.Chat.Type != Telegram.Bot.Types.Enums.ChatType.Private) return;
            User sender = update.Message.From;

            //Il comando Start deve essere "generato" da un join, se no non è valido
            if (parameters.Length == 0)
            {
                botClient.SendTextMessageAsync(sender.Id, "🔵 Ciao, se vuoi unirti alla partita prima devi selezionare 'Join' da un gruppo");
                return;
            }

            //Se non riesco a convertirlo in long non è il nome del gruppo
            bool validGroupId = long.TryParse(parameters[0], out long groupId);
            if (!validGroupId)
            {
                botClient.SendTextMessageAsync(sender.Id, "❌ L'invito alla partita non è valido");
                return;
            }

            //L'utente sta già giocando a un altra partita?
            bool isInAnotherGame = 
                GameManager.GetSigarette.Any(x => x.chatId != groupId && x.GetPlayer.Any(x => x.TgInfo.Id == sender.Id));

            if (isInAnotherGame)
            {
                botClient.SendTextMessageAsync(sender.Id, "❌ Stai giocando a un'altra partita");
                return;
            }

            //Esiste una partita attiva nel gruppo?
            Sigaretta? selectedGame = GameManager.GetSigarette.FirstOrDefault(x => x.chatId == groupId);

            if (selectedGame == null)
            {
                botClient.SendTextMessageAsync(update.Message.From.Id, "❌ Questa partita è finita o è stata cancellata");
                return;
            }

            //Tutto è andato bene? Aggiungiamo il giocatore
            selectedGame.AddPlayer(PlayerManager.GetPlayer(sender));
        }

        [Command(Trigger = "leave", OnlyGroup = false)]
        public static void Leave(Update update, ITelegramBotClient botClient, string[] parameters)
        {
            long senderId = update.Message.From.Id;

            //Il giocatore sta giocando in qualche partita?
            Sigaretta? selected = GameManager.GetSigarette.FirstOrDefault(x => x.GetPlayer.Any(x => x.TgInfo.Id == update.Message.From.Id));

            if (selected == null)
            {
                return;
            }

            Player playerLeaving = PlayerManager.GetPlayer(senderId);

            //Non dovrebbe succedere, ma la gestione degli errori va sempre fatta
            if (playerLeaving == null)
            {
                botClient.SendTextMessageAsync(senderId, "❌ Si è verificato un errore, impossibile lasciare il gioco");
                return;
            }

            //Test superati? Bene, puoi andartene
            selected.LeavePlayer(playerLeaving);
        }
    }
}
