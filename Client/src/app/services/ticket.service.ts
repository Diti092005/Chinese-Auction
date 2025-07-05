import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Ticket } from '../models/ticket.model';
import { Auth } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class TicketService {
  private baseUrl = 'https://localhost:5001/api/Ticket';

  constructor(
    private http: HttpClient,
    private auth: Auth
  ) {}

  private getAuthHeaders(): HttpHeaders {
    const token = this.auth.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  getAll(): Observable<Ticket[]> {
    return this.http.get<Ticket[]>(this.baseUrl, { headers: this.getAuthHeaders() });
  }

  getById(id: number): Observable<Ticket> {
    return this.http.get<Ticket>(`${this.baseUrl}/${id}`, { headers: this.getAuthHeaders() });
  }

  getPaid(): Observable<Ticket[]> {
    return this.http.get<Ticket[]>(`${this.baseUrl}/paid`, { headers: this.getAuthHeaders() });
  }

  getPending(): Observable<Ticket[]> {
    return this.http.get<Ticket[]>(`${this.baseUrl}/pending`, { headers: this.getAuthHeaders() });
  }

  // פונקציה חדשה - קבלת כרטיסים לפי מזהה מתנה
  getByGiftId(giftId: number): Observable<Ticket[]> {
    return this.http.get<Ticket[]>(`${this.baseUrl}/byGift/${giftId}`, { headers: this.getAuthHeaders() });
  }

  add(ticket: Partial<Ticket>): Observable<Ticket> {
    return this.http.post<Ticket>(this.baseUrl, ticket, { headers: this.getAuthHeaders() });
  }
  pay(ids: number[]): Observable<void> {
    console.log(ids)
    return this.http.put<void>(`${this.baseUrl}/pay`, ids, { headers: this.getAuthHeaders() });
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`, { headers: this.getAuthHeaders() });
  }
}
