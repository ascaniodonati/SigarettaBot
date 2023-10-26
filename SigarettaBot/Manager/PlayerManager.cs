using SigarettaBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace SigarettaBot.Manager
{
    public static class PlayerManager
    {
        private static List<Player> Players = new List<Player>();

        public static Player GetPlayer(User user)
        {
            Player? selected = Players.FirstOrDefault(x => x.Id == user.Id);

            if (selected == null)
            {
                Player newPlayer = new Player
                {
                    Id = user.Id,
                    TgInfo = user
                };

                Players.Add(newPlayer);
                return newPlayer;
            }
            else
            {
                return selected;
            }
        }

        public static Player GetPlayer(long userId)
        {
            Player? selected = Players.FirstOrDefault(x => x.Id == userId);
            return selected;
        }
    }
}
