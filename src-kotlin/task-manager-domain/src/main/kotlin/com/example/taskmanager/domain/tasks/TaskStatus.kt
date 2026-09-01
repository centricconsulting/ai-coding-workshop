package com.example.taskmanager.domain.tasks

/**
 * Lifecycle states for a workshop task.
 */
enum class TaskStatus {
    TODO,
    IN_PROGRESS,
    DONE,
    CANCELLED,
    ;

    val isActive: Boolean
        get() = this != DONE && this != CANCELLED
}
