using System;
using static EventUtil.DataRequests;

// GameData compacts the data from the Json into relevant information
public static class GameData {
    public static CombatSkill[] _combatSkillRef = [new CombatSkill()];
    public static Combatant[] _combatantRef = [new Combatant("Leonardo", 0), new Combatant("Davinci", 0), new Combatant("Rick", 1), new Combatant("Astley", 1)];

    public static Combatant[] FulfillCombatantRefRequest() {
        return _combatantRef;
    }

    public static CombatSkill[] FulFillCombatSkillRefRequest() {
        return _combatSkillRef;
    }
}
