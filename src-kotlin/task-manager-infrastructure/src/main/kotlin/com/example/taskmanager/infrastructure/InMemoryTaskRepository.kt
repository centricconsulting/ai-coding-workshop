package com.example.taskmanager.infrastructure

import com.example.taskmanager.domain.tasks.Task
import com.example.taskmanager.domain.tasks.TaskId
import com.example.taskmanager.domain.tasks.TaskRepository
import com.example.taskmanager.domain.tasks.TaskStatus
import java.util.LinkedHashMap

/**
 * Simple in-memory repository used in workshop labs and examples.
 */
class InMemoryTaskRepository : TaskRepository {
    private val tasks = LinkedHashMap<TaskId, Task>()

    override fun save(task: Task): Task {
        tasks[task.id] = task
        return task
    }

    override fun findById(id: TaskId): Task? = tasks[id]

    override fun findByStatus(status: TaskStatus): List<Task> =
        tasks.values.filter { it.status == status }

    override fun findAll(): List<Task> = tasks.values.toList()

    override fun deleteById(id: TaskId) {
        tasks.remove(id)
    }

    override fun existsById(id: TaskId): Boolean = tasks.containsKey(id)

    override fun count(): Long = tasks.size.toLong()

    override fun countByStatus(status: TaskStatus): Long =
        tasks.values.count { it.status == status }.toLong()
}
