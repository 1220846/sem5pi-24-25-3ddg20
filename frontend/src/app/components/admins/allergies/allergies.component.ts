import { Component, ViewChild } from '@angular/core';
import { SearchAllergiesComponent } from './search-allergies/search-allergies.component';
import { ModalCreateAllergiesComponent } from './modal-create-allergies/modal-create-allergies.component';

@Component({
  selector: 'app-allergies',
  standalone: true,
  imports: [SearchAllergiesComponent,
    ModalCreateAllergiesComponent
  ],
  templateUrl: './allergies.component.html',
  styleUrl: './allergies.component.scss'
})
export class AllergiesComponent {
  
  @ViewChild('create-allergy') modalAllergyComponent!: ModalCreateAllergiesComponent;
  @ViewChild(SearchAllergiesComponent, { static: false }) searchAllergiesComponent!: SearchAllergiesComponent;
  listAllergiesComponent: any;

  constructor() {}

  onAllergyCreated() {
    this.listAllergiesComponent.loadAllergies(); 
  }
}
