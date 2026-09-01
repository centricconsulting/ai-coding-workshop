"""Repository protocol for the Task aggregate."""

from __future__ import annotations

from typing import Protocol

from task_manager_domain.tasks import Task, TaskId


class TaskRepository(Protocol):
    """Business-intent repository contract for tasks."""

    async def find_by_id(self, task_id: TaskId) -> Task | None:
        """Return one task by identifier if it exists."""

    async def get_active_tasks(self) -> list[Task]:
        """Return tasks that are still active."""

    async def add_task(self, task: Task) -> None:
        """Add a new task to the repository."""

    async def save_changes(self, task: Task) -> None:
        """Persist changes for an existing task."""
