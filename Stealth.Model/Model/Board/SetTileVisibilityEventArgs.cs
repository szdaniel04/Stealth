
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stealth.Model.Board
{
    public class SetTileVisibilityEventArgs : EventArgs
    {
        private readonly int _x , _y;
        private readonly Tile _tile;
        public int X { get { return _x; } }
        public int Y { get { return _y; } }
        public Tile Tile { get { return _tile; } }

        public SetTileVisibilityEventArgs(Tile tile, int x, int y)
        {
            _x = x;
            _y = y;
            _tile = tile;
        }
    }
}
