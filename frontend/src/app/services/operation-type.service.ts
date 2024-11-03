import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OperationType } from '../domain/operationType';
import { CreatingOperationTypeDto } from '../domain/creatingOperationTypeDto';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OperationTypeService {
  private apiUrl = 'https://localhost:5001/api/operationtypes';
  private header: HttpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });

  constructor(private httpClient: HttpClient) { 

  }

  getAllAndFilter(): Observable<OperationType[]> {
    return this.httpClient.get<OperationType[]>(`${this.apiUrl}/filter`);
  }

  add(operationType: CreatingOperationTypeDto):Observable<OperationType>{
    return this.httpClient.post<OperationType>(this.apiUrl, operationType, { headers: this.header });
  }

}
