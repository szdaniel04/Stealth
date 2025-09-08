using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stealth.Model
{
    public class StatusEventArgs : EventArgs
    {
        private readonly bool _isWon;

        public bool IsWon {  get { return _isWon; } }
        public StatusEventArgs(bool isWon)
        {
            _isWon = isWon;
        }
    }
}
