namespace WorkflowEngine.Models;

// Defines how to move between states
public class ActionDefinition
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public bool Enabled { get; set; } = true;

    // Source states where this action is valid
    public List<string> FromStates { get; set; } = new();

    // Destination state after performing the action
    public string ToState { get; set; } = default!;
}
