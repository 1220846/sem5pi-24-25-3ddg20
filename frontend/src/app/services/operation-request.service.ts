import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { OperationRequest } from '../domain/OperationRequests';
import { CreatingOperationRequestDto } from '../domain/CreatingOperationRequestDto';
import { RemoveOperationRequestDto } from '../domain/RemoveOperationRequestDto';
import { EditingOperationRequestDto } from '../domain/EditingOperationRequestDto';


@Injectable({
  providedIn: 'root'
})
export class OperationRequestService {

  private apiUrl = 'https://localhost:5001/api/operationRequests';
  private header: HttpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });

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

  add(operationRequest: CreatingOperationRequestDto):Observable<OperationRequest>{
    const token = localStorage.getItem('accessToken');
    const headers = this.header.set('Authorization', `Bearer ${token}`);

    return this.http.post<OperationRequest>(this.apiUrl, operationRequest, { headers: headers })
      .pipe(
        catchError((error) => {
          return throwError(() => error.error.message);
        })
      );
  }

  edit(operationRequestId: string, editDto:EditingOperationRequestDto ){
    const token = localStorage.getItem('accessToken'); 
    const headers = this.header.set('Authorization', `Bearer ${token}`);
    return this.http.patch<OperationRequest>(`${this.apiUrl}/${operationRequestId}`, editDto, {headers: headers});
  }

  remove(operationRequestId: string) : Observable<OperationRequest> {
    const token = localStorage.getItem('accessToken');
    const headers = this.header.set('Authorization', `Bearer ${token}`);
    let params = new HttpParams().set('token', token!);
    const dto: RemoveOperationRequestDto = {token: token!};
    return this.http.delete<OperationRequest>(`${this.apiUrl}/${operationRequestId}`, {body: dto, headers: headers });
  }
}
