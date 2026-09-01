"""Infrastructure layer: in-memory repository implementation for labs.

Mirrors TaskManager.Infrastructure (.NET) and taskmanager-infrastructure (Spring Boot).
"""

from .in_memory_task_repository import InMemoryTaskRepository
from .legacy import process_task

__all__ = ["InMemoryTaskRepository", "process_task"]
