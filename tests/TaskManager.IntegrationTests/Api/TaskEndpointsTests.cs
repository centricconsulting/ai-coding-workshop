namespace TaskManager.IntegrationTests.Api;

using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using TaskManager.Api;
using TaskManager.Api.Models;
using Xunit;
using Task = System.Threading.Tasks.Task;

/// <summary>
/// Integration tests for Task API endpoints
/// Following TDD - tests written FIRST before endpoint implementation
/// </summary>
public sealed class TaskEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TaskEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostTasks_WithValidRequest_Returns201CreatedWithTaskDetails()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "Integration test task",
            Description = "Testing POST /tasks endpoint",
            Priority = "High",
            DueDate = DateTime.UtcNow.AddDays(7)
        };

        // Act
        var response = await _client.PostAsJsonAsync("/tasks", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var taskResponse = await response.Content.ReadFromJsonAsync<TaskResponse>();
        Assert.NotNull(taskResponse);
        Assert.NotNull(taskResponse.Id);
        Assert.Equal(request.Title, taskResponse.Title);
        Assert.Equal(request.Description, taskResponse.Description);
        Assert.Equal(request.Priority, taskResponse.Priority);
        Assert.Equal("Todo", taskResponse.Status);
        Assert.NotNull(taskResponse.DueDate);
        
        // Verify Location header
        Assert.NotNull(response.Headers.Location);
        Assert.Contains($"/tasks/{taskResponse.Id}", response.Headers.Location.ToString());
    }

    [Fact]
    public async Task PostTasks_WithInvalidPriority_Returns400BadRequest()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "Test task",
            Description = "Invalid priority",
            Priority = "SuperUrgent", // Invalid priority
            DueDate = DateTime.UtcNow.AddDays(1)
        };

        // Act
        var response = await _client.PostAsJsonAsync("/tasks", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        var problemDetails = await response.Content.ReadAsStringAsync();
        Assert.Contains("priority", problemDetails, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task PostTasks_WithPastDueDate_Returns400BadRequest()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "Past task",
            Description = "Due date in the past",
            Priority = "Medium",
            DueDate = DateTime.UtcNow.AddDays(-1)
        };

        // Act
        var response = await _client.PostAsJsonAsync("/tasks", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        var problemDetails = await response.Content.ReadAsStringAsync();
        Assert.Contains("due date", problemDetails, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task PostTasks_WithInvalidTitle_Returns400BadRequest(string? invalidTitle)
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = invalidTitle!,
            Description = "Invalid title",
            Priority = "Low",
            DueDate = DateTime.UtcNow.AddDays(1)
        };

        // Act
        var response = await _client.PostAsJsonAsync("/tasks", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        var problemDetails = await response.Content.ReadAsStringAsync();
        Assert.Contains("title", problemDetails, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task PostTasks_WithoutDescription_Returns201Created()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "Task without description",
            Description = null,
            Priority = "Low",
            DueDate = null
        };

        // Act
        var response = await _client.PostAsJsonAsync("/tasks", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var taskResponse = await response.Content.ReadFromJsonAsync<TaskResponse>();
        Assert.NotNull(taskResponse);
        Assert.Equal(request.Title, taskResponse.Title);
        Assert.Equal(string.Empty, taskResponse.Description);
    }

    [Fact]
    public async Task PostTasks_WithoutDueDate_Returns201Created()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "Task without due date",
            Description = "No deadline",
            Priority = "Medium",
            DueDate = null
        };

        // Act
        var response = await _client.PostAsJsonAsync("/tasks", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var taskResponse = await response.Content.ReadFromJsonAsync<TaskResponse>();
        Assert.NotNull(taskResponse);
        Assert.Null(taskResponse.DueDate);
    }
}
