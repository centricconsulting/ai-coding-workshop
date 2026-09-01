package com.example.taskmanager.domain.tasks

import java.util.UUID

/**
 * Strongly typed identifier for a [Task].
 */
@JvmInline
value class TaskId(val value: UUID) {
    override fun toString(): String = value.toString()

    companion object {
        fun new(): TaskId = TaskId(UUID.randomUUID())

        fun fromUuid(value: UUID): TaskId = TaskId(value)

        fun fromString(value: String): TaskId = TaskId(UUID.fromString(value))
    }
}
