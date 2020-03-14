using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utilitys;

namespace Components {

    public enum SelectedPieceState {
        None,
        Attacking,
        Moving,
    }

    public class Grid : MonoBehaviour {
        private readonly int GRID_HEIGHT = 32;
        private readonly int GRID_WIDTH = 24;
        private readonly string MAP_NAME = "Layout2";
        public event EventHandler<TurnEventArgs> OnTurnStart;
        public event EventHandler<TurnEventArgs> OnTurnEnd;

        public class TurnEventArgs : EventArgs {
            public int Team;
        }

        private GameObject _selectedPiece;
        private List<Tile> _selectedPieceMoveRange;
        private List<Tile> _selectedPieceAttackRange;
        public int width { get; }
        public int height { get; }
        public float cellSize { get; }
        public Vector3 originPosition;
        public int TurnCounter { get; set; }
        public int numOfTeam { get; set; }
        public bool highlighting { get; set; }

        public SelectedPieceState SelectedPieceState { get; set; }
        private Tile[,] gridArray;
       
        
        string[,] wallLayoutArray;
        
        public Tile[,] tiles;
        


        void Start() {
            CreateGrid();
        }

        void Update() {
            if (Input.GetKeyDown("enter")) {
                EndTurn();
                StartTurn();
            }
            if (selectedPiece != null) {
                Unit unit = selectedPiece.GetComponent<Unit>();
                if (Input.GetKeyDown(KeyCode.M) && !unit.HasMoved) {
                    Highlight(selectedPieceMoveRange, Color.blue);
                    SelectedPieceState = SelectedPieceState.Moving;
                }
                if (Input.GetKeyDown(KeyCode.A) && !unit.HasActed) {
                    Highlight(selectedPieceAttackRange, Color.red);
                    SelectedPieceState = SelectedPieceState.Attacking;
                }
            }
        }

        private int id = 0;
        private Character RetrieveCharacter() {
            return new Character("Unit " + id, id++ % numOfTeam, 100, 100, 10, 5, 2, 3);
        }

        public void SpawnUnit(int x, int y) {
            GameObject unitObject = Instantiate(Resources.Load("Prefabs/target") as GameObject);
            Unit unit = unitObject.AddComponent<Unit>();
            unit.Initialize(RetrieveCharacter(), tiles[x, y]);
        }

        public void SpawnUnit(Tile tile) {
            SpawnUnit(tile.xCoord, tile.yCoord);
        }

        private void StartTurn() {
            int team = TurnCounter % 2;
            Debug.Log("Team " + team + " Turn " + TurnCounter / 2);
            OnTurnStart?.Invoke(this, new TurnEventArgs { Team = team });
        }

        private void EndTurn() {
            int team = TurnCounter % 2;
            Debug.Log("Ending Turn");
            TurnCounter += 1;
            OnTurnEnd?.Invoke(this, new TurnEventArgs { Team = team });
        }

        private void CreateGrid() {
            wallLayoutArray = new string[GRID_WIDTH, GRID_HEIGHT];
            tiles = new Tile[GRID_WIDTH, GRID_HEIGHT];
            string MAP_NAME = "Layout2";
            
            string wallLayoutFile = Directory.GetCurrentDirectory() + "\\Assets\\Resources\\BoardTxtFiles\\" + MAP_NAME + ".txt";
            string wallLayout = string.Join("", File.ReadAllLines(wallLayoutFile));
            int wallIndex = 0;
            numOfTeam = 2;
            Debug.Log("Made teams");
            TurnCounter = 0;
            for (int row = 0; row < GRID_HEIGHT; row++) {
                for (int column = 0; column < GRID_WIDTH; column++) {
                    
                    GameObject gridSquare = Instantiate(Resources.Load("Prefabs/target") as GameObject);
                    Tile tile = gridSquare.AddComponent<Tile>();
                    gridSquare.name = "Tile[" + column + "," + row + "]";
                    tile.Initialize(this, column, -row, -1, wallLayout[wallIndex++]);
                    tiles[column, row] = tile;
                    wallLayoutArray[column, row] = ".";
                }
            }
            

        }
        public GameObject selectedPiece {
            get {
                return _selectedPiece;
            }
            set {
                _selectedPiece = value;
                unHighlight(selectedPieceAttackRange);
                unHighlight(selectedPieceMoveRange);
                if (_selectedPiece != null) {
                    selectedPieceMoveRange = FindTilesInRange(selectedPiece.GetComponent<Unit>().Tile, selectedPiece.GetComponent<Unit>().MoveSpeed, (Tile) => Tile.Terrain.Cost);
                    selectedPieceAttackRange = FindTilesInRange(selectedPiece.GetComponent<Unit>().Tile, 1, (Tile) => 1);
                }
                else {
                    selectedPieceMoveRange = default(List<Tile>);
                    selectedPieceAttackRange = default(List<Tile>);
                    SelectedPieceState = SelectedPieceState.None;
                }
                ;
            }
        }

        public List<Tile> selectedPieceMoveRange {
            get {
                return _selectedPieceMoveRange;
            }
            set {
                _selectedPieceMoveRange = value;
            }
        }

        public List<Tile> selectedPieceAttackRange {
            get {
                return _selectedPieceAttackRange;
            }
            set {
                _selectedPieceAttackRange = value;
            }
        }

        public void Highlight(List<Tile> tiles, Color color) {
            ApplyTileEffects(tiles, (Tile) => ToggleHighlightEffect(Tile, true, color));
            highlighting = true;
        }

        public void unHighlight(List<Tile> tiles) {
            ApplyTileEffects(tiles, (Tile) => ToggleHighlightEffect(Tile, false, Color.white));
            highlighting = false;
        }

        public void SetTileCost(Tile tile, int cost) {
            tile.Cost = cost;
        }

        // Used to find tiles that a board piece can reach with their movespeed. So far uses manhatten distance but must change in order to account for obstacles such as walls.
        public List<Tile> FindTilesInRange(Tile tile, int range, Func<Tile, int> getTileCost) {
            List<Tile> tilesInRange = new List<Tile>();
            Queue<Tile> queue = new Queue<Tile>();
            List<Tile> visitedList = new List<Tile>();
            Tile currentTile;
            int neighborCost;
            tile.Cost = 0;
            queue.Enqueue(tile);
            while (queue.Count > 0) {
                currentTile = queue.Dequeue();
                foreach (Tile neighbor in currentTile.GetNeighbors()) {
                    if (!visitedList.Contains(neighbor)) {
                        visitedList.Add(neighbor);
                        neighborCost = getTileCost(neighbor) + currentTile.Cost;
                        if (neighbor.Terrain.IsWalkable && neighborCost <= range) {
                            neighbor.Cost = neighborCost;
                            neighbor.Parent = currentTile;
                            tilesInRange.Add(neighbor);
                            queue.Enqueue(neighbor);
                        }
                    }
                }
            }
            foreach (Tile inRangeTile in tilesInRange) {
                inRangeTile.Cost = 0;
            }
            return tilesInRange;
        }

        private void ApplyTileEffects(List<Tile> tileZone, Action<Tile> applyEffect) {
            if (tileZone == null) {
                return;
            }
            foreach (Tile tile in tileZone) {
                applyEffect(tile);
                tile.Parent = null;
            }
        }

        // Used to highlight a list of tiles (Tile Zone). As of now used to highlight/unhighlight the tiles in range of the selected board piece
        private void ToggleHighlightEffect(Tile tile, bool toggle, Color color) {
            GameObject tileObject = tile.gameObject.transform.GetChild(0).gameObject;
            if (toggle) {
                tileObject.GetComponent<SpriteRenderer>().color = color;
            }
            tileObject.GetComponent<Renderer>().enabled = toggle;
        }

        public Vector3 GetWorldPosition(int x, int y) {
            return new Vector3(x, y) * cellSize + originPosition;
        }

        private void GetXY(Vector3 worldPosition, out int x, out int y) {
            x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
            y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
        }

        public void SetGridObject(int x, int y, Tile value) {
            if (x >= 0 && y >= 0 && x < width && y < height) {
                tiles[x, y] = value;
                //OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, y = y });
            }
        }

        public void TriggerGridObjectChanged(int x, int y) {
            //OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }

        public void SetGridObject(Vector3 worldPosition, Tile value) {
            int x, y;
            GetXY(worldPosition, out x, out y);
            SetGridObject(x, y, value);
        }

        public Tile GetGridObject(int x, int y) {
            if (x >= 0 && y >= 0 && x < GRID_WIDTH && y < GRID_HEIGHT) {
                return tiles[x, y];
            }
            else {
                return default(Tile);
            }
        }

        public Tile GetGridObject(Vector3 worldPosition) {
            int x, y;
            GetXY(worldPosition, out x, out y);
            return GetGridObject(x, y);
        }
    }
}
