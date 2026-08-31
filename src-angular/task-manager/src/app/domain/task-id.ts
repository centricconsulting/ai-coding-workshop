/**
 * Strongly-typed identifier for Task entities.
 */
export class TaskId {
  private constructor(readonly value: string) {}

  static new(): TaskId {
    return new TaskId(crypto.randomUUID());
  }

  static from(value: string): TaskId {
    return new TaskId(value);
  }

  equals(other: TaskId): boolean {
    return this.value === other.value;
  }

  toString(): string {
    return this.value;
  }
}
