using System;
using System.Collections.Generic;
using UnityEngine;

namespace Components {

    public class Tile : MonoBehaviour {

        private float xCoordf, yCoordf, zCoordf;
        private int column, row, zCoord;
        public Renderer rend;
        bool highlight;
        Grid gameBoard;
        List<Tile> _neighbors;
        bool canMoveUp = true, canMoveDown = true, canMoveRght = true, canMoveLft = true, isWall = false;
        public int xCoord { get; }
        public int yCoord { get; }
        public GameObject BoardPiece { get; set; }

        void Start() {
            rend = GetComponent<Renderer>();
        }

        private static int id = 0;
        private Character RetrieveCharacter() {
            return new Character("Unit " + id++, 100, 100, 10, 5, 2, 3);
        }
        void Update() {

            CheckForCursorHover();
            Vector3 cursorLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            bool cursorIsOnTile = CursorIsOnTile(cursorLocation.x - xCoordf, cursorLocation.y - yCoordf);
            if (!isWall && cursorIsOnTile && Input.GetKeyDown(KeyCode.S) && BoardPiece == null) {
                BoardPiece = Instantiate(Resources.Load("Prefabs/opaqueSquare") as GameObject);
                Unit unit = BoardPiece.AddComponent<Unit>();
                Character character = RetrieveCharacter();
                unit.Initialize(character, this);
                BoardPiece.GetComponent<SpriteRenderer>().color = Color.blue;
                BoardPiece.name = character.Name;
                Debug.Log("Spawned unit on " + column + " " + row);
                Debug.Log(BoardPiece.name);
            }
            if (gameBoard!=null && !gameBoard.highlighting && cursorIsOnTile && gameBoard.selectedPiece != null && Input.GetKeyDown(KeyCode.M)) {
                gameBoard.Highlight(gameBoard.selectedPieceMoveRange, Color.blue);
                gameBoard.SelectedPieceState = SelectedPieceState.Moving;
            }
            if (gameBoard!=null && !gameBoard.highlighting && cursorIsOnTile && gameBoard.selectedPiece != null && Input.GetKeyDown(KeyCode.A)) {
                gameBoard.Highlight(gameBoard.selectedPieceAttackRange, Color.red);
                gameBoard.SelectedPieceState = SelectedPieceState.Attacking;
            }
        }



        public void OnMouseDown() {
            List<Tile> adjacencies = GetNeighbors();
            Debug.Log("Clicked: " + this.ToString());
            /*foreach (Tile g in adjacencies) {
                Debug.Log("adjacency: " + g.ToString());
            }*/
            switch(gameBoard.SelectedPieceState) {
                case SelectedPieceState.None : {
                        gameBoard.selectedPiece = this.BoardPiece;
                        break;
                }
                case SelectedPieceState.Attacking : {
                        
                        if (gameBoard.selectedPieceAttackRange.Contains(this) && this.BoardPiece!=null) {
                            Unit unit = this.BoardPiece.GetComponent<Unit>();
                            gameBoard.selectedPiece.GetComponent<Unit>().AttackUnit(unit);
                        }
                        gameBoard.selectedPiece = null;
                        gameBoard.SelectedPieceState = SelectedPieceState.None;
                        break;
                }
                case SelectedPieceState.Moving : {
                        if (gameBoard.selectedPieceMoveRange.Contains(this)) {
                            gameBoard.selectedPiece.GetComponent<Unit>().MoveTo(this);
                        }
                        gameBoard.selectedPiece = null;
                        gameBoard.SelectedPieceState = SelectedPieceState.None;
                        break;
                    }
            }
        }


        public void Initialize(Grid gameBoard, int xLocation, int yLocation, int zLocation, char layoutSymbol) {
            this.xCoordf = xLocation;
            this.yCoordf = yLocation;
            this.zCoordf = zLocation;

            this.column = xLocation;
            this.row = -yLocation;
            this.Terrain = new Terrain(Terrain.Sprite.None);
            this.gameBoard = gameBoard;
            GameObject highlight = Instantiate(Resources.Load("Prefabs/opaqueSquare") as GameObject);
            highlight.transform.position = new Vector3(xCoordf, yCoordf, 1);
            highlight.GetComponent<Renderer>().enabled = false;
            highlight.GetComponent<SpriteRenderer>().color = Color.red;
            highlight.name = "highlight: " + gameObject.name;
            this.gameObject.AddComponent(typeof(BoxCollider));
            this.transform.position = new Vector3(xCoordf, yCoordf, zCoordf);
            GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
            highlight.transform.parent = this.transform;
            LocateWalls(layoutSymbol);
        }

        // Checks if the cursor is hovering on this tile
        private void CheckForCursorHover() {
            Vector3 cursorLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            bool cursorIsOnTile = CursorIsOnTile(cursorLocation.x - xCoordf, cursorLocation.y - yCoordf);

            // If cursor is inside the tile: highlight the tile
            if (cursorIsOnTile && !isWall) {
                GetComponent<Renderer>().enabled = true;
            }
            else {
                GetComponent<Renderer>().enabled = false;
            }
        }



        // Returns a bool indicating if the cursor is hover on tile
        private bool CursorIsOnTile(float mouseXLocation, float mouseYLocation) {
            if ((-0.5f < mouseXLocation && mouseXLocation < 0.5) && (-0.5 < mouseYLocation && mouseYLocation < 0.5)) {
                return true;
            }
            return false;
        }

        private void LocateWalls(char layoutChar) {
            switch (layoutChar) {
                case 'w':
                    isWall = true;
                    canMoveDown = false;
                    canMoveLft = false;
                    canMoveUp = false;
                    canMoveRght = false;
                    break;
                case '-':   // - means half wall is at top of square
                    canMoveUp = false;
                    break;
                case '|':   // | means half wall on left side of a square
                    canMoveLft = false;
                    break;
                case '/':
                    canMoveLft = false;
                    canMoveUp = false;
                    break;
                case '\\':
                    canMoveUp = false;
                    canMoveRght = false;
                    break;
                case 'L':
                    canMoveLft = false;
                    canMoveDown = false;
                    break;
                case '_':
                    canMoveRght = false;
                    canMoveDown = false;
                    break;
                default:
                    return;
            }
        }

        public List<Tile> GetNeighbors() {
            if (_neighbors != null)
                return _neighbors;

            _neighbors = new List<Tile>();
            //@TODO need to catch outof bounds exception
            Tile rightNeighb = gameBoard.GetGridObject(column + 1, row);  // column, row
            Tile leftNeighb = gameBoard.GetGridObject(column - 1, row);
            Tile aboveNeighb = gameBoard.GetGridObject(column, row - 1);
            Tile belowNeighb = gameBoard.GetGridObject(column, row + 1);

            if (this.canMoveRght && rightNeighb.canMoveLft)
                _neighbors.Add(rightNeighb);

            if (this.canMoveLft && leftNeighb.canMoveRght)
                _neighbors.Add(leftNeighb);

            if (this.canMoveDown && belowNeighb.canMoveUp)
                _neighbors.Add(belowNeighb);

            if (this.canMoveUp && aboveNeighb.canMoveDown)
                _neighbors.Add(aboveNeighb);

            return _neighbors;
        }
        public Grid grid { get; }

        public int Cost { get; set; }

        public Tile Parent { get; set; }

        public Terrain Terrain { get; set; }

        public bool Highlight { get; set; }

        // Checks above, below, left, and right of the tile and checks the grid if those correspond to actual tile objects.
        // Does not account for walls or obstacles yet.
        /*public List<Tile> Neighbors {
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
                    tile = grid.GetGridObject(xCoord + move.Item1, yCoord + move.Item2);
                    if (tile != default(Tile)) {
                        _neighbors.Add(tile);
                    }
                }
                return _neighbors;
            }
        }

        /*public Tile(Grid g, int x, int y) {
            xCoord = x;
            yCoord = y;
            grid = g;
            this.Terrain = new Terrain(Terrain.Sprite.None);
        }*/

        public void SetTerrainSprite(Terrain.Sprite sprite) {
            Terrain.Type = sprite;
            grid.TriggerGridObjectChanged(xCoord, yCoord);
        }

        public override string ToString() {
            string debugString = column + " " + row;
            /*string debugString = Highlight ? "H\n" : "";
            debugString += Terrain.Type == Terrain.Sprite.Puddle ? Terrain.ToString() + "\n" : "";
            debugString += BoardPiece != null ? "Unit " + this.BoardPiece.ToString() : "";*/
            return debugString;
        }
    }
}