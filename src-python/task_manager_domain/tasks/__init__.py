"""Task-focused domain types.

This subpackage mirrors the feature-oriented `Tasks/` folder in the .NET track,
which keeps the workshop paths easy to compare across languages.
"""

from .task import Task
from .task_id import TaskId
from .task_status import TaskStatus

__all__ = ["Task", "TaskId", "TaskStatus"]
