import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { last, Observable } from 'rxjs';
import { Staff } from '../domain/Staff';
import { CreatingStaffDto } from '../domain/CreatingStaffDto';

@Injectable({
  providedIn: 'root'
})
export class StaffService {
  private apiUrl = 'https://localhost:5001/api/staffs';
  private header: HttpHeaders = new HttpHeaders({'Content-Type': 'application/json'});


  constructor(private httpClient: HttpClient) { }

  getAllAndFilter(firstName?: string, lastName?: string, fullName?: string, email?: string, specializationId?: string,
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

    params = params.set('pageSize?', 0x7fffffff); //pagination done by primeng
    
    return this.httpClient.get<Staff[]>(`${this.apiUrl}/filter`, {params, headers: new HttpHeaders({'Authorization': `Bearer ${localStorage.getItem('accessToken')}`})});
}

  add(staff: CreatingStaffDto): Observable<Staff>{
    const token = localStorage.getItem('accessToken'); 
    const headers = this.header.set('Authorization', `Bearer ${token}`);
    return this.httpClient.post<Staff>(this.apiUrl, staff, {headers: headers});
  }

  deactivate(staffId: string) : Observable<Staff> {
    const token = localStorage.getItem('accessToken'); 
    const headers = this.header.set('Authorization', `Bearer ${token}`);
    console.log(staffId);
    //const algo = this.httpClient.delete<Staff>(`${this.apiUrl}/${staffId}`, {headers: headers});
    const algo = this.httpClient.delete<Staff>(`https://localhost:5001/api/staffs/D202400003`, {headers: headers});
    console.log(algo);
    return algo;
  }
}
