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
import { CardModule } from 'primeng/card';
import { Specialization } from '../../../../domain/Specialization';
import { SpecializationService } from '../../../../services/specialization.service';
import { ScrollPanelModule } from 'primeng/scrollpanel';

@Component({
  selector: 'app-list-specializations',
  standalone: true,
  imports: [AvatarModule, BadgeModule, TagModule, CommonModule, ScrollPanelModule, DropdownModule, InputTextModule, FormsModule, OverlayPanelModule, ButtonModule, CardModule],
  templateUrl: './list-specializations.component.html',
  styleUrl: './list-specializations.component.scss'
})
export class ListSpecializationsComponent implements OnInit {
  specializations: Specialization[] = [];

  constructor(private specializationService: SpecializationService) {}

  ngOnInit(): void {
    this.loadSpecializations();
  }

  loadSpecializations(): void {
    this.specializationService.getAll().subscribe({
      next: (data) => {
        this.specializations = data;
      },
      error: (error) => console.error('Error fetching specializations:', error)
    });
  }
}
