import Foundation
import XCTest
@testable import TaskManagerApplication
@testable import TaskManagerInfrastructure

final class CreateTaskUseCaseTests: XCTestCase {
    func testExecuteCreatesAndSavesTask() throws {
        let repository = InMemoryTaskRepository()
        let useCase = CreateTaskUseCase(repository: repository)
        let now = Date(timeIntervalSince1970: 1_725_001_000)

        let task = try useCase.execute(
            CreateTaskCommand(
                title: " Draft Swift setup guide ",
                description: " Add Xcode steps "
            ),
            now: now
        )

        XCTAssertEqual(task.title, "Draft Swift setup guide")
        XCTAssertEqual(task.description, "Add Xcode steps")
        XCTAssertEqual(repository.count(), 1)
        XCTAssertEqual(repository.findAll().first?.id, task.id)
    }
}
