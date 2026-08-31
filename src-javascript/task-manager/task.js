'use strict';

/**
 * Task-related functions for the plain-JavaScript workshop track.
 *
 * There are no classes or layers here on purpose — everything about a
 * "Task" lives in this one file as plain functions and object literals,
 * to keep the mental model simple for a non-technical audience.
 */

const TASK_STATUSES = ['Todo', 'InProgress', 'Done', 'Cancelled'];

/**
 * Creates a new task.
 *
 * @param {string} title
 * @param {string} description
 * @returns {object} a new task object
 */
function createTask(title, description) {
  // TODO: Add validation (title not empty, description not null)
  // This is where Copilot will help you implement validation during the workshop

  const now = new Date();
  return {
    id: crypto.randomUUID(),
    title,
    description,
    status: 'Todo',
    createdAt: now,
    updatedAt: now,
  };
}

/**
 * Returns a copy of the task with a new status.
 *
 * @param {object} task
 * @param {string} newStatus
 * @returns {object} an updated task object
 */
function updateStatus(task, newStatus) {
  // TODO: Add business rules (e.g., can't move a task from Done back to Todo)
  // This will be implemented during the workshop

  return { ...task, status: newStatus, updatedAt: new Date() };
}

/**
 * Returns a copy of the task with a new title and description.
 *
 * @param {object} task
 * @param {string} title
 * @param {string} description
 * @returns {object} an updated task object
 */
function updateDetails(task, title, description) {
  // TODO: Add validation
  // This will be implemented during the workshop

  return { ...task, title, description, updatedAt: new Date() };
}

module.exports = { createTask, updateStatus, updateDetails, TASK_STATUSES };
