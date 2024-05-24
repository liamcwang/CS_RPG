using System;
using UserInput;
using static EventUtil.EventLogs;

/// <summary>
/// An attempt to create a DOD (Data-Oriented Design) version of Combatant
/// </summary>
public struct Combatants {
    public string[] names;
    public int[] teams;
    public int[] skillsPointer;
    public float[] health;
}