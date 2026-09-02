import Foundation
import XCTest
@testable import TaskManagerApplication
@testable import TaskManagerDomain
@testable import TaskManagerInfrastructure

final class CompleteTaskUseCaseTests: XCTestCase {
    func testExecuteMarksTaskDoneAndPersistsIt() throws {
        let createdAt = Date(timeIntervalSince1970: 1_725_000_000)
        let completedAt = Date(timeIntervalSince1970: 1_725_000_900)
        let task = try Task.create(title: "Review Swift lab", now: createdAt)
        let repository = InMemoryTaskRepository(seedTasks: [task])
        let useCase = CompleteTaskUseCase(repository: repository)

        let completedTask = try useCase.execute(id: task.id, now: completedAt)

        XCTAssertEqual(completedTask.status, .done)
        XCTAssertEqual(completedTask.completedAt, completedAt)
        XCTAssertEqual(repository.findByID(task.id)?.status, .done)
    }

    func testExecuteThrowsWhenTaskIsMissing() {
        let repository = InMemoryTaskRepository()
        let useCase = CompleteTaskUseCase(repository: repository)
        let missingID = TaskID(UUID(uuidString: "22222222-2222-2222-2222-222222222222")!)

        XCTAssertThrowsError(try useCase.execute(id: missingID)) { error in
            XCTAssertEqual(error as? TaskUseCaseError, .taskNotFound(missingID))
        }
    }
}
