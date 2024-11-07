import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { Injectable } from '@angular/core';
import { CreatingUserPatientDto } from '../domain/CreatingUserPatientDto';
import { User } from '@auth0/auth0-angular';
import { LoginRequestDto } from '../domain/LoginRequestDto';
import { Login } from '../domain/Login';

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

  login(loginRequest: LoginRequestDto): Observable<Login> {
    return this.httpClient.post<Login>(`${this.apiUrl}/login`, loginRequest, { headers: this.header })
      .pipe(
        tap((response: Login) => {
          localStorage.setItem('accessToken', response.loginToken); 
          localStorage.setItem('roles', JSON.stringify(response.roles));
        })
      );
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('accessToken');
  }

  logout(): void {
    console.log("Logout method called");
    localStorage.removeItem('accessToken');
    localStorage.removeItem('roles');
    console.log("accessToken removed:", !localStorage.getItem('accessToken'));
    console.log("roles removed:", !localStorage.getItem('roles'));
  }
}
