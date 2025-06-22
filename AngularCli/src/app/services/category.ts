import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Auth } from './auth';
import { Observable } from 'rxjs';
import { Category } from '../models/category.model';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  baseUrl: string = "https://localhost:5001/api/Category";

  constructor(private http: HttpClient, private auth: Auth) { }
  getAllCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(this.baseUrl)
  }
  getCategoryById(id: number): Observable<Category> {
    return this.http.get<Category>(`${this.baseUrl}/${id}`)
  }
  addCategory(name: string): Observable<Category> {
    const newCategory = { name };
    const token=this.auth.getToken()
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.post<Category>(this.baseUrl, newCategory, { headers });
  }
  updateCategory(name: string,id:number): Observable<any> {
    const updateCategory = { name };
    const token=this.auth.getToken()
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.put(`${this.baseUrl}/${id}`, updateCategory, { headers });
  }
  deleteCategory(id:number):Observable<any>
  {
    const token=this.auth.getToken()
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.delete(`${this.baseUrl}/${id}`, { headers });
  }
}
