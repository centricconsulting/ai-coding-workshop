import asyncio

from task_manager_domain import Task, TaskStatus
from task_manager_infrastructure import InMemoryTaskRepository


def test_get_active_tasks_excludes_done_and_cancelled_tasks() -> None:
    async def run_test() -> None:
        repository = InMemoryTaskRepository()
        active_task = Task.create("Build sample", "Keep an active task")
        done_task = Task.create("Archive notes", "This one should be filtered out")
        done_task.update_status(TaskStatus.DONE)

        await repository.add_task(active_task)
        await repository.add_task(done_task)

        active_tasks = await repository.get_active_tasks()

        assert active_tasks == [active_task]

    asyncio.run(run_test())
