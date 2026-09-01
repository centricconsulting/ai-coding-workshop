import XCTest
@testable import TaskManagerDomain
@testable import TaskManagerInfrastructure

final class InMemoryTaskRepositoryTests: XCTestCase {
    func testSaveStoresTaskAndFindByIDReturnsIt() throws {
        let repository = InMemoryTaskRepository()
        let task = try Task.create(title: "Prepare Swift workshop", description: "Create the starter track")

        let savedTask = repository.save(task)

        XCTAssertEqual(repository.findByID(task.id), savedTask)
        XCTAssertTrue(repository.exists(task.id))
        XCTAssertEqual(repository.count(), 1)
    }

    func testFindActiveTasksExcludesCompletedAndCancelledTasks() throws {
        let repository = InMemoryTaskRepository()
        let activeTask = try Task.create(title: "Write Lab 2")
        let completedTask = try Task.create(title: "Ship Lab 1").updatingStatus(to: .done)
        let cancelledTask = try Task.create(title: "Draft watchOS shell").updatingStatus(to: .cancelled)

        repository.save(activeTask)
        repository.save(completedTask)
        repository.save(cancelledTask)

        XCTAssertEqual(repository.findActiveTasks(), [activeTask])
        XCTAssertEqual(repository.count(status: .todo), 1)
        XCTAssertEqual(repository.count(status: .done), 1)
        XCTAssertEqual(repository.findByStatus(.inProgress), [])
    }

    func testDeleteByIDRemovesTask() throws {
        let repository = InMemoryTaskRepository()
        let task = try Task.create(title: "Remove me")
        repository.save(task)

        repository.deleteByID(task.id)

        XCTAssertNil(repository.findByID(task.id))
        XCTAssertFalse(repository.exists(task.id))
        XCTAssertEqual(repository.count(), 0)
    }
}
