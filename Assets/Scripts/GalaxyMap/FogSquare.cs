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

        //public void Enable() {
        //    this.rend.material.color = Color.red;
        //}

        public void Initialize(Grid grid, int xLocation, int yLocation, int zLocation) {
            this.xCoordf = xLocation;
            this.yCoordf = yLocation;
            this.zCoordf = zLocation;

            this.column = xLocation;
            this.row = -yLocation;

            this.grid = grid;
            //this.gameObject.AddComponent(typeof(BoxCollider));
            //this.transform.Rotate(0, 0, -25.742f, Space.World);

            this.transform.position = new Vector3(xCoordf, yCoordf, zCoordf);
        }
    }
}