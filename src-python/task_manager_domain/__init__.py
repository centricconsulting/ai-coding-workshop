"""Domain layer: Task entity, TaskId, TaskStatus, and the TaskRepository protocol.

This package stays pure Python and uses a `tasks/` subpackage so the workshop
can mirror the feature-oriented structure used in the .NET reference track.
"""

from .repository import TaskRepository
from .tasks import Task, TaskId, TaskStatus

__all__ = ["Task", "TaskId", "TaskRepository", "TaskStatus"]
