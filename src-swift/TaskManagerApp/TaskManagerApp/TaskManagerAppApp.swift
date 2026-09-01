import SwiftUI

@main
struct TaskManagerAppApp: App {
    @StateObject private var viewModel: TaskListViewModel

    init() {
        _viewModel = StateObject(wrappedValue: TaskListViewModel.makeWorkshopModel())
    }

    var body: some Scene {
        WindowGroup {
            TaskListView(viewModel: viewModel)
        }
    }
}
