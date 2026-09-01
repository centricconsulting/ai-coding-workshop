package com.example.taskmanager.domain.tasks

/**
 * Persistence port for [Task] aggregates.
 */
interface TaskRepository {
    fun save(task: Task): Task

    fun findById(id: TaskId): Task?

    fun findByStatus(status: TaskStatus): List<Task>

    fun findAll(): List<Task>

    fun findTodoTasks(): List<Task> = findByStatus(TaskStatus.TODO)

    fun findInProgressTasks(): List<Task> = findByStatus(TaskStatus.IN_PROGRESS)

    fun findCompletedTasks(): List<Task> = findByStatus(TaskStatus.DONE)

    fun findActiveTasks(): List<Task> = findAll().filter { it.status.isActive }

    fun deleteById(id: TaskId)

    fun existsById(id: TaskId): Boolean

    fun count(): Long

    fun countByStatus(status: TaskStatus): Long
}
