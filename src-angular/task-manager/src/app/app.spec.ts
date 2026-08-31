import { TestBed } from '@angular/core/testing';
import { App } from './app';
import { InMemoryTaskRepository } from './data/in-memory-task-repository';
import { TASK_REPOSITORY } from './domain/task-repository';

describe('App', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [App],
      providers: [{ provide: TASK_REPOSITORY, useClass: InMemoryTaskRepository }],
    })
      .compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(App);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it('should render the task list heading', async () => {
    const fixture = TestBed.createComponent(App);
    await fixture.whenStable();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('h1')?.textContent).toContain('Task Manager');
  });
});
