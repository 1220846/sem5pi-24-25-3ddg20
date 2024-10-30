import { CommonModule } from '@angular/common';
import { Component, Input, ViewChild } from '@angular/core';
import { MatSidenav, MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [ CommonModule, MatSidenavModule, MatToolbarModule, MatIconModule, MatListModule, RouterModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss'
})
export class SidebarComponent {
  @Input() userType: string = 'user';
  //@Input() items: Array<{ label: string; icon: string; link: string }> = [];
  items = [
    { label: 'Início', icon: 'home', link: '/home' },
    { label: 'Sobre', icon: 'info', link: '/about' },
    { label: 'Serviços', icon: 'build', link: '/services' },
    { label: 'Contato', icon: 'contact_mail', link: '/contact' }
  ];

  username: string = 'Username';
  
  constructor(private router: Router) {}

  logout() {
    this.router.navigate(['/login']);
  }

}
