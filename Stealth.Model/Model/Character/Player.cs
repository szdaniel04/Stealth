
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Stealth.Model.Utils;

namespace Stealth.Model.Character
{
    public class Player
    {
        protected int x, y;


        public int Y { get { return y; } }
        public int X { get { return x; } }

        protected string mark = "O";
        public string Mark { get { return mark; } }
        public Player(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void Move(Direction dir)
        {
            switch (dir)
            {
                case Direction.UP:
                    y--;
                    break;
                case Direction.DOWN:
                    y++;
                    break;
                case Direction.LEFT: 
                    x--;
                    break;
                case Direction.RIGHT:
                    x++;
                    break;  
            }
        }

    }
}
