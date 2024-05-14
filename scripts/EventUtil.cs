using System;

namespace EventUtil
{
    public static class GameLogs
    {
        public delegate void LogMessage(string s);
        public static LogMessage SendLog = (s) => {};
    }
}