import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Patient } from '../domain/Patient';
import { first, Observable } from 'rxjs';
import { CreatingPatientDto } from '../domain/CreatingPatientDto';

@Injectable({
  providedIn: 'root'
})
export class PatientService {
  private apiUrl = 'https://localhost:5001/api/patients';
  private header: HttpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });

  constructor(private httpClient: HttpClient) { 
  }

  add(patient: CreatingPatientDto):Observable<Patient>{
    return this.httpClient.post<Patient>(this.apiUrl, patient, { headers: this.header });
  }

  getPage(firstName?: string, lastName?: string, fullName?: string, email?: string, birthDate?: string, phoneNumber?: string,
    id?: string, gender?: string, pageNumber?: number, pageSize?: number): Observable<Patient[]>{
    let params = new HttpParams();
    if (firstName) 
      params = params.set('firstName', firstName);
    if (lastName) 
      params = params.set('lastName', lastName);
    if (fullName) 
      params = params.set('fullName', fullName);
    if (email) 
      params = params.set('email', email);
    if (phoneNumber) 
      params = params.set('phoneNumber', phoneNumber);
    if (birthDate) 
      params = params.set('birthDate', birthDate);
    if (id) 
      params = params.set('id', id);
    if (gender) 
      params = params.set('gender', gender);
    if (pageNumber) 
      params = params.set('pageNumber', pageNumber);
    if (pageSize) 
      params = params.set('pageSize', pageSize);
    
    return this.httpClient.get<Patient[]>(`${this.apiUrl}/`, { params }) 
  }

}
