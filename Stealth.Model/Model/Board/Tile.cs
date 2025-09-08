
namespace Stealth.Model.Board
{
    public abstract class Tile
    {
        protected string mark = "";
        public bool WasSettedThisTurn { get; set; }
        public virtual string GetMark() => mark;

        protected bool visible = false;
        public bool Visibile() => visible;
        public virtual void SetVisibility(bool visibility) => visible = visibility;
        public virtual void SetVisibility(bool visibility, bool wasSetted)
        {
            visible = visibility;
            WasSettedThisTurn = wasSetted;
        }
        public virtual bool IsWall() => false;
        public virtual bool IsFloor() => false;
        public virtual bool IsExit() => false;
    }
}
