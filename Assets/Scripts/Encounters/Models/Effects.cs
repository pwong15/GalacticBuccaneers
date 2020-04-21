using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Encounter;
using Utilitys;
using Views;

namespace Models {
    public class DeathEffect : Effect {

        public override int Duration { get; set; }
        public override Frequency PointOfAction { get { return Frequency.Immediate; } }
        public override void Execute(Effectable actor) {
            Unit unit = (actor as Unit);
            if (unit != null) {
                Debug.Log(actor + " was killed");
                unit.Die();
            }
        }

        public override void Remove(Effectable actor) {
            
        }
    }

    public class PoisonEffect : Effect {
     
        public override int Duration { get; set; }
        public override Frequency PointOfAction { get { return Frequency.StartOfTurn; } }

        public PoisonEffect(int duration) {
            
        }
        public override void Execute(Effectable actor) {
            Debug.Log(actor + " is poisoned");
            (actor as Unit).TakeDamage(25);
        }

        public override void Remove(Effectable actor) {
        }
    }


    public class HealthEffect: Effect {

        public override int Duration { get; set; }
        public override Frequency PointOfAction { get { return Frequency.StartOfTurn; } }

        public HealthEffect(int duration) {
            Duration = duration;
        }
        public override void Execute(Effectable actor) {
            Debug.Log(actor + " is being healed");
            (actor as Unit).Heal(10);
        }

        public override void Remove(Effectable actor) {
        }
    }

    public enum Stat {
        Atk,
        Def,
        Spd,
        InjMul,
        HitMul,
    }
   public class StatChangeEffect: Effect {
       
        private Stat stat;
        private int amount;

        public override int Duration { get; set; }
        public override Frequency PointOfAction { get { return Frequency.Immediate; } }

        public StatChangeEffect(Stat stat, int amount, int duration) {
            this.stat = stat;
            this.amount = amount;
            this.Duration = duration;
        }
        public override void Execute(Effectable actor) {
            Unit unit = (actor as Unit);
           switch(stat) {
                case Stat.Atk:
                    unit.Attack += amount;
                    break;
                case Stat.Spd:
                    unit.MoveSpeed += amount;
                    break;
                case Stat.InjMul:
                    unit.InjuryMultiplier += amount/100;
                    break;
            }
            Debug.Log((actor as Unit) + "has gained " + amount + " to " + stat);
        }

        public override void Remove(Effectable actor) {
            amount = -amount;
            Execute(actor);
        }
    }

    public class DamageEffect: Effect {

        private Unit atkUnit;

        private int hitAdd;

        private int atkAdd;

        private bool friendly;

        private float atkMult;
        public int Damage { get; set; }

        private Frequency pointOfAction;
        public override Frequency PointOfAction { get { return pointOfAction; } set { pointOfAction = value; } }
        public override void Execute(Effectable actor) {
            Unit defUnit = (actor as Unit);
            Damage = DamageCalculation.GetDamage(atkUnit, defUnit, hitAdd, atkMult, atkAdd, friendly);
            defUnit.TakeDamage(Damage);
            
        }

        public DamageEffect(Unit atkUnit, int hitAdd, int atkAdd, float atkMult, bool friendly) {
            this.atkUnit = atkUnit;
            this.hitAdd = hitAdd;
            this.atkAdd = atkAdd;
            this.atkMult = atkMult;
            this.friendly = friendly;
            pointOfAction = Frequency.Immediate;
        }

        public override void Remove(Effectable actor) {
            
        }
    }

    public class SetDamageEffect: Effect {

        public int Damage { get; set; }

        public SetDamageEffect(int damage, int duration) {
            this.Damage = damage;
            this.Duration = duration;
            PointOfAction = Frequency.StartOfTurn;
        }
        public override void Execute(Effectable actor) {
            Unit defUnit = (actor as Unit);
            defUnit.TakeDamage(Damage);
        }

        public override void Remove(Effectable actor) {

        }
    }

}
