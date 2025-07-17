namespace WorkflowEngine.Models;

// Tracks a single execution of a workflow
public class WorkflowInstance
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string WorkflowDefinitionId { get; set; } = default!;
    public string CurrentStateId { get; set; } = default!;
    public List<InstanceHistory> History { get; set; } = new();
}

// Holds the audit log of each transition
public class InstanceHistory
{
    public string ActionId { get; set; } = default!;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}