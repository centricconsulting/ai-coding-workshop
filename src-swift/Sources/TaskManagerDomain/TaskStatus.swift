/// Lifecycle states for a workshop task.
public enum TaskStatus: String, Codable, CaseIterable, Sendable {
    case todo = "TODO"
    case inProgress = "IN_PROGRESS"
    case done = "DONE"
    case cancelled = "CANCELLED"

    public var isActive: Bool {
        self != .done && self != .cancelled
    }

    public var displayName: String {
        switch self {
        case .todo:
            return "To Do"
        case .inProgress:
            return "In Progress"
        case .done:
            return "Done"
        case .cancelled:
            return "Cancelled"
        }
    }
}
