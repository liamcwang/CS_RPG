using System;
using static EventUtil.EventLogs;

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
    // TODO: Reset Combatants after combat
    // TODO: Change lists to arrays to keep data compact


    private static List<Combatant> _activeCombatants = new List<Combatant>();

  

    private GameManager() {
        SendLog += ConsoleLog;
    }

    public static void StartCombat() {
        if (activeCombat != null) {
            throw new NullReferenceException("Combat is still active, shouldn't start another");
        }
        activeCombat = new Combat();
        _activeCombatants.Clear();
        Combatant[] combatantRef = GameData.combatantRef;

        for (int i = 0; i < combatantRef.Count(); i++) {
            Combatant c = combatantRef[i];
            Combatant newCombatant = new Combatant(c.name, c.team);
            newCombatant.skills = c.skills;
            _activeCombatants.Add(newCombatant);
        }
        
        for (int i = 0; i < _activeCombatants.Count(); i++) {
            activeCombat.CombatStart += _activeCombatants[i].OnCombatStart;
        }
        Thread thread = new Thread(activeCombat.Start);
        thread.IsBackground = true;
        thread.Start();
    }

    private static void ConsoleLog(string message) {
        System.Console.WriteLine(message);
    }
}
