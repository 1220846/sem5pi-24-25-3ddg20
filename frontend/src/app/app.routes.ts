import { Routes } from '@angular/router';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { HomePageComponent } from './pages/home-page/home-page.component';

export const routes: Routes = [
  { path: 'home', component: HomePageComponent },
  { path: 'sidebar', component: SidebarComponent },
  { path: '', redirectTo: '/home', pathMatch: 'full' },
];


