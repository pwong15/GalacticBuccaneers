using System;
using System.Collections.Generic;

namespace Components {

    public class Tile {
        public int xCoord { get; }
        public int yCoord { get; }
        public BoardPiece BoardPiece { get; set; }

        public Grid<Tile> grid { get; }

        private List<Tile> _neighbors;

        public int Cost { get; set; }

        public Tile Parent { get; set; }

        public Terrain Terrain { get; set; }

        public bool Highlight { get; set; }

        // Checks above, below, left, and right of the tile and checks the grid if those correspond to actual tile objects.
        // Does not account for walls or obstacles yet.
        public List<Tile> Neighbors {
            get {
                if (_neighbors != null) {
                    return _neighbors;
                }
                _neighbors = new List<Tile>();
                Tile tile;
                Tuple<int, int>[] possibleMoves = {
                Tuple.Create(0, 1),
                Tuple.Create(1, 0),
                Tuple.Create(-1, 0),
                Tuple.Create(0, -1),
            };
                for (int i = 0; i < 4; i++) {
                    Tuple<int, int> move = possibleMoves[i];
                    tile = grid.GetGridObject(grid.GetWorldPosition(xCoord + move.Item1, yCoord + move.Item2));
                    if (tile != default(Tile)) {
                        _neighbors.Add(tile);
                    }
                }
                return _neighbors;
            }
        }

        public Tile(Grid<Tile> g, int x, int y) {
            xCoord = x;
            yCoord = y;
            grid = g;
            this.Terrain = new Terrain(Terrain.Sprite.None);
        }

        public void SetTerrainSprite(Terrain.Sprite sprite) {
            Terrain.Type = sprite;
            grid.TriggerGridObjectChanged(xCoord, yCoord);
        }

        public override string ToString() {
            string debugString = Highlight ? "highlight\n" : "";
            debugString += Terrain.ToString() + "\n";
            debugString += BoardPiece != null ? this.BoardPiece.ToString() : "";
            return debugString;
        }
    }
}