
namespace Stealth.Model.Board
{
    public class Wall : Tile
    {
        private new readonly string mark = "#";
        public override string GetMark() => mark;
        public override void SetVisibility(bool visibility) => throw new GameException("Cannot set visibility to walls");
        public override bool IsWall() => true;
        public override bool IsFloor() => false;
    }
}
