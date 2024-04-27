using System;

public class Enemy : Combatant
{
    public new int team = 1;
    
    public Enemy() {
        name = "Mama";
    }

    public override Combatant AssignTarget(Combat currCombat, TargetType targetType) {
        return this;
    }

    public override Combatant[] AssignTargets(Combat currCombat, TargetType targetType) {
        return [this];
    }
}