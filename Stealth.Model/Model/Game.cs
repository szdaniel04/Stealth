using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using Stealth.Model.Board;
using Stealth.Model.Character;
using Stealth.Model.Utils;
using Stealth.Persistence;

namespace Stealth.Model
{
    public class Game
    {
        private Player _player;
        private readonly List<Guard> guards;

        private StealthMap _map;
        private bool _isWon = false;
        private bool _isLost = false;
        public StealthMap Map { get { return _map; } }
        public List<Guard> Guards { get { return guards; } }
        public Player player { get { return _player; } }
        public bool IsWon { get { return _isWon; } }
        public bool IsLost { get { return _isLost; } }

        public event EventHandler<MoveCharacterEventArgs>? MoveCharacter;
        public event EventHandler<SetTileVisibilityEventArgs>? TileSet;
        public event EventHandler<StatusEventArgs>? StatusChange;
        public Game(string path, Player player, List<Guard> guards)
        {
            FileManager fm = new FileManager(path);
            _map = fm.Load();
            this._player = player;
            this.guards = guards;
            foreach(Guard guard in this.guards)
                SetTileVisibility(guard);
            GameOver();
        }
        public Game(string path)
        {
            FileManager fm = new FileManager(path);
            _map = fm.Load();
            this._player = new Player(_map.Coordinates[0].X, _map.Coordinates[0].Y);
            guards = new List<Guard>();
            for(int i = 1; i < _map.Coordinates.Count; ++i)
            {
                guards.Add(new Guard(_map.Coordinates[i].X, _map.Coordinates[i].Y));
            }
            foreach (Guard guard in this.guards)
                SetTileVisibility(guard);
            GameOver();
        }
        
        public void OnMove(Player player, int oldX, int oldY) => MoveCharacter?.Invoke(this, new MoveCharacterEventArgs(player, oldX, oldY));

        public void OnTileSet(Tile tile, int x, int y) => TileSet?.Invoke(this, new SetTileVisibilityEventArgs(tile, x,y));

        public void OnWin() => StatusChange?.Invoke(this, new StatusEventArgs(true));
        public void OnLose() => StatusChange?.Invoke(this, new StatusEventArgs(false));


        private void SetTileVisibility(Guard guard)
        {
            int cx = guard.X;
            int cy = guard.Y;
            SetAllVisible(cx, cy);
            SetVisibilityForCornerWalls(cx, cy);
            SetVisibilityForAdjacentWalls(cx, cy);
            SubmitVisibility(cx, cy);
        }
        private void SetAllVisible(int cx, int cy)
        {
            for(int i = cy - 2; i <= cy + 2; ++i)
            {
                for (int j = cx - 2; j <= cx + 2; ++j)
                {
                    if (IsInside(i, j) && _map.Board[i, j].IsFloor())
                    {
                        _map.Board[i, j].SetVisibility(true);
                    }
                }
            }
        }
        private void ClearVisibility(int cx, int cy)
        {
            for (int i = cy - 2; i <= cy + 2; ++i)
                for (int j = cx - 2; j <= cx + 2; ++j)
                    if (IsInside(i, j) && _map.Board[i, j].IsFloor())
                    {
                        _map.Board[i, j].SetVisibility(false, false);
                        OnTileSet(_map.Board[i, j], j, i);
                    }
                           
        }
        private bool IsInside(int x)
        {
            return x >= 0 && x < _map.Size;
        }
        private bool IsInside(int x, int y)
        {
            return IsInside(x) && IsInside(y);
        }
        private void SetVisibilityForCornerWalls(int cx, int cy)
        {
            if (IsInside(cy + 1, cy - 1))
            {
                if (_map.Board[cy - 1, cx - 1].IsWall())//upper left corner
                {
                    SetVisForXxY(cx - 2, cy - 2, 2, 2);
                }
                if (_map.Board[cy + 1, cx - 1].IsWall())//lower left corner
                {
                    SetVisForXxY(cx - 2, cy + 1, 2, 2);
                }
            }
            if (IsInside(cx + 1, cx - 1))
            {
                if (_map.Board[cy - 1, cx + 1].IsWall())//upper right corner
                {
                    SetVisForXxY(cx + 1, cy - 2, 2, 2);
                }
                if (_map.Board[cy + 1, cx + 1].IsWall())//lower right corner
                {
                    SetVisForXxY(cx + 1, cy + 1, 2, 2);
                }
            }
        }
        private void SetVisForXxY(int x, int y, int px, int py)
        {
            for(int i = y; i < y + py; ++i)
            {
                for (int j = x; j < x + px; ++j)
                {
                    if (IsInside(i, j) && !_map.Board[i,j].WasSettedThisTurn && _map.Board[i, j].IsFloor())
                    {
                            _map.Board[i, j].SetVisibility(false);
                    }
                }
            }
        }
        private void SetVisibilityForAdjacentWalls(int cx, int cy)
        {
            if (IsInside(cy + 1, cy - 1))
            {
                if (_map.Board[cy + 1, cx].IsWall())
                {
                    SetVisForXxY(cx - 1, cy + 2, 3, 1);
                }
                if (_map.Board[cy - 1, cx].IsWall())
                {
                    SetVisForXxY(cx - 1, cy - 2, 3, 1);
                }
            }
            if (IsInside(cx + 1, cx - 1))
            {
                if (_map.Board[cy, cx - 1].IsWall())
                {
                    SetVisForXxY(cx - 2, cy - 1, 1, 3);
                }
                if (_map.Board[cy, cx + 1].IsWall())
                {
                    SetVisForXxY(cx + 2, cy - 1, 1, 3);
                }
            }
        }
        private void SubmitVisibility(int cx, int cy)
        {
            for (int i = cy - 2; i <= cy + 2; ++i)
            {
                for (int j = cx - 2; j <= cx + 2; ++j)
                {
                    if (IsInside(i, j) && _map.Board[i, j].IsFloor() && _map.Board[i, j].Visibile())
                    {
                       _map.Board[i, j].SetVisibility(true, true);
                        OnTileSet(_map.Board[i, j], j, i);
                            
                    }
                }
            }
        }
        public void MovePlayer(Direction dir)
        {
            Player temp = new Player(player.X, player.Y);
            temp.Move(dir);
            if(_map.Board[temp.Y, temp.X].IsFloor() || _map.Board[temp.Y, temp.X].IsExit())
            {
                OnMove(temp, player.X, player.Y);
                _player = temp;
                GameOver();
            }


        }
        public void MoveAllGuards()
        {
            foreach (Guard guard in guards)
            {
               MoveGuard(guard);
            }
        }
        private void MoveGuard(Guard guard)
        {
           Direction tempD = guard.Dir;
            int tempX = guard.X;
            int tempY = guard.Y;
            switch (tempD)
            {
                case Direction.UP:
                    tempY--;
                    break;
                case Direction.DOWN:
                    tempY++;
                    break;
                case Direction.LEFT:
                    tempX--;
                    break;
                case Direction.RIGHT:
                    tempX++;
                    break;
            }
            Guard temp = new Guard(tempX, tempY, guard.Dir);
            if (_map.Board[tempY, tempX].IsWall() || CollideWithAnotherGuard(temp) || !IsInside(tempY,tempX))
            {
                guard.Collided();
                ClearVisibility(guard.X, guard.Y);
                OnMove(guard, guard.X, guard.Y);
            }
            else
            {
                ClearVisibility(guard.X, guard.Y);
                OnMove(temp, guard.X, guard.Y);
                guard.Move();
            }
            SetTileVisibility(guard);
            GameOver();
        }
        private bool CollideWithAnotherGuard(Guard temp)
        {
            foreach(Guard guard in guards)
            {

                if (guard.X == temp.X && guard.Y == temp.Y)
                    return true;

            }
            return false;
        }

        public void GameOver() // -1 - lose, 0 - not yet, 1 - win
        {
            if (_map.Board[_player.Y, _player.X].IsExit())
            {
                _isWon = true;
                OnWin();
            }
            if (IsCaught())
            {
                _isLost = true;
                OnLose();
            }
                
        }
        private bool IsCaught()
        {
            for (int i = 0; i < _map.Size; ++i)
            {
                for(int j = 0; j < _map.Size; ++j)
                {
                    if (_map.Board[_player.Y, _player.X].Visibile())
                        return true;
                }
            }
            return false;
        }
    }
}
