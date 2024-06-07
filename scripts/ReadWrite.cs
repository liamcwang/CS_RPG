using System.IO;
using System.Text.Json;
using DataEditing;

public class FileReadWrite {
    public static string? globalFilePath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

    public static void Run() {
        Console.WriteLine(globalFilePath); 
        string filePath = globalFilePath + "\\Data";

        try
        {
            if (!Directory.Exists(filePath)) {
               Directory.CreateDirectory(filePath);
            }

            Console.WriteLine("Writing combatskills");
            StreamWriter sw = new StreamWriter(filePath + $"\\CombatSkills.json");
            CombatSkillData combatSkillData = DataEditor.AssembleCombatSkillData();
            string jsonString = JsonSerializer.Serialize(combatSkillData);
            sw.WriteLine(jsonString);
            sw.Close();

            Console.WriteLine("Writing Combatants");
            sw = new StreamWriter(filePath + $"\\Combatants.json");
            CombatantData combatantData = DataEditor.AssembleCombatantData();
            jsonString = JsonSerializer.Serialize(combatantData);
            sw.WriteLine();
            sw.Close();
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
}