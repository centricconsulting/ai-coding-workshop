package com.example.taskmanager.infrastructure

import com.example.taskmanager.domain.tasks.Task
import com.example.taskmanager.domain.tasks.TaskStatus
import org.junit.jupiter.api.Assertions.assertEquals
import org.junit.jupiter.api.Assertions.assertFalse
import org.junit.jupiter.api.Assertions.assertSame
import org.junit.jupiter.api.Assertions.assertTrue
import org.junit.jupiter.api.DisplayName
import org.junit.jupiter.api.Test

@DisplayName("InMemoryTaskRepository")
class InMemoryTaskRepositoryTest {
    @Test
    @DisplayName("save stores a task and findById returns it")
    fun saveStoresTasks() {
        val repository = InMemoryTaskRepository()
        val task = Task.create("Prepare Kotlin workshop", "Create the starter track")

        val savedTask = repository.save(task)

        assertSame(savedTask, repository.findById(task.id))
        assertTrue(repository.existsById(task.id))
        assertEquals(1L, repository.count())
    }

    @Test
    @DisplayName("findActiveTasks excludes completed and cancelled tasks")
    fun findActiveTasksExcludesInactiveTasks() {
        val repository = InMemoryTaskRepository()
        val activeTask = Task.create("Write Lab 2")
        val completedTask = Task.create("Ship Lab 1").updateStatus(TaskStatus.DONE)
        val cancelledTask = Task.create("Draft Android shell").updateStatus(TaskStatus.CANCELLED)

        repository.save(activeTask)
        repository.save(completedTask)
        repository.save(cancelledTask)

        assertEquals(listOf(activeTask), repository.findActiveTasks())
        assertEquals(1L, repository.countByStatus(TaskStatus.TODO))
        assertEquals(1L, repository.countByStatus(TaskStatus.DONE))
        assertFalse(repository.findByStatus(TaskStatus.IN_PROGRESS).isNotEmpty())
    }
}
