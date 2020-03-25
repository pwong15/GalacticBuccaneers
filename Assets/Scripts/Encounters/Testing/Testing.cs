using Views;
using UnityEngine;
using Utilitys;

/*public class Testing : MonoBehaviour {
    private Components.Board board;

    // Wasn't sure how nesting of systems is suppose to work and just initialized them to test
    private TileSystem tileSystem;

    //[SerializeField] private BoardVisual boardVisual;
    private BoardSystem boardSystem;
    private BoardPieceSystem boardPieceSystem;
    private UnitSystem unitSystem;
    private MovementSystem movementSystem;
    private CharacterSystem characterSystem;
    private CombatSystem combatSystem;

    private void Start() {
        boardSystem = new BoardSystem();
        board = boardSystem.CreateBoard(30, 20, 30f);
        boardVisual.SetGrid(board);
        tileSystem = new TileSystem();
        boardPieceSystem = new BoardPieceSystem();
        unitSystem = new UnitSystem();
        movementSystem = new MovementSystem();
        characterSystem = new CharacterSystem();
        combatSystem = new CombatSystem();
    }

    // Update is called once per frame
    private void Update() {
        Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
        Tile tile = board.GetTile(mouseWorldPosition);
        if (tile == default(Tile)) { return; }
        // Spawn a unit on the board if the mouse is hovered over valid space
        if (Input.GetKeyDown(KeyCode.S)) {
            Character character = characterSystem.RetrieveCharacter();
            if (movementSystem.IsValidSpace(tile)) {
                board.createUnit(character, tile);
                tileSystem.updateTile(tile);
            }
        }

        // Create a wall
        if (Input.GetKey(KeyCode.W)) {
            tile.SetTerrainSprite(Components.Terrain.Sprite.Wall);
        }
        if (Input.GetKey(KeyCode.C)) {
            tile.SetTerrainSprite(Components.Terrain.Sprite.ClosedDoor);
        }
        if (Input.GetKey(KeyCode.O)) {
            tile.SetTerrainSprite(Components.Terrain.Sprite.OpenDoor);
        }
        if (Input.GetKey(KeyCode.P)) {
            tile.SetTerrainSprite(Components.Terrain.Sprite.Puddle);
        }
        if (Input.GetKey(KeyCode.Space)) {
            tile.SetTerrainSprite(Components.Terrain.Sprite.None);
        }

        // Select board piece
        if (Input.GetMouseButtonDown(1)) {
            board.selectedPiece = tile.BoardPiece;
            //Debug.Log(board.selectedPiece);
        }

        // Making a unit attack another unit
        if (board.selectedPiece != null && Input.GetKey(KeyCode.A)) {
            if (board.selectedPiece is Unit && tile.BoardPiece is Unit) {
                combatSystem.Attack(board, (Unit)board.selectedPiece, (Unit)tile.BoardPiece);
                board.selectedPiece = null;
            }
        }

        if (Input.GetKeyDown("enter")) {
            board.EndTurn();
            board.StartTurn();
        }

        // Move the selected board piece
        if (board.selectedPiece != null && Input.GetMouseButtonDown(0)) {
            Debug.Log(board.selectedPiece);
            Tile selectedTile = board.selectedPiece.Tile;
            if (board.selectedPiece is Unit) {
                movementSystem.MoveUnit(board, (Unit) board.selectedPiece, tile);
                tileSystem.updateTile(selectedTile);
                tileSystem.updateTile(tile);
                board.selectedPiece = null;
            }
        }

    }
}*/