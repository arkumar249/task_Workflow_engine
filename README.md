# Configurable Workflow Engine (State-Machine API)

This project implements a lightweight backend system to define and execute **workflow state machines**. It supports defining custom workflows with states and transitions (called actions), instantiating them, and driving them through valid transitions. It uses **.NET 8**, follows a minimalist architecture, and persists state using local JSON files—making it ideal for local or test environments.

---

##  Quick Start

### Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
* A REST client like **Postman** or `curl`

### ▶ Run the Application

```bash
dotnet run
```

The server will start at:

```
http://localhost:5000
```

> No Swagger/OpenAPI is provided as per assignment guidelines.

---

##  Core Features & Usage

### 1. Define a Workflow

```http
POST /workflows
```

Define states (including initial/final), and transitions (actions). Each action moves the instance from one state to another.

### 2. Start an Instance

```http
POST /instances?workflowId=order-process
```

Creates an instance of a workflow with its state set to the initial state.

### 3. Trigger an Action

```http
POST /instances/{id}/actions?actionId=ship
```

Moves the instance forward based on the defined transition.

### 4. Fetch Instance Details

```http
GET /instances/{id}
```

Shows the current state and transition history.

---
## 🛠 Tech Stack
- **Language:** C#
- **Framework:** .NET 8 (Minimal API)
- **Persistence:** JSON file-based storage (no database)
- **API Testing:** Postman / curl
- **Architecture:** Stateless, modular, service-based in-memory system
---

## Project Structure

```
WorkflowEngine/
├── Models/                # Definitions: states, actions, workflows, instances
├── Services/              # In-memory + file-backed repository
├── Program.cs             # Minimal API endpoints
├── definitions.json       # Saved workflow definitions
├── instances.json         # Saved workflow instances

```

---

## Persistence

* Uses local JSON files (`definitions.json`, `instances.json`) to store workflows and their runtime instances.
* Automatically loads data at startup and saves changes after modifications.
* Designed for simplicity — avoids need for a database.

---

##  Assumptions

* Each workflow must have **exactly one** initial state
* States and actions must have **unique IDs**
* Final states cannot transition further
* Actions must specify valid `fromStates` and one valid `toState`
* Transitions are only possible if explicitly allowed
* Actions can be disabled using the `enabled` flag

---

## Shortcuts & Known Limitations

* No listing APIs for workflows or instances (only fetch by ID)
* No Swagger, auth, or RBAC
* JSON file persistence assumes single-threaded use — not concurrent safe
* No rollback/versioning of state
* No automated testing (manual validation done)

---



## Summary of Implemented Requirements

| Feature                                    | Status |
| ------------------------------------------ | ------ |
| Define workflows as state machines         | ✅      |
| Start workflow instances                   | ✅      |
| Execute transitions between states         | ✅      |
| Retrieve instance state and action history | ✅      |
| In-memory + file-based persistence         | ✅      |
| Validation: states/actions/duplicates      | ✅      |
| Minimal .NET 8 API (no overengineering)    | ✅      |
| Documentation and reasoning in README      | ✅      |

---



