import { Routes } from '@angular/router';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { HomePageComponent } from './pages/home-page/home-page.component';
import { AdminComponent } from './pages/admins/admin.component';
import { DoctorsComponent } from './pages/doctors/doctors.component';
import { ModalCreateUserPatientComponent } from './pages/authentication/modal-create-user-patient/modal-create-user-patient.component';
import { OperationTypesComponent } from './pages/admins/operation-types/operation-types.component';
import { LoginComponent } from './pages/authentication/login/login.component';
import { roleGuard } from './guards/role.guard';
import { HospitalComponent } from './components/hospital/hospital.component';
import { ListStaffProfilesComponent } from './pages/admins/staffs/list-staff-profiles/list-staff-profiles.component';

export const routes: Routes = [
  { path: 'home', component: HomePageComponent },
  { path: 'doctor', component: DoctorsComponent
    //,canActivate: [roleGuard],  
    //data: { roles: ['Doctor']} 
  },
  { path: 'sidebar', component: SidebarComponent },
  { 
    path: 'admin', 
    component: AdminComponent,
    //canActivate: [roleGuard],  
    //data: { roles: ['Admin'] },
    children: [{ path: 'operation-types', component: OperationTypesComponent },
              { path: '', redirectTo: 'operation-types', pathMatch: 'full' }]
  },
  { path: 'create-user-patient', component: ModalCreateUserPatientComponent },
  { path: 'login', component: LoginComponent },
  { path: 'hospital', component: HospitalComponent },
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'ads', component: ListStaffProfilesComponent},
];

