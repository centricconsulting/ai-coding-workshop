import Foundation
import XCTest
@testable import TaskManagerDomain

final class TaskTests: XCTestCase {
    func testCreateTrimsTitleAndSetsDefaultValues() throws {
        let now = Date(timeIntervalSince1970: 1_725_000_000)
        let task = try Task.create(
            title: " Draft Swift labs ",
            description: " Mirror the workshop structure ",
            now: now,
            id: TaskID(UUID(uuidString: "11111111-1111-1111-1111-111111111111")!)
        )

        XCTAssertEqual(task.title, "Draft Swift labs")
        XCTAssertEqual(task.description, "Mirror the workshop structure")
        XCTAssertEqual(task.status, .todo)
        XCTAssertEqual(task.createdAt, now)
        XCTAssertEqual(task.updatedAt, now)
        XCTAssertNil(task.completedAt)
    }

    func testCreateRejectsBlankTitle() {
        XCTAssertThrowsError(try Task.create(title: "   ")) { error in
            XCTAssertEqual(error as? TaskError, .titleRequired)
        }
    }

    func testCreateRejectsLongTitle() {
        XCTAssertThrowsError(try Task.create(title: String(repeating: "a", count: 201))) { error in
            XCTAssertEqual(error as? TaskError, .titleTooLong(maxLength: 200))
        }
    }

    func testUpdatingStatusMarksTaskAsCompleted() throws {
        let createdAt = Date(timeIntervalSince1970: 1_725_000_000)
        let completedAt = Date(timeIntervalSince1970: 1_725_000_900)
        let task = try Task.create(title: "Ship Swift starter", now: createdAt)

        let completedTask = try task.updatingStatus(to: .done, now: completedAt)

        XCTAssertEqual(completedTask.status, .done)
        XCTAssertEqual(completedTask.completedAt, completedAt)
        XCTAssertEqual(completedTask.updatedAt, completedAt)
    }

    func testCompletedTasksCannotBeReopened() throws {
        let task = try Task.create(title: "Ship Swift starter")
        let completedTask = try task.updatingStatus(to: .done)

        XCTAssertThrowsError(try completedTask.updatingStatus(to: .todo)) { error in
            XCTAssertEqual(error as? TaskError, .completedTaskCannotBeReopened)
        }
    }

    func testUpdatingDetailsReturnsEditedCopyForActiveTask() throws {
        let createdAt = Date(timeIntervalSince1970: 1_725_000_000)
        let updatedAt = Date(timeIntervalSince1970: 1_725_000_500)
        let task = try Task.create(
            title: "Write lab",
            description: "Add Swift examples",
            now: createdAt
        )

        let updatedTask = try task.updatingDetails(
            title: "Write Swift lab",
            description: "Add SwiftUI examples",
            now: updatedAt
        )

        XCTAssertEqual(updatedTask.title, "Write Swift lab")
        XCTAssertEqual(updatedTask.description, "Add SwiftUI examples")
        XCTAssertEqual(updatedTask.updatedAt, updatedAt)
        XCTAssertEqual(updatedTask.createdAt, createdAt)
    }

    func testCompletedTasksCannotBeEdited() throws {
        let task = try Task.create(title: "Write lab")
        let completedTask = try task.updatingStatus(to: .done)

        XCTAssertThrowsError(
            try completedTask.updatingDetails(title: "Rewrite lab", description: "Add notes")
        ) { error in
            XCTAssertEqual(error as? TaskError, .inactiveTaskCannotBeEdited)
        }
    }
}
