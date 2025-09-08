using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stealth.Model.Utils
{
    public enum Direction { UP, RIGHT, DOWN, LEFT }
    public static class Directions
    {   
        public static Direction Rotate()
        {
            Random r = new Random();
            int n = r.Next(0, 4);
            switch (n)
            {
                case 0:
                    return Direction.UP;
                case 1:
                    return Direction.RIGHT;
                case 2:
                    return Direction.DOWN;
                case 3:
                    return Direction.LEFT;
            }
            throw new GameException("Cannot rotate");
        }
    }
   
}
