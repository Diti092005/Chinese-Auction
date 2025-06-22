import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {jwtDecode} from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class Auth {

  constructor() { }

  // שמירת הטוקן והשם
  setSession(token: string) {
    localStorage.setItem('token', token);
  }

  // שליפת הטוקן
  getToken(): string | null {
    return localStorage.getItem('token');
  }

  // שליפת fullName
  getFullName(): string | null {
     const decoded = this.getDecodedToken();
    return decoded ? decoded.fullName : null;
  }

  // פענוח הטוקן
  getDecodedToken(): any {
    const token = this.getToken();
    if (token) {
      return jwtDecode(token);
    }
    return null;
  }

  // שליפת userId מתוך ה-token
  getUserId(): number | null {
    const decoded = this.getDecodedToken();
    return decoded ? +decoded.id : null;
  }

  // שליפת role מתוך ה-token
  getUserRole(): string | null {
    const decoded = this.getDecodedToken();
    return decoded ? decoded.role : null;
  }

}
