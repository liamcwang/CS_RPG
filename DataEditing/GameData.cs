using System;

// GameData compacts the data from the Json into relevant information
public static class GameData {
    public static CombatSkill[] combatSkillRef;
    public static Combatant[] combatantRef;
    // Is it a good idea to create a dictionary? Remains to be seen
    public static Dictionary<string, CombatSkill> combatSkillDict;

}
