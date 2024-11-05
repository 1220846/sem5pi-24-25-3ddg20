import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Staff } from '../domain/Staff';
import { CreatingStaffDto } from '../domain/CreatingStaffDto';

@Injectable({
  providedIn: 'root'
})
export class StaffService {
  private apiUrl = 'https://localhost:5001/api/staffs';
  private header: HttpHeaders = new HttpHeaders({'Content-Type': 'application/json'});


  constructor(private httpClient: HttpClient) { }

  getAllAndFilter(): Observable<Staff[]> {
    return this.httpClient.get<Staff[]>(`${this.apiUrl}/filter`);
  }

  add(staff: CreatingStaffDto): Observable<Staff>{
    return this.httpClient.post<Staff>(this.apiUrl, staff, {headers: this.header});
  }
}
