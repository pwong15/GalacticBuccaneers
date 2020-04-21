using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GalaxyMap {
    public class FogSquare : global::GridSquare {
        protected Grid grid;


        void Start() {
            rend = GetComponent<Renderer>();
            //this.rend.material.color = Color.red;
        }

        public void Initialize(Grid grid, int xLocation, int yLocation, int zLocation) {
            this.xCoordf = xLocation;
            this.yCoordf = yLocation;
            this.zCoordf = zLocation;

            this.column = xLocation;
            this.row = -yLocation;

            this.grid = grid;


            this.transform.position = new Vector3(xCoordf, yCoordf, zCoordf);
        }
    }
}