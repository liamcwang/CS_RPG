using System;
using System.Collections;
using CollectionsUtil;



public class CombatAction {

    public float priority = 0;
    public CombatSkill combatSkill = new CombatSkill();

    public Combatant origin;
    public List<Combatant> targets = new List<Combatant>();
    
    public CombatAction(Combatant source) {
        origin = source;
        combatSkill = new CombatSkill();
    }

    /// <summary>
    /// When targets have been set, we want to fire at the targets set
    /// </summary>
    public void FireAction() {
        Console.WriteLine($"{origin.name}'s turn");
        foreach(Combatant target in targets) {
            Console.WriteLine($"{origin.name} used {combatSkill.name} on {target.name}!");
            combatSkill.Trigger(target);
        }
        targets.Clear();
    }

    public void PrepareAction(Combat currCombat) {
        targets.Add(origin.AssignTarget(currCombat, combatSkill.targetType));

        currCombat.actionQueue.Add(this);
    }

}