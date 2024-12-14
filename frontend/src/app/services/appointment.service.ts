import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Appointment } from '../domain/Appointment';
import { catchError, Observable, throwError } from 'rxjs';
import { CreatingAppointmentDto } from '../domain/CreatingAppointmentDto';
import { UpdateAppointmentDto } from '../domain/UpdateAppointmentDto';

@Injectable({
  providedIn: 'root'
})
export class AppointmentService {

  private apiUrl = 'https://localhost:5001/api/appointments';
  private header: HttpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });

  constructor(private http: HttpClient) {}

  getByPatientId(medicalRecordNumber: string): Observable<Appointment[]> {
    const token = localStorage.getItem('accessToken'); 
    const headers = this.header.set('Authorization', `Bearer ${token}`);

    return this.http.get<Appointment[]>(`${this.apiUrl}/patient/${medicalRecordNumber}`, {headers: headers});
  }

  getAll(): Observable<Appointment[]> {
    const token = localStorage.getItem('accessToken'); 
    const headers = this.header.set('Authorization', `Bearer ${token}`);
    return this.http.get<Appointment[]>(this.apiUrl, {headers});
  }

  add(appointment: CreatingAppointmentDto): Observable<Appointment> {
    const token = localStorage.getItem('accessToken'); 
    const headers = this.header.set('Authorization', `Bearer ${token}`);
    return this.http.post<Appointment>(this.apiUrl, appointment, {headers})
    .pipe(
      catchError((error) => {
        console.log(error.message);
        return throwError(() => error.error.message)
      })
    ) 
  }

  getByDoctorId(doctorId: string): Observable<Appointment[]> {
    const token = localStorage.getItem('accessToken'); 
    const headers = this.header.set('Authorization', `Bearer ${token}`);

    return this.http.get<Appointment[]>(`${this.apiUrl}/doctor/${doctorId}`, {headers: headers});
  }
  
  updateAppointment(id: string, updateAppointmentDto: UpdateAppointmentDto): Observable<Appointment> {
    const token = localStorage.getItem('accessToken');
    const headers = this.header.set('Authorization', `Bearer ${token}`);

    return this.http.patch<Appointment>(`${this.apiUrl}/${id}`, updateAppointmentDto, { headers })
      .pipe(
        catchError((error) => {
          return throwError(() => error.error.message);
        })
      );
  }
}
