import Foundation

/// Strongly typed identifier for a workshop task.
public struct TaskID: Hashable, Codable, Sendable, CustomStringConvertible {
    public let value: UUID

    public init(_ value: UUID) {
        self.value = value
    }

    public init?(string: String) {
        guard let uuid = UUID(uuidString: string) else {
            return nil
        }

        self.value = uuid
    }

    public static func new() -> TaskID {
        TaskID(UUID())
    }

    public var description: String {
        value.uuidString
    }
}
