import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, forkJoin } from 'rxjs';
import { Auth } from './auth';

@Injectable({
  providedIn: 'root'
})
export class TicketService {
  private baseUrl = 'https://localhost:5001/api/ticket';

  constructor(private http: HttpClient, private auth: Auth) {}

  private getAuthHeaders(): HttpHeaders {
    const token = this.auth.getToken();
    if (!token) {
      throw new Error('No token found');
    }
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  getAllTickets(): Observable<any> {
    return this.http.get<any>(this.baseUrl, { headers: this.getAuthHeaders() });
  }

  getPaidTicketsByUser(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/paid`, { headers: this.getAuthHeaders() });
  }

  getPendingTicketsByUser(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/pending`, { headers: this.getAuthHeaders() });
  }

  getTicketById(id: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/${id}`, { headers: this.getAuthHeaders() });
  }

  addTicket(ticketDto: any): Observable<any> {
    return this.http.post<any>(this.baseUrl, ticketDto, { headers: this.getAuthHeaders() });
  }

  payTicket(id: number): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/pay/${id}`, null, { headers: this.getAuthHeaders() });
  }

  deleteTicket(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`, { headers: this.getAuthHeaders() });
  }

  /** תשלום מרוכז עבור רשימת טיקט ID-ים */
  payTickets(ticketIds: number[]): Observable<any> {
    const requests = ticketIds.map(id => this.payTicket(id));
    return forkJoin(requests);
  }
}
