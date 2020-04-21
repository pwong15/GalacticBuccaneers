using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Views;

namespace Encounter {

    
    public static class EnemyCharacters {
        public static Character enemy1 = new Character("e1", Team.Enemy, 10, 100, 2, 25, 2, 5);
        public static Character enemy2 = new Character("e2", Team.Enemy, 1000, 100, 2, 25, 2, 5);
        public static Character enemy3 = new Character("e3", Team.Enemy, 1000, 100, 2, 25, 2, 5);
        public static Character enemy4 = new Character("e4", Team.Enemy, 1000, 100, 2, 25, 2, 5);
        public static Character enemy5 = new Character("e5", Team.Enemy, 1000, 100, 2, 25, 2, 5);
        public static List<Character> GetEnemies() {
            List<Character> enemies = new List<Character>();
            enemies.Add(enemy1);
            enemies.Add(enemy2);
            enemies.Add(enemy3);
            enemies.Add(enemy4);
            enemies.Add(enemy5);
            return enemies;

        }
    }
}
