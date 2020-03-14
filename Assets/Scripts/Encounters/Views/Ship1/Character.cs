namespace Components {

    public class Character {
        public string Name { get; }
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }

        public int AttackRange { get; set; }
        public int MoveSpeed { get; set; }

        public Character(string name, int maxHealth, int health, int attack, int defense, int attackRange, int movespeed) {
            this.Name = name;
            this.MaxHealth = maxHealth;
            this.AttackRange = attackRange;
            this.Health = health;
            this.Attack = attack;
            this.Defense = defense;
            this.MoveSpeed = movespeed;
        }
    }
}