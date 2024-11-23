import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Patient } from '../domain/Patient';
import { BehaviorSubject, catchError, first, Observable, throwError } from 'rxjs';
import { CreatingPatientDto } from '../domain/CreatingPatientDto';
import { UpdatePatientDto } from '../domain/UpdatePatientDto';

@Injectable({
  providedIn: 'root'
})
export class PatientService {
  private apiUrl = 'https://localhost:5001/api/patients';
  private header: HttpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });
  private patientSubject = new BehaviorSubject<Patient | null>(null);

  constructor(private httpClient: HttpClient) { 
  }

  add(patient: CreatingPatientDto):Observable<Patient>{
    const token = localStorage.getItem('accessToken'); 
    const headers = this.header.set('Authorization', `Bearer ${token}`);
    return this.httpClient.post<Patient>(this.apiUrl, patient, { headers: headers });
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
    
    return this.httpClient.get<Patient[]>(`${this.apiUrl}/filter`, { params, headers: new HttpHeaders({'Authorization': `Bearer ${localStorage.getItem('accessToken')}`}) }) 
  }

  patientCount(): Observable<number> {
    const token = localStorage.getItem('accessToken'); 
    const headers = this.header.set('Authorization', `Bearer ${token}`);
    return this.httpClient.get<number>(`${this.apiUrl}/count`, { headers: headers });
  }

  getPatientByEmail(email:string): Observable<Patient> {
    return this.httpClient.get<Patient>(`${this.apiUrl}/byemail/${email}`);
  }

  setPatient(patient: Patient | null): void {
    this.patientSubject.next(patient);
  }

  getPatient(): Observable<Patient | null> {
    return this.patientSubject.asObservable();
  }

  getById(id: string):Observable<Patient>{
    return this.httpClient.get<Patient>(`${this.apiUrl}/${id}`);
  }
  
  getAll(): Observable<Patient[]> {
    const token = localStorage.getItem('accessToken');
    const headers = this.header.set('Authorization', `Bearer ${token}`);
    
    return this.httpClient.get<Patient[]>(this.apiUrl, {headers: headers});
  }

  delete(patientId: string): Observable<Patient>{
    const token = localStorage.getItem('accessToken');
    const headers = this.header.set('Authorization', `Bearer ${token}`);
    return this.httpClient.delete<Patient>(`${this.apiUrl}/${patientId}`, {headers: headers});
  }
  
  updatePatient(id: string, updatePatientDto: UpdatePatientDto): Observable<Patient> {
    const token = localStorage.getItem('accessToken');
    const headers = this.header.set('Authorization', `Bearer ${token}`);

    return this.httpClient.patch<Patient>(`${this.apiUrl}/${id}`, updatePatientDto, { headers })
      .pipe(
        catchError((error) => {
          return throwError(() => error);
        })
      );
  }
}
