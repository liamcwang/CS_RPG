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

}