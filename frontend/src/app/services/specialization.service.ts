import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Specialization } from '../domain/specialization';

@Injectable({
  providedIn: 'root',
})
export class SpecializationService {
  private apiUrl = 'https://localhost:5001/api/specializations';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Specialization[]> {
    return this.http.get<Specialization[]>(this.apiUrl);
  }

  getById(id: number): Observable<Specialization> {
    return this.http.get<Specialization>(`${this.apiUrl}/${id}`);
  }
}
