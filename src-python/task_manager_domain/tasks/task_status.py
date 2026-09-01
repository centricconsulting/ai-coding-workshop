"""Task status values used by the Task aggregate."""

from enum import Enum


class TaskStatus(str, Enum):
    """Represents the lifecycle state of a task."""

    TODO = "TODO"
    IN_PROGRESS = "IN_PROGRESS"
    DONE = "DONE"
    CANCELLED = "CANCELLED"
