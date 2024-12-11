import { Component, ViewChild } from '@angular/core';
import { SearchMedicalConditionsComponent } from "./search-medical-conditions/search-medical-conditions.component";

@Component({
  selector: 'app-medical-conditions',
  standalone: true,
  imports: [SearchMedicalConditionsComponent],
  templateUrl: './medical-conditions.component.html',
  styleUrl: './medical-conditions.component.scss'
})
export class MedicalConditionsComponent {

  @ViewChild(SearchMedicalConditionsComponent, { static: false }) searchMedicalConditionsComponent!: SearchMedicalConditionsComponent;
  listAppointmentsComponent: any;

  constructor(){}

  onAppointmentCreated(){
    this.listAppointmentsComponent.loadAppointments();
  }
}
