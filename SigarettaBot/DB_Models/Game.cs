using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigarettaBot.Models
{
    public class Game
    {
        public int GameId { get; set; }

        public long ChatId { get; set; }
        public long StarterId { get; set; }

        List<Player> Players { get; set; } = new();
        PhrasePacket Phrases { get; set; } = new();
    }

    public class PhrasePacket
    {
        public int Id { get; set; }
        public string? Phrases { get; set; }
    }
}
