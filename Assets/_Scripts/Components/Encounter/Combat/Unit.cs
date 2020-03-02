using UnityEngine;

namespace Components {

    public class Unit : MonoBehaviour {
        public Character Character { get; set; }
        public Tile Tile { get; set; }

        public bool HasMoved { get; set; }

        private bool _hasActed;

        public bool HasActed {
            get { return _hasActed; }
            set {
                HasMoved = value;
                _hasActed = value;
            }
        }

        public int Team { get { return 0; } set {; } }

        public int MoveSpeed { get; set; }

        public void Initialize(Character character, Tile tile) {
            this.Character = character;
            tile.BoardPiece = this.gameObject;
            Vector3 tileLocation = tile.transform.position;
            this.transform.position = new Vector3(tileLocation.x, tileLocation.y, tileLocation.z + 1);
            this.MoveSpeed = character.MoveSpeed;
            this.Tile = tile;
            _hasActed = true;
            HasMoved = true;
            GetComponent<Renderer>().enabled = true;


        }

        public void MoveTo(Tile targetLocation) {
            Tile.BoardPiece = null;
            targetLocation.BoardPiece = this.gameObject;
            Components.Grid grid = Tile.grid;
            grid.TriggerGridObjectChanged(Tile.xCoord, Tile.yCoord);
            Tile = targetLocation;
            grid.TriggerGridObjectChanged(targetLocation.xCoord, targetLocation.yCoord);
        }

        // Method will be moved to combat system
        public void AttackUnit(Unit otherUnit) {
            otherUnit.TakeDamage(Character.Attack - otherUnit.Character.Defense);
        }

        // Method will be moved to combat system
        public void TakeDamage(int damageAmount) {
            Character.Health -= damageAmount;
        }

        public override string ToString() {
            return Character.Name.ToString();
        }
    }
}