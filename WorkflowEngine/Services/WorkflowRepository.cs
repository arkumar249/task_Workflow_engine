using System.Text.Json;
using WorkflowEngine.Models;

namespace WorkflowEngine.Services;

public static class WorkflowRepository
{
    private const string DefFile = "definitions.json";
    private const string InstFile = "instances.json";

    public static Dictionary<string, WorkflowDefinition> Definitions { get; set; } = new();
    public static Dictionary<string, WorkflowInstance> Instances { get; set; } = new();

    public static void SaveData()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };

        File.WriteAllText(DefFile, JsonSerializer.Serialize(Definitions, options));
        File.WriteAllText(InstFile, JsonSerializer.Serialize(Instances, options));
    }

    public static void LoadData()
    {
        if (File.Exists(DefFile))
        {
            string defs = File.ReadAllText(DefFile);
            Definitions = JsonSerializer.Deserialize<Dictionary<string, WorkflowDefinition>>(defs) ?? new();
        }

        if (File.Exists(InstFile))
        {
            string insts = File.ReadAllText(InstFile);
            Instances = JsonSerializer.Deserialize<Dictionary<string, WorkflowInstance>>(insts) ?? new();
        }
    }
}
