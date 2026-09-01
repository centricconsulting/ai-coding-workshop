import Foundation

/// Domain errors raised when task invariants are violated.
public enum TaskError: Error, Equatable, LocalizedError {
    case titleRequired
    case titleTooLong(maxLength: Int)
    case completedTaskCannotBeReopened
    case cancelledTaskCannotChangeStatus
    case inactiveTaskCannotBeEdited

    public var errorDescription: String? {
        switch self {
        case .titleRequired:
            return "Task title is required."
        case let .titleTooLong(maxLength):
            return "Task title must be \(maxLength) characters or fewer."
        case .completedTaskCannotBeReopened:
            return "Completed tasks cannot be reopened."
        case .cancelledTaskCannotChangeStatus:
            return "Cancelled tasks cannot change status."
        case .inactiveTaskCannotBeEdited:
            return "Completed or cancelled tasks cannot be edited."
        }
    }
}
