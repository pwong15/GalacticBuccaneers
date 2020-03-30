using UnityEngine;

using Views;
using System.Collections.Generic;
using Utilitys;

public class EnemyAI : MonoBehaviour {

    Unit unit;
    List<Tile> AttackRange {
        get {
            return EncounterUtils.FindTilesInRange(gameObject.GetComponent<Unit>().Tile, attackRange, (Tile) => { return 1; });
        }
    }
    int attackRange;
    int moveRange;
    public enum State {
        Attacking,
        Moving,
    }
    // Use this for initialization
    void Start() {
        unit = gameObject.GetComponent<Unit>();
        attackRange = unit.Character.AttackRange;
        moveRange = unit.Character.MoveSpeed;
    }

    // Update is called once per frame
    void Update() {

    }

    public List<Unit> getPlayerUnits() {
        return gameObject.GetComponent<Unit>().Tile.gameBoard.Teams[(unit.Character.Team + 1) % 2];
    }

    public List<Tile> PathToClosestPlayerUnit() {
        List<Unit> playerUnits = getPlayerUnits();
        foreach (Unit unit in playerUnits) {
            Debug.Log(unit);
        }
        int currentLength = 100;
        Tile aITile = gameObject.GetComponent<Unit>().Tile;
        List<Tile> path = null;
        foreach (Unit player in playerUnits) {
            Debug.Log(aITile);
            Debug.Log(player.Tile);
            List<Tile> newPath = EncounterUtils.PathFinding(aITile, player.Tile);
            if (newPath == null) {
                Debug.Log("Wft");
            }
            foreach (Tile tile in newPath) {
                Debug.Log(tile);
            }
            if (newPath != null && newPath.Count < currentLength) {
                path = newPath;
                currentLength = path.Count;
            }
        }
        return path;
    }

    public State GetCurrentState() {
        List<Unit> playerUnits = getPlayerUnits();
        return State.Attacking;
    }

    public List<Unit> getUnitsInRange() {
        List<Unit> unitsInRange = new List<Unit>();
        Unit playerUnit;
        foreach (Tile tile in AttackRange) {
            playerUnit = null;
            if (tile.BoardPiece != null) {
                playerUnit = tile.BoardPiece.GetComponent<Unit>();
            }
            if (playerUnit!= null && playerUnit.Team == 0) {
                unitsInRange.Add(playerUnit);
            }
        }
        return unitsInRange;
    }

    public void Act() {
        List<Unit> playerUnitsInRange = getUnitsInRange();
        bool hasAttacked = false;
        if (playerUnitsInRange.Count > 0) {
            hasAttacked = true;
            unit.AttackUnit(playerUnitsInRange[0]);
        } else {
            Move();
        }
    
    }

    public void Attack() {
        Unit playerUnit;
        foreach (Tile tile in AttackRange) {
            playerUnit = tile.BoardPiece.GetComponent<Unit>();
            if (playerUnit.Team == 0) {
                unit.AttackUnit(playerUnit);
                break;
            }
        }
    }

    public void Move() {
        List<Tile> path = PathToClosestPlayerUnit();
        Tile dest = null;
        path = path.Count < moveRange ? path : path.GetRange(0, moveRange); 
        foreach (Tile tile in path) {
            if (tile.BoardPiece == null) {
                dest = tile;
            }
        }
        if (dest != null) {
            Debug.Log("Trying to move to " + dest);
            Debug.Log(unit.HasMoved);
            unit.MoveTo(dest);
        }

    }

}
