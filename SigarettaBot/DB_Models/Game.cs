using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigarettaBot.Models
{
    public class Game
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long[] Players { get; set; }
        public bool InGame { get; set; } = false;
    }
}
