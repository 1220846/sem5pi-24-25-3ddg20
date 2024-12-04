import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { Specialization } from '../domain/Specialization';
import { CreatingSpecializationDto } from '../domain/CreatingSpecializationDto';

@Injectable({
  providedIn: 'root',
})
export class SpecializationService {
  private apiUrl = 'https://localhost:5001/api/specializations';
  private header: HttpHeaders = new HttpHeaders({'Content-Type': 'application/json'});

  constructor(private http: HttpClient) {}

  getAll(): Observable<Specialization[]> {
    const token = localStorage.getItem('accessToken'); 
    const headers = this.header.set('Authorization', `Bearer ${token}`);
    return this.http.get<Specialization[]>(this.apiUrl, {headers});
  }

  getById(id: number): Observable<Specialization> {
    const token = localStorage.getItem('accessToken'); 
    const headers = this.header.set('Authorization', `Bearer ${token}`);
    return this.http.get<Specialization>(`${this.apiUrl}/${id}`, {headers});
  }

  add(specialization: CreatingSpecializationDto): Observable<Specialization> {
    const token = localStorage.getItem('accessToken'); 
    const headers = this.header.set('Authorization', `Bearer ${token}`);
    return this.http.post<Specialization>(this.apiUrl, specialization, {headers})
    .pipe(
      catchError((error) => {
        return throwError(() => error.error.message)
      })
    ) 
  }
}
