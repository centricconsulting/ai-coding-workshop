namespace TaskManager.IntegrationTests;

/// <summary>
/// Integration tests for Task Manager API - Lab 4
/// 
/// LAB 4 INSTRUCTIONS:
/// Use Copilot to generate integration tests for the API endpoints.
/// 
/// Example prompts:
/// - "Generate integration tests for POST /tasks endpoint"
/// - "Create integration test for GET /tasks/{id} endpoint"
/// - "Add integration tests with proper HTTP status codes"
/// </summary>
public class TaskApiIntegrationTests
{
    // TODO: Lab 4 - Participants will use Copilot to generate integration tests
    // These will test the full API endpoints after they're implemented in Lab 3
    
    [Fact]
    public System.Threading.Tasks.Task CreateTask_WithValidData_ShouldReturn201()
    {
        // TODO: Lab 4 - Generate this integration test with Copilot
        // Test the POST /tasks endpoint
        throw new NotImplementedException("Lab 4: Generate this integration test with Copilot assistance");
    }

    [Fact]
    public System.Threading.Tasks.Task GetTask_WithExistingId_ShouldReturn200()
    {
        // TODO: Lab 4 - Generate this integration test with Copilot
        // Test the GET /tasks/{id} endpoint
        throw new NotImplementedException("Lab 4: Generate this integration test with Copilot assistance");
    }

    [Fact]
    public System.Threading.Tasks.Task GetTask_WithNonExistentId_ShouldReturn404()
    {
        // TODO: Lab 4 - Generate this integration test with Copilot
        throw new NotImplementedException("Lab 4: Generate this integration test with Copilot assistance");
    }

    [Fact]
    public System.Threading.Tasks.Task UpdateTaskStatus_WithValidData_ShouldReturn200()
    {
        // TODO: Lab 4 - Generate this integration test with Copilot
        // Test the PUT /tasks/{id}/status endpoint
        throw new NotImplementedException("Lab 4: Generate this integration test with Copilot assistance");
    }

    [Fact]
    public System.Threading.Tasks.Task GetActiveTasks_ShouldReturn200WithTaskList()
    {
        // TODO: Lab 4 - Generate this integration test with Copilot
        // Test the GET /tasks endpoint
        throw new NotImplementedException("Lab 4: Generate this integration test with Copilot assistance");
    }
}
