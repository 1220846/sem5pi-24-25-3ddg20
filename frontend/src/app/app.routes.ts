import { Routes } from '@angular/router';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { HomePageComponent } from './pages/home-page/home-page.component';
import { AdminComponent } from './pages/admins/admin.component';
import { DoctorsComponent } from './pages/doctors/doctors.component';
import { ModalCreateUserPatientComponent } from './pages/authentication/modal-create-user-patient/modal-create-user-patient.component';
import { OperationTypesComponent } from './pages/admins/operation-types/operation-types.component';

export const routes: Routes = [
  { path: 'home', component: HomePageComponent },
  { path: 'doctor', component: DoctorsComponent },
  { path: 'sidebar', component: SidebarComponent },
  { path: 'admin', component: AdminComponent, children: [
    { path: 'operation-types', component: OperationTypesComponent },
    { path: '', redirectTo: 'operation-types', pathMatch: 'full' }] },
  { path: 'create-user-patient', component: ModalCreateUserPatientComponent },
  { path: '', redirectTo: '/home', pathMatch: 'full' },
];

