using System;

namespace EventUtil
{
    public static class EventLogs
    {
        public delegate void LogMessage(string s);
        public static LogMessage SendLog = (s) => {};
    }
}