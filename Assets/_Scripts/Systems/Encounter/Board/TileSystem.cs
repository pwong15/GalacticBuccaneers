using Components;

public class TileSystem {

    public void updateTile(Tile tile) {
        tile.grid.TriggerGridObjectChanged(tile.xCoord, tile.yCoord);
    }
}