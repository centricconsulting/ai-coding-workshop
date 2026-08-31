import { TaskId } from './task-id';
import { TaskStatus } from './task-status';

/**
 * Task aggregate root representing a work item to be completed.
 */
export class Task {
  readonly id: TaskId;
  private _title: string;
  private _description: string;
  private _status: TaskStatus;
  readonly createdAt: Date;
  private _updatedAt: Date;

  private constructor(
    id: TaskId,
    title: string,
    description: string,
    status: TaskStatus,
    createdAt: Date,
  ) {
    this.id = id;
    this._title = title;
    this._description = description;
    this._status = status;
    this.createdAt = createdAt;
    this._updatedAt = createdAt;
  }

  get title(): string {
    return this._title;
  }

  get description(): string {
    return this._description;
  }

  get status(): TaskStatus {
    return this._status;
  }

  get updatedAt(): Date {
    return this._updatedAt;
  }

  /**
   * Factory method to create a new task.
   */
  static create(title: string, description: string): Task {
    // TODO: Add validation (title not null/empty, description not null)
    // This is where Copilot will help participants implement validation

    return new Task(TaskId.new(), title, description, TaskStatus.Todo, new Date());
  }

  /**
   * Business method to update task status.
   */
  updateStatus(newStatus: TaskStatus): void {
    // TODO: Add business rules (e.g., can't move from Done to Todo directly)
    // This will be implemented during the workshop

    this._status = newStatus;
    this._updatedAt = new Date();
  }

  /**
   * Business method to update task details.
   */
  updateDetails(title: string, description: string): void {
    // TODO: Add validation
    // This will be implemented during the workshop

    this._title = title;
    this._description = description;
    this._updatedAt = new Date();
  }
}
