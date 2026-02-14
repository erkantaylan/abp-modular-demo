import { Injectable, inject } from '@angular/core';
import { RestService } from '@abp/ng.core';
import { Observable } from 'rxjs';

export interface TodoDto {
  id: string;
  title: string;
  description?: string;
  isCompleted: boolean;
  completedBy?: string;
  completionTime?: string;
  creationTime: string;
  creatorId?: string;
  creatorUserName?: string;
  completedByUserName?: string;
}

export interface CreateTodoDto {
  title: string;
  description?: string;
}

export interface ListResultDto<T> {
  items: T[];
}

@Injectable({ providedIn: 'root' })
export class TodoService {
  private rest = inject(RestService);

  getList(): Observable<ListResultDto<TodoDto>> {
    return this.rest.request<void, ListResultDto<TodoDto>>({
      method: 'GET',
      url: '/api/app/todo',
    });
  }

  create(input: CreateTodoDto): Observable<TodoDto> {
    return this.rest.request<CreateTodoDto, TodoDto>({
      method: 'POST',
      url: '/api/app/todo',
      body: input,
    });
  }

  complete(id: string): Observable<TodoDto> {
    return this.rest.request<void, TodoDto>({
      method: 'POST',
      url: `/api/app/todo/${id}/complete`,
    });
  }
}
