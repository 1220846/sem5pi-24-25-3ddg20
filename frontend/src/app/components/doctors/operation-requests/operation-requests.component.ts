import { Component, OnInit, ViewChild } from '@angular/core';
import { ModalCreateOperationRequestComponent } from './modal-create-operation-request/modal-create-operation-request.component';
import { ListOperationRequestsComponent } from './list-operation-requests/list-operation-requests.component';
import { ModalUpdateOperationRequestsComponent } from './modal-update-operation-requests/modal-update-operation-requests.component';
import { UserService } from '../../../services/user.service';
import { User } from '../../../domain/User';

@Component({
  selector: 'app-operation-requests',
  standalone: true,
  imports: [ModalCreateOperationRequestComponent, ListOperationRequestsComponent, ModalUpdateOperationRequestsComponent],
  templateUrl: './operation-requests.component.html',
  styleUrl: './operation-requests.component.scss'
})
export class OperationRequestsComponent implements OnInit{
  user: User | null = null;

  constructor(private userService: UserService) {}

  ngOnInit():void{
    this.userService.loggedInUser().subscribe(user => {
      this.user = user;
    });
  }
}