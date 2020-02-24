using Components;
using UnityEngine;

public class BoardSystem {

    public Components.Board CreateBoard(int width, int height, float cellSize) {
        return new Components.Board(new Grid<Tile>(width, height, cellSize, Vector3.zero, (Grid<Tile> g, int x, int y) => new Tile(g, x, y)));
    }
}