using System;
using System.Collections;
using CollectionsUtil;

public class CombatSkill {
    public string name = "Big Chungus";
    public int priority = 0;

    public TargetType targetType;
    public RangeType rangeType;
    public List<Effect> effects = new List<Effect>();


    public CombatSkill() {
        Effect e = new Effect();
        effects.Add(e);

    }

    public void Trigger(Combatant target) {
        foreach(Effect e in effects) {
            e.effectTrigger(target);
        }
    }
}
