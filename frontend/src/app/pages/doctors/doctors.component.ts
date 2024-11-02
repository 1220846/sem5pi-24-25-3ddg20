import { Component, ViewChild } from '@angular/core';
import { SidebarComponent } from "../../components/sidebar/sidebar.component";
import { ModalCreateOperationRequestComponent } from "./operation-requests/modal-create-operation-request/modal-create-operation-request.component";
import { ModalListOperationRequestsComponent } from "./operation-requests/modal-list-operation-requests/modal-list-operation-requests.component";
import { ModalUpdateOperationRequestsComponent } from "./operation-requests/modal-update-operation-requests/modal-update-operation-requests.component";
import { OperationRequestService } from '../../services/operation-request.service';


@Component({
  selector: 'app-doctors',
  standalone: true,
  imports: [SidebarComponent, ModalListOperationRequestsComponent,ModalCreateOperationRequestComponent,ModalUpdateOperationRequestsComponent],
  templateUrl: './doctors.component.html',
  styleUrl: './doctors.component.scss'
})
export class DoctorsComponent {
  @ViewChild('sidebar') sidebar: SidebarComponent | undefined;
  @ViewChild('modalList') modalList: ModalListOperationRequestsComponent | undefined;
  @ViewChild('modalCreate') modalCreate: ModalCreateOperationRequestComponent | undefined;
  @ViewChild('modalUpdate') modalUpdate: ModalUpdateOperationRequestsComponent | undefined;

  constructor(private operationRequestService: OperationRequestService) {}

  ngAfterViewInit() {
    this.updateSidebarItems();
  }
  
  updateSidebarItems() {
    if (this.sidebar) {
      this.sidebar.items = [
        { label: 'Home', icon: 'home', link: '/doctors' },
        { label: 'Informations', icon: 'info', link: '/' }
      ];
      this.sidebar.setUserTitle('doctor');
      //this.sidebar.setUserType();
    }
  }
}
