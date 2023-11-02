using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace SigarettaBot.Models
{
    public class Player
    {
        public long Id { get; set; }
        public long TgId { get; set; }
        public string Name { get; set; }

        [NotMapped]
        public User TgInfo { get; set; }
    }
}
