import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { MedicalCondition } from '../domain/MedicalCondition';
import { CreatingMedicalConditionDto } from '../domain/CreatingMedicalConditionDto';

@Injectable({
  providedIn: 'root'
})
export class MedicalConditionService {

  private apiUrl = 'http://localhost:4000/api/medicalconditions';

  private header: HttpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });
  

  constructor(private http : HttpClient) { }

  getAll(): Observable<MedicalCondition[]>{
    const token = localStorage.getItem('AccessToken');
    const headers = this.header.set('Authorixation', `Bearer ${token}`);
    return this.http.get<MedicalCondition[]>(this.apiUrl,{ headers: headers });
    
  }

  add(medicalCondition: CreatingMedicalConditionDto): Observable<MedicalCondition> {
    const token = localStorage.getItem('accessToken');
    const headers = this.header.set('Authorization', `Bearer ${token}`);
    return this.http.post<MedicalCondition>(this.apiUrl, medicalCondition, { headers: headers });
  }
}
