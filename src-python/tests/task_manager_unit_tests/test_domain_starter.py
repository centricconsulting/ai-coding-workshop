from task_manager_domain import Task, TaskId, TaskStatus


def test_task_id_new_creates_uuid_backed_value() -> None:
    task_id = TaskId.new()

    assert str(task_id)
    assert task_id.value.version == 4


def test_task_create_uses_default_todo_status() -> None:
    task = Task.create("Write workshop notes", "Summarize the Python track")

    assert task.status is TaskStatus.TODO
    assert task.created_at == task.updated_at


def test_update_status_changes_status_and_timestamp() -> None:
    task = Task.create("Prepare demo", "Set up the FastAPI walkthrough")
    original_updated_at = task.updated_at

    task.update_status(TaskStatus.IN_PROGRESS)

    assert task.status is TaskStatus.IN_PROGRESS
    assert task.updated_at >= original_updated_at
