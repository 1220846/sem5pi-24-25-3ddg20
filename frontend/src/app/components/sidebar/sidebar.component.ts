import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button'; 
import { ToolbarModule } from 'primeng/toolbar'; 
import { MenuModule } from 'primeng/menu'; 
import { MenubarModule } from 'primeng/menubar';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ButtonModule,
    ToolbarModule,
    MenuModule,
    MenubarModule
  ],
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent {
  @Input() userType: string = 'user';
  items = [
    { label: 'Início', icon: 'home', link: '/home' },
    { label: 'Sobre', icon: 'info', link: '/about' },
    { label: 'Serviços', icon: 'build', link: '/services' },
    { label: 'Contato', icon: 'contact_mail', link: '/contact' }
  ];

  username: string = 'Username';
  isSidenavOpen: boolean = true;

  constructor(private router: Router) {}

  logout() {
    this.router.navigate(['/login']);
  }

  toggleSidenav() {
    this.isSidenavOpen = !this.isSidenavOpen;
  }
}
