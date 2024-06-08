using System;
using static EventUtil.DataRequests;

// GameData compacts the data from the Json into relevant information
public class GameData {
    private static CombatSkill[] _combatSkillRef = [new CombatSkill()];
    private static Combatant[] _combatantRef = [new Combatant("Leonardo", 0), new Combatant("Davinci", 0), new Combatant("Rick", 1), new Combatant("Astley", 1)];

    public static void FulfillCombatantRefRequest(ref Combatant[] combatantRef) {
        combatantRef = _combatantRef;
    }

    public static void FulFillCombatSkillRefRequest(ref CombatSkill[] combatSkillRef) {
        combatSkillRef = _combatSkillRef;
    }
}
