import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection!: signalR.HubConnection;

  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(environment.SIGNALR_URL)
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err))
  }
  
  public addTransferDataListener(eventName: string, callback: (data: any) => void): void {
    this.hubConnection.on(eventName, (data : any) => {
      callback(data);
    });
  }
}
