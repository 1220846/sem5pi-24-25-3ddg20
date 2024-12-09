import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Allergy } from '../domain/Allergy';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AllergyService {

  private apiUrl = 'http://localhost:4000/api/allergies';

  private header: HttpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });
  
  constructor(private http : HttpClient) { }

  getAll(): Observable<Allergy[]>{
    const token = localStorage.getItem('accessToken');
    const headers = this.header.set('Authorization', `Bearer ${token}`);

    return this.http.get<Allergy[]>(this.apiUrl,{ headers: headers });
  }
}
