import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class Auth {

  constructor(@Inject(PLATFORM_ID) private platformId: Object) { }

  // שמירת הטוקן
  setSession(token: string): void {
    if (this.isBrowser()) {
      localStorage.setItem('token', token);
    }
  }

  // שליפת הטוקן
  getToken(): string | null {
    if (this.isBrowser()) {
      return localStorage.getItem('token');
    }
    return null;
  }

  // שליפת fullName מתוך הטוקן
  getFullName(): string | null {
    const decoded = this.getDecodedToken();
    return decoded ? decoded.fullName : null;
  }

  // שליפת userId מתוך הטוקן
  getUserId(): number | null {
    const decoded = this.getDecodedToken();
    return decoded ? +decoded.id : null;
  }

  // שליפת role מתוך הטוקן
  getUserRole(): string | null {
    const decoded = this.getDecodedToken();
    return decoded ? decoded.role : null;
  }

  // פענוח הטוקן
  private getDecodedToken(): any {
    const token = this.getToken();
    if (token) {
      return jwtDecode(token);
    }
    return null;
  }

  // בדיקה אם הקוד רץ בדפדפן
  private isBrowser(): boolean {
    return isPlatformBrowser(this.platformId);
  }

}
