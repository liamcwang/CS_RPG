using System;
using System.Collections;
using CollectionsUtil;

public enum TargetType {NONE, ENEMY, ALLY, ANY};
public enum RangeType {NONE, SELF, SINGLE, MULTIPLE};

public class CombatSkill {
    public string name = "Big Chungus";
    public int priority = 0;

    public TargetType targetType;
    public RangeType rangeType;
    public Effect[] effects = new Effect[100];

    public CombatSkill() {
        Effect e = new Effect();
        e.effectType = EffectType.DAMAGE;
        effects[0] = e;

    }

    public void Trigger(Combatant target) {
        foreach(Effect ed in effects) {
            ed.effectTrigger(target);
        }
    }
}
