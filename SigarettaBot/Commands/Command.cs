using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigarettaBot.Commands
{
    internal class Command : Attribute
    {
        public string Trigger { get; set; }
        public bool OnlyGroup { get; set; } = true;
        public bool OnlyAdmins { get; set; } = false;
    }
}
