using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utilitys;
using Models;
using Encounter;

namespace Views {

    public enum SelectedPieceState {
        None,
        Attacking,
        Moving,
        SelectingAbility,
        Casting,
    }

    public class Grid : MonoBehaviour {
        private readonly int GRID_HEIGHT = 32;
        private readonly int GRID_WIDTH = 24;
        private readonly string MAP_NAME = "Layout2";
        public event EventHandler<TurnEventArgs> OnTurnStart;
        public event EventHandler<TurnEventArgs> OnTurnEnd;
        public event EventHandler<MapEventArgs> OnMapOver;
        public Dictionary<int, List<Unit>> Teams;
        public class TurnEventArgs : EventArgs {
            public int Team;
        }

        public class MapEventArgs : EventArgs {
            public bool gameOver;

        }

        private GameObject _selectedPiece;
        private List<Tile> _selectedPieceAbilityRange;
        private List<Tile> _selectedAbilityZone;

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
            if (selectedPiece != null && SelectedPieceState == SelectedPieceState.None) {
                Unit unit = selectedPiece.GetComponent<Unit>();
                if (Input.GetKeyDown(KeyCode.M) && !unit.HasMoved) {
                    EncounterUtils.Highlight(SelectedPieceMoveRange, Color.blue);
                    SelectedPieceState = SelectedPieceState.Moving;
                }
                if (Input.GetKeyDown(KeyCode.A) && !unit.HasActed) {
                    EncounterUtils.Highlight(SelectedPieceAttackRange, Color.red);
                    SelectedPieceState = SelectedPieceState.Attacking;
                }
                if (Input.GetKeyDown(KeyCode.K) && !unit.HasActed) {
                    DeathEffect dE = new DeathEffect();
                    SelectedAbility = dE;
                    //Debug.Log(dE.Range);
                    SelectedAbilityRange = EncounterUtils.FindTilesInRange(unit.Tile, 4, (Tile t) => { return 1; });

                    EncounterUtils.Highlight(SelectedAbilityRange, Color.green);
                    SelectedPieceState = SelectedPieceState.Casting;
                }
                if (Input.GetKeyDown(KeyCode.P) && !unit.HasActed) {
                    PoisonEffect pE = new PoisonEffect(3);
                    SelectedAbility = pE;
                    //Debug.Log(pE.Range);
                    SelectedAbilityRange = EncounterUtils.FindTilesInRange(unit.Tile, 4, (Tile t) => { return 1; });

                    EncounterUtils.Highlight(SelectedAbilityRange, Color.green);
                    SelectedPieceState = SelectedPieceState.Casting;
                }
            }
        }

        public Effect SelectedAbility { get; set; }

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
            if (TurnCounter > 0) {
                OnTurnStart += ExecuteAI;
            }
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
            Teams = new Dictionary<int, List<Unit>>();
            for (int row = 0; row < GRID_HEIGHT; row++) {
                for (int column = 0; column < GRID_WIDTH; column++) {
                    
                    GameObject gridSquare = Instantiate(Resources.Load("Prefabs/target") as GameObject);
                    Tile tile = gridSquare.AddComponent<Tile>();
                    gridSquare.name = "Tile[" + column + "," + row + "]";
                    tile.Initialize(this, column, -row, -1, wallLayout[wallIndex]);
                    if (wallIndex < wallLayout.Length - 1) {
                        wallIndex++;
                    }
                    tiles[column, row] = tile;
                    wallLayoutArray[column, row] = ".";
                }
            }
            
            Debug.Log("Press NumberPad Enter to change turns");
            Debug.Log("All initially spawned units can't move at first and units can only move on their turn and can only move and act once");
            

        }
        public GameObject selectedPiece {
            get {
                return _selectedPiece;
            }
            set {
                _selectedPiece = value;
                EncounterUtils.unHighlight(SelectedPieceMoveRange);
                EncounterUtils.unHighlight(SelectedAbilityRange);
                EncounterUtils.unHighlight(_selectedAbilityZone);
                if (_selectedPiece != null) {
                    SelectedPieceMoveRange = EncounterUtils.FindTilesInRange(selectedPiece.GetComponent<Unit>().Tile, selectedPiece.GetComponent<Unit>().MoveSpeed, (Tile) => Tile.Terrain.Cost);
                    SelectedPieceAttackRange = EncounterUtils.FindTilesInRange(selectedPiece.GetComponent<Unit>().Tile, 1, (Tile) => 1);
                }
                else {
                    SelectedPieceMoveRange = default(List<Tile>);
                    SelectedPieceAttackRange = default(List<Tile>);
                    SelectedAbilityRange = default(List<Tile>);
                    SelectedPieceState = SelectedPieceState.None;
                }
                ;
            }
        }

        public List<Tile> SelectedPieceMoveRange { get; set; }

        public List<Tile> SelectedPieceAttackRange { get; set; }

        public List<Tile> SelectedAbilityRange { get; set; }

        public List<Tile> SelectedAbilityZone { 
            get {
                return _selectedAbilityZone; 
            } 
            set {
                if (_selectedAbilityZone != null) {
                    EncounterUtils.unHighlight(_selectedAbilityZone);
                    EncounterUtils.Highlight(SelectedAbilityRange, Color.green);
                }
                
                _selectedAbilityZone = value;
            } 
        }

        public void ExecuteAI(object sender, Grid.TurnEventArgs turnEvent) {
            if (turnEvent.Team == 1) {
                foreach (Unit enemy in Teams[1]) {
                    Debug.Log("Executing AI");
                    enemy.gameObject.GetComponent<EnemyAI>().Act();
                }
            }
        }

        /*public void Highlight(List<Tile> tiles, Color color) {
            EncounterUtils.ApplyTileEffects(tiles, (Tile) => ToggleHighlightEffect(Tile, true, color));
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

        public void ApplyTileEffects(List<Tile> tileZone, Action<Tile> applyEffect) {
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
        }*/

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
