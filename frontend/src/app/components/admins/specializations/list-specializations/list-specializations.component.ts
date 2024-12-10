import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AvatarModule } from 'primeng/avatar';
import { BadgeModule } from 'primeng/badge';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';
import { OverlayPanel, OverlayPanelModule } from 'primeng/overlaypanel';
import { TagModule } from 'primeng/tag';
import { Specialization } from '../../../../domain/Specialization';
import { SpecializationService } from '../../../../services/specialization.service';
import { ScrollerModule } from 'primeng/scroller';
import { AccordionModule } from 'primeng/accordion';
import { ModalEditSpecializationComponent } from '../modal-edit-specialization/modal-edit-specialization.component';
import { ModalRemoveSpecializationComponent } from '../modal-remove-specialization/modal-remove-specialization.component';

@Component({
  selector: 'app-list-specializations',
  standalone: true,
  imports: [
    AccordionModule,
    AvatarModule,
    BadgeModule,
    TagModule,
    CommonModule,
    ScrollerModule,
    DropdownModule,
    InputTextModule,
    FormsModule,
    OverlayPanelModule,
    ButtonModule,
    ModalEditSpecializationComponent,
    ModalRemoveSpecializationComponent
  ],
  templateUrl: './list-specializations.component.html',
  styleUrl: './list-specializations.component.scss'
})
export class ListSpecializationsComponent implements OnInit {
  @ViewChild('filterPanel') filterPanel!: OverlayPanel;

  specializations: Specialization[] = [];

  filterNamePartial: string = '';
  filterCodeExact: string = '';
  filterDescriptionPartial: string = '';

  constructor(private specializationService: SpecializationService) {}

  ngOnInit(): void {
    this.loadSpecializations();
  }

  applyFilters(): void {
    console.log("a");
    this.loadSpecializations();
    this.filterPanel.hide();
  }

  clearFilters(): void {
    this.filterNamePartial = '';
    this.filterCodeExact = '';
    this.filterDescriptionPartial = '';
  }

  specializationText(specialization: Specialization): string {
    return specialization.description ? specialization.description : "No description";
  }

  loadSpecializations(): void {
    this.specializationService.getAllAndFilter(
      this.filterNamePartial,
      this.filterCodeExact,
      this.filterDescriptionPartial
    ).subscribe({
      next: (data) => {
        this.specializations = data;
      },
      error: (error) => console.error('Error fetching specializations:', error)
    });
  }

  onSpecializationCreated(): void {
    this.loadSpecializations();
  }

  onSpecializationEdited(): void {
    this.loadSpecializations();
  }

  onSpecializationRemoved(): void {
    this.loadSpecializations();
  }
}
