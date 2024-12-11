import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { AccordionModule } from 'primeng/accordion';
import { AvatarModule } from 'primeng/avatar';
import { BadgeModule } from 'primeng/badge';
import { ScrollerModule } from 'primeng/scroller';
import { TagModule } from 'primeng/tag';
import { MedicalCondition } from '../../../../domain/MedicalCondition';
import { MedicalConditionService } from '../../../../services/medical-condition.service';

@Component({
  selector: 'app-search-medical-conditions',
  standalone: true,
  imports: [AccordionModule,
    AvatarModule,
    BadgeModule,
    TagModule,
    CommonModule,
    ScrollerModule],
  templateUrl: './search-medical-conditions.component.html',
  styleUrl: './search-medical-conditions.component.scss'
})
export class SearchMedicalConditionsComponent {

  medicalConditions: MedicalCondition[]= [];
  responsiveOptions: any[] | undefined;

  constructor(
    private medicalConditionsService: MedicalConditionService
  ){
  }

  ngOnInit(){
    this.loadMedicalConditions();
  }

  loadMedicalConditions(): void{
    this.medicalConditionsService.getAll().subscribe({next: (data) => {
      this.medicalConditions = data;},
    error: (error) => console.error('Error fetching Medical Conditions:', error)
  })
  }


  medicalConditionDescription(medicalCondition: MedicalCondition): string{
    return medicalCondition.description ? medicalCondition.description : "No description";
  }

}
