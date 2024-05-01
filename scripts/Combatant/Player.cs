using System;

public class Player : Combatant
{
    public new int team = 1;

    public Player(string newName) {
        name = newName;
    }

    // TODO: Player input, requires us listing potential options though
    public override Combatant AssignTarget(Combat currCombat, TargetType targetType) {
        return this;
    }

    public override Combatant[] AssignTargets(Combat currCombat, TargetType targetType) {
        return [this];
    }
}