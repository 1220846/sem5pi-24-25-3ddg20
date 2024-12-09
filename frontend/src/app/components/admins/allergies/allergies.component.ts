import { Component, ViewChild } from '@angular/core';
import { SearchAllergiesComponent } from './search-allergies/search-allergies.component';

@Component({
  selector: 'app-allergies',
  standalone: true,
  imports: [SearchAllergiesComponent],
  templateUrl: './allergies.component.html',
  styleUrl: './allergies.component.scss'
})
export class AllergiesComponent {
  
  //@ViewChild('create-allergy') modalAppointmentComponent!: ModalCreateAppointmentComponent;
  @ViewChild(SearchAllergiesComponent, { static: false }) searchAllergiesComponent!: SearchAllergiesComponent;
  listAppointmentsComponent: any;

  constructor() {}

  onAppointmentCreated() {
    this.listAppointmentsComponent.loadAppointments(); 
  }
}
