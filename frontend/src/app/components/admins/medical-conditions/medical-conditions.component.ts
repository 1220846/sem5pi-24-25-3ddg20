import { Component, ViewChild } from '@angular/core';
import { SearchMedicalConditionsComponent } from "./search-medical-conditions/search-medical-conditions.component";
import { ModalCreateMedicalConditionComponent } from "./modal-create-medical-condition/modal-create-medical-condition.component";

@Component({
  selector: 'app-medical-conditions',
  standalone: true,
  imports: [SearchMedicalConditionsComponent, ModalCreateMedicalConditionComponent],
  templateUrl: './medical-conditions.component.html',
  styleUrl: './medical-conditions.component.scss'
})
export class MedicalConditionsComponent {

  @ViewChild(SearchMedicalConditionsComponent, { static: false }) searchMedicalConditionsComponent!: SearchMedicalConditionsComponent;
  listMedicalConditionComponent: any;

  constructor(){}

  onMedicalConditionCreated(){
    this.listMedicalConditionComponent.loadMedicalConditions();
  }
  
}
