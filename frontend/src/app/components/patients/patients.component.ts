import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { UserService } from '../../services/user.service';
import { Observable, Subscription } from 'rxjs';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { User } from '../../domain/User';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { Router } from '@angular/router';
import { Patient } from '../../domain/Patient';
import { PatientService } from '../../services/patient.service';

@Component({
  selector: 'app-patients',
  standalone: true,
  imports: [CommonModule,RouterOutlet,SidebarComponent,ProgressSpinnerModule],
  templateUrl: './patients.component.html',
  styleUrl: './patients.component.scss'
})
export class PatientsComponent implements OnInit, AfterViewInit {
  
  @ViewChild('sidebar') sidebar: SidebarComponent | undefined;

  user$!: Observable<User | null>;
  user: User | null = null;
  patient : Patient | null = null;
  isLoading = true;
  private timeout: any;
  private timeoutDuration = 10000;

  constructor(private userService: UserService,private patientService: PatientService, private router: Router) {}

  ngOnInit(): void {
    this.userService.getLoggedInUser();
    this.user$ = this.userService.loggedInUser();

    this.timeout = setTimeout(() => {
      if (this.isLoading) {
        this.router.navigate(['../']);
      }
    }, this.timeoutDuration);
  }

  ngAfterViewInit(): void {
    this.user$.subscribe(user => {
      if (user) {
        this.user = user;
        this.isLoading = false; 
        clearTimeout(this.timeout);
        this.updateSidebar(user);
        this.loadPatient(user.email)
      }
    });
  }

  updateSidebar(user: User): void {
    if (this.sidebar) {
      this.sidebar.items = [
        { label: 'Appointments', icon: '', link: '/patient/appointments' },
        { label: 'Account', icon: '', link: '/patient/account' }
      ];
      this.sidebar.setUserTitle('patient');
      this.sidebar.setUsername(user.username);
    }
  }
  private loadPatient(email: string): void {
    if (email) {
      this.patientService.getPatientByEmail(email).subscribe({
        next: (patient) => {
          this.patient = patient;
          this.patientService.setPatient(patient);
        },
        error: () => {
          console.error('Error fetching patient data');
        }
      });
    }
  }
}