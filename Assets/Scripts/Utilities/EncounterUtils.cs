using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Views;

namespace Utilitys {
    public static class EncounterUtils {
        public static void Highlight(List<Tile> tiles, Color color) {
            ApplyTileEffects(tiles, (Tile) => ToggleHighlightEffect(Tile, true, color));
            
        }

        public static void unHighlight(List<Tile> tiles) {
            ApplyTileEffects(tiles, (Tile) => ToggleHighlightEffect(Tile, false, Color.white));
           
        }

        public static void SetTileCost(Tile tile, int cost) {
            tile.Cost = cost;
        }

        // Used to find tiles that a board piece can reach with their movespeed. So far uses manhatten distance but must change in order to account for obstacles such as walls.
        public static List<Tile> FindTilesInRange(Tile tile, int range, Func<Tile, int> getTileCost) {
            List<Tile> tilesInRange = new List<Tile>();
            Queue<Tile> queue = new Queue<Tile>();
            List<Tile> visitedList = new List<Tile>();
            Tile currentTile;
            int neighborCost;
            tile.Cost = 0;
            queue.Enqueue(tile);
            while (queue.Count > 0) {
                currentTile = queue.Dequeue();
                foreach (Tile neighbor in currentTile.GetNeighbors()) {
                    if (!visitedList.Contains(neighbor)) {
                        visitedList.Add(neighbor);
                        neighborCost = getTileCost(neighbor) + currentTile.Cost;
                        if (neighbor.Terrain.IsWalkable && neighborCost <= range) {
                            neighbor.Cost = neighborCost;
                            neighbor.Parent = currentTile;
                            tilesInRange.Add(neighbor);
                            queue.Enqueue(neighbor);
                        }
                    }
                }
            }
            foreach (Tile inRangeTile in tilesInRange) {
                inRangeTile.Cost = 0;
            }
            return tilesInRange;
        }

        public static List<Tile> PathFinding(Tile start, Tile dest) {
            List<Tile> path = new List<Tile>();
            List<Tile> visited = new List<Tile>();
            Tile currentTile = start;
            Queue<Tile> queue = new Queue<Tile>();
            visited.Add(start);
            queue.Enqueue(start);
            while (queue.Count > 0) {
                currentTile = queue.Dequeue();
                if (currentTile == dest) {
                    Debug.Log("Current Tiel" + currentTile);
                    break;
                }
                foreach (Tile neighbor in currentTile.GetNeighbors()) {
                    if (!visited.Contains(neighbor)) {       
                        neighbor.Parent = currentTile;
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }
            /*if (!visited.Contains(dest)) {
                foreach (Tile tile in visited) {
                    tile.Parent = null;
                }
                return null;
            }*/
            while (currentTile != start) {
                Debug.Log("Building path");
                path.Insert(0, currentTile);
                currentTile = currentTile.Parent;
            }
            foreach (Tile tile in visited) {
                tile.Parent = null;
            }
            return path;
        }

        public static void ApplyTileEffects(List<Tile> tileZone, Action<Tile> applyEffect) {
            if (tileZone == null) {
                return;
            }
            foreach (Tile tile in tileZone) {
                applyEffect(tile);
            }
        }

        // Used to highlight a list of tiles (Tile Zone). As of now used to highlight/unhighlight the tiles in range of the selected board piece
        private static void ToggleHighlightEffect(Tile tile, bool toggle, Color color) {
            GameObject tileObject = tile.gameObject.transform.GetChild(0).gameObject;
            if (toggle) {
                tileObject.GetComponent<SpriteRenderer>().color = color;
            }
            tileObject.GetComponent<Renderer>().enabled = toggle;
        }


    }
}
