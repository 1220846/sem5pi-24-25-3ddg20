import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OperationType } from '../domain/OperationType';
import { catchError, Observable, throwError } from 'rxjs';
import { CreatingOperationTypeDto } from '../domain/CreatingOperationTypeDto';
import { EditingOperationTypeDto } from '../domain/EditingOperationTypeDto';

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

  add(operationType: CreatingOperationTypeDto): Observable<OperationType> {
    const token = localStorage.getItem('accessToken');
    const headers = this.header.set('Authorization', `Bearer ${token}`);

    return this.httpClient.post<OperationType>(this.apiUrl, operationType, { headers: headers })
      .pipe(
        catchError((error) => {
          return throwError(() => error.error.message);
        })
      );
  }

  getAll(): Observable<OperationType[]> {
    const token = localStorage.getItem('accessToken');
    const headers = this.header.set('Authorization', `Bearer ${token}`);

    return this.httpClient.get<OperationType[]>(this.apiUrl, { headers: headers });
  }

  getById(id: string): Observable<OperationType> {
    return this.httpClient.get<OperationType>(`${this.apiUrl}/${id}`);
  }

  deactivateOperationType(operationTypeId: string):Observable<OperationType>{
    const token = localStorage.getItem('accessToken'); 
    const headers = this.header.set('Authorization', `Bearer ${token}`);
    console.log(operationTypeId);
    return this.httpClient.delete<OperationType>(`${this.apiUrl}/${operationTypeId}`, {headers: headers})
  }

  editOperationType(operationTypeId: string, editOperationType: EditingOperationTypeDto){
    const token = localStorage.getItem('accessToken');
    const headers = this.header.set('Authorization', `Bearer ${token}`);

    return this.httpClient.patch<OperationType>(`${this.apiUrl}/${operationTypeId}`, editOperationType, { headers })
      .pipe(
        catchError((error) => {
          return throwError(() => error);
        })
      );
  }
}
