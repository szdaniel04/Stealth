
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Stealth.Model.Utils;

namespace Stealth.Model.Character
{
    public class Guard : Player
    {
        private Direction dir;
        public Direction Dir { get { return dir; } }

        private new readonly string mark = "X";

        public new string Mark {  get { return mark; } }

        public Guard(int x, int y, Direction dir) : base(x, y) { this.dir = dir; }
        public Guard(int x, int y) : base(x,y) { dir = Direction.UP;}
        
        public void Collided()
        {
                dir = Directions.Rotate();
        }
        public void Move()
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
