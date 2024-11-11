import { Component, ViewChild } from '@angular/core';
import { ModalCreatePatientComponent } from './modal-create-patient/modal-create-patient.component';
import { ListPatientsComponent } from './list-patients/list-patients.component';

@Component({
  selector: 'app-patients',
  standalone: true,
  imports: [ModalCreatePatientComponent, ListPatientsComponent],
  templateUrl: './admin-patients.component.html',
  styleUrl: './admin-patients.component.scss'
})
export class AdminPatientsComponent {
  @ViewChild('create-patient') modalCreatePatientComponent!: ModalCreatePatientComponent;
  @ViewChild(ListPatientsComponent, { static: false }) 
  listPatientsComponent!: ListPatientsComponent;

  

  onPatientCreated() {
    this.listPatientsComponent.loadPatients(); 
    this.listPatientsComponent.countTotalPatients();
  }
}
