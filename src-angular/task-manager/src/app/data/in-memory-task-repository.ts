import { Injectable } from '@angular/core';
import { Task } from '../domain/task';
import { TaskId } from '../domain/task-id';
import { TaskRepository } from '../domain/task-repository';
import { TaskStatus } from '../domain/task-status';

/**
 * In-memory implementation of TaskRepository, used for the workshop labs.
 * Mirrors the "in-memory repository" pattern used in the .NET/Spring Boot/etc. tracks'
 * Infrastructure layer.
 */
@Injectable({ providedIn: 'root' })
export class InMemoryTaskRepository implements TaskRepository {
  private readonly tasks = new Map<string, Task>();

  // TODO: Participants will implement these methods during the workshop

  async findById(taskId: TaskId): Promise<Task | undefined> {
    throw new Error('Not implemented: findById');
  }

  async getActiveTasks(): Promise<Task[]> {
    throw new Error('Not implemented: getActiveTasks');
  }

  async addTask(task: Task): Promise<void> {
    throw new Error('Not implemented: addTask');
  }

  async saveChanges(task: Task): Promise<void> {
    throw new Error('Not implemented: saveChanges');
  }
}
