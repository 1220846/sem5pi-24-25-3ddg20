import { Component, OnInit, Output, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TagModule } from 'primeng/tag';
import { OperationTypeService } from '../../../../services/operation-type.service';
import { OperationType } from '../../../../domain/OperationType';
import { AccordionModule } from 'primeng/accordion';
import { AvatarModule } from 'primeng/avatar';
import { BadgeModule } from 'primeng/badge';
import { ScrollerModule } from 'primeng/scroller';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';
import { FormsModule } from '@angular/forms';
import { SpecializationService } from '../../../../services/specialization.service';
import { Specialization } from '../../../../domain/Specialization';
import { OverlayPanel, OverlayPanelModule } from 'primeng/overlaypanel';
import { ButtonModule } from 'primeng/button';
import { ModalDeleteOperationTypeComponent } from '../modal-delete-operation-type/modal-delete-operation-type.component';
import { ModalEditOperationTypeComponent } from '../modal-edit-operation-type/modal-edit-operation-type.component';

@Component({
  selector: 'app-list-operation-types',
  standalone: true,
  imports: [AccordionModule,AvatarModule,BadgeModule,TagModule,CommonModule,ScrollerModule,DropdownModule,InputTextModule,FormsModule,OverlayPanelModule,ButtonModule,ModalDeleteOperationTypeComponent, ModalEditOperationTypeComponent],
  templateUrl: './list-operation-types.component.html',
  styleUrl: './list-operation-types.component.scss'
})

export class ListOperationTypesComponent implements OnInit {

  @ViewChild('filterPanel') filterPanel!: OverlayPanel;
  
  operationTypes: OperationType[] = [];
  specializations: Specialization[] = [];
  filterName: string = '';
  filterSpecializationId: string = '';
  filterStatus: string = '';

  statusOptions = [
    { label: 'None', value: null },
    { label: 'Active', value: 'ACTIVE' },
    { label: 'Inactive', value: 'INACTIVE' }
  ];

  constructor(
    private operationTypeService: OperationTypeService,
    private specializationService: SpecializationService
  ) {}

  ngOnInit(): void {
    this.loadOperationTypes();
    this.loadSpecializations();
  }

  applyFilters(): void {
    this.loadOperationTypes();
    this.filterPanel.hide();
  }

  clearFilters(): void {
    this.filterName = '';
    this.filterSpecializationId = '';
    this.filterStatus = '';
    this.loadOperationTypes();
    this.filterPanel.hide();
  }

  loadOperationTypes(): void {
    this.operationTypeService.getAllAndFilter(this.filterName,this.filterSpecializationId!,this.filterStatus).subscribe({next: (data) => {
        this.operationTypes = data;},
      error: (error) => console.error('Error fetching operation types:', error)
    });
  }

  deactivateOperationType(){
    this.loadOperationTypes();
  }

  editedOperationType(){
    this.loadOperationTypes();
  }

  loadSpecializations(): void {
    this.specializationService.getAll().subscribe({
      next: (data) => {
        this.specializations = [{ id: '', name: 'None', code: '000000' }, ...data];
        this.filterSpecializationId = ''; 
      },
      error: (error) => console.error('Error fetching specializations:', error)
    });
  }
}