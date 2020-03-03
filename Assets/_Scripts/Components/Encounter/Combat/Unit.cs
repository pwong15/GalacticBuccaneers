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
                if (_tile != null) {
                    Vector3 tileLocation = value.transform.position;
                    this.transform.position = new Vector3(tileLocation.x, tileLocation.y, tileLocation.z + 1);
                }
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
            if (targetLocation.BoardPiece == null) {
                Tile.BoardPiece = null;
                targetLocation.BoardPiece = this.gameObject;
                Tile = targetLocation;
            }
        }

        // Method will be moved to combat system
        public void AttackUnit(Unit otherUnit) {
            otherUnit.TakeDamage(2 * Character.Attack - otherUnit.Character.Defense);
        }

        // Method will be moved to combat system
        public void TakeDamage(int damageAmount) {
            Character.Health -= damageAmount;
            Debug.Log(Character.Name + " took " + damageAmount + " dmg");
            if (Character.Health <= 0) {
                Debug.Log(Character.Name + " has Died");
                Die();
            }
        }

        private void Die() {
            Tile.BoardPiece = null;
            this.Tile = null;
            GetComponent<Renderer>().enabled = false;
        }

        public override string ToString() {
            return Character.Name.ToString();
        }
    }
}