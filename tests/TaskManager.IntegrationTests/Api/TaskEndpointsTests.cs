using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using TaskManager.Api;
using TaskManager.Api.Models;

namespace TaskManager.IntegrationTests.Api;

/// <summary>
/// Integration tests for Task API endpoints
/// Uses WebApplicationFactory to test the full HTTP pipeline
/// </summary>
public sealed class TaskEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public TaskEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    [Fact]
    public async Task PostTask_WithAllFieldsValid_Returns201CreatedWithTaskDetailsAndLocationHeader()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "Integration Test Task",
            Description = "This is a comprehensive test",
            Priority = "High",
            DueDate = DateTime.UtcNow.AddDays(7)
        };

        // Act
        var response = await _client.PostAsJsonAsync("/tasks", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        // Verify Location header
        Assert.NotNull(response.Headers.Location);
        Assert.StartsWith("/tasks/", response.Headers.Location.ToString());

        // Verify response body
        var taskResponse = await response.Content.ReadFromJsonAsync<TaskResponse>(_jsonOptions);
        Assert.NotNull(taskResponse);
        Assert.NotEqual(Guid.Empty, taskResponse.Id);
        Assert.Equal(request.Title, taskResponse.Title);
        Assert.Equal(request.Description, taskResponse.Description);
        Assert.Equal(request.Priority, taskResponse.Priority);
        Assert.Equal(request.DueDate, taskResponse.DueDate);
        Assert.Equal("Todo", taskResponse.Status);
        Assert.False(taskResponse.IsCompleted);
        Assert.Null(taskResponse.CompletedAt);
        Assert.True(taskResponse.CreatedAt <= DateTime.UtcNow);
        Assert.True(taskResponse.UpdatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public async Task PostTask_WithOnlyRequiredFields_Returns201Created()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "Minimal Task",
            Description = null,
            Priority = "Low",
            DueDate = null
        };

        // Act
        var response = await _client.PostAsJsonAsync("/tasks", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var taskResponse = await response.Content.ReadFromJsonAsync<TaskResponse>(_jsonOptions);
        Assert.NotNull(taskResponse);
        Assert.Equal(request.Title, taskResponse.Title);
        Assert.Equal(request.Priority, taskResponse.Priority);
        Assert.Null(taskResponse.DueDate);
    }

    [Fact]
    public async Task PostTask_WithMediumPriority_Returns201Created()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "Medium Priority Task",
            Priority = "Medium"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/tasks", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var taskResponse = await response.Content.ReadFromJsonAsync<TaskResponse>(_jsonOptions);
        Assert.NotNull(taskResponse);
        Assert.Equal("Medium", taskResponse.Priority);
    }

    [Fact]
    public async Task PostTask_WithCriticalPriority_Returns201Created()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "Critical Priority Task",
            Priority = "Critical"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/tasks", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var taskResponse = await response.Content.ReadFromJsonAsync<TaskResponse>(_jsonOptions);
        Assert.NotNull(taskResponse);
        Assert.Equal("Critical", taskResponse.Priority);
    }

    [Fact]
    public async Task PostTask_WithInvalidPriority_Returns400BadRequestWithProblemDetails()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "Invalid Priority Task",
            Priority = "SuperUrgent" // Invalid priority
        };

        // Act
        var response = await _client.PostAsJsonAsync("/tasks", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        // Verify ProblemDetails response
        var contentType = response.Content.Headers.ContentType?.MediaType;
        Assert.Contains("application/problem+json", contentType ?? "application/json");
    }

    [Fact]
    public async Task PostTask_WithEmptyPriority_Returns400BadRequest()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "Task with empty priority",
            Priority = "" // Empty priority
        };

        // Act
        var response = await _client.PostAsJsonAsync("/tasks", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostTask_WithPastDueDate_Returns400BadRequestWithProblemDetails()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "Past Due Date Task",
            Priority = "High",
            DueDate = DateTime.UtcNow.AddDays(-1) // Past date
        };

        // Act
        var response = await _client.PostAsJsonAsync("/tasks", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        // Verify ProblemDetails response
        var contentType = response.Content.Headers.ContentType?.MediaType;
        Assert.Contains("application/problem+json", contentType ?? "application/json");
        
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("past", content.ToLower());
    }

    [Fact]
    public async Task PostTask_WithNullTitle_Returns400BadRequest()
    {
        // Arrange
        var request = new
        {
            Title = (string?)null,
            Priority = "Medium"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/tasks", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostTask_WithEmptyTitle_Returns400BadRequest()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "",
            Priority = "Medium"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/tasks", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostTask_WithWhitespaceTitle_Returns400BadRequest()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "   ",
            Priority = "Medium"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/tasks", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostTask_WithTitleTooLong_Returns400BadRequest()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = new string('A', 201), // 201 characters (max is 200)
            Priority = "Medium"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/tasks", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostTask_WithDescriptionTooLong_Returns400BadRequest()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "Task with long description",
            Description = new string('A', 2001), // 2001 characters (max is 2000)
            Priority = "Medium"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/tasks", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostTask_WithNullDescription_Returns201Created()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "Task without description",
            Description = null,
            Priority = "Low"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/tasks", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var taskResponse = await response.Content.ReadFromJsonAsync<TaskResponse>(_jsonOptions);
        Assert.NotNull(taskResponse);
        Assert.Equal("Task without description", taskResponse.Title);
    }

    [Fact]
    public async Task PostTask_WithFutureDueDate_Returns201Created()
    {
        // Arrange
        var futureDate = DateTime.UtcNow.AddMonths(3);
        var request = new CreateTaskRequest
        {
            Title = "Long-term task",
            Priority = "Low",
            DueDate = futureDate
        };

        // Act
        var response = await _client.PostAsJsonAsync("/tasks", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var taskResponse = await response.Content.ReadFromJsonAsync<TaskResponse>(_jsonOptions);
        Assert.NotNull(taskResponse);
        Assert.Equal(futureDate, taskResponse.DueDate);
    }

    [Fact]
    public async Task PostTask_WithNullDueDate_Returns201Created()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "Task with no due date",
            Priority = "Medium",
            DueDate = null
        };

        // Act
        var response = await _client.PostAsJsonAsync("/tasks", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var taskResponse = await response.Content.ReadFromJsonAsync<TaskResponse>(_jsonOptions);
        Assert.NotNull(taskResponse);
        Assert.Null(taskResponse.DueDate);
    }

    [Fact]
    public async Task PostTask_MultipleRequests_CreateMultipleTasks()
    {
        // Arrange
        var request1 = new CreateTaskRequest { Title = "Task 1", Priority = "High" };
        var request2 = new CreateTaskRequest { Title = "Task 2", Priority = "Low" };

        // Act
        var response1 = await _client.PostAsJsonAsync("/tasks", request1);
        var response2 = await _client.PostAsJsonAsync("/tasks", request2);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response1.StatusCode);
        Assert.Equal(HttpStatusCode.Created, response2.StatusCode);
        
        var task1 = await response1.Content.ReadFromJsonAsync<TaskResponse>(_jsonOptions);
        var task2 = await response2.Content.ReadFromJsonAsync<TaskResponse>(_jsonOptions);
        
        Assert.NotNull(task1);
        Assert.NotNull(task2);
        Assert.NotEqual(task1.Id, task2.Id);
    }
}
