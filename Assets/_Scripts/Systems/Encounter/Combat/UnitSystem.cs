using Components;

public class UnitSystem {

    public Unit createUnit(Character character, Tile tile) {
        if (character != null && tile != null && tile.BoardPiece == null) {
            return new Unit(character, tile);
        }
        else {
            return null;
        }
    }

    public Character getCharacter(Unit unit) {
        return unit.Character;
    }
}