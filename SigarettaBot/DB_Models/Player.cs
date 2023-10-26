using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace SigarettaBot.Models
{
    public class Player
    {
        public long Id { get; set; }
        public User TgInfo { get; set; }
    }
}
