using System;
using System.Collections;
using static EventUtil.EventLogs;

public class Combat {
    
    public List<Combatant> combatants = new List<Combatant>();
    public Dictionary<int, List<Combatant>> combatTeams = new Dictionary<int, List<Combatant>>();
    public delegate void Phase(Combat currCombat);

    // public delegate void CombatStartEventHandler(object? source, EventArgs args);
    // public event CombatStartEventHandler CombatStart;
    // Lesson: we don't need to do this everytime, we can instead use the generic form

    public event EventHandler CombatStart = (s, e) => {};

    public event Phase StartPhase = (c) => {};
    public event Phase MainPhase = (c) => {};
    public event Phase EndPhase = (c) => {};

    public int turnNumber = 0;
    public List<CombatAction> actionQueue = new List<CombatAction>();

    public bool combatActive = true;

    public Combat() {
        
    }

    public void Start() {
        OnCombatStart();
        
    }

    public void OnCombatStart() {
        if (CombatStart != null) {
            CombatStart?.Invoke(this, EventArgs.Empty);

            for (int i = 0; i < combatants.Count(); i++) {
                Combatant combatant = combatants[i];
                int team = combatant.team;
                if (combatTeams.ContainsKey(team)) {
                    combatTeams[team].Add(combatant);
                } else {
                    combatTeams[team] = new List<Combatant>();
                    combatTeams[team].Add(combatant);
                }
            }
            combatActive = true;
            turnNumber = 0;
            while(combatActive) {
                turnNumber++;
                SendLog?.Invoke($"Turn: {turnNumber}");
                CombatLoop();
            }
            // TODO: Determine victor of combat
            SendLog?.Invoke("Combat concluded");
            GameManager.activeCombat = null;
        }
    }

    // TODO: implement turn loop
    public void CombatLoop() {
        // Start Phase
        StartPhase?.Invoke(this);

        // Main Phase
        MainPhase?.Invoke(this); // combatants choose actions on this step
        // establish turn priority, and all combatants use up their action accordingly
        actionQueue.Sort((x, y) => x.priority.CompareTo(y.priority));
        for (int i = 0; i < actionQueue.Count(); i++) {
            foreach (var team in combatTeams.Keys) {
                int teamLength = combatTeams[team].Count();
                int numDefeated = 0;
                for (int j = 0; j <  teamLength; j++) {
                    if (combatTeams[team][j].isDefeated) {
                        numDefeated++;
                    }
                }
                if (numDefeated == teamLength) {
                    combatActive = false;
                    return;
                }
            }

            actionQueue[i].FireAction();

        } // since we have this kind of loop for the mainphase, we could reuse it for each phase of combat
        actionQueue.Clear();

        // End Phase
        EndPhase?.Invoke(this);
    }
}