'use strict';

const { test } = require('node:test');
const assert = require('node:assert/strict');
const { createTask, updateStatus, updateDetails } = require('./task');

test('createTask creates a task with a Todo status', () => {
  const task = createTask('Write report', 'Draft the quarterly report');

  assert.equal(task.title, 'Write report');
  assert.equal(task.description, 'Draft the quarterly report');
  assert.equal(task.status, 'Todo');
  assert.ok(task.id, 'task should have an id');
});

test('updateStatus returns a copy of the task with the new status', () => {
  const task = createTask('Write report', 'Draft the quarterly report');

  const updated = updateStatus(task, 'InProgress');

  assert.equal(updated.status, 'InProgress');
  assert.equal(task.status, 'Todo', 'original task should be unchanged');
});

test('updateDetails returns a copy of the task with the new title and description', () => {
  const task = createTask('Write report', 'Draft the quarterly report');

  const updated = updateDetails(task, 'Write final report', 'Final version for the board');

  assert.equal(updated.title, 'Write final report');
  assert.equal(updated.description, 'Final version for the board');
  assert.equal(task.title, 'Write report', 'original task should be unchanged');
});
