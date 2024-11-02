import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { OperationRequest } from '../domain/OperationRequests';

@Injectable({
  providedIn: 'root'
})
export class OperationRequestService {

  private apiUrl = 'http://localhost:5000/api/operationRequests/filter';

  constructor(private http: HttpClient) {}

  getAll(): Observable<OperationRequest[]> {
    return this.http.get<OperationRequest[]>(this.apiUrl);
  }

  getById(id: number): Observable<OperationRequest> {
    return this.http.get<OperationRequest>(`${this.apiUrl}/${id}`);
  }

  getFilteredOperationRequests(patientId?: string, operationTypeId?: string, priority?: string, status?: string): Observable<OperationRequest[]> {
    let params = new HttpParams();
    
    if (patientId) {
      params = params.set('patientId', patientId);
    }
    if (operationTypeId) {
      params = params.set('operationTypeId', operationTypeId);
    }
    if (priority) {
      params = params.set('priority', priority);
    }
    if (status) {
      params = params.set('status', status);
    }
  
    return this.http.get<OperationRequest[]>(`${this.apiUrl}/filter`, { params });
  }

  update(patientId:string, priority:string, Deadline:string){
    
  }
}
