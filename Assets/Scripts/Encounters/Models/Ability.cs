using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Views;
using Encounter;

namespace Models {
    public abstract class Ability {

        public virtual string Name { get;}

        public virtual string Description { get;}

        public abstract int Range { get; }
        
        public abstract int ZoneRange { get; }

        public abstract List<Effect> EnemyEffects { get; set; }

        public abstract List<Effect> SelfEffects { get; set; }

        public abstract int Cooldown { get; }
        public abstract int lastUsed { get; set; }

        public bool CanUse { get { return lastUsed >= Cooldown; } }

        public abstract Unit AbilityUser { get; set; }

        public void Apply(Effectable actor) {
            foreach (Effect effect in EnemyEffects) {
                (actor as Unit).AddEffect(effect);
            } foreach (Effect effect in SelfEffects) {
                AbilityUser.AddEffect(effect);
            }
            lastUsed = 0;
        }

        public override string ToString() {
            return Name;
        }



    }
}
