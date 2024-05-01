using System;

public class GameManager {

    // Lazy is necessary to avoid the wrong instance being called.
    private static Lazy<GameManager> _instance = new Lazy<GameManager>(() => new GameManager());
    public static GameManager Instance => _instance.Value;

    private Combat _activeCombat = null!;
    public static Combat activeCombat  {
        get => Instance._activeCombat;
        set => Instance._activeCombat = value;
    }

    // TODO: need to figure out how to get the combatants into combat
    public static Player[] players = [new Player("Leonardo"), new Player("Davinci")];
    public static Enemy[] enemies = [new Enemy("Rick"), new Enemy("Astley")];

    private GameManager() {

    }

    public static void StartCombat() {
        activeCombat = new Combat();
        for (int i = 0; i < players.Length; i++) {
            activeCombat.CombatStart += players[i].OnCombatStart;
            activeCombat.CombatStart += enemies[i].OnCombatStart;
        }
        activeCombat.Start();


    }

    
}