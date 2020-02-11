namespace Components {

    public class Terrain {

        public enum Sprite {
            None = 1,
            Puddle = 2,
            Wall,
            ClosedDoor,
            OpenDoor
        }

        public Sprite Type { get; set; }

        public int Cost {
            get {
                if (Type == Sprite.Puddle) {
                    return 2;
                }
                return 1;
            }
            set {; }
        }

        public bool IsWalkable {
            get {
                return Type != Sprite.Wall && Type != Sprite.ClosedDoor;
            }
            set {
                IsWalkable = value;
            }
        }

        public Terrain(Sprite type) {
            this.Type = type;
        }

        public override string ToString() {
            return Type.ToString();
        }
    }
}