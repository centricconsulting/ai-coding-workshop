using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using TaskManager.Api;
using TaskManager.Api.Models;
using Xunit;

namespace TaskManager.IntegrationTests.Api;

public sealed class UpdateTaskEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public UpdateTaskEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    [Fact]
    public async System.Threading.Tasks.Task PutTask_WithValidRequest_ReturnsOkWithUpdatedTask()
    {
        // Arrange - First create a task
        var createRequest = new CreateTaskRequest
        {
            Title = "Original Title",
            Description = "Original Description",
            Priority = "Low",
            DueDate = null
        };

        var createResponse = await _client.PostAsJsonAsync("/tasks", createRequest);
        createResponse.EnsureSuccessStatusCode();
        
        var createdTask = await createResponse.Content.ReadFromJsonAsync<TaskResponse>(_jsonOptions);
        Assert.NotNull(createdTask);

        // Act - Update the task
        var updateRequest = new UpdateTaskRequest
        {
            Title = "Updated Title",
            Description = "Updated Description",
            Priority = "High",
            DueDate = DateTime.UtcNow.AddDays(7)
        };

        var updateResponse = await _client.PutAsJsonAsync($"/tasks/{createdTask.Id}", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
        Assert.Equal("application/json; charset=utf-8", updateResponse.Content.Headers.ContentType?.ToString());

        var updatedTask = await updateResponse.Content.ReadFromJsonAsync<TaskResponse>(_jsonOptions);
        Assert.NotNull(updatedTask);
        Assert.Equal(createdTask.Id, updatedTask.Id);
        Assert.Equal("Updated Title", updatedTask.Title);
        Assert.Equal("Updated Description", updatedTask.Description);
        Assert.Equal("High", updatedTask.Priority);
        Assert.NotNull(updatedTask.DueDate);
    }

    [Fact]
    public async System.Threading.Tasks.Task PutTask_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var updateRequest = new UpdateTaskRequest
        {
            Title = "Test Title",
            Description = "Test Description",
            Priority = "Medium",
            DueDate = null
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/tasks/{nonExistentId}", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(_jsonOptions);
        Assert.NotNull(problemDetails);
        Assert.Equal(404, problemDetails.Status);
        Assert.Contains("not found", problemDetails.Detail, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async System.Threading.Tasks.Task PutTask_WithInvalidPriority_ReturnsBadRequest()
    {
        // Arrange - First create a task
        var createRequest = new CreateTaskRequest
        {
            Title = "Test Task",
            Description = "Test Description",
            Priority = "Low",
            DueDate = null
        };

        var createResponse = await _client.PostAsJsonAsync("/tasks", createRequest);
        var createdTask = await createResponse.Content.ReadFromJsonAsync<TaskResponse>(_jsonOptions);
        Assert.NotNull(createdTask);

        // Act - Try to update with invalid priority
        var updateRequest = new
        {
            Title = "Test Title",
            Description = "Test Description",
            Priority = "InvalidPriority",
            DueDate = (DateTime?)null
        };

        var response = await _client.PutAsJsonAsync($"/tasks/{createdTask.Id}", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(_jsonOptions);
        Assert.NotNull(problemDetails);
        Assert.Equal(400, problemDetails.Status);
    }

    [Fact]
    public async System.Threading.Tasks.Task PutTask_WithPastDueDate_ReturnsBadRequest()
    {
        // Arrange - First create a task
        var createRequest = new CreateTaskRequest
        {
            Title = "Test Task",
            Description = "Test Description",
            Priority = "Medium",
            DueDate = null
        };

        var createResponse = await _client.PostAsJsonAsync("/tasks", createRequest);
        var createdTask = await createResponse.Content.ReadFromJsonAsync<TaskResponse>(_jsonOptions);
        Assert.NotNull(createdTask);

        // Act - Try to update with past due date
        var updateRequest = new UpdateTaskRequest
        {
            Title = "Test Title",
            Description = "Test Description",
            Priority = "High",
            DueDate = DateTime.UtcNow.AddDays(-1) // Past date
        };

        var response = await _client.PutAsJsonAsync($"/tasks/{createdTask.Id}", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(_jsonOptions);
        Assert.NotNull(problemDetails);
        Assert.Equal(400, problemDetails.Status);
        Assert.Contains("past", problemDetails.Detail, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async System.Threading.Tasks.Task PutTask_WithMissingTitle_ReturnsBadRequest()
    {
        // Arrange - First create a task
        var createRequest = new CreateTaskRequest
        {
            Title = "Test Task",
            Description = "Test Description",
            Priority = "Low",
            DueDate = null
        };

        var createResponse = await _client.PostAsJsonAsync("/tasks", createRequest);
        var createdTask = await createResponse.Content.ReadFromJsonAsync<TaskResponse>(_jsonOptions);
        Assert.NotNull(createdTask);

        // Act - Try to update without title
        var json = "{\"Description\":\"Test\",\"Priority\":\"High\"}";
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PutAsync($"/tasks/{createdTask.Id}", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(_jsonOptions);
        Assert.NotNull(problemDetails);
        Assert.Equal(400, problemDetails.Status);
        Assert.Contains("title", problemDetails.Detail, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async System.Threading.Tasks.Task PutTask_WithEmptyTitle_ReturnsBadRequest()
    {
        // Arrange - First create a task
        var createRequest = new CreateTaskRequest
        {
            Title = "Test Task",
            Description = "Test Description",
            Priority = "Low",
            DueDate = null
        };

        var createResponse = await _client.PostAsJsonAsync("/tasks", createRequest);
        var createdTask = await createResponse.Content.ReadFromJsonAsync<TaskResponse>(_jsonOptions);
        Assert.NotNull(createdTask);

        // Act - Try to update with empty title
        var updateRequest = new UpdateTaskRequest
        {
            Title = "",
            Description = "Test Description",
            Priority = "Medium",
            DueDate = null
        };

        var response = await _client.PutAsJsonAsync($"/tasks/{createdTask.Id}", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(_jsonOptions);
        Assert.NotNull(problemDetails);
        Assert.Equal(400, problemDetails.Status);
    }

    [Fact]
    public async System.Threading.Tasks.Task PutTask_WithTitleTooLong_ReturnsBadRequest()
    {
        // Arrange - First create a task
        var createRequest = new CreateTaskRequest
        {
            Title = "Test Task",
            Description = "Test Description",
            Priority = "Low",
            DueDate = null
        };

        var createResponse = await _client.PostAsJsonAsync("/tasks", createRequest);
        var createdTask = await createResponse.Content.ReadFromJsonAsync<TaskResponse>(_jsonOptions);
        Assert.NotNull(createdTask);

        // Act - Try to update with title > 200 characters
        var updateRequest = new UpdateTaskRequest
        {
            Title = new string('A', 201),
            Description = "Test Description",
            Priority = "Medium",
            DueDate = null
        };

        var response = await _client.PutAsJsonAsync($"/tasks/{createdTask.Id}", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(_jsonOptions);
        Assert.NotNull(problemDetails);
        Assert.Equal(400, problemDetails.Status);
        Assert.Contains("200", problemDetails.Detail);
    }

    [Fact]
    public async System.Threading.Tasks.Task PutTask_ClearsDueDate_WhenSetToNull()
    {
        // Arrange - First create a task with a due date
        var createRequest = new CreateTaskRequest
        {
            Title = "Test Task",
            Description = "Test Description",
            Priority = "Low",
            DueDate = DateTime.UtcNow.AddDays(5)
        };

        var createResponse = await _client.PostAsJsonAsync("/tasks", createRequest);
        var createdTask = await createResponse.Content.ReadFromJsonAsync<TaskResponse>(_jsonOptions);
        Assert.NotNull(createdTask);
        Assert.NotNull(createdTask.DueDate);

        // Act - Update with null due date
        var updateRequest = new UpdateTaskRequest
        {
            Title = "Updated Task",
            Description = "Updated Description",
            Priority = "High",
            DueDate = null
        };

        var response = await _client.PutAsJsonAsync($"/tasks/{createdTask.Id}", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var updatedTask = await response.Content.ReadFromJsonAsync<TaskResponse>(_jsonOptions);
        Assert.NotNull(updatedTask);
        Assert.Null(updatedTask.DueDate);
    }
}
