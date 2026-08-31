import { describe, expect, it } from 'vitest';
import { Task } from './task';
import { TaskStatus } from './task-status';

describe('Task', () => {
  describe('create', () => {
    it('should create a task with title and description', () => {
      const title = 'Complete project documentation';
      const description = 'Write comprehensive API docs';

      const task = Task.create(title, description);

      expect(task.id).toBeDefined();
      expect(task.title).toBe(title);
      expect(task.description).toBe(description);
      expect(task.status).toBe(TaskStatus.Todo);
      expect(task.createdAt).toBeInstanceOf(Date);
    });

    it('should generate unique ids for different tasks', () => {
      const task1 = Task.create('Task 1', 'Description 1');
      const task2 = Task.create('Task 2', 'Description 2');

      expect(task1.id.equals(task2.id)).toBe(false);
    });

    // TODO (Lab 1 exercise): once validation is added to Task.create, add tests
    // covering empty/blank titles and any other business rules participants implement.
  });

  describe('updateStatus', () => {
    it('should update the status and updatedAt timestamp', () => {
      const task = Task.create('Task', 'Description');
      const originalUpdatedAt = task.updatedAt;

      task.updateStatus(TaskStatus.InProgress);

      expect(task.status).toBe(TaskStatus.InProgress);
      expect(task.updatedAt.getTime()).toBeGreaterThanOrEqual(originalUpdatedAt.getTime());
    });

    // TODO (Lab 1 exercise): once transition rules are added to updateStatus,
    // add tests covering invalid transitions (e.g. Done -> Todo).
  });

  describe('updateDetails', () => {
    it('should update the title and description', () => {
      const task = Task.create('Original title', 'Original description');

      task.updateDetails('Updated title', 'Updated description');

      expect(task.title).toBe('Updated title');
      expect(task.description).toBe('Updated description');
    });

    // TODO (Lab 1 exercise): once validation is added to updateDetails, add tests
    // covering invalid input.
  });
});
