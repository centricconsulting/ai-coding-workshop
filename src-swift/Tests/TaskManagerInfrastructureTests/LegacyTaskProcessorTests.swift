import XCTest
@testable import TaskManagerInfrastructure

final class LegacyTaskProcessorTests: XCTestCase {
    func testProcessUpdateUsesMissingMarkersForAbsentFields() {
        let processor = LegacyTaskProcessor()

        let result = processor.processUpdate(
            LegacyTaskUpdateRequest(
                taskID: nil,
                title: nil,
                description: nil,
                estimateMinutes: nil,
                assignee: nil,
                status: nil,
                notifyChannel: nil,
                notifyRecipient: nil
            )
        )

        XCTAssertEqual(
            result,
            "Task unknown | title=(missing) | description=(missing) | estimate=unknown | assignee=unassigned | state=open | notify=skipped"
        )
    }

    func testProcessUpdateFormatsPresentFields() {
        let processor = LegacyTaskProcessor()

        let result = processor.processUpdate(
            LegacyTaskUpdateRequest(
                taskID: "swift-123",
                title: "  Build iOS shell  ",
                description: "  Wire the package into SwiftUI  ",
                estimateMinutes: 45,
                assignee: "  Taylor  ",
                status: "DONE",
                notifyChannel: "email",
                notifyRecipient: "  ios@example.com  "
            )
        )

        XCTAssertEqual(
            result,
            "Task swift-123 | title=Build iOS shell | description=Wire the package into SwiftUI | estimate=45m | assignee=Taylor | state=complete | notify=email:ios@example.com"
        )
    }
}
