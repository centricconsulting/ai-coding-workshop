import { Component, OnInit, inject, signal } from '@angular/core';
import { Task } from '../../domain/task';
import { TaskApplicationService } from '../../application/task-application.service';

/**
 * Minimal presentation-layer component for the Lab 1-4 exercises.
 * Delegates all business logic to TaskApplicationService — this component
 * should stay a thin view, not a place for domain/application rules.
 */
@Component({
  selector: 'app-task-list',
  standalone: true,
  templateUrl: './task-list.html',
  styleUrl: './task-list.css',
})
export class TaskListComponent implements OnInit {
  private readonly taskApplicationService = inject(TaskApplicationService);

  protected readonly tasks = signal<Task[]>([]);

  async ngOnInit(): Promise<void> {
    try {
      // TODO: Participants will wire this up once InMemoryTaskRepository is implemented
      this.tasks.set(await this.taskApplicationService.listActiveTasks());
    } catch {
      // Expected until InMemoryTaskRepository.getActiveTasks() is implemented (Lab 2).
      this.tasks.set([]);
    }
  }
}
