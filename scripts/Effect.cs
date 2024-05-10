using System;

/// <summary>
/// Holds a single trigger effect, defined when constructed.
/// </summary>
public struct Effect {
    public float value = 1;
    public delegate void Trigger(Combatant target);
    public Trigger effectTrigger;

    // when I think of scaling this, it doesn't allow for enough arbitrary effects
    // need to understand how to separate it more.
    public Effect() {
        effectTrigger = Damage;
    }

    // TODO: Define the logic of damage better
    public void Damage(Combatant target) {
        target.TakeDamage(value);
    }

    public void Heal(Combatant target) {
        target.health += value;
    }
}