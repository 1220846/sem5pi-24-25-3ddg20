import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { Injectable } from '@angular/core';
import { CreatingUserPatientDto } from '../domain/CreatingUserPatientDto';
import { User } from '@auth0/auth0-angular';
import { LoginRequestDto } from '../domain/LoginRequestDto';

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

  login(loginRequest: LoginRequestDto): Observable<any> {
    return this.httpClient.post(`${this.apiUrl}/login`, loginRequest, { headers: this.header })
      .pipe(tap((response: any) => {
            localStorage.setItem('accessToken', response.token);
            localStorage.setItem('roles', JSON.stringify(response.roles));
        })
      );
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('accessToken');
  }

  logout(): void {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('roles');
  }
}
