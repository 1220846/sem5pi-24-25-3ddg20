import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { CreatingUserPatientDto } from '../domain/CreatingUserPatientDto';
import { User } from '@auth0/auth0-angular';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private apiUrl = 'https://localhost:5001/api/users';
  private header: HttpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });

  constructor(private httpClient: HttpClient) { 

  }

  createUserPatient(user: CreatingUserPatientDto):Observable<User>{
    return this.httpClient.post<User>(`${this.apiUrl}/patients`, user, { headers: this.header });
  }
}
