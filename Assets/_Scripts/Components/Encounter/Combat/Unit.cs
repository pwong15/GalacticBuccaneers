using UnityEngine;

namespace Components {

    public class Unit : MonoBehaviour {
        public Character Character { get; set; }

        private Tile _tile;
        public Tile Tile {
            get {
                return _tile;
            }
            set {
                _tile = value;
                Vector3 tileLocation = value.transform.position;
                this.transform.position = new Vector3(tileLocation.x, tileLocation.y, tileLocation.z + 1);
            }
        }

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
            this.MoveSpeed = character.MoveSpeed;
            this.Tile = tile;
            _hasActed = true;
            HasMoved = true;
            GetComponent<Renderer>().enabled = true;


        }

        public void MoveTo(Tile targetLocation) {
            Tile.BoardPiece = null;
            targetLocation.BoardPiece = this.gameObject;
            Tile = targetLocation;
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