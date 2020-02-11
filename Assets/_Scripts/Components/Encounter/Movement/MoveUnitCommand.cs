﻿using Components;

/*
 * Class created before we decided to do ecs and make movement system and command system manage  movement
 */

public class MoveUnitCommand : Command {
    public readonly Unit unit;
    public readonly Tile tile;
    private Tile tileBefore;

    public MoveUnitCommand(Unit unit, Tile tile) {
        this.unit = unit;
        this.tile = tile;
    }

    public override void execute() {
        tileBefore = unit.Tile;
        unit.MoveTo(tile);
    }

    public void undo() {
        unit.MoveTo(tileBefore);
    }
}