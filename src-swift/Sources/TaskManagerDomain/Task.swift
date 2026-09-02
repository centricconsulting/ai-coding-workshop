import Foundation

/// Aggregate root representing work tracked in the workshop task manager.
public struct Task: Identifiable, Equatable, Sendable {
    public static let maximumTitleLength = 200

    public let id: TaskID
    public let title: String
    public let description: String?
    public let status: TaskStatus
    public let createdAt: Date
    public let updatedAt: Date
    public let completedAt: Date?

    private init(
        id: TaskID,
        title: String,
        description: String?,
        status: TaskStatus,
        createdAt: Date,
        updatedAt: Date,
        completedAt: Date?
    ) {
        self.id = id
        self.title = title
        self.description = description
        self.status = status
        self.createdAt = createdAt
        self.updatedAt = updatedAt
        self.completedAt = completedAt
    }

    public static func create(
        title: String,
        description: String? = nil,
        now: Date = Date(),
        id: TaskID = .new()
    ) throws -> Task {
        try Task(
            id: id,
            title: validateTitle(title),
            description: normalizeDescription(description),
            status: .todo,
            createdAt: now,
            updatedAt: now,
            completedAt: nil
        )
    }

    public static func reconstitute(
        id: TaskID,
        title: String,
        description: String?,
        status: TaskStatus,
        createdAt: Date,
        updatedAt: Date? = nil,
        completedAt: Date? = nil
    ) throws -> Task {
        let resolvedUpdatedAt = updatedAt ?? createdAt
        let resolvedCompletedAt = status == .done ? (completedAt ?? resolvedUpdatedAt) : nil

        return try Task(
            id: id,
            title: validateTitle(title),
            description: normalizeDescription(description),
            status: status,
            createdAt: createdAt,
            updatedAt: resolvedUpdatedAt,
            completedAt: resolvedCompletedAt
        )
    }

    public func updatingStatus(
        to newStatus: TaskStatus,
        now: Date = Date()
    ) throws -> Task {
        if status == .done && newStatus != .done {
            throw TaskError.completedTaskCannotBeReopened
        }

        if status == .cancelled && newStatus != .cancelled {
            throw TaskError.cancelledTaskCannotChangeStatus
        }

        if newStatus == status {
            return self
        }

        return Task(
            id: id,
            title: title,
            description: description,
            status: newStatus,
            createdAt: createdAt,
            updatedAt: now,
            completedAt: newStatus == .done ? (completedAt ?? now) : nil
        )
    }

    public func updatingDetails(
        title newTitle: String,
        description newDescription: String?,
        now: Date = Date()
    ) throws -> Task {
        guard status.isActive else {
            throw TaskError.inactiveTaskCannotBeEdited
        }

        return try Task(
            id: id,
            title: Self.validateTitle(newTitle),
            description: Self.normalizeDescription(newDescription),
            status: status,
            createdAt: createdAt,
            updatedAt: now,
            completedAt: completedAt
        )
    }

    private static func validateTitle(_ title: String) throws -> String {
        let trimmedTitle = title.trimmingCharacters(in: .whitespacesAndNewlines)

        guard !trimmedTitle.isEmpty else {
            throw TaskError.titleRequired
        }

        guard trimmedTitle.count <= maximumTitleLength else {
            throw TaskError.titleTooLong(maxLength: maximumTitleLength)
        }

        return trimmedTitle
    }

    private static func normalizeDescription(_ description: String?) -> String? {
        guard let description else {
            return nil
        }

        let trimmedDescription = description.trimmingCharacters(in: .whitespacesAndNewlines)
        return trimmedDescription.isEmpty ? nil : trimmedDescription
    }
}
