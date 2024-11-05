import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Patient } from '../domain/Patient';
import { Observable } from 'rxjs';
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

}
