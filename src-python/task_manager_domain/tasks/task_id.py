"""Strongly typed identifier for Task entities."""

from __future__ import annotations

from dataclasses import dataclass
from uuid import UUID, uuid4


@dataclass(frozen=True, slots=True)
class TaskId:
    """Small value type that wraps a UUID for task identity."""

    value: UUID

    @classmethod
    def new(cls) -> "TaskId":
        """Create a new task identifier."""
        return cls(uuid4())

    @classmethod
    def from_value(cls, value: UUID | str) -> "TaskId":
        """Create a task identifier from an existing UUID value."""
        return cls(UUID(str(value)))

    def __str__(self) -> str:
        return str(self.value)
