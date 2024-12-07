import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SurgeryRoom } from '../domain/SurgeryRoom';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SurgeryRoomService {

  private apiUrl = 'https://localhost:5001/api/surgeryrooms';
  private header: HttpHeaders = new HttpHeaders({'Content-Type': 'application/json'});

  constructor(private http: HttpClient) {}

  getAll(): Observable<SurgeryRoom[]> {
    const token = localStorage.getItem('accessToken'); 
    const headers = this.header.set('Authorization', `Bearer ${token}`);
    return this.http.get<SurgeryRoom[]>(this.apiUrl, {headers});
  }
}
