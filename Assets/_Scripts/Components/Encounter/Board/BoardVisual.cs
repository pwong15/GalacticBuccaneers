using System.Collections.Generic;
using UnityEngine;

namespace Components {

    public class BoardVisual : MonoBehaviour {

        [System.Serializable]
        public struct TerrainSpriteUV {
            public Terrain.Sprite terrainSprite;
            public Vector2Int uv00Pixels;
            public Vector2Int uv11Pixels;
        }

        private struct UVCoords {
            public Vector2 uv00;
            public Vector2 uv11;
        }

        [SerializeField] private TerrainSpriteUV[] terrainSpriteUVs;
        private Grid<Tile> grid;
        private Mesh mesh;
        private bool updateMesh;
        private Dictionary<Terrain.Sprite, UVCoords> uvCoordsDictionary;

        private void Awake() {
            mesh = new Mesh();

            GetComponent<MeshFilter>().mesh = mesh;

            Texture texture = GetComponent<MeshRenderer>().material.mainTexture;
            float textureWidth = texture.width;
            float textureHeight = texture.height;
            uvCoordsDictionary = new Dictionary<Terrain.Sprite, UVCoords>();

            foreach (TerrainSpriteUV terrainSpriteUV in terrainSpriteUVs) {
                uvCoordsDictionary[terrainSpriteUV.terrainSprite] = new UVCoords {
                    uv00 = new Vector2(terrainSpriteUV.uv00Pixels.x / textureWidth, terrainSpriteUV.uv00Pixels.y / textureHeight),
                    uv11 = new Vector2(terrainSpriteUV.uv11Pixels.x / textureWidth, terrainSpriteUV.uv11Pixels.y / textureHeight),
                };
            }
        }

        public void SetGrid(Grid<Tile> grid) {
            this.grid = grid;
            UpdateBoardVisual();
            grid.OnGridObjectChanged += Grid_OnGridValueChanged;
        }

        private void Grid_OnGridValueChanged(object sender, Grid<Tile>.OnGridObjectChangedEventArgs e) {
            updateMesh = true;
        }

        // Start is called before the first frame update
        private void Start() {
        }

        // Update is called once per frame
        private void Update() {
        }

        private void LateUpdate() {
            if (updateMesh) {
                updateMesh = false;
                UpdateBoardVisual();
            }
        }

        private void UpdateBoardVisual() {
            MeshUtils.CreateEmptyMeshArrays(grid.width * grid.height, out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

            for (int x = 0; x < grid.width; x++) {
                for (int y = 0; y < grid.height; y++) {
                    int index = x * grid.height + y;
                    Vector3 quadSize = new Vector3(1, 1) * grid.cellSize;
                    Tile tile = grid.GetGridObject(x, y);
                    Terrain.Sprite sprite = tile.Terrain.Type;
                    Vector2 uv1 = Vector2.one;
                    Vector2 uv2 = Vector2.one;

                    UVCoords uvCoords = uvCoordsDictionary[sprite];

                    uv1 = uvCoords.uv00;
                    uv2 = uvCoords.uv11;

                    /*if (sprite == Terrain.Sprite.None) {
                        uv1 = Vector2.zero;
                        uv2 = Vector2.one;
                    }
                    else if (sprite == Terrain.Sprite.Wall) {
                        uv1 = Vector2.zero;
                        uv2 = Vector2.zero;
                    }*/
                    MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, grid.GetWorldPosition(x, y) + quadSize * .5f, 0f, quadSize, uv1, uv2);
                }
            }
            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;
        }
    }
}