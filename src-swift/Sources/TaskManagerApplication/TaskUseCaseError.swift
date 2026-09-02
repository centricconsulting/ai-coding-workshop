import Foundation
import TaskManagerDomain

/// Application-layer errors for task workflows.
public enum TaskUseCaseError: Error, Equatable, LocalizedError {
    case taskNotFound(TaskID)

    public var errorDescription: String? {
        switch self {
        case let .taskNotFound(id):
            return "Task not found with ID: \(id)"
        }
    }
}
