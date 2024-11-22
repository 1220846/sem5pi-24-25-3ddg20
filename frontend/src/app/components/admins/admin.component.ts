import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { SidebarComponent } from "../sidebar/sidebar.component";
import { Router, RouterOutlet } from '@angular/router';
import { User } from '../../domain/User';
import { Observable } from 'rxjs';
import { UserService } from '../../services/user.service';


@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [RouterOutlet, SidebarComponent],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.scss'
})

export class AdminComponent implements OnInit ,AfterViewInit {

  @ViewChild('sidebar') sidebar: SidebarComponent | undefined;
  user$!: Observable<User | null>;
  user: User | null = null;
  isLoading = true;
  private timeout: any;
  private timeoutDuration = 10000;

  constructor(private userService: UserService, private router: Router) {}

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
        this.updateSidebarItems(user);
      }
    });
  }

  updateSidebarItems(user: User) {
    if (this.sidebar) {
      this.sidebar.items = [
        { label: 'Operation Types', icon: '', link: '/admin/operation-types' },
        { label: 'Staffs', icon: '', link: '/admin/staffs' },
        { label: 'Patients', icon: '', link: '/admin/patients' },
        { label: 'Operation Requests', icon: '', link: '/admin/operation-requests' },
        { label: '3D Hospital', icon: '', link: '/hospital'}
      ];
      this.sidebar.setUserTitle('Admin');
      this.sidebar.setUsername(user.username);
    }
  }
}
