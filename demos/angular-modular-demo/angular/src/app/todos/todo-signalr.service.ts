import { Injectable, inject, NgZone } from '@angular/core';
import { Subject } from 'rxjs';
import { EnvironmentService } from '@abp/ng.core';
import { OAuthService } from 'angular-oauth2-oidc';
import * as signalR from '@microsoft/signalr';
import { TodoDto } from './todo.service';

@Injectable({ providedIn: 'root' })
export class TodoSignalRService {
  private hubConnection?: signalR.HubConnection;
  private zone = inject(NgZone);
  private environment = inject(EnvironmentService);
  private oAuthService = inject(OAuthService);

  todoCreated$ = new Subject<TodoDto>();
  todoCompleted$ = new Subject<TodoDto>();

  init(): void {
    const apiUrl = this.environment.getEnvironment().apis?.['default']?.url || '';
    const accessToken = this.oAuthService.getAccessToken();

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${apiUrl}/signalr-hubs/todo`, {
        accessTokenFactory: () => accessToken,
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.on('TodoCreated', (todo: TodoDto) => {
      this.zone.run(() => this.todoCreated$.next(todo));
    });

    this.hubConnection.on('TodoCompleted', (todo: TodoDto) => {
      this.zone.run(() => this.todoCompleted$.next(todo));
    });

    this.hubConnection
      .start()
      .catch((err: any) => console.error('SignalR connection error:', err));
  }

  stop(): void {
    if (this.hubConnection) {
      this.hubConnection.stop();
    }
  }
}
