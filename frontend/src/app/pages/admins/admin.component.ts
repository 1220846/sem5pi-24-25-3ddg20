import { Component } from '@angular/core';
import { SidebarComponent } from "../../components/sidebar/sidebar.component";
import { ModalCreateOperationTypeComponent } from "./operation-types/modal-create-operation-type/modal-create-operation-type.component";

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [ModalCreateOperationTypeComponent,SidebarComponent],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.scss'
})

export class AdminComponent {

}
