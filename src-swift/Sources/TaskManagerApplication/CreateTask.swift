import Foundation
import TaskManagerDomain

/// Input for creating a new task.
public struct CreateTaskCommand: Equatable, Sendable {
    public let title: String
    public let description: String?

    public init(title: String, description: String? = nil) {
        self.title = title
        self.description = description
    }
}

/// Creates and persists a new task using the configured repository.
public final class CreateTaskUseCase {
    private let repository: any TaskRepository

    public init(repository: any TaskRepository) {
        self.repository = repository
    }

    public func execute(
        _ command: CreateTaskCommand,
        now: Date = Date()
    ) throws -> Task {
        let task = try Task.create(
            title: command.title,
            description: command.description,
            now: now
        )

        return repository.save(task)
    }
}
