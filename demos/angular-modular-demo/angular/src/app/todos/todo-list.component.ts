import { Component, OnInit, OnDestroy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PermissionService, LocalizationPipe } from '@abp/ng.core';
import { Subscription } from 'rxjs';
import { TodoService, TodoDto, CreateTodoDto } from './todo.service';
import { TodoSignalRService } from './todo-signalr.service';

@Component({
  selector: 'app-todo-list',
  standalone: true,
  imports: [CommonModule, FormsModule, LocalizationPipe],
  template: `
    <div class="container">
      <div class="card">
        <div class="card-header">
          <div class="row align-items-center">
            <div class="col">
              <h5 class="card-title mb-0">{{ '::Todos' | abpLocalization }}</h5>
            </div>
            <div class="col-auto" *ngIf="canCreate">
              <button class="btn btn-primary btn-sm" (click)="showCreateForm = !showCreateForm">
                <i class="fa fa-plus me-1"></i>{{ '::NewTodo' | abpLocalization }}
              </button>
            </div>
          </div>
        </div>

        <div class="card-body" *ngIf="showCreateForm">
          <div class="card bg-light mb-3">
            <div class="card-body">
              <div class="mb-3">
                <label class="form-label">{{ '::TodoTitle' | abpLocalization }}</label>
                <input type="text" class="form-control" [(ngModel)]="newTodo.title" maxlength="256" />
              </div>
              <div class="mb-3">
                <label class="form-label">{{ '::TodoDescription' | abpLocalization }}</label>
                <textarea class="form-control" [(ngModel)]="newTodo.description" maxlength="1024" rows="2"></textarea>
              </div>
              <div class="d-flex gap-2">
                <button class="btn btn-primary btn-sm" (click)="createTodo()" [disabled]="!newTodo.title">
                  <i class="fa fa-save me-1"></i>Save
                </button>
                <button class="btn btn-secondary btn-sm" (click)="showCreateForm = false">Cancel</button>
              </div>
            </div>
          </div>
        </div>

        <div class="card-body p-0">
          <div class="table-responsive">
            <table class="table table-hover mb-0">
              <thead class="table-light">
                <tr>
                  <th>{{ '::TodoStatus' | abpLocalization }}</th>
                  <th>{{ '::TodoTitle' | abpLocalization }}</th>
                  <th>{{ '::TodoDescription' | abpLocalization }}</th>
                  <th>{{ '::TodoCreatedBy' | abpLocalization }}</th>
                  <th>{{ '::TodoCreationTime' | abpLocalization }}</th>
                  <th>{{ '::TodoCompletedBy' | abpLocalization }}</th>
                  <th>{{ '::TodoCompletionTime' | abpLocalization }}</th>
                  <th *ngIf="canComplete">Actions</th>
                </tr>
              </thead>
              <tbody>
                <tr *ngFor="let todo of todos" [class.table-success]="todo.isCompleted">
                  <td>
                    <span class="badge" [class.bg-warning]="!todo.isCompleted" [class.bg-success]="todo.isCompleted">
                      {{ todo.isCompleted ? ('::TodoCompleted' | abpLocalization) : ('::TodoPending' | abpLocalization) }}
                    </span>
                  </td>
                  <td [class.text-decoration-line-through]="todo.isCompleted">{{ todo.title }}</td>
                  <td>{{ todo.description }}</td>
                  <td>{{ todo.creatorUserName }}</td>
                  <td>{{ todo.creationTime | date:'short' }}</td>
                  <td>{{ todo.completedByUserName }}</td>
                  <td>{{ todo.completionTime | date:'short' }}</td>
                  <td *ngIf="canComplete">
                    <button
                      *ngIf="!todo.isCompleted"
                      class="btn btn-success btn-sm"
                      (click)="completeTodo(todo.id)">
                      <i class="fa fa-check me-1"></i>{{ '::MarkComplete' | abpLocalization }}
                    </button>
                  </td>
                </tr>
                <tr *ngIf="todos.length === 0">
                  <td [attr.colspan]="canComplete ? 8 : 7" class="text-center text-muted py-4">
                    No todos found.
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  `,
})
export class TodoListComponent implements OnInit, OnDestroy {
  private todoService = inject(TodoService);
  private signalRService = inject(TodoSignalRService);
  private permissionService = inject(PermissionService);

  todos: TodoDto[] = [];
  showCreateForm = false;
  newTodo: CreateTodoDto = { title: '', description: '' };

  private subscriptions: Subscription[] = [];

  get canCreate(): boolean {
    return this.permissionService.getGrantedPolicy('AngularDemo.Todos.Create');
  }

  get canComplete(): boolean {
    return this.permissionService.getGrantedPolicy('AngularDemo.Todos.Complete');
  }

  ngOnInit(): void {
    this.loadTodos();
    this.signalRService.init();

    this.subscriptions.push(
      this.signalRService.todoCreated$.subscribe(todo => {
        const exists = this.todos.find(t => t.id === todo.id);
        if (!exists) {
          this.todos.unshift(todo);
        }
      }),
      this.signalRService.todoCompleted$.subscribe(updated => {
        const index = this.todos.findIndex(t => t.id === updated.id);
        if (index >= 0) {
          this.todos[index] = updated;
        }
      })
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(s => s.unsubscribe());
    this.signalRService.stop();
  }

  loadTodos(): void {
    this.todoService.getList().subscribe(result => {
      this.todos = result.items;
    });
  }

  createTodo(): void {
    this.todoService.create(this.newTodo).subscribe(() => {
      this.newTodo = { title: '', description: '' };
      this.showCreateForm = false;
    });
  }

  completeTodo(id: string): void {
    this.todoService.complete(id).subscribe();
  }
}
