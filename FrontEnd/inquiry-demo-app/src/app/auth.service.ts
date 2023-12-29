import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private tokenKey : string = 'token';
  constructor(private http: HttpClient) { }


  login(username: string, password: string): Observable<any> {
    const body = { username, password };
    return this.http.post(environment.AUTH_URL, body).pipe(
      tap(
        (response : any) =>{
          this.setToken(response['token'].toString());
        }
        ) // Assuming the response has a token field
    );
  }

  private setToken(token: string): void {
    localStorage.setItem(this.tokenKey, token); // Storing the token in local storage (consider security implications)
  }

  getToken(): string | null {
    let token = localStorage.getItem(this.tokenKey); 
    return token?.toString() ?? null;
  }
}
