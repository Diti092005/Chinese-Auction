import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { Donor } from '../models/donor.model';
import { Auth } from './auth.service';
import { DonorDto } from '../models/donor.dto';

@Injectable({
  providedIn: 'root'
})
export class DonorService {
  private baseUrl = 'https://localhost:5001/api/Donor';

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

  getAll(): Observable<Donor[]> {
    return this.http.get<Donor[]>(this.baseUrl, { headers: this.getAuthHeaders() });
  }

  getById(id: number): Observable<Donor> {
    return this.http.get<Donor>(`${this.baseUrl}/${id}`, { headers: this.getAuthHeaders() });
  }

  add(donor: Partial<Donor>): Observable<Donor> {
    return this.http.post<Donor>(this.baseUrl, donor, { headers: this.getAuthHeaders() });
  }

  update(id: number, donor: DonorDto): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, donor, { headers: this.getAuthHeaders() });
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`, { headers: this.getAuthHeaders() });
  }

  search(name?: string, email?: string, giftName?: string): Observable<Donor[]> {
    let params = new HttpParams();
    if (name) params = params.set('name', name);
    if (email) params = params.set('email', email);
    if (giftName) params = params.set('giftName', giftName);
    return this.http.get<Donor[]>(`${this.baseUrl}/search`, {
      headers: this.getAuthHeaders(),
      params: params
    }).pipe(
      map(donors => donors.map(donor => ({
        ...donor,
        giftsCount: donor.gifts ? donor.gifts.length : 0
      })))
    );
  }

  // דוגמה לפונקציה נוספת (אם קיימת בשרת)
  // getCountOfGifts(id: number): Observable<number> {
  //   return this.http.get<number>(`${this.baseUrl}/${id}/countOfGifts`, { headers: this.getAuthHeaders() });
  // }
}
