import { Component, ViewChild } from '@angular/core';
import { ListSpecializationsComponent } from './list-specializations/list-specializations.component';
import { ModalCreateSpecializationComponent } from './modal-create-specialization/modal-create-specialization.component';

@Component({
  selector: 'app-specializations',
  standalone: true,
  imports: [ListSpecializationsComponent, ModalCreateSpecializationComponent],
  templateUrl: './specializations.component.html',
  styleUrl: './specializations.component.scss'
})
export class SpecializationsComponent {
  @ViewChild(ModalCreateSpecializationComponent) modalCreateSpecializationComponent!: ModalCreateSpecializationComponent;
  @ViewChild(ListSpecializationsComponent, { static: false }) listSpecializationsController!: ListSpecializationsComponent;

  onSpecializationCreated(): void {
    this.listSpecializationsController.onSpecializationCreated();
  }
}
