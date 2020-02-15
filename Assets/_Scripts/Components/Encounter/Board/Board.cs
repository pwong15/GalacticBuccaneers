using Components;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Board {
    private readonly Grid<Tile> _grid;
    private BoardPiece _selectedPiece;
    private List<Tile> _selectedPieceRange;
    public int TurnCounter { get; }

    public int numOfTeam { get; }
    public Dictionary<int, List<Unit>> Teams { get; }

    public Grid<Tile> grid { get { return _grid; } }
    public int width { get { return _grid.width; } }
    public int height { get { return _grid.height; } }
    public float cellSize { get { return _grid.cellSize; } }

    public BoardPiece selectedPiece {
        get {
            return _selectedPiece;
        }
        set {
            _selectedPiece = value;
            if (_selectedPiece != null) {
                selectedPieceRange = FindTilesInRange(selectedPiece.Tile, selectedPiece.MoveSpeed, (Tile) => Tile.Terrain.Cost);
            } else {
                selectedPieceRange = default(List<Tile>);
            }
            ;
        }
    }

    public Board(Grid<Tile> grid) {
        this._grid = grid;
    }

    public List<Tile> selectedPieceRange {
        get {
            return _selectedPieceRange;
        }
        set {
            if (_selectedPieceRange != null) {
                Action<Tile> unHighlight = (Tile) => ToggleHighlightEffect(Tile, false);
                Action<Tile> clearPathFindingCost = (Tile) => SetTileCost(Tile, 0);
                ApplyTileEffects(_selectedPieceRange, unHighlight);
            }
            _selectedPieceRange = value;
            Action<Tile> Highlight = (Tile) => ToggleHighlightEffect(Tile, true);
            ApplyTileEffects(_selectedPieceRange, Highlight);
        }
    }

    public void SetTileCost(Tile tile, int cost) {
        tile.Cost = cost;
    }

    public List<Tile> FindTilesInRange(Tile tile, int range, Func<Tile, int> getTileCost) {
        List<Tile> tilesInRange = new List<Tile>();
        Queue<Tile> queue = new Queue<Tile>();
        List<Tile> visitedList = new List<Tile>();
        Tile currentTile;
        int neighborCost;
        tile.Cost = 0;
        queue.Enqueue(tile);
        while (queue.Count > 0) {
            currentTile = queue.Dequeue();
            foreach (Tile neighbor in currentTile.Neighbors) {
                if (!visitedList.Contains(neighbor)) {
                    visitedList.Add(neighbor);
                    neighborCost = getTileCost(neighbor) + currentTile.Cost;
                    if (neighbor.Terrain.IsWalkable && neighborCost <= range) {
                        neighbor.Cost = neighborCost;
                        tilesInRange.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }
        return tilesInRange;
    }

    private void ApplyTileEffects(List<Tile> tileZone, Action<Tile> applyEffect) {
        if (tileZone == null) {
            return;
        }
        foreach (Tile tile in tileZone) {
            applyEffect(tile);
            _grid.TriggerGridObjectChanged(tile.xCoord, tile.yCoord);
        }
    }

    private void ToggleHighlightEffect(Tile tile, bool toggle) {
        tile.Highlight = toggle;
    }

    public void TriggerTileChange(Tile tile) {
        _grid.TriggerGridObjectChanged(tile.xCoord, tile.yCoord);
    }

    public Tile GetTile(Vector3 mousePosition) {
        return _grid.GetGridObject(mousePosition);
    }

    public Tile GetTile(int x, int y) {
        return _grid.GetGridObject(x, y);
    }

    public Vector3 GetWorldPosition(int x, int y) {
        return _grid.GetWorldPosition(x, y);
    }
}