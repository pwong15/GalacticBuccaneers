using System;
using System.Collections.Generic;
using UnityEngine;
using Views;

namespace Utilitys {

    public static class EncounterUtils {

        public struct Point {
            public int x;
            public int y;

            public Point(int x, int y) {
                this.x = x;
                this.y = y;
            }
        }

        public enum Difficulty {
            Easy,
            Medium,
            Hard
        }

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
            //tile.Cost = 0;
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

        public static void SwapUnits(Unit a, Unit b) {
            Tile tileA = a.Tile;
            Tile tileB = b.Tile;
            a.Tile = tileB;
            b.Tile = tileA;
            tileA.BoardPiece = a.gameObject;
            tileB.BoardPiece = b.gameObject;
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

            while (currentTile != start) {
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

        private static List<Point> playerSpawn = new List<Point>() {
            new Point(15, 1),
            new Point(14, 1),
            new Point(16, 2),
            new Point(13, 2)
        };

        private static List<Point> enemySpawn = new List<Point>() {
            new Point(10, 14),
            new Point(14, 14),
            new Point(18, 14),
            new Point(10, 17),
            new Point(18, 17),
            new Point(14, 17),
            new Point(9, 22),
            new Point(12, 22)
        };

        public static List<Point> GetSpawnPoints(Team team, String mapName, Difficulty difficulty) {
            if (team == Team.Player) {
                switch (mapName) {
                    case "Layout1":
                        if (difficulty == Difficulty.Easy) {
                            return playerSpawn;
                        } else if (difficulty == Difficulty.Medium) {
                            return playerSpawn;
                        } else {
                            return playerSpawn;
                        }
                }
            } else {
                Debug.Log("Creating enemy spawn points");
                switch (mapName) {
                    
                    case "Layout1":
                        if (difficulty == Difficulty.Easy) {
                            return enemySpawn;
                        } else if (difficulty == Difficulty.Medium) {
                            return enemySpawn;
                        } else {
                            return enemySpawn;
                        }
                }
            }
            return null;
        }
    }
}