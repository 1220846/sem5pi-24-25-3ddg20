import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AvatarModule } from 'primeng/avatar';
import { BadgeModule } from 'primeng/badge';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';
import { OverlayPanelModule } from 'primeng/overlaypanel';
import { TagModule } from 'primeng/tag';
import { Specialization } from '../../../../domain/Specialization';
import { SpecializationService } from '../../../../services/specialization.service';
import { ScrollerModule } from 'primeng/scroller';
import { AccordionModule } from 'primeng/accordion';

@Component({
  selector: 'app-list-specializations',
  standalone: true,
  imports: [AccordionModule, AvatarModule, BadgeModule, TagModule, CommonModule, ScrollerModule, DropdownModule, InputTextModule, FormsModule, OverlayPanelModule, ButtonModule],
  templateUrl: './list-specializations.component.html',
  styleUrl: './list-specializations.component.scss'
})
export class ListSpecializationsComponent implements OnInit {
  specializations: Specialization[] = [];

  constructor(private specializationService: SpecializationService) {}

  ngOnInit(): void {
    this.loadSpecializations();
  }

  specializationText(specialization: Specialization): string {
    return specialization.description ? specialization.description : "No description";
  }

  loadSpecializations(): void {
    this.specializationService.getAll().subscribe({
      next: (data) => {
        this.specializations = data;
      },
      error: (error) => console.error('Error fetching specializations:', error)
    });
  }

  onSpecializationCreated(): void {
    this.loadSpecializations();
  }
}
