using SigarettaBot.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
        public long starterId;
        List<Player> players = new List<Player>();
        List<string> phrases = Settings.DEFAULT_PHRASES;

        Thread gameThread;
        List<Message> messagesToDelete = new List<Message>();

        public List<Player> GetPlayer { get { return players; } }

        public Message playerTgMessage;

        //Un oggetto di tipo Sigaretta creato corrisponde a una nuova partita cominciata
        public Sigaretta(long _chatId, long _starterId)
        {
            chatId = _chatId;
            starterId = _starterId;

            //Inizializziamo il thread relativo al ciclo del gioco
            gameThread = new Thread(new ThreadStart(GameCycle));

            //Comunichiamo al gruppo che la partita è stata cominciata, diamo la possibilità di partecipare
            playerTgMessage = Bot.Client.SendTextMessageAsync(
                chatId,
                playerMessageContent,
                replyMarkup: JoinMarkup
            ).Result;

            //Il messaggio iniziale deve essere eliminato appena iniziata la partita
            messagesToDelete.Add(playerTgMessage);

            Console.WriteLine($"Partita iniziata sul gruppo {chatId}");
        }

        public async Task UpdatePlayerMessage()
        {
            await Bot.Client.EditMessageTextAsync(
                chatId,
                playerTgMessage.MessageId,
                playerMessageContent,
                replyMarkup: JoinMarkup);
        }

        public string playerMessageContent
        {
            get
            {
                string _playerMessageContent = "🚬 Una nuova partita è stata iniziata su questo gruppo, clicca su Join se vuoi entrare";
                _playerMessageContent += "\n\n";

                string[] playerNames = players.Select(x => x.TgInfo.FirstName).ToArray();
                _playerMessageContent += $"🙍‍ Giocatori partecipanti ({players.Count}):\n{String.Join("\n", playerNames)}";

                return _playerMessageContent;
            }
        }

        public void AddPlayer(Player player)
        {
            //Il giocatore sta già giocando?
            if (players.Any(x => x.Id == player.Id)) { return; }

            players.Add(player);
            Bot.Client.SendTextMessageAsync(player.TgInfo.Id, "✅ Hai joinato la partita con successo");
            UpdatePlayerMessage();

            if (players.Count == 1)
            {
                //Se entra anche solo un giocatore faccio partire il ciclo del gioco   
                gameThread.Start();
            }
        }

        public void KickPlayer(Player player, bool kicked = false)
        {
            int deletedCount = players.RemoveAll(x => x.Id == player.Id);

            //Il giocatore ha abbandonato con successo (infame)
            if (deletedCount == 1)
            {
                if (!kicked)
                {
                    Bot.Client.SendTextMessageAsync(player.Id, "❌ Vile, hai abbadonato!");
                    Bot.Client.SendTextMessageAsync(chatId, $"🔪 {player.TgInfo.FirstName} ha abbandonato la partita e perciò sarà soggetto alla gogna pubblica");
                }
                else
                {
                    Bot.Client.SendTextMessageAsync(chatId, $"⛔ {player.TgInfo.FirstName} è stato cacciato dalla partita");
                }

                UpdatePlayerMessage();
            }
            //WTF?
            else if (deletedCount == 0)
            {
                Console.WriteLine("Questo è strano...");
            }
        }

        public void GameCycle()
        {
            List<double> secondsTick = new List<double> { 50, 30, 10, 5 };
            Stopwatch sw = Stopwatch.StartNew();
            double countdownStart = 10d;

            //Countdown per l'inizio del gioco
            while (sw.IsRunning)
            {
                double remainingTime = Math.Floor(countdownStart - (sw.ElapsedMilliseconds / 1000d));
                Thread.Sleep(100);
                Console.WriteLine(remainingTime);

                if (secondsTick.Contains(remainingTime))
                {
                    SendStartTimeMessage(remainingTime);
                    secondsTick.Remove(remainingTime);
                }

                if (remainingTime < 1) sw.Stop();
            }

            //Elimino tutti i messaggi di "introduzione" e conta del tempo per evitare lo spam
            messagesToDelete.ForEach((message) =>
            {
                Bot.Client.DeleteMessageAsync(chatId, message.MessageId);
            });

            //Inizia la partita
            Bot.Client.SendTextMessageAsync(chatId, "🚬 La partita sta per iniziare");
        }

        public void SendStartTimeMessage(double seconds)
        {
            Message timeMessage = Bot.Client.SendTextMessageAsync(chatId, $"Mancano {seconds} secondi all'inizio del gioco").Result;

            //I messaggi relativi ai secondi vanno eliminati all'inizio della partita per evitare spam
            messagesToDelete.Add(timeMessage);
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
