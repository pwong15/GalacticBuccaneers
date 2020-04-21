using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Encounter;
using Views;


namespace Models { 
    public class DeathAbility: Ability {
        public override int Range { get { return 5; } }

        public override int ZoneRange { get { return 2; } }

        public override List<Effect> EnemyEffects { get; set; }

        public override List<Effect> SelfEffects { get; set; }

        public override Unit AbilityUser { get; set; }

        public override int Cooldown { get { return 5; } }

        public override int lastUsed { get; set; }
        public DeathAbility(Unit unit) {
            AbilityUser = unit;
            EnemyEffects = new List<Effect>();
            Effect effect = new DeathEffect();
            EnemyEffects.Add(effect);
            
        }
    }

    public class SunderingBlast: Ability {
        public override string Name { get { return "Sundering Blast"; } }

        public override string Description { get { return "Deal moderate damage, target takes 25% more damage until next turn"; } }

        private bool friendlyFire = false;

        public override int Range { get { return 1; } }

        public override int ZoneRange { get { return 1; } }

        public override List<Effect> EnemyEffects { get; set; }

        public override List<Effect> SelfEffects { get; set; }

        public int HitAdd { get { return 40; } }
        public float AttackMultiplier { get { return 1; } }
        public int AttackAdd { get { return 30; } }

        public override Unit AbilityUser { get; set; }

        public override int Cooldown { get { return 2; } }

        public override int lastUsed { get; set; }

        public SunderingBlast(Unit unit) {
            AbilityUser = unit;
            EnemyEffects = new List<Effect>();
            SelfEffects = new List<Effect>();
            Effect dmgEffect = new DamageEffect(unit, HitAdd, AttackAdd, AttackMultiplier, friendlyFire);
            Effect dmgDebuff = new StatChangeEffect(Stat.InjMul, 25, 2);
            EnemyEffects.Add(dmgEffect);
            EnemyEffects.Add(dmgDebuff);
        }
    }

    public class Expertise : Ability {
        public override string Name { get { return "Expertise"; } }

        public override string Description { get { return "Deal moderate damage, target takes 25% more damage until next turn"; } }

        private bool friendlyFire = false;

        public override int Range { get { return 1; } }

        public override int ZoneRange { get { return 0; } }

        public override List<Effect> EnemyEffects { get; set; }

        public override List<Effect> SelfEffects { get; set; }

        public int HitAdd { get { return 40; } }
        public float AttackMultiplier { get { return 0.66f; } }
        public int AttackAdd { get { return 20; } }

        public override Unit AbilityUser { get; set; }

        public override int Cooldown { get { return 1; } }

        public override int lastUsed { get; set; }

        public Expertise(Unit unit) {
            AbilityUser = unit;
            EnemyEffects = new List<Effect>();
            SelfEffects = new List<Effect>();
            Effect dmgEffect = new DamageEffect(unit, HitAdd, AttackAdd, AttackMultiplier, friendlyFire);
            dmgEffect.PointOfAction = Effect.Frequency.StartOfTurn;
            EnemyEffects.Add(dmgEffect);
        }
    }

    public class EldertichBlast : Ability {
        public override string Name { get { return "Expertise"; } }

        public override string Description { get { return "Deal moderate damage, target takes 25% more damage until next turn"; } }

        private bool friendlyFire = false;

        public override int Range { get { return 6; } }

        public override int ZoneRange { get { return 1; } }

        public override List<Effect> EnemyEffects { get; set; }

        public override List<Effect> SelfEffects { get; set; }

        public int HitAdd { get { return 40; } }
        public float AttackMultiplier { get { return 0.66f; } }
        public int AttackAdd { get { return 20; } }

        public override Unit AbilityUser { get; set; }

        public override int Cooldown { get { return 1; } }

        public override int lastUsed { get; set; }

        public EldertichBlast(Unit unit) {
            AbilityUser = unit;
            EnemyEffects = new List<Effect>();
            SelfEffects = new List<Effect>();
            Effect dmgEffect = new DamageEffect(unit, HitAdd, AttackAdd, AttackMultiplier, friendlyFire);
            dmgEffect.PointOfAction = Effect.Frequency.StartOfTurn;
            EnemyEffects.Add(dmgEffect);
        }
    }



}

