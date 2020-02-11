using Components;
using System.Collections.Generic;

public class MovementSystem {

    public bool IsValidSpace(Tile target) {
        return target.BoardPiece == null && target.Terrain.IsWalkable;
    }

    // This a combination of CreateMoveCommand and Move functions commented below just for testing purposes.
    public void Move(BoardPiece boardPiece, Tile destination) {
        if (boardPiece != null && IsValidSpace(destination)) {
            List<Tile> validMoves = boardPiece.Tile.grid.FindTilesInRange(boardPiece.Tile, boardPiece.MoveSpeed, (tile) => tile.Terrain.Cost);
            if (validMoves != null && validMoves.Contains(destination)) {
                boardPiece.Tile.BoardPiece = null;
                boardPiece.Tile = destination;
                destination.BoardPiece = boardPiece;
            }
        }
    }

    //public void Move(BoardPiece boardPiece, Tile destination) {
    //    boardPiece.Tile.BoardPiece = null;
    //    boardPiece.Tile = destination;
    //    destination.BoardPiece = boardPiece;
    //}

    //public MoveCommand CreateMoveCommand(BoardPiece boardPiece, Tile destination) {
    //    if (boardPiece != null && IsValidSpace(destination)) {
    //        List<Tile> validMoves = boardPiece.Tile.grid.FindValidMoves(boardPiece);
    //        if (validMoves != null && validMoves.Contains(destination)) {
    //            return new MoveCommand(boardPiece, destination);
    //        }
    //    }
    //    return default(MoveCommand);
    //}
}