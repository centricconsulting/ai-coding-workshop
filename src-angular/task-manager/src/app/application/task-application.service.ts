import { Inject, Injectable } from '@angular/core';
import { Task } from '../domain/task';
import { TASK_REPOSITORY, TaskRepository } from '../domain/task-repository';
import { TaskStatus } from '../domain/task-status';

/**
 * Application service orchestrating Task use cases.
 * Keeps business orchestration out of components (presentation layer).
 */
@Injectable({ providedIn: 'root' })
export class TaskApplicationService {
  constructor(@Inject(TASK_REPOSITORY) private readonly taskRepository: TaskRepository) {}

  async createTask(title: string, description: string): Promise<Task> {
    const task = Task.create(title, description);
    await this.taskRepository.addTask(task);
    return task;
  }

  async completeTask(task: Task): Promise<void> {
    task.updateStatus(TaskStatus.Done);
    await this.taskRepository.saveChanges(task);
  }

  async listActiveTasks(): Promise<Task[]> {
    return this.taskRepository.getActiveTasks();
  }
}
