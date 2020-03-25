namespace Views {

    public interface BoardPiece {
        Tile Tile { get; set; }
        int MoveSpeed { get; set; }
    }
}