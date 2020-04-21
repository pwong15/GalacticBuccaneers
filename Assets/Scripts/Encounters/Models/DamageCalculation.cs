using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Encounter;
using Views;


namespace Utilitys {
    public static class DamageCalculation {

        public static int GetDamage(Unit attackingUnit, Unit defendingUnit, int hitAdd, float atkMult, int atkAdd, bool friendly) {
            if (attackingUnit.Team == defendingUnit.Team && !friendly) {
                return 0;
            }
            Debug.Log(attackingUnit.HitMultiplier);
            int hitCap = (int)((200 + hitAdd) * attackingUnit.HitMultiplier);
            Debug.Log("HitCap: " + hitCap);
            int dmgCap = (int)(attackingUnit.Attack * atkMult) + atkAdd;
            Debug.Log("DmgCap: " + dmgCap);
            int hitVal = Random.Range(0, hitCap);
            Debug.Log("HitVal: " + hitVal);
            return hitVal > defendingUnit.Defense ? Random.Range((int)(atkMult + atkAdd), dmgCap) : 0; 
        }
    }
}
