import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Allergy } from '../domain/Allergy';
import { catchError, Observable, throwError } from 'rxjs';
import { CreatingAllergyDto } from '../domain/CreatingAllergy';
import { UpdateAppointmentDto } from '../domain/UpdateAppointmentDto';
import { UpdateAllergyDto } from '../domain/UpdateAllergyDto';

@Injectable({
  providedIn: 'root'
})
export class AllergyService {
  

  private apiUrl = 'http://localhost:4000/api/allergies';

  private header: HttpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });
  
  constructor(private http : HttpClient) { }

  getAll(): Observable<Allergy[]>{
    const token = localStorage.getItem('accessToken');
    const headers = this.header.set('Authorization', `Bearer ${token}`);

    return this.http.get<Allergy[]>(this.apiUrl,{ headers: headers });
  }

  add(allergy: CreatingAllergyDto) {
    const token = localStorage.getItem('accessToken');
    const headers = this.header.set('Authorization', `Bearer ${token}`);
    return this.http.post<Allergy>(this.apiUrl, allergy, { headers: headers });
  }

  updateAllergy(updateAllergyDto: UpdateAllergyDto): Observable<Allergy> {
    const token = localStorage.getItem('accessToken');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    
    return this.http.patch<Allergy>(`${this.apiUrl}/${updateAllergyDto.id}`, updateAllergyDto, { headers })
      .pipe(
        catchError((error) => {
          return throwError(() => new Error(error.message));
        })
      );
  }
}
