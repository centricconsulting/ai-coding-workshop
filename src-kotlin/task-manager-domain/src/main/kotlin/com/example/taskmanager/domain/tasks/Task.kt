package com.example.taskmanager.domain.tasks

import java.time.Instant

/**
 * Aggregate root for a task in the workshop task manager.
 */
@ConsistentCopyVisibility
data class Task private constructor(
    val id: TaskId,
    val title: String,
    val description: String?,
    val status: TaskStatus,
    val createdAt: Instant,
    val updatedAt: Instant,
    val completedAt: Instant?,
) {
    fun updateStatus(newStatus: TaskStatus): Task {
        if (status == TaskStatus.DONE && newStatus != TaskStatus.DONE) {
            throw IllegalStateException("Completed tasks cannot be reopened.")
        }

        if (status == TaskStatus.CANCELLED && newStatus != TaskStatus.CANCELLED) {
            throw IllegalStateException("Cancelled tasks cannot change status.")
        }

        if (newStatus == status) {
            return this
        }

        val now = Instant.now()

        return copy(
            status = newStatus,
            updatedAt = now,
            completedAt = if (newStatus == TaskStatus.DONE) completedAt ?: now else null,
        )
    }

    fun updateDetails(newTitle: String, newDescription: String?): Task {
        if (!status.isActive) {
            throw IllegalStateException("Completed or cancelled tasks cannot be edited.")
        }

        return copy(
            title = validateTitle(newTitle),
            description = normalizeDescription(newDescription),
            updatedAt = Instant.now(),
        )
    }

    companion object {
        fun create(title: String, description: String? = null): Task {
            val now = Instant.now()

            return Task(
                id = TaskId.new(),
                title = validateTitle(title),
                description = normalizeDescription(description),
                status = TaskStatus.TODO,
                createdAt = now,
                updatedAt = now,
                completedAt = null,
            )
        }

        fun reconstitute(
            id: TaskId,
            title: String,
            description: String?,
            status: TaskStatus,
            createdAt: Instant,
            updatedAt: Instant = createdAt,
            completedAt: Instant? = null,
        ): Task = Task(
            id = id,
            title = validateTitle(title),
            description = normalizeDescription(description),
            status = status,
            createdAt = createdAt,
            updatedAt = updatedAt,
            completedAt = completedAt,
        )

        private fun validateTitle(title: String): String {
            val trimmedTitle = title.trim()

            require(trimmedTitle.isNotBlank()) { "Task title is required." }
            require(trimmedTitle.length <= 200) { "Task title must be 200 characters or fewer." }

            return trimmedTitle
        }

        private fun normalizeDescription(description: String?): String? =
            description
                ?.trim()
                ?.takeIf { it.isNotEmpty() }
    }
}
