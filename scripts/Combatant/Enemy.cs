using System;

public class Enemy : Combatant
{
    public new int team = 1;
    
    public Enemy(string newName) {
        name = newName;
    }

    public override Combatant AssignTarget(Combat currCombat, TargetType targetType) {
        return RandomTarget(currCombat, targetType);
    }

    public override Combatant[] AssignTargets(Combat currCombat, TargetType targetType) {
        return [this];
    }
}