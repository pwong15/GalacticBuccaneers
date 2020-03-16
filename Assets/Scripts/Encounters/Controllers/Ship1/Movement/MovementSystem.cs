using Components;
using System.Collections.Generic;
using UnityEngine;

public class MovementSystem {

    public bool IsValidSpace(Tile target) {
        return target.BoardPiece == null && target.Terrain.IsWalkable;
    }

    // This a combination of CreateMoveCommand and Move functions commented below just for testing purposes.
    public bool Move(Components.Board board, BoardPiece boardPiece, Tile destination) {
        if (boardPiece != null && IsValidSpace(destination)) {
            List<Tile> validMoves = board.FindTilesInRange(boardPiece.Tile, boardPiece.MoveSpeed, (tile) => tile.Terrain.Cost);
            if (validMoves != null && validMoves.Contains(destination)) {
                boardPiece.Tile.BoardPiece = null;
                boardPiece.Tile = destination;
                //destination.BoardPiece = boardPiece;
                return true;
            }
        }
        return false;
    }

    public void MoveUnit(Components.Board board, Unit unit, Tile destination) {
        if (!unit.HasMoved) {
           /* if (Move(board, unit, destination)) {
                unit.HasMoved = true;   
            }*/

        } else {
            Debug.Log(unit.Character.Name + " has already moved");
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