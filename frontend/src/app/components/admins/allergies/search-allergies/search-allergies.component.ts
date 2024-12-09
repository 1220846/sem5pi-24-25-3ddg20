import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { AccordionModule } from 'primeng/accordion';
import { AvatarModule } from 'primeng/avatar';
import { BadgeModule } from 'primeng/badge';
import { ScrollerModule } from 'primeng/scroller';
import { TagModule } from 'primeng/tag';
import { Allergy } from '../../../../domain/Allergy';
import { AllergyService } from '../../../../services/allergy.service';

@Component({
  selector: 'app-search-allergies',
  standalone: true,
  imports: [AccordionModule,
    AvatarModule,
    BadgeModule,
    TagModule,
    CommonModule,
    ScrollerModule],
  templateUrl: './search-allergies.component.html',
  styleUrl: './search-allergies.component.scss'
})
export class SearchAllergiesComponent {

  allergies: Allergy[] = [];
  responsiveOptions: any[] | undefined;

  constructor(
    private allergyService: AllergyService
  ) {}

  ngOnInit(): void {
    this.loadAllergies();
  }

  loadAllergies(): void {
    this.allergyService.getAll().subscribe({next: (data) => {
        this.allergies = data;},
      error: (error) => console.error('Error fetching allergies:', error)
    });
  }

  allergyDescription(allergy: Allergy): string {
    return allergy.description ? allergy.description : "No description";
  }
}
