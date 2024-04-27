using System;

public abstract class Combatant {
    public int team = -1;
    public string name = "";
    public CombatAction? action;
    public float health = 10f;

    public virtual void OnCombatStart(object? source, EventArgs e) {
        System.Console.WriteLine($"{name} has joined combat");
        Combat? c = (Combat?) source ?? throw new ArgumentException("No combat found");
        c.StartPhase += onStartPhase;
        c.MainPhase += onMainPhase;
        c.EndPhase += onEndPhase;
    }

    

    public abstract Combatant AssignTarget(Combat currCombat, TargetType targetType);

    public abstract Combatant[] AssignTargets(Combat currCombat, TargetType targetType);

    public virtual void onStartPhase(Combat currCombat) {
        Console.WriteLine($"{name} is in start phase");
    }

    public virtual void onMainPhase(Combat currCombat) {
        Console.WriteLine($"{name} is in main phase");
        action = new CombatAction(this);
        action.PrepareAction(currCombat);
    }

    public virtual void onEndPhase(Combat currCombat) {
        Console.WriteLine($"{name} is in end phase");

    }

}