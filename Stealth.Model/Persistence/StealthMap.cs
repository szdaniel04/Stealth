using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Stealth.Model.Board;
using Stealth.Model.Utils;


namespace Stealth.Persistence
{
    public class StealthMap
    {
        private readonly int _size;
        private Tile[,] _board;
        private List<Coordinate> _coordinates;
        public int Size { get { return _size; } }
        public Tile[,] Board { get { return _board; } }
        public List<Coordinate> Coordinates { get { return _coordinates; } }

        public StealthMap(int size, string[] map, List<Coordinate> coordinates)
        {
            _size = size;
            _board = new Tile[_size, _size];
            InitMap(map);
            this._coordinates = coordinates;
        }
        private void InitMap(string[] lines)
        {

            for (int i = 0; i < _size; ++i)
            {
                string[] line = lines[i].Split();
                for (int j = 0; j < _size; ++j)
                {
                    if (line[j] == "#")
                        _board[i, j] = new Wall();
                    else if (line[j] == "_")
                    {
                        _board[i, j] = new Floor();
                        _board[i, j].SetVisibility(false);
                    }
                    else if (line[j] == "E")
                        _board[i, j] = new Exit(i, j);

                }
            }
        }
    }
}
