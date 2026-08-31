using FakeItEasy;
using Microsoft.Extensions.Logging;
using TaskManager.Application.Services;
using TaskManager.Domain.Repositories;
using TaskManager.Domain.Tasks;

namespace TaskManager.UnitTests;

/// <summary>
/// Unit tests for TaskService - Lab 2 & Lab 4
/// 
/// LAB 2 INSTRUCTIONS:
/// Use Copilot to implement the TaskService.AddTaskAsync method and its tests.
/// 
/// LAB 4 INSTRUCTIONS: 
/// Use Copilot to generate additional unit tests covering edge cases.
/// 
/// Example prompts:
/// - "Implement the AddTaskAsync method with validation and logging"
/// - "Generate unit tests for happy path and validation scenarios"
/// - "Add tests for null parameters and empty strings"
/// </summary>
public class TaskServiceTests
{
    private readonly ITaskRepository _mockRepository;
    private readonly ILogger<TaskService> _mockLogger;
    private readonly TaskService _taskService;

    public TaskServiceTests()
    {
        // Setup mocks using FakeItEasy
        _mockRepository = A.Fake<ITaskRepository>();
        _mockLogger = A.Fake<ILogger<TaskService>>();
        _taskService = new TaskService(_mockRepository, _mockLogger);
    }

    // TODO: Lab 2 - Participants will implement this test with Copilot
    [Fact]
    public System.Threading.Tasks.Task AddTaskAsync_WithValidData_ShouldReturnTaskId()
    {
        // Arrange
        // TODO: Set up test data and mock expectations
        
        // Act
        // TODO: Call TaskService.AddTaskAsync
        
        // Assert
        // TODO: Verify the result and mock interactions
        
        throw new NotImplementedException("Lab 2: Implement this test with Copilot assistance");
    }

    // TODO: Lab 2 - Participants will implement this test with Copilot
    [Fact]
    public System.Threading.Tasks.Task AddTaskAsync_WithNullTitle_ShouldThrowArgumentException()
    {
        // TODO: Test validation for null title
        throw new NotImplementedException("Lab 2: Implement this test with Copilot assistance");
    }

    // TODO: Lab 4 - Participants will generate additional tests with Copilot
    [Fact]
    public System.Threading.Tasks.Task AddTaskAsync_WithEmptyDescription_ShouldThrowArgumentException()
    {
        // TODO: Lab 4 - Generate this test with Copilot
        throw new NotImplementedException("Lab 4: Generate this test with Copilot assistance");
    }

    // TODO: Lab 4 - Participants will generate more comprehensive tests
    [Fact]
    public System.Threading.Tasks.Task GetTaskAsync_WithExistingId_ShouldReturnTask()
    {
        // TODO: Lab 4 - Generate this test with Copilot
        throw new NotImplementedException("Lab 4: Generate this test with Copilot assistance");
    }

    [Fact]
    public System.Threading.Tasks.Task GetTaskAsync_WithNonExistentId_ShouldReturnNull()
    {
        // TODO: Lab 4 - Generate this test with Copilot
        throw new NotImplementedException("Lab 4: Generate this test with Copilot assistance");
    }

    [Fact]
    public System.Threading.Tasks.Task UpdateTaskStatusAsync_WithValidData_ShouldUpdateSuccessfully()
    {
        // TODO: Lab 4 - Generate this test with Copilot
        throw new NotImplementedException("Lab 4: Generate this test with Copilot assistance");
    }
}
