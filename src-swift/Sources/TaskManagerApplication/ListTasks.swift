import TaskManagerDomain

/// Lists all tasks from newest to oldest.
public final class ListTasksUseCase {
    private let repository: any TaskRepository

    public init(repository: any TaskRepository) {
        self.repository = repository
    }

    public func execute() -> [Task] {
        repository.findAll().sorted { $0.createdAt > $1.createdAt }
    }
}

/// Lists active tasks from newest to oldest.
public final class ListActiveTasksUseCase {
    private let repository: any TaskRepository

    public init(repository: any TaskRepository) {
        self.repository = repository
    }

    public func execute() -> [Task] {
        repository.findActiveTasks().sorted { $0.createdAt > $1.createdAt }
    }
}
