using System;

namespace EventUtil
{
    public static class EventLogs
    {
        public delegate void LogMessage(string s);
        public static LogMessage SendLog = (s) => {};
    }

    public static class DataRequests
    {
        public delegate void RequestDataArray<T>(ref T[] DataReference);
        public static RequestDataArray<CombatSkill> RequestCombatSkillsRef = GameData.FulFillCombatSkillRefRequest;
        public static RequestDataArray<Combatant> RequestCombatantsRef = GameData.FulfillCombatantRefRequest;
    }
}