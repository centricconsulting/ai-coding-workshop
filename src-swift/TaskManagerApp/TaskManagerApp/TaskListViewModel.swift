import Combine
import Foundation
import TaskManagerApplication
import TaskManagerDomain
import TaskManagerInfrastructure

final class TaskListViewModel: ObservableObject {
    @Published private(set) var tasks: [Task] = []
    @Published var draftTitle = ""
    @Published var draftDescription = ""
    @Published var errorMessage: String?

    private let createTaskUseCase: CreateTaskUseCase
    private let listTasksUseCase: ListTasksUseCase
    private let completeTaskUseCase: CompleteTaskUseCase

    init(repository: InMemoryTaskRepository) {
        createTaskUseCase = CreateTaskUseCase(repository: repository)
        listTasksUseCase = ListTasksUseCase(repository: repository)
        completeTaskUseCase = CompleteTaskUseCase(repository: repository)
        reloadTasks()
    }

    static func makeWorkshopModel() -> TaskListViewModel {
        TaskListViewModel(repository: InMemoryTaskRepository(seedTasks: sampleTasks()))
    }

    func addTask() {
        do {
            _ = try createTaskUseCase.execute(
                CreateTaskCommand(
                    title: draftTitle,
                    description: draftDescription
                )
            )
            draftTitle = ""
            draftDescription = ""
            errorMessage = nil
            reloadTasks()
        } catch {
            errorMessage = error.localizedDescription
        }
    }

    func completeTask(_ task: Task) {
        guard task.status.isActive else {
            return
        }

        do {
            _ = try completeTaskUseCase.execute(id: task.id)
            errorMessage = nil
            reloadTasks()
        } catch {
            errorMessage = error.localizedDescription
        }
    }

    private func reloadTasks() {
        tasks = listTasksUseCase.execute()
    }

    private static func sampleTasks() -> [Task] {
        let firstDate = Date(timeIntervalSince1970: 1_725_000_000)
        let secondDate = Date(timeIntervalSince1970: 1_725_000_600)
        let thirdDate = Date(timeIntervalSince1970: 1_725_001_200)

        return [
            try? Task.reconstitute(
                id: TaskID(UUID(uuidString: "AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA")!),
                title: "Review the Swift package",
                description: "Open src-swift in VS Code and scan the Domain layer.",
                status: .todo,
                createdAt: firstDate,
                updatedAt: firstDate
            ),
            try? Task.reconstitute(
                id: TaskID(UUID(uuidString: "BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB")!),
                title: "Wire the SwiftUI shell",
                description: "Connect the form and complete button to application use cases.",
                status: .inProgress,
                createdAt: secondDate,
                updatedAt: secondDate
            ),
            try? Task.reconstitute(
                id: TaskID(UUID(uuidString: "CCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC")!),
                title: "Ship the outline",
                description: "Keep the UI minimal and architecture-friendly.",
                status: .done,
                createdAt: thirdDate,
                updatedAt: thirdDate,
                completedAt: thirdDate
            ),
        ]
        .compactMap { $0 }
    }
}
