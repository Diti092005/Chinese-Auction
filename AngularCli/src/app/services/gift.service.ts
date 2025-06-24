import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Gift } from '../models/gift.model';
import { Auth } from './auth';
import { log } from 'console';

@Injectable({
  providedIn: 'root'
})
export class GiftService {

  baseUrl: string = 'https://localhost:5001/api/Gifts';

  constructor(private http: HttpClient, private auth: Auth) { }

  getAllGifts(): Observable<Gift[]> {
  const token = this.auth.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.get<Gift[]>(this.baseUrl,{headers});
  }

  getGiftById(id: number): Observable<Gift> {
    const token = this.auth.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.get<Gift>(`${this.baseUrl}/${id}`, { headers });
  }

  addGift(gift: Partial<Gift>): Observable<Gift> {
    const newGift = {
      ...gift,
      categoryId:
        typeof (gift.categoryId as any) === 'object' && gift.categoryId !== null
          ? (gift.categoryId as any).value
          : gift.categoryId ?? 0,
      donorId:
        typeof (gift.donorId as any) === 'object' && gift.donorId !== null
          ? (gift.donorId as any).value
          : gift.donorId ?? 0,
    };

    const token = this.auth.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    
    return this.http.post<Gift>(this.baseUrl, newGift, { headers });
  }

  updateGift(id: number, gift: Partial<Gift>): Observable<any> {
    const token = this.auth.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.put(`${this.baseUrl}/${id}`, gift, { headers });
  }

  deleteGift(id: number): Observable<any> {
    const token = this.auth.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.delete(`${this.baseUrl}/${id}`, { headers });
  }
}
