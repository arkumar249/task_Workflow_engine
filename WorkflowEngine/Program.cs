// Program.cs
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using WorkflowEngine.Models;
using WorkflowEngine.Services;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Load persisted data on startup
WorkflowRepository.LoadData();

// Endpoint to create a workflow definition
app.MapPost("/workflows", (WorkflowDefinition def) =>
{
    if (WorkflowRepository.Definitions.ContainsKey(def.Id))
        return Results.BadRequest($"Workflow with ID '{def.Id}' already exists.");

    var initialStates = def.States.Count(s => s.IsInitial);
    if (initialStates != 1)
        return Results.BadRequest("Workflow must have exactly one initial state.");

    var stateIds = def.States.Select(s => s.Id).ToList();
    if (stateIds.Distinct().Count() != stateIds.Count)
        return Results.BadRequest("Duplicate state IDs found.");

    var actionIds = def.Actions.Select(a => a.Id).ToList();
    if (actionIds.Distinct().Count() != actionIds.Count)
        return Results.BadRequest("Duplicate action IDs found.");

    foreach (var act in def.Actions)
    {
        if (!stateIds.Contains(act.ToState))
            return Results.BadRequest($"Action '{act.Id}' refers to unknown ToState '{act.ToState}'.");

        if (act.FromStates.Any(from => !stateIds.Contains(from)))
            return Results.BadRequest($"Action '{act.Id}' refers to unknown FromState.");
    }

    WorkflowRepository.Definitions[def.Id] = def;
    WorkflowRepository.SaveData();
    return Results.Ok($"Workflow '{def.Id}' created.");
});

app.MapGet("/workflows/{id}", (string id) =>
{
    if (!WorkflowRepository.Definitions.TryGetValue(id, out var def))
        return Results.NotFound("Workflow not found.");
    return Results.Ok(def);
});

app.MapPost("/instances", (string workflowId) =>
{
    if (!WorkflowRepository.Definitions.TryGetValue(workflowId, out var def))
        return Results.BadRequest("Invalid workflow ID.");

    var initialState = def.States.First(s => s.IsInitial);

    var instance = new WorkflowInstance
    {
        WorkflowDefinitionId = def.Id,
        CurrentStateId = initialState.Id
    };

    WorkflowRepository.Instances[instance.Id] = instance;
    WorkflowRepository.SaveData();
    return Results.Ok(instance);
});

app.MapPost("/instances/{id}/actions", (string id, string actionId) =>
{
    if (!WorkflowRepository.Instances.TryGetValue(id, out var instance))
        return Results.NotFound("Instance not found.");

    var def = WorkflowRepository.Definitions[instance.WorkflowDefinitionId];
    var currentState = def.States.First(s => s.Id == instance.CurrentStateId);

    if (currentState.IsFinal)
        return Results.BadRequest("Cannot act on a final state.");

    if (def.Actions == null || def.Actions.Count == 0)
        return Results.BadRequest("No actions defined in this workflow.");

    var action = def.Actions.FirstOrDefault(a => a.Id == actionId);
    if (action == null)
        return Results.BadRequest("Action not found in workflow definition.");

    if (!action.Enabled)
        return Results.BadRequest("Action is currently disabled.");

    if (!action.FromStates.Contains(currentState.Id))
        return Results.BadRequest($"Action '{action.Id}' cannot be executed from current state '{currentState.Id}'.");

    instance.CurrentStateId = action.ToState;
    instance.History.Add(new InstanceHistory
    {
        ActionId = action.Id,
        Timestamp = DateTime.UtcNow
    });

    WorkflowRepository.SaveData();
    return Results.Ok(instance);
});

app.MapGet("/instances/{id}", (string id) =>
{
    if (!WorkflowRepository.Instances.TryGetValue(id, out var instance))
        return Results.NotFound("Instance not found.");

    return Results.Ok(instance);
});

app.Run();
