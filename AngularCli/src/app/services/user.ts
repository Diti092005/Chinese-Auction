import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { Auth } from './auth';

@Injectable({
  providedIn: 'root'
})
export class User {
  baseUrl: string = "https://localhost:5001/api/User";

  constructor(private http: HttpClient, private auth: Auth) { }

  login(username: string, password: string): Observable<any> {
    const body = { username, password };
    return this.http.post<{ token: string }>(`${this.baseUrl}/login`, body).pipe(
      tap(response => {
        if (response && response.token) {
          this.auth.setSession(response.token);
        }
      })
    );
  }
  register(username: string, password: string, fullName: string, email: string, phone: string)
    : Observable<any> {
    const body = { username, password, phone, email, fullName }
    return this.http.post<{msg:string}>(this.baseUrl+"/register",body)
  }
}

