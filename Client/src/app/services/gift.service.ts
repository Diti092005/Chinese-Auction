import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Gift } from '../models/gift.model';
import { Auth } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class GiftService {

  baseUrl: string = 'https://localhost:5001/api/Gifts';

  constructor(private http: HttpClient, private auth: Auth) { }

  getAllGifts(): Observable<Gift[]> {
    return this.http.get<Gift[]>(this.baseUrl);
  }

  getGiftById(id: number): Observable<Gift> {
    return this.http.get<Gift>(`${this.baseUrl}/${id}`);
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

  updateGift(id: number, gift: Partial<Gift>): Observable<void> {
    const token = this.auth.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    // שלח רק את השדות הנדרשים לפי GiftDTO
    const dto: any = {
      giftName: gift.giftName,
      categoryId: gift.categoryId,
      donorId: gift.donorId,
      price: gift.price,
      details: gift.details,
      imageUrl: gift.imageUrl
    };
    return this.http.put<void>(`${this.baseUrl}/${id}`, dto, { headers });
  }

  deleteGift(id: number): Observable<any> {
    const token = this.auth.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.delete(`${this.baseUrl}/${id}`, { headers });
  }

  uploadImage(formData: FormData): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/upload-image`, formData);
  }
  raffleGift(giftId: number): Observable<string> {
    const token = this.auth.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.put(`${this.baseUrl}/raffle/${giftId}`, null, {
      headers,
      responseType: 'text'
    });
  }
  
}
