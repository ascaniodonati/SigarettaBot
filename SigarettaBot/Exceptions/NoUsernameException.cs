using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigarettaBot.Exceptions
{
    internal class NoUsernameException : Exception
    {
        public NoUsernameException() : base() { }

        public NoUsernameException(string message) : base(message) { }
    }
}
