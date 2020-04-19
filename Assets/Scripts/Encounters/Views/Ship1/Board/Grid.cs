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

        public int GRID_HEIGHT;
        public int GRID_WIDTH;
        public string MAP_NAME;
        public Unit SelectedDeploymentUnit { get; set; }

        public event EventHandler<TurnEventArgs> OnTurnStart;
        public event EventHandler<TurnEventArgs> OnTurnEnd;
        public event EventHandler<MapEventArgs> OnMapOver;
        public Dictionary<Team, List<Unit>> Teams;
        public class TurnEventArgs : EventArgs {
            public Team Team;
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

        private List<Tile> pointsToTiles(List<EncounterUtils.Point> points) {
            List<Tile> mapTiles = new List<Tile>();
            foreach (EncounterUtils.Point point in points) {
                mapTiles.Add(tiles[point.x, point.y]);
            }
            return mapTiles;
        }
        void Start() {
            CreateGrid();
            List<EncounterUtils.Point> playerSpawnPoints = EncounterUtils.GetSpawnPoints(Team.Player, MAP_NAME, EncounterUtils.Difficulty.Easy);
            List<EncounterUtils.Point> enemySpawnPoints = EncounterUtils.GetSpawnPoints(Team.Player, MAP_NAME, EncounterUtils.Difficulty.Easy);
            playerSpawnArea = pointsToTiles(playerSpawnPoints);
            enemySpawnArea = pointsToTiles(enemySpawnPoints);
            foreach (Tile tile in playerSpawnArea) {
                tile.SpawnUnit(Team.Player);
            }
            EncounterUtils.Highlight(enemySpawnArea, Color.red);
        }

        void Update() {
            CheckMapEndConditions();
            if (Input.GetKeyDown("enter")) {
                EndTurn();
                StartTurn();
            }
            if (Input.GetKeyDown("q")) {
                SceneController.LoadScene("GalaxyMap");
            }

            EncounterUtils.Highlight(playerSpawnArea, Color.yellow);
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
            return new Character("Unit " + id, (Team)(id++ % numOfTeam), 100, 100, 10, 5, 2, 3);
        }

        public void SpawnUnit(int x, int y) {
            GameObject unitObject = Instantiate(Resources.Load("Prefabs/target") as GameObject);
            Unit unit = unitObject.AddComponent<Unit>();
            unit.Initialize(RetrieveCharacter(), tiles[x, y]);
        }

        private void CheckMapEndConditions() {
            if (TurnCounter > 0 && WinCondition()) {
                OnMapOver?.Invoke(this, new MapEventArgs { gameOver = false });
                OnMapOver -= MapOverHandler;
            } else if (TurnCounter > 0 && LoseCondition()) {
                OnMapOver?.Invoke(this, new MapEventArgs { gameOver = true });
                OnMapOver -= MapOverHandler;
            }
        }

        private bool WinCondition() {
            if (TurnCounter > 0 && HasBeenEliminated(Team.Enemy)) {
                return true;
            }
            return false;


        }

        private bool HasBeenEliminated(Team side) {
            List<Unit> team = Teams[side];
            bool allEliminated = true;
            foreach (Unit unit in team) {
                if (unit.HasDied == false) {
                    allEliminated = false;
                }
            }
            return allEliminated;
        }

        private bool LoseCondition() {
            if (TurnCounter > 0 && HasBeenEliminated(Team.Player)) {
                return true;
            }
            return false;
        }

        public void MapOverHandler(object sender, MapEventArgs e) {
            if (e.gameOver) {
                Debug.Log("Game Over");
            } else {
                Debug.Log("Victory");
            }
        }

         

        public void SpawnUnit(Tile tile) {
            SpawnUnit(tile.xCoord, tile.yCoord);
        }

        private void StartTurn() {
            Team team = TurnCounter % 2 == 1 ? Team.Player: Team.Enemy;
            Debug.Log("Team " + team + " Turn " + TurnCounter / 2);
            if (TurnCounter == 1) {
                selectedPiece = null;
                OnTurnStart += ExecuteAI;
            }
            OnTurnStart?.Invoke(this, new TurnEventArgs { Team = team });
        }

        private void EndTurn() {
            Team team = TurnCounter % 2 == 1 ? Team.Player : Team.Enemy;
            Debug.Log("Ending Turn");
            TurnCounter += 1;
            OnTurnEnd?.Invoke(this, new TurnEventArgs { Team = team });
        }

        private List<Tile> playerSpawnArea;
        private List<Tile> enemySpawnArea;
        private void CreateGrid() {
            wallLayoutArray = new string[GRID_WIDTH, GRID_HEIGHT];
            tiles = new Tile[GRID_WIDTH, GRID_HEIGHT];
            Debug.Log(MAP_NAME);
            string wallLayoutFile = Directory.GetCurrentDirectory() + "\\Assets\\Resources\\BoardTxtFiles\\" + MAP_NAME + ".txt";
            string wallLayout = string.Join("", File.ReadAllLines(wallLayoutFile));
            int wallIndex = 0;
            numOfTeam = 2;
            Debug.Log("Made teams");
            
            TurnCounter = 0;
            Teams = new Dictionary<Team, List<Unit>>();
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
            OnMapOver += MapOverHandler;

            Debug.Log("Press NumberPad Enter to change turns");
            Debug.Log("All initially spawned units can't move at first and units can only move on their turn and can only move and act once");
            

        }


        public GameObject selectedPiece {
            get {
                return _selectedPiece;
            }
            set {
                Unit unit = null;
                if (_selectedPiece != null) {
                    unit = _selectedPiece.GetComponent<Unit>();
                    unit.actionMenu.HidePanel();
                }
                _selectedPiece = value;
                EncounterUtils.unHighlight(SelectedPieceMoveRange);
                EncounterUtils.unHighlight(SelectedAbilityRange);
                EncounterUtils.unHighlight(_selectedAbilityZone);
                if (_selectedPiece != null) {
                    unit = _selectedPiece.GetComponent<Unit>();
                    SelectedPieceMoveRange = EncounterUtils.FindTilesInRange(unit.Tile, unit.MoveSpeed, (Tile) => Tile.Terrain.Cost);
                    SelectedPieceAttackRange = EncounterUtils.FindTilesInRange(unit.Tile, 1, (Tile) => 1);
                    if (TurnCounter != 0) {
                        unit.actionMenu.DisplayPanel();
                    }
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
            if (turnEvent.Team == Team.Enemy) {
                foreach (Unit enemy in Teams[Team.Enemy]) {
                    Debug.Log("Executing AI");
                    enemy.gameObject.GetComponent<EnemyAI>().Act();
                }
                EndTurn();
                StartTurn();
            }
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
