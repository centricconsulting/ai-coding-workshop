/// Persistence port for `Task` aggregates.
public protocol TaskRepository: AnyObject {
    @discardableResult
    func save(_ task: Task) -> Task

    func findByID(_ id: TaskID) -> Task?

    func findByStatus(_ status: TaskStatus) -> [Task]

    func findAll() -> [Task]

    func deleteByID(_ id: TaskID)

    func exists(_ id: TaskID) -> Bool

    func count() -> Int

    func count(status: TaskStatus) -> Int
}

public extension TaskRepository {
    func findTodoTasks() -> [Task] {
        findByStatus(.todo)
    }

    func findInProgressTasks() -> [Task] {
        findByStatus(.inProgress)
    }

    func findCompletedTasks() -> [Task] {
        findByStatus(.done)
    }

    func findActiveTasks() -> [Task] {
        findAll().filter { $0.status.isActive }
    }
}
