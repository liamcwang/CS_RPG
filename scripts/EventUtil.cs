using System;

namespace EventUtil
{
    public static class EventLogs
    {
        /// <summary>
        /// Add whatever should be handling a logged string input (such as Console.Writeline)
        /// </summary>
        /// <param name="s"></param>
        public delegate void LogMessage(string s);
        public static LogMessage SendLog = (s) => {};
    }

    public static class DataRequests
    {
        /// <summary>
        /// Because we are returning arrays, we will pass the reference to the array
        /// </summary>
        /// <param name="DataReference"></param>
        /// <typeparam name="T"></typeparam>
        public delegate T[] RequestDataArray<T>();
        public static RequestDataArray<CombatSkill> RequestCombatSkillsRef = GameData.FulFillCombatSkillRefRequest;
        public static RequestDataArray<Combatant> RequestCombatantsRef = GameData.FulfillCombatantRefRequest;
    }
}