
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stealth.Model.Board
{
    public class Exit : Wall
    {
        private new  readonly string mark = "E";
        private readonly int x, y;
        public override string GetMark() => mark;
        public Exit(int i, int j) : base()
        {
            x = i;
            y = j;
        }
        public override bool IsExit() => true;

        public int Y { get { return y; } }
        public int X { get { return x; } }

    }
}
