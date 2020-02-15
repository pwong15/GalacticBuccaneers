using Components;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem {
    // Right now unit and the character they reference have the same attack but I will probably augment unit with field that keeps
    // tracks of statuseffects(buffs/debuffs) that could make the unit attack different from the character it references

    public void Attack(Board board, Unit attacker, Unit target) {
        Debug.Log("Checking if can Attack");
        if (CanAttack(board, attacker, target)) {
            int damageAmount = DamageCalculation(attacker, target);
            if (damageAmount < target.Character.Health) {
                target.TakeDamage(damageAmount);
                Debug.Log(target + " took" + damageAmount + " damage");
                Debug.Log(target + " has " + target.Character.Health + " health left");
            } else {
                Debug.Log(target + " has died");
                TerminateUnit(target);
            }
        }
    }

    public void TerminateUnit(Unit unit) {
        unit.Tile.BoardPiece = null;
        unit.Tile.grid.TriggerGridObjectChanged(unit.Tile.xCoord, unit.Tile.yCoord);
    }

    //public void LaunchAttack(Unit attacker, Unit target) {
    //        int damageAmount = DamageCalculation(attacker, target);
    //        target.TakeDamage(attacker.Character.Attack);
    //        Debug.Log(target + "took" + damageAmount + "damage");
    //}

    public int DamageCalculation(Unit attacker, Unit target) {
        int damageAmount = attacker.Character.Attack - target.Character.Defense;
        damageAmount = damageAmount > 0 ? damageAmount : 0;
        return damageAmount;
    }

    public bool CanAttack(Board board, Unit attacker, Unit target) {
        if (attacker == null || target == null) {
            return false;
        }
        bool notSameTeam = attacker.Team != target.Team;
        Grid<Tile> boardSystem = attacker.Tile.grid;
        List<Tile> attackRange = board.FindTilesInRange(attacker.Tile, attacker.Character.AttackRange, (tile) => 1);
        return notSameTeam && attackRange.Contains(target.Tile);
    }

    //public AttackCommand(Unit attacker, Unit target) {
    //    if (CanAttack) {
    //        return new Command(attacker, target)
    //    }
    //}
}