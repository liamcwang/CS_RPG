using System;

public class Enemy : Combatant
{
    
    public Enemy(string newName) {
        team = 1;
        name = newName;
    }

    public override Combatant AssignTarget(Combat currCombat, TargetType targetType) {
        return RandomTarget(currCombat, targetType);
    }

    public override Combatant[] AssignTargets(Combat currCombat, TargetType targetType) {
        return [this];
    }
}