import { Component, ViewChild } from '@angular/core';
import { ModalCreateOperationRequestComponent } from './modal-create-operation-request/modal-create-operation-request.component';
import { ListOperationRequestsComponent } from './list-operation-requests/list-operation-requests.component';
import { ModalUpdateOperationRequestsComponent } from './modal-update-operation-requests/modal-update-operation-requests.component';

@Component({
  selector: 'app-operation-requests',
  standalone: true,
  imports: [ModalCreateOperationRequestComponent, ListOperationRequestsComponent, ModalUpdateOperationRequestsComponent],
  templateUrl: './operation-requests.component.html',
  styleUrl: './operation-requests.component.scss'
})
export class OperationRequestsComponent {
  @ViewChild('create-operation-requets')modalCreateOperationRequestComponent!: ModalCreateOperationRequestComponent;
  @ViewChild(ListOperationRequestsComponent, { static: false }) 
  listOperationTypesComponent!: ListOperationRequestsComponent;
}
