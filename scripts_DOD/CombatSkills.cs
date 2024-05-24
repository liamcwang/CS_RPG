using System;
using System.Collections;
using CollectionsUtil;

public struct CombatSkills {
    public string[] names;
    public int[] priorities;
    public TargetType[] targetTypes;
    public RangeType[] rangeTypes;
    public Effects[] effects;

    public void Trigger(ref Combatants combatants, int targetID, int skillID) {
        Effects currEffects = effects[skillID];
        for (int i = 0; i < currEffects.Triggers.Length; i++) {
            currEffects.Triggers[i](ref combatants, targetID, currEffects.values[i]);
        }
    }
}