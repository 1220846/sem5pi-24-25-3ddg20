import { Component, Input, ViewChild } from '@angular/core';
import { SidebarModule } from 'primeng/sidebar';
import { ButtonModule } from 'primeng/button';
import { MenubarModule } from 'primeng/menubar';
import { RippleModule } from 'primeng/ripple';
import { AvatarModule } from 'primeng/avatar';
import { StyleClassModule } from 'primeng/styleclass';
import { Sidebar } from 'primeng/sidebar';
import { Router, RouterModule } from '@angular/router';
import { ImageModule } from 'primeng/image';
import { CommonModule } from '@angular/common';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [SidebarModule, ButtonModule, ImageModule ,RippleModule, AvatarModule, StyleClassModule,RouterModule,MenubarModule,CommonModule],
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent {
  @Input() userType: string = 'user';
  userTitle: string='User Dashboard';
  items = [
    { label: 'Início', icon: 'home', link: '/sidebar' },
    { label: 'Sobre', icon: 'spin pi-info', link: '/about' },
    { label: 'Serviços', icon: 'spin pi-cog', link: '/services' },
    { label: 'Contato', icon: 'contact_mail', link: '/contact' }
  ];

  @ViewChild('sidebarRef') sidebarRef!: Sidebar;

  closeCallback(e:Event): void {
      this.sidebarRef.close(e);
  }

  sidebarVisible: boolean = false;

  @Input() username: string = 'Username';
  isSidenavOpen: boolean = true;

  constructor(private router: Router, private userService: UserService) {}

  logout() {
    this.userService.logout();
    this.router.navigate(['/home']);
  }

  setUserTitle(user: string) {
    if (user === 'doctor') {
      this.userTitle = 'Doctor Dashboard';
    } else if (user === 'patient') {
      this.userTitle = 'Patient Dashboard';
    }else if (user === 'Admin') {
      this.userTitle = 'Admin Dashboard';
    }else if (user === 'Technitiant') {
      this.userTitle = 'Technitiant Dashboard';
    }else if (user === 'Nurse') {
      this.userTitle = 'Nurse Dashboard';
    }
  }

  setUsername(username: string){
    this.username=username;
  }

  toggleSidenav() {
    this.isSidenavOpen = !this.isSidenavOpen;
  }
}
