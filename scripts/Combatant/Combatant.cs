using System;

public abstract class Combatant {
    public int team = -1;
    public string name = "";
    public CombatAction? action;
    public List<CombatSkill>? skills;
    public float health = 10f;

    public virtual void OnCombatStart(object? source, EventArgs e) {
        System.Console.WriteLine($"{name} has joined combat");
        Combat? c = (Combat?) source ?? throw new ArgumentException("No combat found");
        c.combatants.Add(this);
        c.StartPhase += onStartPhase;
        c.MainPhase += onMainPhase;
        c.EndPhase += onEndPhase;
    }

    

    public abstract Combatant AssignTarget(Combat currCombat, TargetType targetType);

    public abstract Combatant[] AssignTargets(Combat currCombat, TargetType targetType);


    // TODO: is it wise to have the combatants be doing everything here? Should we not have some kind of phaseHandler that learns what combatants exist and have it manage this logic flow?
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

    public virtual Combatant RandomTarget(Combat currCombat, TargetType targetType){
        // TODO: Improve random selection
        var rand = new Random();
        int randInt = rand.Next(0, currCombat.combatants.Count());
        return currCombat.combatants[randInt];
    }
}