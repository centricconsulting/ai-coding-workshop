import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { InMemoryTaskRepository } from './data/in-memory-task-repository';
import { TASK_REPOSITORY } from './domain/task-repository';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    { provide: TASK_REPOSITORY, useClass: InMemoryTaskRepository },
  ]
};
