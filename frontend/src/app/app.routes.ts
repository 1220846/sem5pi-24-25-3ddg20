import { Routes } from '@angular/router';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { HomePageComponent } from './pages/home-page/home-page.component';
import { AdminComponent } from './components/admins/admin.component';
import { DoctorsComponent } from './components/doctors/doctors.component';
import { ModalCreateUserPatientComponent } from './components/authentication/modal-create-user-patient/modal-create-user-patient.component';
import { OperationTypesComponent } from './components/admins/operation-types/operation-types.component';
import { LoginComponent } from './components/authentication/login/login.component';
import { roleGuard } from './guards/role.guard';
import { HospitalComponent } from './components/hospital/hospital.component';
import { ListStaffProfilesComponent } from './components/admins/staffs/list-staff-profiles/list-staff-profiles.component';
import { AccountComponent } from './components/patients/account/account.component';
import { PatientsComponent } from './components/patients/patients.component';
import { StaffsComponent } from './components/admins/staffs/staffs.component';
import { AdminPatientsComponent } from './components/admins/patients/admin-patients.component';
import { OperationRequestsComponent } from './components/doctors/operation-requests/operation-requests.component';
import { AppointmentsComponent } from './components/patients/appointments/appointments.component';

export const routes: Routes = [
  { path: 'home', component: HomePageComponent },
  { path: 'doctor', component: DoctorsComponent
    ,canActivate: [roleGuard],  
    data: { roles: ['Doctor']} 
  },
  { path: 'sidebar', component: SidebarComponent },
  { 
    path: 'admin', 
    component: AdminComponent,
    canActivate: [roleGuard],  
    data: { roles: ['Admin'] },
    children: [{ path: 'operation-types', component: OperationTypesComponent },
              { path: 'staffs', component: StaffsComponent},
              { path: 'patients', component: AdminPatientsComponent},
              { path: 'operation-requests', component:OperationRequestsComponent},
              { path: '', redirectTo: 'operation-types', pathMatch: 'full' }]
  },
  { path: 'hospital', component: HospitalComponent },
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { 
    path: 'patient', 
    component: PatientsComponent,
    canActivate: [roleGuard],  
    data: { roles: ['Patient'] },
    children: [{ path: 'account', component: AccountComponent },
              { path: 'appointments', component: AppointmentsComponent },
              { path: '', redirectTo: 'appointments', pathMatch: 'full' }]
  }
  
];

