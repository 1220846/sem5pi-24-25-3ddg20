import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OperationType } from '../domain/OperationType';
import { Observable } from 'rxjs';
import { CreatingOperationTypeDto } from '../domain/creatingOperationTypeDto';

@Injectable({
  providedIn: 'root'
})
export class OperationTypeService {
  private apiUrl = 'https://localhost:5001/api/operationtypes';
  private header: HttpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });

  constructor(private httpClient: HttpClient) { 

  }

  getAllAndFilter(name?: string, specializationId?: string, status?: string): Observable<OperationType[]> {
    let params = new HttpParams();
    if (name) 
      params = params.set('name', name);
    if (specializationId) 
      params = params.set('specializationId', specializationId);
    if (status) 
      params = params.set('status', status);

    return this.httpClient.get<OperationType[]>(`${this.apiUrl}/filter`, { params });
  }

  add(operationType: CreatingOperationTypeDto):Observable<OperationType>{
    return this.httpClient.post<OperationType>(this.apiUrl, operationType, { headers: this.header });
  }

}
