using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utilitys;

namespace Components {
    /*
     * This class was made by following the code monkey tutorial and it doesn't exactly follow the ecs architecture since it
     * contains some board component fields and board and movement system methods.
     */

    public enum SelectedPieceState {
        None,
        Attacking,
        Moving,
    }

    public class Grid : MonoBehaviour {
        private readonly int GRID_HEIGHT = 32;
        private readonly int GRID_WIDTH = 24;
        private readonly string MAP_NAME = "Layout2";
        /*public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;

        public class OnGridObjectChangedEventArgs : EventArgs {
            public int x;
            public int y;
        }*/

        private GameObject _selectedPiece;
        private List<Tile> _selectedPieceMoveRange;
        private List<Tile> _selectedPieceAttackRange;
        public int width { get; }
        public int height { get; }
        public float cellSize { get; }
        public Vector3 originPosition;

        public int TurnCounter { get; set; }

        public int numOfTeam { get; set; }
        public Dictionary<int, List<Unit>> Teams { get; set;  }
        public bool highlighting { get; set; }

        public SelectedPieceState SelectedPieceState { get; set; }
        private Tile[,] gridArray;

        
        //private readonly string MAP_NAME = "LatestMap";
        
        string[,] wallLayoutArray;
        //private List<Tile> _neighbors;
        //public Gridgrid { get; }
        public Tile[,] tiles;
        //public int xCoord { get; }
        //public int yCoord { get; }


        void Start() {
         
            CreateGrid();
        }

        void Update() {
            if (Input.GetKeyDown("enter")) {
                EndTurn();
                StartTurn();
            }
        }

        private int id = 0;
        private Character RetrieveCharacter() {
            return new Character("Unit " + id++, 100, 100, 10, 5, 2, 3);
        }

        public void SpawnUnit(int x, int y) {
            GameObject unitObject = Instantiate(Resources.Load("Prefabs/target") as GameObject);
            Unit unit = unitObject.AddComponent<Unit>();
            unit.Initialize(RetrieveCharacter(), tiles[x, y]);
            Teams[unit.Team].Add(unit);

        }

        private void StartTurn() {
            int team = TurnCounter % 2;
            Debug.Log("Team " + team + " Turn " + TurnCounter / 2);
            foreach (Unit unit in Teams[0]) {
                Debug.Log("can act");
                unit.HasMoved = false;
                unit.HasActed = false;
            }
        }

        private void EndTurn() {
            int team = TurnCounter % 2;
            foreach (Unit unit in Teams[0]) {
                Debug.Log("Can't Act");
                unit.HasActed = true;
            }
            Debug.Log("Ending Turn");
            TurnCounter += 1;
        }


        private void CreateGrid() {
            wallLayoutArray = new string[GRID_WIDTH, GRID_HEIGHT];
            tiles = new Tile[GRID_WIDTH, GRID_HEIGHT];
            string MAP_NAME = "Layout2";
            
            string wallLayoutFile = Directory.GetCurrentDirectory() + "\\Assets\\Utilities\\BoardCreationUtility\\output\\" + MAP_NAME + ".txt";
            string wallLayout = string.Join("", File.ReadAllLines(wallLayoutFile));
            int wallIndex = 0;
            numOfTeam = 2;
            Teams = new Dictionary<int, List<Unit>>();
            for (int i = 0; i < numOfTeam; i++) {
                Teams[i] = new List<Unit>();
            }
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


        /*public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid, int, int, Tile> createGridObject) {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.originPosition = originPosition;

            gridArray = new Tile[width, height];

            for (int x = 0; x < gridArray.GetLength(0); x++) {
                for (int y = 0; y < gridArray.GetLength(1); y++) {
                    gridArray[x, y] = createGridObject(this, x, y);
                }
            }

            bool showDebug = true;
            if (showDebug) {
                TextMesh[,] debugTextArray = new TextMesh[width, height];

                for (int x = 0; x < gridArray.GetLength(0); x++) {
                    for (int y = 0; y < gridArray.GetLength(1); y++) {
                        debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y]?.ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 100, Color.white, TextAnchor.MiddleCenter);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.black, 100f);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.black, 100f);
                    }
                }
                Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.black, 100f);
                Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.black, 100f);

                /*OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) => {
                    debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
                };
            }
        }*/

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
