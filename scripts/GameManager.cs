using System;
using static EventUtil.GameLogs;

public class GameManager {

    // Lazy is necessary to avoid the wrong instance being called.
    private static Lazy<GameManager> _instance = new Lazy<GameManager>(() => new GameManager());
    public static GameManager Instance => _instance.Value;

    private Combat? _activeCombat = null!;
    public static Combat? activeCombat  {
        get => Instance._activeCombat;
        set => Instance._activeCombat = value;
    }

    // TODO: need to figure out how to get the combatants into combat
    // TODO: Figure out how to randomly send a preset group of enemies
    public static Combatant[] players = [new Combatant("Leonardo", 0), new Combatant("Davinci", 0)];
    public static Combatant[] enemies = [new Combatant("Rick", 1), new Combatant("Astley", 1)];

    private GameManager() {
        SendLog += ConsoleLog;
    }

    public static void StartCombat() {
        if (activeCombat == null) {
            activeCombat = new Combat();
            for (int i = 0; i < players.Length; i++) {
                activeCombat.CombatStart += players[i].OnCombatStart;
                activeCombat.CombatStart += enemies[i].OnCombatStart;
            }
            Thread thread = new Thread(activeCombat.Start);
            thread.IsBackground = true;
            thread.Start();
        }
    }

    private static void ConsoleLog(string message) {
        System.Console.WriteLine(message);
    }
}

[System.Serializable]
public struct CombatantGroup{
    public string name = "";
    public List<Combatant> combatants = new List<Combatant>();

    public CombatantGroup(string newName) {
        name = newName;
    }
}