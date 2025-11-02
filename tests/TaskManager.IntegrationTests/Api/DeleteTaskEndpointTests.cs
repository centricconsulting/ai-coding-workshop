using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using TaskManager.Api;
using TaskManager.Api.Models;

namespace TaskManager.IntegrationTests.Api;

public sealed class DeleteTaskEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public DeleteTaskEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteTask_WithExistingTask_ReturnsNoContent()
    {
        // Arrange - Create a task first
        var createRequest = new CreateTaskRequest
        {
            Title = "Task to Delete",
            Description = "This task will be deleted",
            Priority = "Low",
            DueDate = null
        };

        var createResponse = await _client.PostAsJsonAsync("/tasks", createRequest);
        createResponse.EnsureSuccessStatusCode();
        
        var createdTask = await createResponse.Content.ReadFromJsonAsync<TaskResponse>(_jsonOptions);
        Assert.NotNull(createdTask);

        // Act - Delete the task
        var deleteResponse = await _client.DeleteAsync($"/tasks/{createdTask.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        Assert.Equal(0, deleteResponse.Content.Headers.ContentLength ?? 0);
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteTask_WithNonExistentTask_ReturnsNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync($"/tasks/{nonExistentId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteTask_VerifyTaskIsRemoved()
    {
        // Arrange - Create a task
        var createRequest = new CreateTaskRequest
        {
            Title = "Task to Verify Deletion",
            Description = "This task will be deleted and verified",
            Priority = "Medium",
            DueDate = null
        };

        var createResponse = await _client.PostAsJsonAsync("/tasks", createRequest);
        var createdTask = await createResponse.Content.ReadFromJsonAsync<TaskResponse>(_jsonOptions);
        Assert.NotNull(createdTask);

        // Act - Delete the task
        var deleteResponse = await _client.DeleteAsync($"/tasks/{createdTask.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        // Assert - Verify task is removed by trying to delete again
        var secondDeleteResponse = await _client.DeleteAsync($"/tasks/{createdTask.Id}");
        Assert.Equal(HttpStatusCode.NotFound, secondDeleteResponse.StatusCode);
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteTask_WithInvalidGuid_ReturnsNotFound()
    {
        // Arrange
        var invalidId = Guid.Empty;

        // Act
        var response = await _client.DeleteAsync($"/tasks/{invalidId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteTask_MultipleTasksOneDeleted_OthersRemain()
    {
        // Arrange - Create multiple tasks
        var task1 = new CreateTaskRequest { Title = "Task 1", Priority = "Low" };
        var task2 = new CreateTaskRequest { Title = "Task 2", Priority = "Medium" };
        var task3 = new CreateTaskRequest { Title = "Task 3", Priority = "High" };

        var response1 = await _client.PostAsJsonAsync("/tasks", task1);
        var response2 = await _client.PostAsJsonAsync("/tasks", task2);
        var response3 = await _client.PostAsJsonAsync("/tasks", task3);

        var createdTask1 = await response1.Content.ReadFromJsonAsync<TaskResponse>(_jsonOptions);
        var createdTask2 = await response2.Content.ReadFromJsonAsync<TaskResponse>(_jsonOptions);
        var createdTask3 = await response3.Content.ReadFromJsonAsync<TaskResponse>(_jsonOptions);

        Assert.NotNull(createdTask1);
        Assert.NotNull(createdTask2);
        Assert.NotNull(createdTask3);

        // Act - Delete task 2
        var deleteResponse = await _client.DeleteAsync($"/tasks/{createdTask2.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        // Assert - Verify task 2 is deleted
        var getDeleted = await _client.DeleteAsync($"/tasks/{createdTask2.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getDeleted.StatusCode);

        // Verify other tasks still exist by getting all tasks
        var getAllResponse = await _client.GetAsync("/tasks");
        var allTasks = await getAllResponse.Content.ReadFromJsonAsync<List<TaskResponse>>(_jsonOptions);
        
        Assert.NotNull(allTasks);
        Assert.Contains(allTasks, t => t.Id == createdTask1.Id);
        Assert.DoesNotContain(allTasks, t => t.Id == createdTask2.Id);
        Assert.Contains(allTasks, t => t.Id == createdTask3.Id);
    }
}
