using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Views;
public abstract class Objective {
    public abstract string Description { get; }

    public abstract bool Success { get; }

    public override string ToString() {
        return Description;
    }

}

public class ReachDestination : Objective {
    public override string Description { get { return "Reach " + Destination; } }
    public Tile Destination { get; set; }

    private bool success;
    public override bool Success { 
        get {
            if (success) {
                return success;
            }
            Unit unit = null;
            if (Destination.BoardPiece != null) {
                unit = Destination.BoardPiece.GetComponent<Unit>();
            }
            if(unit != null && unit.Team == Team.Player) {
                success = true;
                return success;
            }
            return false;
        }
    }

    public ReachDestination(Tile tile) {
        this.Destination = tile;
        success = false;
    }
}

public class KillTarget : Objective {
    public override string Description { get { return "Kill " + Target; } }

    public Unit Target { get; set; }

    private bool success;
    public override bool Success {
        get {
            if (success) {
                return success;
            }
            
            if (Target.HasDied) {
                success = true;
                return success;
            }
            return false;
        }
    }
    public KillTarget(Unit unit) {
        Target = unit;
    }

}
