"""Task aggregate root representing a work item to be completed."""

from __future__ import annotations

from dataclasses import dataclass
from datetime import datetime, timezone

from .task_id import TaskId
from .task_status import TaskStatus


@dataclass(slots=True)
class Task:
    """Domain entity for a task in the workshop task manager."""

    id: TaskId
    title: str
    description: str
    status: TaskStatus
    created_at: datetime
    updated_at: datetime

    @classmethod
    def create(cls, title: str, description: str) -> "Task":
        """Factory method to create a new task."""
        # TODO: Add validation (title not null/empty, description not null).
        # This is where Copilot will help participants implement validation.
        now = datetime.now(timezone.utc)
        return cls(
            id=TaskId.new(),
            title=title,
            description=description,
            status=TaskStatus.TODO,
            created_at=now,
            updated_at=now,
        )

    def update_status(self, new_status: TaskStatus) -> None:
        """Update the task status."""
        # TODO: Add business rules (e.g., can't move from DONE to TODO directly).
        # This will be implemented during the workshop.
        self.status = new_status
        self.updated_at = datetime.now(timezone.utc)

    def update_details(self, title: str, description: str) -> None:
        """Update the task details."""
        # TODO: Add validation.
        # This will be implemented during the workshop.
        self.title = title
        self.description = description
        self.updated_at = datetime.now(timezone.utc)
