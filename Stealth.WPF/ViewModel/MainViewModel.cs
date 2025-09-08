using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Stealth.Model;
using Stealth.Model.Board;
using System.Drawing;
using System.Windows.Media;
using Stealth.Persistence;
using Stealth.Model.Character;
using Stealth.Model.Utils;
using System.Windows.Input;
using System.Windows.Xps.Serialization;
using System.Windows;

namespace Stealth.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private Game _game = null!;
        private bool _isWon;
        private bool _isStopped;
        private System.Windows.Threading.DispatcherTimer _timer;
        private int _size;
        private List<string> _mapPaths = new List<string>() { "../../../../maps/map1.txt", "../../../../maps/map2.txt", "../../../../maps/map3.txt" };
        private List<string> _maps = new List<string>() { "map1", "map2", "map3" };
        public List<string> Maps { get { return _maps; }}

        public ObservableCollection<string> Buttons { get; private set; }

        public int Size { get { return _size; }
            set
            {
                _size = value;
                OnPropertyChanged();
            }
        }
        public bool IsWon
        {
            get => _isWon;
            set
            {
                _isWon = value;
                OnPropertyChanged();
            }
        }
        public bool IsStopped
        {
            get => _isStopped;
            set
            {
                _isStopped = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ResumeButtonLabel));
            }
        }
        public string ResumeButtonLabel { get => IsStopped ? "Resume Game" : "Stop Game"; }

        public DelegateCommand StopCommand { get; set; }
        public DelegateCommand NewGameCommand { get; set; }

        public MainViewModel()
        {
            _timer = new System.Windows.Threading.DispatcherTimer();
            _timer.Interval = new TimeSpan(0,0,1);
            _timer.Tick += MoveGuards;
            Buttons = new ObservableCollection<string>();
            NewGameCommand = new DelegateCommand(p =>
            {
                CreateNewGame(_mapPaths[(int)p!] ?? string.Empty);
            });
            StopCommand = new DelegateCommand(p =>
            {
                IsStopped = !_isStopped;
                if (IsStopped)
                {
                    _timer.Stop();
                }
                else
                {
                    _timer.Start();
                }
            });
        }

        private void MoveGuards(object? sender, EventArgs e)
        {
            if(!_isStopped && !_isWon)
            {
                _game.MoveAllGuards();
            }
        }

        private void CreateNewGame(string p)
        {
            _game = new Game(p);
            _game.MoveCharacter += new EventHandler<MoveCharacterEventArgs>(CharacterMove);
            _game.StatusChange += new EventHandler<StatusEventArgs>(CheckGameOver);
            _game.TileSet += new EventHandler<SetTileVisibilityEventArgs>(SetTileVis);
            IsStopped = false;
            IsWon = false;
            _timer.Start();
            Size = _game.Map.Size;
            Buttons.Clear();
            for(int i = 0; i < _size; ++i)
                for(int j = 0; j < _size; ++j)
                {
                    int pos = (i * Size) + j;
                    if (_game.player.X == j && _game.player.Y == i)
                        Buttons.Add("O");
                    else
                        Buttons.Add(_game.Map.Board[i, j].GetMark());

                }
        }

        public void KeyPressed(object? sender, KeyEventArgs e)
        {
            if (!IsStopped && !IsWon)
            {
                switch (e.Key)
                {
                    case Key.A:
                    case Key.Left:
                        _game.MovePlayer(Direction.LEFT);
                        break;
                    case Key.D:
                    case Key.Right:
                        _game.MovePlayer(Direction.RIGHT);
                        break;
                    case Key.S:
                    case Key.Down:
                        _game.MovePlayer(Direction.DOWN);
                        break;
                    case Key.W:
                    case Key.Up:
                        _game.MovePlayer(Direction.UP);
                        break;
                }
            }
        }
        private void CharacterMove(object? sender, MoveCharacterEventArgs e)
        {
            Buttons[(e.oldY * Size) + e.oldX] = "";
            int pos = (e.player.Y * Size) + e.player.X;
            if (e.player is Guard)
            {
                Buttons[pos] = ((Guard)e.player).Mark;
            }
            else
            {
                Buttons[pos] = e.player.Mark;
            }
        }
        private void CheckGameOver(object? sender, StatusEventArgs e)
        {
            if (e.IsWon)
            {
                IsStopped = true;
                MessageBox.Show("You won.", "Game over", MessageBoxButton.OK);
            }
            else
            {
                IsStopped = true;
                MessageBox.Show("You lost.", "Game over", MessageBoxButton.OK);
            }
        }
        private void SetTileVis(object? sender, SetTileVisibilityEventArgs e)
        {
            int pos = (e.Y * Size) + e.X;
            if (e.Tile.Visibile())
                Buttons[pos] = "*";
            else if (!e.Tile.WasSettedThisTurn && !(e.Y == _game.player.Y && e.X == _game.player.X))
                Buttons[pos] = "";
        }

    }
}
