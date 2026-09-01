import SwiftUI

struct AddTaskSection: View {
    @ObservedObject var viewModel: TaskListViewModel

    var body: some View {
        Section("Add Task") {
            TextField("Title", text: $viewModel.draftTitle)
            TextField(
                "Description (optional)",
                text: $viewModel.draftDescription,
                axis: .vertical
            )
            .lineLimit(2...4)

            Button("Add Task") {
                viewModel.addTask()
            }
            .buttonStyle(.borderedProminent)
            .disabled(viewModel.draftTitle.trimmingCharacters(in: .whitespacesAndNewlines).isEmpty)
        }
    }
}
