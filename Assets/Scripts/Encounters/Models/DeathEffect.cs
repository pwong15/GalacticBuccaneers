using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Encounter;
using Views;

namespace Models {
    public class DeathEffect : Effect {

        public override int Range { get { return 5; } }

        public override int ZoneRange { get { return 2; } }

        public override int Duration { get; set; }
        public override void Execute(Effectable actor) {
            (actor as Unit).Die();
        }

        public override void Remove(Effectable actor) {
            
        }
    }

    public class PoisonEffect : Effect {
        public override int Range { get { return 5; } }

        public override int ZoneRange { get { return 3; } }

        public override int Duration { get; set; }
        public const Frequency pointOfAction = Frequency.StartOfTurn;

        public PoisonEffect(int duration) {
            Duration = duration;
        }
        public override void Execute(Effectable actor) {
            (actor as Unit).TakeDamage(25);
        }

        public override void Remove(Effectable actor) {
        }
    }

}
