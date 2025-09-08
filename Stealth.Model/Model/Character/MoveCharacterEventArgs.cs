using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stealth.Model.Character
{
    public class MoveCharacterEventArgs : EventArgs
    {
        private readonly Player _player;
        private readonly int _oldX,_oldY;

        public int oldX { get { return _oldX; } }
        public int oldY { get { return _oldY; } }
        public Player player { get { return _player; } }

        public MoveCharacterEventArgs(Player player, int x, int y)
        {
            _player = player;
            _oldX = x;
            _oldY = y;
        }


    }
}
