using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Views {

    [Serializable]
    public class Character {

        // Serializor must have instances to serialize can not just use property methods, hence the explicit get+set methods

        [SerializeField] private string name;
        public string Name { get { return name; } }

        [SerializeField] private Team team;
        public Team Team { get { return team; } set { team = value;} }

        [SerializeField] private int maxHealth;
        public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }

        [SerializeField] private int health;
        public int Health { get { return health; } set { health = value; } }

        [SerializeField] private int attack;
        public int Attack { get { return attack; } set { attack = value; } }

        [SerializeField] private int defense;
        public int Defense { get { return defense; } set { defense = value; } }

        [SerializeField] private int attackRange;
        public int AttackRange { get { return attackRange; } set { attackRange = value; } }

        [SerializeField] private int moveSpeed;
        public int MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }

        [SerializeField] private float injuryMultiplier;

        public float InjuryMultiplier { get { return injuryMultiplier; } set { injuryMultiplier = value;  } }

        [SerializeField] private float hitMultiplier;

        public float HitMultiplier { get { return HitMultiplier; } set { injuryMultiplier = value; } }

        public Character(string name, Team team, int maxHealth, int health, int attack, int defense, int attackRange, int movespeed) {
            this.name = name;
            this.team = team;
            this.maxHealth = maxHealth;
            this.attackRange = attackRange;
            this.health = health;
            this.attack = attack;
            this.defense = defense;
            this.moveSpeed = movespeed;
            this.injuryMultiplier = 1.0f;
            this.hitMultiplier = 1.0f;
        }

        public string Serialize() {
            return JsonUtility.ToJson(this);
        }

        public static Character Deserialize(string input) {
            return JsonUtility.FromJson<Character>(input);
        }

        // Overworld calls upgrades with amount == 10 | 100 | 1000
        public void UpgradeAttribute(string attribute, int amount) {

            switch (attribute) {
                case "health":
                    maxHealth += amount; // or however the conversion from credits to levels is implemented
                    break;
                case "speed":
                    moveSpeed += amount; // or however the conversion from credits to levels is implemented
                    break;
                case "defense":
                    defense += amount; // or however the conversion from credits to levels is implemented
                    break;
            }
        }

        public static List<Character> GetDefaultCharacters() {
            List<Character> characters = new List<Character>();

            characters.Add(new Character("Bane", Team.Player, 100, 100, 80, 150, 1, 4));
            characters.Add(new Character("Chon-Ka", Team.Player, 120, 120, 80, 150, 1, 4));
            characters.Add(new Character("Korvid-19", Team.Player, 80, 80, 80, 140, 1, 5));
            characters.Add(new Character("Tasador", Team.Player, 80, 80, 80, 140, 1, 4));

            return characters;
        }

        public static List<Character> GetCurrentCharacters() {
            Dictionary<string, string> data = new Dictionary<string, string>();
            string readFile = Directory.GetCurrentDirectory() + "\\Assets\\Resources\\BoardTxtFiles\\Encounter.txt";
            List<string> lines = new List<string>();
            List<Character> characters = new List<Character>();

            // Read in all lines
            foreach (string line in File.ReadLines(readFile)) {
                lines.Add(line);
            }

            characters.Add(Character.Deserialize(lines[2]));
            characters.Add(Character.Deserialize(lines[3]));
            characters.Add(Character.Deserialize(lines[4]));
            characters.Add(Character.Deserialize(lines[5]));

            return characters;
        }
    }
}