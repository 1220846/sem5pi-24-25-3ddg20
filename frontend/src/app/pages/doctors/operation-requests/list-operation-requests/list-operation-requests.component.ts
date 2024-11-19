import { Component, OnInit, ViewChild } from '@angular/core';
import { InputTextModule } from 'primeng/inputtext';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FloatLabelModule } from 'primeng/floatlabel';
import { CalendarModule } from 'primeng/calendar';
import { DropdownModule } from 'primeng/dropdown';
import { AccordionModule } from 'primeng/accordion';
import { DialogModule } from 'primeng/dialog';
import { DataViewModule } from 'primeng/dataview';
import { TagModule } from 'primeng/tag';
import { AvatarModule } from 'primeng/avatar';
import { BadgeModule } from 'primeng/badge';
import { ScrollerModule } from 'primeng/scroller';
import { OperationRequest } from '../../../../domain/OperationRequests';
import { OperationRequestService } from '../../../../services/operation-request.service';
import { OverlayPanel } from 'primeng/overlaypanel';
import { OverlayPanelModule } from 'primeng/overlaypanel';
import { OperationTypeService } from '../../../../services/operation-type.service';
import { OperationType } from '../../../../domain/OperationType';
import { ModalUpdateOperationRequestsComponent } from '../modal-update-operation-requests/modal-update-operation-requests.component';
import { Observable } from 'rxjs';


@Component({
  selector: 'list-operation-request',
  standalone: true,
  imports: [
    InputTextModule, FormsModule, OverlayPanelModule, FloatLabelModule, AvatarModule,
    TagModule, BadgeModule, ScrollerModule, CalendarModule, DropdownModule,
    DataViewModule, AccordionModule, CommonModule, ModalUpdateOperationRequestsComponent ,DialogModule
  ],
  templateUrl: './list-operation-requests.component.html',
  styleUrls: ['./list-operation-requests.component.scss'],
  providers: [OperationRequestService]
})
export class ListOperationRequestsComponent implements OnInit {
  @ViewChild('filterPanel') filterPanel!: OverlayPanel;
  DoctorId: string | undefined;
  OperationTypeId: string | undefined;
  MedicalRecordNumber: string | undefined;
  Deadline: Date | undefined;
  Priority: string | undefined;

  optionsPriority = [
    { label: 'Elective', value: 'ELECTIVE' },
    { label: 'Urgent', value: 'URGENT' },
    { label: 'Emergency', value: 'EMERGENCY' }
  ];
  optionsStatus = [
    { label: 'Waiting', value: 'WAITING' },
    { label: 'Scheduled', value: 'SCHEDULED' }
  ];

  selectedStatus: string | undefined;
  selectedPriority: string | undefined;

  expandedPanels: string[] = [];
  operationRequests: OperationRequest[] = [];

  visible: boolean = false;

  constructor(private operationRequestService: OperationRequestService, private OperationTypeService: OperationTypeService) {}

  clearFilters(): void {
    this.OperationTypeId = '';
    this.selectedStatus = undefined;
    this.selectedPriority = undefined;
    this.MedicalRecordNumber = '';
    this.loadOperationRequests();
    this.filterPanel.hide();
  }

  applyFilters(operationTypeId: string, selectedStatus: any, selectedPriority: any, medicalRecordNumber: string): void {
    this.loadOperationRequests();
    this.filterPanel.hide();
  }

  operationTypes: OperationType[] = [];

loadOperationRequests(): void {
  this.operationRequestService.getFilteredOperationRequests(
    this.DoctorId,
    this.OperationTypeId,
    this.selectedPriority,
    this.selectedStatus
  ).subscribe({
    next: (data) => {
      this.operationRequests = data;
      this.operationRequests.forEach((request) => {
        this.OperationTypeService.getById(request.OperationTypeId).subscribe({
          next: (operationType) => {
            this.operationTypes.push(operationType);
          },
          error: (error) => {
            console.error('Erro ao buscar o tipo de operação:', error);
          }
        });
      });
    },
    error: (error) => {
      console.error('Erro ao buscar as requisições de operação:', error);
    }
  });
}
  loadOperationType(operationTypeId: string): void {
    this.OperationTypeService.getById(operationTypeId).subscribe({
      next: (operationType) => {
        return operationType
        if(this.operationTypes.includes(operationType)){
          
        }
      },
      error: (error) => {
        console.error('Erro ao buscar o tipo de operação:', error);
      }
    });
  }

  ngOnInit() {
    this.loadOperationRequests();
  }

  showDialog() {
    this.visible = true;
  }

  hideDialog() {
    this.visible = false;
  }

  getPrioritySeverity(priority: string): 'success' | 'info' | 'warning' | 'danger' | 'secondary' | 'contrast' {
    switch (priority) {
      case 'ELECTIVE':
        return 'info';
      case 'URGENT':
        return 'danger';
      case 'EMERGENCY':
        return 'warning';
      default:
        return 'info';
    }
  }
}
