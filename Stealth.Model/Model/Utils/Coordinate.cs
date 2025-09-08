using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stealth.Model.Utils
{
    public class Coordinate
    {
        private int _x,_y;
        public int X { get { return _x; } }
        public int Y { get { return _y; } }

        public Coordinate(int x, int y) { _x = x; _y = y; }

    }
}
