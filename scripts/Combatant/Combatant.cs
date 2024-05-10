using System;
using UserInput;

public class Combatant {
    public delegate Combatant TargetFunction(Combat currCombat, TargetType targetType);

    public int team = -1;
    public string name = "";
    public CombatAction action;
    public List<CombatSkill>? skills;
    public float health = 10f;
    private TargetFunction targetter = (c, tt) => throw new NullReferenceException("No valid target function set");
    public bool isDefeated = false;

    public Combatant(string newName, int teamID) {
        action = new CombatAction(this);
        name = newName;
        team = teamID;
        switch(team) {
            case 0:
                targetter = RandomTarget;
                break;
            case 1:
                targetter = UserPromptedTarget;
                break;
            default:
                break;
        }
    }

    public void OnCombatStart(object? source, EventArgs e) {
        System.Console.WriteLine($"{name} has joined combat");
        Combat? c = (Combat?) source ?? throw new ArgumentException("No combat found");
        c.combatants.Add(this);
        c.StartPhase += onStartPhase;
        c.MainPhase += onMainPhase;
        c.EndPhase += onEndPhase;
    }

    public void TakeDamage(float num) {
        health -= num;
        if (health <= 0.1) {
            Console.WriteLine($"{name} is defeated!");
            isDefeated = true;
        }
    }

    public Combatant AssignTarget(Combat currCombat, TargetType targetType) {
        return this;
    }

    public Combatant[] AssignTargets(Combat currCombat, TargetType targetType) {
        return [this];
    }

    // TODO: is it wise to have the combatants be doing everything here? Should we not have some kind of phaseHandler that learns what combatants exist and have it manage this logic flow? Ah the struggles of OOP
    // Maybe a better question, will these ever be used on anything except combatants? No? Then keep it here
    public void onStartPhase(Combat currCombat) {
        // Console.WriteLine($"{name} is in start phase");
    }

    public void onMainPhase(Combat currCombat) {
        Console.WriteLine($"{name} is preparing an action");
        action.PrepareAction(currCombat);
    }

    public void onEndPhase(Combat currCombat) {
        // Console.WriteLine($"{name} is in end phase");

    }

    public Combatant RandomTarget(Combat currCombat, TargetType targetType){
        // TODO: Improve random selection
        var rand = new Random();
        int randInt = rand.Next(0, currCombat.combatants.Count());
        return currCombat.combatants[randInt];
    }

    public Combatant UserPromptedTarget(Combat currCombat, TargetType targetType) {
        return ConsoleInput.ObtainTarget(this, currCombat, targetType);
    }
}