import Foundation
import TaskManagerDomain

/// Marks an existing task as complete.
public final class CompleteTaskUseCase {
    private let repository: any TaskRepository

    public init(repository: any TaskRepository) {
        self.repository = repository
    }

    public func execute(
        id: TaskID,
        now: Date = Date()
    ) throws -> Task {
        guard let existingTask = repository.findByID(id) else {
            throw TaskUseCaseError.taskNotFound(id)
        }

        let completedTask = try existingTask.updatingStatus(to: .done, now: now)
        return repository.save(completedTask)
    }
}
