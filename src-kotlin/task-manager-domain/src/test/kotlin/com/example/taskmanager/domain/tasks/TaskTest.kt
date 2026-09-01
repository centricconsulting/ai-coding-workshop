package com.example.taskmanager.domain.tasks

import org.junit.jupiter.api.Assertions.assertEquals
import org.junit.jupiter.api.Assertions.assertNotNull
import org.junit.jupiter.api.Assertions.assertNull
import org.junit.jupiter.api.Assertions.assertThrows
import org.junit.jupiter.api.Assertions.assertTrue
import org.junit.jupiter.api.DisplayName
import org.junit.jupiter.api.Test

@DisplayName("Task")
class TaskTest {
    @Test
    @DisplayName("create sets the default status and timestamps")
    fun createSetsDefaultValues() {
        val task = Task.create(" Draft Kotlin labs ", "Mirror the workshop structure")

        assertNotNull(task.id)
        assertEquals("Draft Kotlin labs", task.title)
        assertEquals("Mirror the workshop structure", task.description)
        assertEquals(TaskStatus.TODO, task.status)
        assertEquals(task.createdAt, task.updatedAt)
        assertNull(task.completedAt)
    }

    @Test
    @DisplayName("create rejects a blank title")
    fun createRejectsBlankTitle() {
        val exception = assertThrows(IllegalArgumentException::class.java) {
            Task.create("   ", "Missing title")
        }

        assertTrue(exception.message!!.contains("required"))
    }

    @Test
    @DisplayName("create rejects titles longer than 200 characters")
    fun createRejectsLongTitles() {
        val exception = assertThrows(IllegalArgumentException::class.java) {
            Task.create("a".repeat(201))
        }

        assertTrue(exception.message!!.contains("200"))
    }

    @Test
    @DisplayName("update status marks a task as completed")
    fun updateStatusMarksTaskAsCompleted() {
        val task = Task.create("Ship Kotlin starter")

        val completedTask = task.updateStatus(TaskStatus.DONE)

        assertEquals(TaskStatus.DONE, completedTask.status)
        assertNotNull(completedTask.completedAt)
        assertTrue(completedTask.updatedAt >= task.updatedAt)
    }

    @Test
    @DisplayName("completed tasks cannot be reopened")
    fun completedTasksCannotBeReopened() {
        val task = Task.create("Ship Kotlin starter").updateStatus(TaskStatus.DONE)

        val exception = assertThrows(IllegalStateException::class.java) {
            task.updateStatus(TaskStatus.TODO)
        }

        assertTrue(exception.message!!.contains("reopened"))
    }

    @Test
    @DisplayName("update details returns an edited copy for active tasks")
    fun updateDetailsReturnsEditedCopy() {
        val task = Task.create("Write lab", "Add Kotlin examples")

        val updatedTask = task.updateDetails("Write Kotlin lab", "Add null-safety examples")

        assertEquals("Write Kotlin lab", updatedTask.title)
        assertEquals("Add null-safety examples", updatedTask.description)
        assertTrue(updatedTask.updatedAt >= task.updatedAt)
    }

    @Test
    @DisplayName("completed tasks cannot be edited")
    fun completedTasksCannotBeEdited() {
        val task = Task.create("Write lab").updateStatus(TaskStatus.DONE)

        val exception = assertThrows(IllegalStateException::class.java) {
            task.updateDetails("Rewrite lab", "Add more notes")
        }

        assertTrue(exception.message!!.contains("cannot be edited"))
    }
}
