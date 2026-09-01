import SwiftUI
import TaskManagerDomain

struct TaskListView: View {
    @ObservedObject var viewModel: TaskListViewModel

    var body: some View {
        NavigationStack {
            List {
                if let errorMessage = viewModel.errorMessage {
                    Section {
                        Text(errorMessage)
                            .foregroundStyle(.red)
                    }
                }

                AddTaskSection(viewModel: viewModel)

                Section("Tasks") {
                    if viewModel.tasks.isEmpty {
                        Text("No tasks yet. Add one above.")
                            .foregroundStyle(.secondary)
                    } else {
                        ForEach(viewModel.tasks) { task in
                            HStack(alignment: .top, spacing: 12) {
                                VStack(alignment: .leading, spacing: 6) {
                                    Text(task.title)
                                        .font(.headline)

                                    if let description = task.description {
                                        Text(description)
                                            .font(.subheadline)
                                            .foregroundStyle(.secondary)
                                    }

                                    Text(task.status.displayName)
                                        .font(.caption)
                                        .foregroundStyle(color(for: task.status))
                                }

                                Spacer()

                                if task.status.isActive {
                                    Button("Complete") {
                                        viewModel.completeTask(task)
                                    }
                                    .buttonStyle(.bordered)
                                } else {
                                    Image(systemName: task.status == .done ? "checkmark.circle.fill" : "xmark.circle.fill")
                                        .foregroundStyle(color(for: task.status))
                                }
                            }
                            .padding(.vertical, 4)
                        }
                    }
                }
            }
            .navigationTitle("Task Manager")
        }
    }

    private func color(for status: TaskStatus) -> Color {
        switch status {
        case .todo:
            return .blue
        case .inProgress:
            return .orange
        case .done:
            return .green
        case .cancelled:
            return .red
        }
    }
}
