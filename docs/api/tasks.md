# Tasks API Documentation

This document describes the endpoints, request/response formats, and error handling for the Tasks API in the sample Task Manager application.

---

## Base URL
`/tasks`

## Endpoints

### 1. Create a Task
- **POST** `/tasks`
- **Description:** Create a new task.
- **Request Body:**
  ```json
  {
    "title": "string",
    "description": "string",
    "dueDate": "2025-10-31T23:59:59Z",
    "priority": "Low|Medium|High"
  }
  ```
- **Response:**
  - `201 Created`
  - Body:
    ```json
    {
      "id": "string",
      "title": "string",
      "description": "string",
      "dueDate": "2025-10-31T23:59:59Z",
      "priority": "Low|Medium|High",
      "status": "Pending|Completed"
    }
    ```

### 2. Get a Task by ID
- **GET** `/tasks/{id}`
- **Description:** Retrieve details for a specific task.
- **Response:**
  - `200 OK`
  - Body: Same as above
  - `404 Not Found` if task does not exist

### 3. List All Tasks
- **GET** `/tasks`
- **Description:** Retrieve all tasks (optionally filter by status, priority, or due date).
- **Query Parameters:**
  - `status` (optional): `Pending|Completed`
  - `priority` (optional): `Low|Medium|High`
- **Response:**
  - `200 OK`
  - Body:
    ```json
    [
      {
        "id": "string",
        "title": "string",
        "description": "string",
        "dueDate": "2025-10-31T23:59:59Z",
        "priority": "Low|Medium|High",
        "status": "Pending|Completed"
      },
      // ...more tasks
    ]
    ```

### 4. Update a Task
- **PUT** `/tasks/{id}`
- **Description:** Update details of an existing task.
- **Request Body:** Same as Create
- **Response:**
  - `200 OK` with updated task
  - `404 Not Found` if task does not exist

### 5. Delete a Task
- **DELETE** `/tasks/{id}`
- **Description:** Delete a task by ID.
- **Response:**
  - `204 No Content` on success
  - `404 Not Found` if task does not exist

---

## Error Handling
- All errors use [ProblemDetails](https://datatracker.ietf.org/doc/html/rfc7807) format for consistency.
- Example error response:
  ```json
  {
    "type": "https://example.com/probs/task-not-found",
    "title": "Task Not Found",
    "status": 404,
    "detail": "No task found with the specified ID.",
    "instance": "/tasks/123"
  }
  ```

---

*See also: [Sample Solution Architecture](../design/architecture.md), [Glossary](../design/glossary.md)*
