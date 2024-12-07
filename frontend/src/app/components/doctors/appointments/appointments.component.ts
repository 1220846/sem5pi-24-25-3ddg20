import { Component, OnInit, ViewChild } from '@angular/core';
import { ModalCreateAppointmentComponent } from './modal-create-appointment/modal-create-appointment.component';
import { UserService } from '../../../services/user.service';
import { User } from '../../../domain/User';
import { ListAppointmentsComponent } from './list-appointments/list-appointments.component';

@Component({
  selector: 'app-appointments',
  standalone: true,
  imports: [ModalCreateAppointmentComponent,ListAppointmentsComponent],
  templateUrl: './appointments.component.html',
  styleUrl: './appointments.component.scss'
})
export class AppointmentsComponent implements OnInit{
  user: User | null = null;

  @ViewChild('create-appointment') modalAppointmentComponent!: ModalCreateAppointmentComponent;
  @ViewChild(ListAppointmentsComponent, { static: false }) listAppointmentsComponent!: ListAppointmentsComponent;

  constructor(private userService: UserService) {}

  onAppointmentCreated() {
    this.listAppointmentsComponent.loadAppointments(); 
  }
  ngOnInit():void{
    this.userService.loggedInUser().subscribe(user => {
      this.user = user;
    });
  }
}
