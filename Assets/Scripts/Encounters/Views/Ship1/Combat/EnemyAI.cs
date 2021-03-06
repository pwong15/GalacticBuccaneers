﻿using UnityEngine;

using Views;
using System.Collections.Generic;
using Utilitys;
using System.Collections;

public class EnemyAI : MonoBehaviour {

    Unit unit;
    List<Tile> AttackRange {
        get {
            Tile dest = unit.destinationTile;
            if (dest == null) {
                dest = unit.Tile;
            }
            return EncounterUtils.FindTilesInRange(dest, attackRange, (Tile) => { return 1; });
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
        return gameObject.GetComponent<Unit>().Tile.gameBoard.Teams[Team.Player];
    }

    public List<Tile> PathToClosestPlayerUnit() {
        List<Unit> playerUnits = getPlayerUnits();
      
        int currentLength = 100;
        Tile aITile = gameObject.GetComponent<Unit>().Tile;
        List<Tile> path = null;
        foreach (Unit player in playerUnits) {
            
            List<Tile> newPath = EncounterUtils.PathFinding(aITile, player.Tile);
            if (newPath == null) {
                Debug.Log("No Path");
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
            if (playerUnit!= null && playerUnit.Team == Team.Player) {
                unitsInRange.Add(playerUnit);
            }
        }
        return unitsInRange;
    }

    public void Act() {
        List<Unit> playerUnitsInRange = getUnitsInRange();
        Debug.Log(unit.Tile);
        
        if (playerUnitsInRange.Count > 0) {
            //Debug.Log("Checking Attack");
            unit.AttackUnit(playerUnitsInRange[0]);
        } else {
            Move();
            //Debug.Log("Executing Movement");
            /*while (unit.moving) {
                float delta = 1 * Time.deltaTime;
            }*/
           
            playerUnitsInRange = getUnitsInRange();
            foreach (Unit player in playerUnitsInRange) {
                Debug.Log(player);
            }
            if (playerUnitsInRange.Count > 0) {
                unit.AttackUnit(playerUnitsInRange[0]);
            }
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
        if (path == null) {
            return;
        }
        path = path.Count < moveRange ? path : path.GetRange(0, moveRange); 
        foreach (Tile tile in path) {
            if (tile.BoardPiece == null) {
                dest = tile;
            }
        }
        if (dest != null) {
            unit.MoveTo(dest);
        }

    }

}
