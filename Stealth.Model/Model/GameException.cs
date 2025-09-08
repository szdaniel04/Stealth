using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stealth.Model
{
    public class GameException : Exception
    {
        public GameException() : base() { }
        public GameException(string message) : base(message) { }
        public GameException(string message, Exception e) : base(message, e) { }
    }
}
