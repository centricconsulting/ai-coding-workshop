"""In-memory repository implementation for workshop purposes."""

from __future__ import annotations

from task_manager_domain import Task, TaskId, TaskStatus


class InMemoryTaskRepository:
    """Store tasks in memory using a plain dictionary."""

    def __init__(self) -> None:
        self._tasks: dict[TaskId, Task] = {}

    async def find_by_id(self, task_id: TaskId) -> Task | None:
        return self._tasks.get(task_id)

    async def get_active_tasks(self) -> list[Task]:
        return [
            task
            for task in self._tasks.values()
            if task.status not in {TaskStatus.DONE, TaskStatus.CANCELLED}
        ]

    async def add_task(self, task: Task) -> None:
        self._tasks[task.id] = task

    async def save_changes(self, task: Task) -> None:
        self._tasks[task.id] = task
