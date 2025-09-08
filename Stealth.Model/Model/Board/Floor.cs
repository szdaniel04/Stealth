namespace Stealth.Model.Board
{
    public class Floor : Tile
    {
        private new readonly string mark = " ";

        public override string GetMark() => visible ? "*" : mark;
        public override bool IsFloor() => true;
    }
}
