import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, last, Observable, throwError } from 'rxjs';
import { Staff } from '../domain/Staff';
import { CreatingStaffDto } from '../domain/CreatingStaffDto';
import { EditingStaffDto } from '../domain/EditingStaffDto';

@Injectable({
  providedIn: 'root'
})
export class StaffService {
  private apiUrl = 'https://localhost:5001/api/staffs';
  private header: HttpHeaders = new HttpHeaders({'Content-Type': 'application/json'});


  constructor(private httpClient: HttpClient) { }

  get(id: string) {
    return this.httpClient.get<Staff>(`${this.apiUrl}/${id}`, { headers: new HttpHeaders({'Authorization': `Bearer ${localStorage.getItem('accessToken')}`})});
  }

  getAllAndFilter(pageNumber: number, pageSize: number, firstName?: string, lastName?: string, fullName?: string, email?: string, specializationId?: string,
    phoneNumber?: string, id?: string, licenseNumber?:
    string, status?: string): Observable<Staff[]>
  { 
    let params = new HttpParams();
    if (firstName)
      params = params.set('firstName', firstName);
    if (lastName)
      params = params.set('lastName', lastName);
    if (fullName)
      params = params.set('fullName', fullName);
    if (email)
      params = params.set('email', email);
    if (specializationId)
      params = params.set('specializationId', specializationId);
    if (phoneNumber)
      params = params.set('phoneNumber', phoneNumber);
    if (id)
      params = params.set('id', id);
    if (licenseNumber)
      params = params.set('licenseNumber', licenseNumber);
    if (status)
      params = params.set('status', status);

    params = params.set('pageNumber', pageNumber);
    params = params.set('pageSize', pageSize);
    
    return this.httpClient.get<Staff[]>(`${this.apiUrl}/filter`, {params, headers: new HttpHeaders({'Authorization': `Bearer ${localStorage.getItem('accessToken')}`})});
  }
  staffCount(): Observable<number> {
    const token = localStorage.getItem('accessToken'); 
    const headers = this.header.set('Authorization', `Bearer ${token}`);
    return this.httpClient.get<number>(`${this.apiUrl}/count`, { headers: headers });
  }

  add(staff: CreatingStaffDto): Observable<Staff>{
    const token = localStorage.getItem('accessToken'); 
    const headers = this.header.set('Authorization', `Bearer ${token}`);
    return this.httpClient.post<Staff>(this.apiUrl, staff, {headers: headers})
    .pipe(
      catchError((error) => {
        return throwError(() => error.error.message);
      })
    );
  }

  deactivate(staffId: string) : Observable<Staff> {
    const token = localStorage.getItem('accessToken'); 
    const headers = this.header.set('Authorization', `Bearer ${token}`);
    console.log(staffId);
    return this.httpClient.delete<Staff>(`${this.apiUrl}/${staffId}`, {headers: headers});
  }

  edit(staffId: string, editDto: EditingStaffDto) {
    const token = localStorage.getItem('accessToken'); 
    const headers = this.header.set('Authorization', `Bearer ${token}`);
    return this.httpClient.patch<Staff>(`${this.apiUrl}/${staffId}`, editDto, {headers: headers})
    .pipe(
      catchError((error) => {
        return throwError(() => error.error.message);
      })
    );
  }
  getAll(): Observable<Staff[]> {
    const token = localStorage.getItem('accessToken'); 
    const headers = this.header.set('Authorization', `Bearer ${token}`);
    return this.httpClient.get<Staff[]>(this.apiUrl, {headers});
  }
}
