using System;

/// <summary>
/// Holds a single trigger effect, defined when constructed.
/// </summary>
public struct Effect {
    public float value = 1;
    public delegate void Trigger(Combatant target);
    public Trigger effectTrigger;

    public Effect() {
        effectTrigger = Damage;
    }

    // TODO: Define the logic of damage better
    public void Damage(Combatant target) {
        target.health -= value;
    }

    public void Heal(Combatant target) {
        target.health += value;
    }
}