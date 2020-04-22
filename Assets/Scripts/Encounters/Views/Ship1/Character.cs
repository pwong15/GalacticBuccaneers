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

        public float HitMultiplier { get { return hitMultiplier; } set { injuryMultiplier = value; } }

        [SerializeField] private List<String> abilities;

        public List<String> Abilities { get { return abilities; } set { abilities = value; } }

        [SerializeField] private bool isDead;

        public bool IsDead { get { return isDead; } set { isDead = value; } }

        [SerializeField] private int healthXp;
        public int HealthXp { get { return healthXp; } set { healthXp = value; } }

        [SerializeField] private int speedXp;
        public int SpeedXp { get { return speedXp; } set { speedXp = value; } }

        [SerializeField] private int defenseXp;
        public int DefenseXp { get { return defenseXp; } set { defenseXp = value; } }

        [SerializeField] private int healthLvl;
        public int HealthLvl { get { return healthLvl; } set { healthLvl = value; } }

        [SerializeField] private int speedLvl;
        public int SpeedLvl { get { return speedLvl; } set { speedLvl = value; } }

        [SerializeField] private int defenseLvl;
        public int DefenseLvl { get { return defenseLvl; } set { defenseLvl = value; } }



        public Character(string name, Team team, int maxHealth, int health, int attack, int defense, int attackRange, int movespeed) {
            this.name = name;
            this.team = team;
            this.maxHealth = maxHealth;
            this.attackRange = attackRange;
            this.health = health;
            this.attack = attack;
            this.defense = defense;
            this.moveSpeed = movespeed;
            this.InjuryMultiplier = 1.0f;
            this.HitMultiplier = 1.0f;
            this.healthXp = 0;
            this.defenseXp = 0;
            this.speedXp = 0;
            this.healthLvl = 1;
            this.defenseLvl = 1;
            this.speedLvl = 1;
        }

        public string Serialize() {
            return JsonUtility.ToJson(this);
        }

        public static Character Deserialize(string input) {
            return JsonUtility.FromJson<Character>(input);
        }

        // Overworld calls upgrades with amount == 10 | 100 | 1000
        public string UpgradeAttribute(string attribute, int amount) {

            switch (attribute) {
                case "health":
                    healthXp += amount; 
                    if(healthXp > 1500 && healthLvl < 4) { LevelUp("health"); }
                    return "HEALTH LVL: " + healthLvl + "/4 \nXP: " + healthXp + "/1500";

                case "speed":
                    speedXp += amount;
                    if (speedXp > 1500 && speedLvl < 4) { LevelUp("speed"); }
                    return "SPEED LVL: " + speedLvl + "/4 \nXP: " + speedXp + "/1500";

                case "defense":
                    defenseXp += amount;
                    if (defenseXp > 1500 && defenseLvl < 4) { LevelUp("defense"); }
                    return "DEFENSE LVL: " + defenseLvl + "/4 \nXP: " + defenseXp + "/1500";

                default: return "";
            }
        }

        private void LevelUp(string skill) {
            Dictionary<string, int> characterIncrements = new Dictionary<string, int>() 
            {   {"Banehealth", 60 }, {"Banespeed", 1 }, {"Banedefense", 2 },
                {"Chon-Kahealth", 70 }, {"Chon-Kaspeed", 1 }, {"Chon-Kadefense", 1 },
                {"Korvid-19health", 50 }, {"Korvid-19speed", 2 }, {"Korvid-19defense", 1 },
                {"Tasadarhealth", 50 }, {"Tasadarspeed", 1 }, {"Tasadardefense", 2 },
            };

            switch (skill) {
                case "health":
                    healthLvl++;
                    health += characterIncrements[name + skill];
                    healthXp -= 1500;
                    break;

                case "speed":
                    speedLvl++;
                    moveSpeed += characterIncrements[name + skill];
                    speedXp -= 1500;
                    break;

                case "defense":
                    defenseLvl++;
                    defense += characterIncrements[name + skill];
                    defenseXp -= 1500;
                    break;
            }

        }

        public static List<Character> GetDefaultCharacters() {
            List<Character> characters = new List<Character>();

            characters.Add(new Character("Bane", Team.Player, 100, 100, 80, 150, 1, 4));
            characters.Add(new Character("Chon-Ka", Team.Player, 120, 120, 80, 150, 1, 4));
            characters.Add(new Character("Korvid-19", Team.Player, 80, 80, 80, 140, 1, 5));
            characters.Add(new Character("Tasadar", Team.Player, 80, 80, 80, 140, 1, 4));

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