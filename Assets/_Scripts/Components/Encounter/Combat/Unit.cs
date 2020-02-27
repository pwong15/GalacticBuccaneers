﻿namespace Components {

    public class Unit : BoardPiece {
        public Character Character { get; }
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

        public int Team { get { return Character.Name % 2; } set {; } }

        public int MoveSpeed { get; set; }

        public Unit(Character character, Tile tile) {
            this.Character = character;
            this.MoveSpeed = character.MoveSpeed;
            tile.BoardPiece = this;
            this.Tile = tile;
            _hasActed = true;
            HasMoved = true;
        }

        public void MoveTo(Tile targetLocation) {
            Tile.BoardPiece = null;
            targetLocation.BoardPiece = this;
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