using System;

/// <summary>
/// Holds a single trigger effect, defined when constructed.
/// </summary>
public class Effect {
    float value = 1;
    public delegate void Trigger(Combatant target);
    public Trigger effectTrigger;

    public Effect() {
        effectTrigger = Damage;
    }

    // will need to revisit this in the future
    public void Damage(Combatant target) {
        target.health -= value;
    }
}