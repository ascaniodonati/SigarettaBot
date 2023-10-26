using SigarettaBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace SigarettaBot
{
    public class Sigaretta : IAsyncDisposable
    {
        public long gameId;
        public long chatId;

        List<Player> players = new List<Player>();
        public List<Player> GetPlayer {  get { return players; } }

        public Message playerTgMessage;

        //Un oggetto di tipo Sigaretta creato corrisponde a una nuova partita cominciata
        public Sigaretta(long _chatId)
        {
            chatId = _chatId;

            //Comunichiamo al gruppo che la partita è stata cominciata, diamo la possibilità di partecipare
            Bot.Client.SendTextMessageAsync(
                chatId,
                "🚬 Una nuova partita è stata iniziata su questo gruppo, clicca su Join se vuoi entrare",
                replyMarkup: JoinMarkup
            );

            playerTgMessage = 
                Bot.Client.SendTextMessageAsync(
                chatId,
                playerMessageContent).Result;

            Console.WriteLine($"Partita iniziata sul gruppo {chatId}");
        }

        public async Task UpdatePlayerMessage()
        {
            await Bot.Client.EditMessageTextAsync(chatId, playerTgMessage.MessageId, playerMessageContent);
        }

        public string playerMessageContent
        {
            get
            {
                string[] playerNames = players.Select(x => x.TgInfo.FirstName).ToArray();
                return $"🙍‍ Giocatori partecipanti ({players.Count}):\n{String.Join("\n", playerNames)}";
            }
        }

        public void AddPlayer(Player player)
        {
            //Il giocatore sta già giocando?
            if (players.Any(x => x.Id == player.Id)) { return; }

            players.Add(player);
            Bot.Client.SendTextMessageAsync(player.TgInfo.Id, "✅ Hai joinato la partita con successo");
            UpdatePlayerMessage();
        }

        public void LeavePlayer(Player player)
        {
            int deletedCount = players.RemoveAll(x => x.Id == player.Id);

            //Il giocatore ha abbandonato con successo (infame)
            if (deletedCount == 1)
            {
                Bot.Client.SendTextMessageAsync(player.Id, "❌ Vile, hai abbadonato!");
                Bot.Client.SendTextMessageAsync(chatId, $"🔪 {player.TgInfo.FirstName} ha abbandonato la partita e perciò sarà soggetto alla gogna pubblica");
                UpdatePlayerMessage();
            }
            //WTF?
            else if (deletedCount == 0)
            {
                Console.WriteLine("Questo è strano...");
            }
        }

        InlineKeyboardMarkup JoinMarkup
        {
            get
            {
                string callbackData = $"http://telegram.me/sigarettagiocobot?start={chatId}";
                InlineKeyboardButton joinButton = InlineKeyboardButton.WithUrl("Gioca", callbackData);
                return new InlineKeyboardMarkup(joinButton);
            }
        }

        public ValueTask DisposeAsync()
        {
            throw new NotImplementedException();
        }
    }

}
