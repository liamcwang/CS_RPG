using System;
using System.Collections;
using CollectionsUtil;

public enum TargetType {NONE, ENEMY, ALLY, ANY};
public enum RangeType {NONE, SELF, SINGLE, MULTIPLE};

public class CombatAction {
    // we have a problem, this assumes we are creating actions using this template.
    // however, this also governs when it has targets or not
    // perhaps more of this should belong in combat skill
    public float priority = 0;
    public CombatSkill combatSkill = new CombatSkill();

    public Combatant origin;
    public List<Combatant> targets = new List<Combatant>();
    
    
    public CombatAction(Combatant caster) {
        origin = caster;
        combatSkill = new CombatSkill();
    }

    /// <summary>
    /// When targets have been set, we want to fire at the targets set
    /// </summary>
    public void FireAction() {
        foreach(Combatant target in targets) {
            Console.WriteLine($"{origin.name} used {combatSkill.name} on {target.name}!");
            combatSkill.Trigger(target);
        }
    }

    public virtual void PrepareAction(Combat currCombat) {
        targets.Add(origin.AssignTarget(currCombat, combatSkill.targetType));

        currCombat.actionQueue.Add(this);
    }

    // public static int ComparePriority(CombatAction a, CombatAction b) {
    //     return a.priority >= b.priority ? a : b;
    // }
}