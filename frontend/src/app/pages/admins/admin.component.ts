import { Component } from '@angular/core';
import { ModalCreateOperationTypeComponent } from "./operation-types/modal-create-operation-type/modal-create-operation-type.component";

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [ModalCreateOperationTypeComponent],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.scss'
})

export class AdminComponent {

}
