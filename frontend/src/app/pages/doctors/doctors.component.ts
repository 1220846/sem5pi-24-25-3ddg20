import { Component, ViewChild } from '@angular/core';
import { SidebarComponent } from "../../components/sidebar/sidebar.component";
import { OperationRequestsComponent } from '../../pages/doctors/operation-requests/operation-requests.component';
import { ModalCreateOperationRequestComponent } from './operation-requests/modal-create-operation-request/modal-create-operation-request.component';
import { ListOperationRequestsComponent } from './operation-requests/list-operation-requests/list-operation-requests.component';
import { ModalUpdateOperationRequestsComponent } from './operation-requests/modal-update-operation-requests/modal-update-operation-requests.component';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { User } from '../../domain/User';


@Component({
  selector: 'app-doctors',
  standalone: true,
  imports: [SidebarComponent, OperationRequestsComponent, ModalCreateOperationRequestComponent, ListOperationRequestsComponent,ModalUpdateOperationRequestsComponent],
  templateUrl: './doctors.component.html',
  styleUrl: './doctors.component.scss'
})
export class DoctorsComponent {

  user$!: Observable<User | null>;
  user: User | null = null;
  isLoading = true;
  private timeout: any;
  private timeoutDuration = 10000;

  constructor(private userService: UserService,private router: Router){

  }

  @ViewChild('sidebar') sidebar!: SidebarComponent;
  @ViewChild('operation-types') operationRequests : OperationRequestsComponent |  undefined;

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
        { label: 'Operation Requests', icon: '', link: '/operationRequests' }
      ];
      this.sidebar.setUserTitle('doctor');
      this.sidebar.setUsername(user.username);
    }
  }
}
