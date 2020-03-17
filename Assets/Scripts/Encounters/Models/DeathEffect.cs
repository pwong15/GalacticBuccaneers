using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Encounter;
using Views;

namespace Models {
    public class DeathEffect : Effect {

        public int range { get { return 3; } }
        public override void Execute(Effectable actor) {
            (actor as Unit).Die();
        }

        public override void Remove(Effectable actor) {
            
        }
    }
}
