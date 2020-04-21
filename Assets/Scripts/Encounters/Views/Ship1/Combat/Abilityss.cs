using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using Encounter;

namespace Views {

    public class Abilitysss {
        
        public Character AbilityUser { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public bool DamagingAbility { get; set; }
        public bool FriendlyFire { get; set; }

        public Effect[] SecondaryEffects { get; set; }

        public int HitAdd { get; set; }
        public float AttackMultiplier { get; set; }
        public int AttackAdd { get; set; }

        public int Range { get; set; }

        public int Cooldown { get; set; }
        public int lastUsed { get; set; }

        public bool CanUse { get { return lastUsed >= Cooldown; } }
        /*public Ability(Character character, string name, string description, bool damage, bool friendly, Effect[] effects,
            int hitadd, int multiplier, int attackadd, int range) {
            this.AbilityUser = character;
            this.Name = name;
            this.Cooldown = this.Cooldown;
            this.lastUsed = this.Cooldown;
            this.Description = description;

            DamagingAbility = damage;
            FriendlyFire = friendly;

            this.SecondaryEffects = effects;

            this.HitAdd = hitadd;
            this.AttackMultiplier = multiplier;
            this.AttackAdd = attackadd;

            this.Range = range;
        }*/

        public virtual void UseAbility(Effectable[] allTargets) {
            foreach (Effectable target in allTargets) {
                Unit unit = target as Unit;
                if (Targetable(unit)) {
                    if (this.DamagingAbility) {
                        int DamageValue = GetDamageValue();
                        if (ConfirmHit(unit)) {
                            unit.TakeDamage(DamageValue);
                        }
                    }
                    foreach(Effect effect in SecondaryEffects) {
                        target.AddEffect(effect);
                    }
                } 
            }
            lastUsed = 0;
        }

        private int GetHitCap() {
            return (int)((200 + this.HitAdd) * AbilityUser.HitMultiplier);
        }
        private int GetDamageCap() {
            return (int)((AbilityUser.Attack * this.AttackMultiplier) + this.AttackAdd);
        }
        public int GetHitValue() {

            return Random.Range(0, GetHitCap());
        }

        //Minimum damage designed around assuming rolling 1 on each die (multiplier) plus added damage
        public int GetDamageValue() {
            return Random.Range((int) (this.AttackMultiplier + this.AttackAdd), GetDamageCap());
        }

        public bool Targetable(Unit target) {
            //Can only target allies with damaging abilities
            if (TargetIsAlly(target)) {
                if (this.DamagingAbility) {
                    if (this.FriendlyFire) {
                        return true;
                    }
                    return false;
                }
                return true;
            }
            //Can only target enemies with damaging abilities
            else {
                if (this.DamagingAbility) {
                    return true;
                }
                return false;
            }
        }

        public bool TargetIsAlly(Unit target) {
            return target.Team == AbilityUser.Team;
        }

        public bool ConfirmHit(Unit target) {
            return (GetHitValue() > target.Character.Defense);
        }

    }
}