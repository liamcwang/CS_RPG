using System;
using UserInput;
using static EventUtil.EventLogs;

public class Combatant {
    public delegate Combatant TargetFunction(Combat currCombat, TargetType targetType);

    public int team = -1;
    public string name = "";
    public CombatAction action;
    public CombatSkill[] skills = new CombatSkill[1];
    public float health = 10f;
    private TargetFunction targetter = (c, tt) => throw new NullReferenceException("No valid target function set");
    public bool isDefeated = false; // TODO: Find way to remove this

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
        
        skills[0] = new CombatSkill(); // TODO: remove after proper implementation
    }

    public void OnCombatStart(object? source, EventArgs e) {
        SendLog?.Invoke($"{name} has joined combat");
        Combat? c = (Combat?) source ?? throw new ArgumentException("No combat found");
        c.combatants.Add(this);
        c.StartPhase += onStartPhase;
        c.MainPhase += onMainPhase;
        c.EndPhase += onEndPhase;
    }

    public void TakeDamage(float num) {
        health -= num;
        if (health <= 0.1) {
            SendLog?.Invoke($"{name} is defeated!");
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
        SendLog?.Invoke($"{name} is preparing an action");
        // Decide skill()
        var rand = new Random();
        int randInt = rand.Next(0, skills.Count());
        action.combatSkill = skills[randInt];
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

public class CombatAction {

    public float priority = 0;
    public CombatSkill combatSkill;

    public Combatant origin;
    public List<Combatant> targets = new List<Combatant>();
    
    public CombatAction(Combatant source) {
        origin = source;
    }

    /// <summary>
    /// When targets have been set, we want to fire at the targets set
    /// </summary>
    public void FireAction() {
        SendLog?.Invoke($"{origin.name}'s turn");
        foreach(Combatant target in targets) {
            SendLog?.Invoke($"{origin.name} used {combatSkill.name} on {target.name}!");
            combatSkill.Trigger(target);
        }
        targets.Clear();
    }

    /// <summary>
    /// Runs prior to FireAction, establishes targets of action
    /// </summary>
    /// <param name="currCombat"></param>
    public void PrepareAction(Combat currCombat) {
        targets.Add(origin.AssignTarget(currCombat, combatSkill.targetType));

        currCombat.actionQueue.Add(this);
    }

}