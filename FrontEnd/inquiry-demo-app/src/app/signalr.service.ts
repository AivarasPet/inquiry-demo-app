import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection!: signalR.HubConnection;
  
  constructor(private authService: AuthService) {}
    
  public startConnection = () => {
    let token = this.authService.getToken()?.toString() ?? '';

    this.hubConnection = new signalR.HubConnectionBuilder()
    .withUrl(`${environment.SIGNALR_URL}`, {
        transport: signalR.HttpTransportType.WebSockets,
        accessTokenFactory: () => token
    })
    .configureLogging(signalR.LogLevel.Information)
    .build();

    this.hubConnection
      .start()
      .then(() => alert('dsdsd'))
      .catch(err => console.log('Error while starting connection: ' + err))
  }
  
  public addTransferDataListener(eventName: string, callback: (data: any) => void): void {
    this.hubConnection.on(eventName, (data : any) => {
      callback(data);
    });
  }
}
