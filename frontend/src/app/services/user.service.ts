import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, catchError, Observable, of, tap, throwError } from 'rxjs';
import { Injectable } from '@angular/core';
import { CreatingUserPatientDto } from '../domain/CreatingUserPatientDto';
import { LoginRequestDto } from '../domain/LoginRequestDto';
import { Login } from '../domain/Login';
import { User } from '../domain/User';
import { UpdateUserPatientDto } from '../domain/UpdateUserPatientDto';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private apiUrl = 'https://localhost:5001/api/users';
  private header: HttpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });

  private userSubject = new BehaviorSubject<User | null>(null);
  public readonly user$ = this.userSubject.asObservable();

  constructor(private httpClient: HttpClient) {
    this.getLoggedInUser();
  }

  createUserPatient(user: CreatingUserPatientDto): Observable<User> {
    return this.httpClient.post<User>(`${this.apiUrl}/patients`, user, { headers: this.header });
  }

  login(loginRequest: LoginRequestDto): Observable<Login> {
    return this.httpClient.post<Login>(`${this.apiUrl}/login`, loginRequest, { headers: this.header })
      .pipe(
        tap((response: Login) => {
          localStorage.setItem('accessToken', response.loginToken);
          localStorage.setItem('roles', JSON.stringify(response.roles));
          this.getLoggedInUser();
        }),
        catchError((error) => {
          return throwError(() => new Error(error.error.message));
        })
      );
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('accessToken');
  }

  logout(): void {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('roles');
    this.userSubject.next(null);
  }

  getLoggedInUser(): void {
    const token = localStorage.getItem('accessToken');
    if (!token) {
      this.userSubject.next(null);
      return;
    }

    const headers = this.header.set('Authorization', `Bearer ${token}`);
    this.httpClient
      .get<User>(`${this.apiUrl}/loggedIn-user`, { headers })
      .pipe(
        tap((user) => {
          this.userSubject.next(user);
        }),
        catchError((error) => {
          this.userSubject.next(null);
          return of(null);
        })
      ).subscribe();
  }

  loggedInUser(): Observable<User | null> {
    return this.user$;
  }

  deleteUserPatientAccount(username: string): Observable<string> {
    const token = localStorage.getItem('accessToken');
    const headers = this.header.set('Authorization', `Bearer ${token}`);

    return this.httpClient.post<string>(`${this.apiUrl}/patients/request-delete/${username}`, {}, { headers, responseType: 'text' as 'json' });
  }

  updateUserPatient(username: string, updateUserPatientDto: UpdateUserPatientDto): Observable<User> {
    const token = localStorage.getItem('accessToken');
    const headers = this.header.set('Authorization', `Bearer ${token}`);

    return this.httpClient.put<User>(`${this.apiUrl}/patients/${username}`, updateUserPatientDto, { headers })
      .pipe(
        catchError((error) => {
          return throwError(() => error);
        })
      );
  }
}
