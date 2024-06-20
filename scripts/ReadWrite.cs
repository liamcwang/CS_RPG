using System.IO;
using System.Text.Json;
using DataEditing;

public class FileReadWrite {
    // should be the path to our current directory
    public static string? globalFilePath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

    public static void WriteProgramData() {
        Console.WriteLine(globalFilePath); 
        string filePath = globalFilePath + "\\Data";

        if (!Directory.Exists(filePath)) {
            Directory.CreateDirectory(filePath);
        }

        // Console.WriteLine("Writing combatskills");
        CombatSkillData combatSkillData = DataEditor.AssembleCombatSkillData();
        WriteDataToFile<CombatSkillData>(filePath, "CombatSkills.json", combatSkillData);

        // Console.WriteLine("Writing Combatants");
        CombatantData combatantData = DataEditor.AssembleCombatantData();
        WriteDataToFile<CombatantData>(filePath, "Combatant.json", combatantData);

        // TODO: Test deserializing the data into the form it was serialized from.
    }

    public static void ReadProgramData() {
        Console.WriteLine(globalFilePath); 
        string filePath = globalFilePath + "\\Data";

        if (!Directory.Exists(filePath)) {
            Directory.CreateDirectory(filePath);
        }

        // Console.WriteLine("Reading combatskills");
        CombatSkillData combatSkillData = ReadDataFromFile<CombatSkillData>(filePath, "CombatSkills.json");
        DataEditor.UnpackCombatSkillDataBundle(combatSkillData);

        // Console.WriteLine("Reading Combatants");
        CombatantData combatantData = ReadDataFromFile<CombatantData>(filePath, "Combatants.json");
        DataEditor.UnpackCombatantDataBundle(combatantData);
    }

    private static void WriteDataToFile<T>(string filePath, string fileName, T data) {
        Console.WriteLine($"Writing {fileName}");
        StreamWriter writer = new StreamWriter(filePath + $"\\{fileName}");
        string jsonString = JsonSerializer.Serialize(data);
        writer.WriteLine(jsonString);
        writer.Close();
    }

    private static T? ReadDataFromFile<T>(string filePath, string fileName) {
        Console.WriteLine($"Reading {fileName}");
        StreamReader reader = new (filePath + $"\\{fileName}");
        string jsonString = reader.ReadToEnd();
        T data = JsonSerializer.Deserialize<T>(jsonString);
        reader.Close();
        return data;
    }
}