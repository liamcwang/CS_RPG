using System.IO;
using System.Text.Json;
using DataEditing;

public class FileReadWrite {
    // should be the path to our current directory
    public static string? globalFilePath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

    public static void WriteData() {
        Console.WriteLine(globalFilePath); 
        string filePath = globalFilePath + "\\Data";

        try
        {
            if (!Directory.Exists(filePath)) {
               Directory.CreateDirectory(filePath);
            }

            Console.WriteLine("Writing combatskills");
            StreamWriter writer = new StreamWriter(filePath + $"\\CombatSkills.json");
            CombatSkillData combatSkillData = DataEditor.AssembleCombatSkillData();
            string jsonString = JsonSerializer.Serialize(combatSkillData);
            writer.WriteLine(jsonString);
            writer.Close();

            Console.WriteLine("Writing Combatants");
            writer = new StreamWriter(filePath + $"\\Combatants.json");
            CombatantData combatantData = DataEditor.AssembleCombatantData();
            jsonString = JsonSerializer.Serialize(combatantData);
            writer.WriteLine(jsonString);
            writer.Close();
        }
        catch(Exception e)
        {
            Console.WriteLine("Exception " + e.Message);
        }
        finally
        {
            Console.WriteLine("Executing finally");
        }

        // TODO: Test deserializing the data into the form it was serialized from.
    }

    public static void ReadData() {
        Console.WriteLine(globalFilePath); 
        string filePath = globalFilePath + "\\Data";

        try
        {
            if (!Directory.Exists(filePath)) {
               Directory.CreateDirectory(filePath);
            }

            Console.WriteLine("Reading combatskills");
            StreamReader reader = new (filePath + $"\\CombatSkills.json");
            string jsonString = reader.ReadToEnd();
            CombatSkillData combatSkillData = JsonSerializer.Deserialize<CombatSkillData>(jsonString);
            DataEditor.UnpackCombatSkillDataBundle(combatSkillData);
            reader.Close();

            Console.WriteLine("Reading Combatants");
            reader = new (filePath + $"\\Combatants.json");
            jsonString = reader.ReadToEnd();
            CombatantData combatantData = JsonSerializer.Deserialize<CombatantData>(jsonString);
            reader.Close();
        }
        catch(Exception e)
        {
            Console.WriteLine("Exception " + e.Message);
        }
        finally
        {
            Console.WriteLine("Executing finally");
        }
    }
}