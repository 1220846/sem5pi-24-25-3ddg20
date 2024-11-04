import { Component, ViewChild } from '@angular/core';
import { SidebarComponent } from "../../components/sidebar/sidebar.component";
import { ModalCreateOperationTypeComponent } from "./operation-types/modal-create-operation-type/modal-create-operation-type.component";
import { ListOperationTypesComponent } from "./operation-types/list-operation-types/list-operation-types.component";
import { OperationTypesComponent } from './operation-types/operation-types.component';


@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [ModalCreateOperationTypeComponent, SidebarComponent, ListOperationTypesComponent, OperationTypesComponent],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.scss'
})

export class AdminComponent {
  @ViewChild('sidebar') sidebar: SidebarComponent | undefined;
  @ViewChild('operation-types') operationTypes : OperationTypesComponent |  undefined;

  updateSidebarItems() {
    if (this.sidebar) {
      this.sidebar.items = [
        { label: 'Home', icon: 'home', link: '/doctors' },
        { label: 'Informations', icon: 'info', link: '/' }
      ];
      this.sidebar.setUserTitle('Admin');
      //this.sidebar.setUserType();
    }
  }
}
