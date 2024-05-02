using System;
using System.Collections;
using CollectionsUtil;

public enum TargetType {NONE, ENEMY, ALLY, ANY};
public enum RangeType {NONE, SELF, SINGLE, MULTIPLE};

public struct CombatSkill {
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
