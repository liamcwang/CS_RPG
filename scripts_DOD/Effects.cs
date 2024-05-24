using System;

public struct Effects {
    public EffectType[] effectTypes;
    public float[] values;
    public delegate void EffectTrigger(ref Combatants combatants, int targetHealth, float value);
    public EffectTrigger[] Triggers;

    public void Damage(ref Combatants combatants, int targetID, float damage) {
        combatants.health[targetID] -= damage;
    }

    public void Heal(ref Combatants combatants, int targetID, float healAmount) {
        combatants.health[targetID] += healAmount;
    }
}