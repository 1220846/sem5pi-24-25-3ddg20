import { Component, Input, OnInit } from '@angular/core';
import { DeleteAccountComponent } from './delete-account/delete-account.component';
import { UserService } from '../../../services/user.service';
import { UpdateUserPatientComponent } from './update-user-patient/update-user-patient.component';
import { PatientService } from '../../../services/patient.service';
import { Patient } from '../../../domain/Patient';
import { CommonModule } from '@angular/common';
import { User } from '../../../domain/User';

@Component({
  selector: 'app-account',
  standalone: true,
  imports: [CommonModule,DeleteAccountComponent,UpdateUserPatientComponent],
  templateUrl: './account.component.html',
  styleUrl: './account.component.scss'
})

export class AccountComponent implements OnInit {
  user: User | null = null;
  patient: Patient | null = null;

  constructor(private userService: UserService,private patientService : PatientService) {}

  ngOnInit(): void {
    this.userService.loggedInUser().subscribe(user => {
      this.user = user;
    });
    this.patientService.getPatient().subscribe(patient => {
      this.patient = patient;
      console.log(patient);
    })
  }
}
