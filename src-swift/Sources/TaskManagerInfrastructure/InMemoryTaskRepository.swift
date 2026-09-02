import TaskManagerDomain

/// Simple in-memory repository used in workshop labs and examples.
public final class InMemoryTaskRepository: TaskRepository {
    private var tasksByID: [TaskID: Task] = [:]
    private var taskOrder: [TaskID] = []

    public init(seedTasks: [Task] = []) {
        seedTasks.forEach { _ = save($0) }
    }

    @discardableResult
    public func save(_ task: Task) -> Task {
        if tasksByID[task.id] == nil {
            taskOrder.append(task.id)
        }

        tasksByID[task.id] = task
        return task
    }

    public func findByID(_ id: TaskID) -> Task? {
        tasksByID[id]
    }

    public func findByStatus(_ status: TaskStatus) -> [Task] {
        findAll().filter { $0.status == status }
    }

    public func findAll() -> [Task] {
        taskOrder.compactMap { tasksByID[$0] }
    }

    public func deleteByID(_ id: TaskID) {
        tasksByID.removeValue(forKey: id)
        taskOrder.removeAll { $0 == id }
    }

    public func exists(_ id: TaskID) -> Bool {
        tasksByID[id] != nil
    }

    public func count() -> Int {
        tasksByID.count
    }

    public func count(status: TaskStatus) -> Int {
        tasksByID.values.filter { $0.status == status }.count
    }
}
