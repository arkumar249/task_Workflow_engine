namespace WorkflowEngine.Models;

// Represents a single state in a workflow
public class State
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public bool IsInitial { get; set; }   // One state must be the starting point
    public bool IsFinal { get; set; }     // No further transitions allowed from final
    public bool Enabled { get; set; } = true;
}
