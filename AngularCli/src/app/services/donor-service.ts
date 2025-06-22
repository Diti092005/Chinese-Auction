import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Donor } from '../models/donor.model';
import { Auth } from './auth';

@Injectable({
  providedIn: 'root'
})
export class DonorService {

  private baseUrl = 'https://localhost:5001/api/Donor';

  constructor(private http: HttpClient, private auth: Auth) {}

  private getAuthHeaders(): HttpHeaders {
    const token = this.auth.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  getAllDonors(): Observable<Donor[]> {
    return this.http.get<Donor[]>(this.baseUrl, { headers: this.getAuthHeaders() });
  }

  getDonorById(id: number): Observable<Donor> {
    return this.http.get<Donor>(`${this.baseUrl}/${id}`, { headers: this.getAuthHeaders() });
  }

  addDonor(donor: { name: string, email: string }): Observable<Donor> {
    return this.http.post<Donor>(this.baseUrl, donor, { headers: this.getAuthHeaders() });
  }

  updateDonor(donor: Donor): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${donor.id}`, donor, { headers: this.getAuthHeaders() });
  }

  deleteDonor(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`, { headers: this.getAuthHeaders() });
  }

  searchDonors(name?: string, email?: string, giftName?: string): Observable<Donor[]> {
    let params = new HttpParams();
    if (name) params = params.set('name', name);
    if (email) params = params.set('email', email);
    if (giftName) params = params.set('giftName', giftName);

    return this.http.get<Donor[]>(`${this.baseUrl}/search`, {
      headers: this.getAuthHeaders(),
      params: params
    });
  }
}
