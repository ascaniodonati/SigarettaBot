using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigarettaBot.Manager
{
    public static class GameManager
    {
        private static List<Sigaretta> ActiveGames = new List<Sigaretta>();

        public static void AddGame(Sigaretta game)
        {
            ActiveGames.Add(game);
        }

        public static List<Sigaretta> GetSigarette {  get { return ActiveGames; } }
    }
}
