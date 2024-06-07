using System;

public enum EffectType {NONE, DAMAGE, HEAL};

/// <summary>
/// Holds a single trigger effect, defined when constructed.
/// </summary>
public struct Effect {
    public EffectType effectType;
    public float value = 1;
    public delegate void Trigger(Combatant target);
    public Trigger effectTrigger;

    // when I think of scaling this, it doesn't allow for enough arbitrary effects
    // need to understand how to separate it more.
    public Effect(EffectType newEffectType) {
        this.effectType = newEffectType;
        switch (effectType) {
            case EffectType.DAMAGE:
                effectTrigger = Damage;
                break;
            case EffectType.HEAL:
                effectTrigger = Heal;
                break;
            default:
                break;
        }
    }

    // TODO: Define the logic of damage better
    public void Damage(Combatant target) {
        target.TakeDamage(value);
    }

    public void Heal(Combatant target) {
        target.health += value;
    }
}