using SigarettaBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigarettaBot.DB_Models
{
    public class Group
    {
        public int Id { get; set; }
        public long TgId { get; set; }
        public string Name { get; set; }

        public List<PhrasePacket> PhrasesPackets { get; set; }

        //Impostazioni
        public bool AlertOnGroupIfPlayerLeaves { get; set; }
        public bool OnEndShowCigarettesOnGroup { get; set; }

        //Statistiche
        public int GamesPlayed { get; set; }
    }
}
