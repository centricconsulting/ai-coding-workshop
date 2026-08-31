import { InjectionToken } from '@angular/core';
import { Task } from './task';
import { TaskId } from './task-id';

/**
 * Repository interface for the Task aggregate.
 * Uses business-intent method names (not generic CRUD).
 */
export interface TaskRepository {
  // TODO: Participants will implement these methods during the workshop
  // Note: Using business-intent names, not generic CRUD operations

  findById(taskId: TaskId): Promise<Task | undefined>;

  getActiveTasks(): Promise<Task[]>;

  addTask(task: Task): Promise<void>;

  saveChanges(task: Task): Promise<void>;
}

/**
 * Injection token for TaskRepository, since interfaces have no runtime
 * representation in TypeScript and can't be used directly as DI tokens.
 */
export const TASK_REPOSITORY = new InjectionToken<TaskRepository>('TaskRepository');
