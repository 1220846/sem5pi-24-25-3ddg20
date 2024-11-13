import { Component, ViewChild } from '@angular/core';
import { SidebarComponent } from "../../components/sidebar/sidebar.component";
import { OperationRequestsComponent } from '../../pages/doctors/operation-requests/operation-requests.component';
import { ModalCreateOperationRequestComponent } from './operation-requests/modal-create-operation-request/modal-create-operation-request.component';
import { ListOperationRequestsComponent } from './operation-requests/list-operation-requests/list-operation-requests.component';
import { ModalUpdateOperationRequestsComponent } from './operation-requests/modal-update-operation-requests/modal-update-operation-requests.component';


@Component({
  selector: 'app-doctors',
  standalone: true,
  imports: [SidebarComponent, OperationRequestsComponent, ModalCreateOperationRequestComponent, ListOperationRequestsComponent,ModalUpdateOperationRequestsComponent],
  templateUrl: './doctors.component.html',
  styleUrl: './doctors.component.scss'
})
export class DoctorsComponent {
  @ViewChild('sidebar') sidebar!: SidebarComponent;
  @ViewChild('operation-types') operationRequests : OperationRequestsComponent |  undefined;

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
