using System;

public class Player : Combatant
{

    public Player(string newName) {
        team = 0;
        name = newName;
    }

    // TODO: Player input, requires us listing potential options though
    // Oh inheritance my old enemy
    public override Combatant AssignTarget(Combat currCombat, TargetType targetType) {
        return InputHandler.ObtainUserInput(this, currCombat, targetType);
    }

    public override Combatant[] AssignTargets(Combat currCombat, TargetType targetType) {
        return [this];
    }
}