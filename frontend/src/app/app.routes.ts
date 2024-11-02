import { Routes } from '@angular/router';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { HomePageComponent } from './pages/home-page/home-page.component';
import { AdminComponent } from './pages/admins/admin.component';
import { DoctorsComponent } from './pages/doctors/doctors.component';

export const routes: Routes = [
  { path: 'home', component: HomePageComponent },
  { path: 'doctor', component: DoctorsComponent },
  { path: 'sidebar', component: SidebarComponent },
  { path: 'admin', component: AdminComponent },
  { path: '', redirectTo: '/home', pathMatch: 'full' },
];

