import { CommonModule } from '@angular/common';
import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccordionModule } from 'primeng/accordion';
import { AvatarModule } from 'primeng/avatar';
import { BadgeModule } from 'primeng/badge';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';
import { OverlayPanelModule } from 'primeng/overlaypanel';
import { ScrollerModule } from 'primeng/scroller';
import { TagModule } from 'primeng/tag';
import { AppointmentService } from '../../../../services/appointment.service';
import { Appointment } from '../../../../domain/Appointment';
import { User } from '../../../../domain/User';

@Component({
  selector: 'app-list-appointments',
  standalone: true,
  imports: [AccordionModule, AvatarModule, BadgeModule, TagModule, CommonModule, ScrollerModule, DropdownModule, InputTextModule, FormsModule, OverlayPanelModule, ButtonModule],
  templateUrl: './list-appointments.component.html',
  styleUrl: './list-appointments.component.scss'
})
export class ListAppointmentsComponent implements OnChanges {
  @Input() user: User | null = null;
  appointments: Appointment[] = [];
  doctorId: string | null = null;

  constructor(private appointmentService: AppointmentService) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['user'] && this.user) {
      this.doctorId = this.user.username.split('@')[0];
      this.loadAppointments();
    }
  }

  loadAppointments(): void {
    if (this.doctorId) {
      this.appointmentService.getByDoctorId(this.doctorId).subscribe({
        next: (data) => {
          this.appointments = data;
        },
        error: (error) => {
          console.error('Error fetching appointments:', error);
        }
      });
    } else {
      console.error('Doctor ID is missing');
    }
  }
}
