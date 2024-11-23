import { CommonModule } from '@angular/common';
import { Component, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccordionModule } from 'primeng/accordion';
import { AvatarModule } from 'primeng/avatar';
import { BadgeModule } from 'primeng/badge';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';
import { OverlayPanel, OverlayPanelModule } from 'primeng/overlaypanel';
import { ScrollerModule } from 'primeng/scroller';
import { TagModule } from 'primeng/tag';
import { Appointment } from '../../../domain/Appointment';
import { AppointmentService } from '../../../services/appointment.service';
import { Patient } from '../../../domain/Patient';
import { PatientService } from '../../../services/patient.service';
import { catchError, of, switchMap } from 'rxjs';

@Component({
  selector: 'app-appointments',
  standalone: true,
  imports: [AccordionModule,AvatarModule,BadgeModule,TagModule,CommonModule,ScrollerModule,DropdownModule,InputTextModule,FormsModule,OverlayPanelModule,ButtonModule],
  templateUrl: './appointments.component.html',
  styleUrl: './appointments.component.scss'
})
export class AppointmentsComponent {
  
  @ViewChild('filterPanel') filterPanel!: OverlayPanel;
  patient: Patient | null = null;

  appointments: Appointment[] = [];

  constructor(
    private appointmentService: AppointmentService,
    private patientService: PatientService
  ) {}

  ngOnInit(): void {
    this.patientService.getPatient()
      .pipe(
        switchMap((patient) => {
          this.patient = patient; 

          if (patient?.id) {
            return this.appointmentService.getByPatientId(patient.id);
          }
          return of([]);
        }),
        catchError((error) => {
          console.error('Error fetching patient or appointments:', error);
          return of([]); 
        })
      )
      .subscribe((appointments) => {
        this.appointments = appointments;
      });
  }
}
