import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { SidebarComponent } from "../sidebar/sidebar.component";
import { OperationRequestsComponent } from './operation-requests/operation-requests.component';
import { ModalCreateOperationRequestComponent } from './operation-requests/modal-create-operation-request/modal-create-operation-request.component';
import { ModalUpdateOperationRequestsComponent } from './operation-requests/modal-update-operation-requests/modal-update-operation-requests.component';
import { UserService } from '../../services/user.service';
import { Router, RouterOutlet } from '@angular/router';
import { Observable } from 'rxjs';
import { User } from '../../domain/User';
import { ListOperationRequestComponent } from './operation-requests/list-operation-request/list-operation-request.component';


@Component({
  selector: 'app-doctors',
  standalone: true,
  imports: [SidebarComponent,RouterOutlet],
  templateUrl: './doctors.component.html',
  styleUrl: './doctors.component.scss'
})
export class DoctorsComponent implements OnInit, AfterViewInit{

  user$!: Observable<User | null>;
  user: User | null = null;
  isLoading = true;
  private timeout: any;
  private timeoutDuration = 10000;

  constructor(private userService: UserService,private router: Router){

  }
  @ViewChild('sidebar') sidebar!: SidebarComponent;

  ngOnInit(): void {
    this.userService.getLoggedInUser();
    this.user$ = this.userService.loggedInUser();

    this.timeout = setTimeout(() => {
      if (this.isLoading) {
        this.router.navigate(['../']);
      }
    }, this.timeoutDuration);

  }

  ngAfterViewInit():void {
    this.user$.subscribe(user => {
      if (user) {
        this.user = user;
        this.isLoading = false; 
        clearTimeout(this.timeout);
        this.updateSidebarItems(user);
        //this.loadPatient(user.email)
      }
    });
  }
  
  updateSidebarItems(user: User) {
    if (this.sidebar) {
      this.sidebar.items = [
        { label: 'Operation Requests', icon: '', link: '/doctor/operation-requests' },
        { label: 'Appointments', icon: '', link: '/doctor/appointments' },
      ];
      this.sidebar.setUserTitle('doctor');
      this.sidebar.setUsername(user.username);
    }
  }
}
