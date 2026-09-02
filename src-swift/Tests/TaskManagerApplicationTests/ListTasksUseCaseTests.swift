import Foundation
import XCTest
@testable import TaskManagerApplication
@testable import TaskManagerDomain
@testable import TaskManagerInfrastructure

final class ListTasksUseCaseTests: XCTestCase {
    func testExecuteReturnsNewestTasksFirst() throws {
        let oldest = try Task.reconstitute(
            id: TaskID(UUID(uuidString: "33333333-3333-3333-3333-333333333333")!),
            title: "First draft",
            description: nil,
            status: .todo,
            createdAt: Date(timeIntervalSince1970: 100)
        )
        let newest = try Task.reconstitute(
            id: TaskID(UUID(uuidString: "44444444-4444-4444-4444-444444444444")!),
            title: "Latest draft",
            description: nil,
            status: .inProgress,
            createdAt: Date(timeIntervalSince1970: 300)
        )
        let middle = try Task.reconstitute(
            id: TaskID(UUID(uuidString: "55555555-5555-5555-5555-555555555555")!),
            title: "Middle draft",
            description: nil,
            status: .done,
            createdAt: Date(timeIntervalSince1970: 200)
        )
        let repository = InMemoryTaskRepository(seedTasks: [oldest, newest, middle])

        let listedTasks = ListTasksUseCase(repository: repository).execute()

        XCTAssertEqual(listedTasks.map(\.title), ["Latest draft", "Middle draft", "First draft"])
    }

    func testActiveListExcludesDoneAndCancelledTasks() throws {
        let todoTask = try Task.reconstitute(
            id: TaskID(UUID(uuidString: "66666666-6666-6666-6666-666666666666")!),
            title: "Write setup doc",
            description: nil,
            status: .todo,
            createdAt: Date(timeIntervalSince1970: 100)
        )
        let inProgressTask = try Task.reconstitute(
            id: TaskID(UUID(uuidString: "77777777-7777-7777-7777-777777777777")!),
            title: "Build SwiftUI shell",
            description: nil,
            status: .inProgress,
            createdAt: Date(timeIntervalSince1970: 200)
        )
        let completedTask = try Task.reconstitute(
            id: TaskID(UUID(uuidString: "88888888-8888-8888-8888-888888888888")!),
            title: "Ship outline",
            description: nil,
            status: .done,
            createdAt: Date(timeIntervalSince1970: 300),
            completedAt: Date(timeIntervalSince1970: 350)
        )
        let cancelledTask = try Task.reconstitute(
            id: TaskID(UUID(uuidString: "99999999-9999-9999-9999-999999999999")!),
            title: "Prototype Linux container",
            description: nil,
            status: .cancelled,
            createdAt: Date(timeIntervalSince1970: 400)
        )
        let repository = InMemoryTaskRepository(seedTasks: [todoTask, inProgressTask, completedTask, cancelledTask])

        let activeTasks = ListActiveTasksUseCase(repository: repository).execute()

        XCTAssertEqual(activeTasks.map(\.title), ["Build SwiftUI shell", "Write setup doc"])
    }
}
