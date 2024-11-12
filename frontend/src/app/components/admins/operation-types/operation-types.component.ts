import { Component, ViewChild } from '@angular/core';
import { ListOperationTypesComponent } from './list-operation-types/list-operation-types.component';
import { ModalCreateOperationTypeComponent } from './modal-create-operation-type/modal-create-operation-type.component';

@Component({
  selector: 'app-operation-types',
  standalone: true,
  imports: [ModalCreateOperationTypeComponent,ListOperationTypesComponent],
  templateUrl: './operation-types.component.html',
  styleUrl: './operation-types.component.scss'
})
export class OperationTypesComponent {
  @ViewChild('create-operation-type') modalCreateOperationTypeComponent!: ModalCreateOperationTypeComponent;
  @ViewChild(ListOperationTypesComponent, { static: false }) 
  listOperationTypesComponent!: ListOperationTypesComponent;

  

  onOperationTypeCreated() {
    console.log("onOperationTypeCreated chamado");
    this.listOperationTypesComponent.loadOperationTypes(); 
  }
}
