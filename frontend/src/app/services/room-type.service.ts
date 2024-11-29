import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { RoomType } from '../domain/RoomType';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CreatingRoomTypeDto } from '../domain/CreatingRoomTypeDto';

@Injectable({
  providedIn: 'root'
})
export class RoomTypeService {

  private apiUrl = 'https://localhost:5001/api/roomtypes';

  private header: HttpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });

  constructor(private http : HttpClient) { }

  getAll(): Observable<RoomType[]>{
    const token = localStorage.getItem('accessToken');
    const headers = this.header.set('Authorization', `Bearer ${token}`);

    return this.http.get<RoomType[]>(this.apiUrl,{ headers: headers });
  }

  getById(id : string): Observable<RoomType>{
    const token = localStorage.getItem('accessToken');
    const headers = this.header.set('Authorization', `Bearer ${token}`);

    return this.http.get<RoomType>(`${this.apiUrl}/${id}`,{ headers: headers });
  }

  add(roomtype : CreatingRoomTypeDto): Observable<RoomType>{
    const token = localStorage.getItem('accessToken');
    const headers = this.header.set('Authorization', `Bearer ${token}`);

    return this.http.post<RoomType>(this.apiUrl, roomtype,{ headers: headers })
      .pipe(
        catchError((error) => {
          return throwError(() => error.error.message);
        })
      );
  }
}
