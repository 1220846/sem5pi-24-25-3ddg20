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
import { ModalRemoveOperationRequestComponent } from '../modal-remove-operation-request/modal-remove-operation-request.component';
import { MessageService } from 'primeng/api';
import { Patient } from '../../../../domain/Patient';
import { PatientService } from '../../../../services/patient.service';

@Component({
  selector: 'app-list-operation-request',
  standalone: true,
  imports: [InputTextModule, FormsModule, OverlayPanelModule, FloatLabelModule, AvatarModule,
    TagModule, BadgeModule, ScrollerModule, CalendarModule, DropdownModule,
    DataViewModule, AccordionModule, CommonModule, ModalUpdateOperationRequestsComponent, DialogModule,
    ModalRemoveOperationRequestComponent],
  templateUrl: './list-operation-request.component.html',
  styleUrl: './list-operation-request.component.scss',
  providers: [OperationRequestService]
})
export class ListOperationRequestComponent implements OnInit{
  @ViewChild('filterPanel') filterPanel!: OverlayPanel;
  DoctorId: string | undefined;
  OperationTypeId: string | undefined;
  MedicalRecordNumber: string | undefined;
  Deadline: Date | undefined;
  Priority: string | undefined;

  filterStatus=undefined;
  filterPriority=undefined;
  filterOperationType='';
  filterMedicalRecordNumber='';

  optionsPriority = [
    { label: 'None', value: '' },
    { label: 'Elective', value: 'ELECTIVE' },
    { label: 'Urgent', value: 'URGENT' },
    { label: 'Emergency', value: 'EMERGENCY' }
  ];
  optionsStatus = [
    { label: 'None', value: '' },
    { label: 'Waiting', value: 'WAITING' },
    { label: 'Scheduled', value: 'SCHEDULED' }
  ];

  selectedStatus: string | undefined;
  selectedPriority: string | undefined;

  expandedPanels: string[] = [];
  operationRequests: OperationRequest[] = [];

  operationTypes: OperationType[] = [];
  availableOperationTypes: OperationType[] = [];

  patients: Patient[] = [];
  availablePatients: Patient[] = [];

  dataOpTypes: [string, string][] = [];
  dataOpTypesRev: [string, string][] = [];
  dataPatients: [string, string][] = [];

  visible: boolean = false;

  constructor(private operationRequestService: OperationRequestService, private patientService: PatientService ,private OperationTypeService: OperationTypeService) { }

  clearFilters(): void {
    this.filterOperationType = '';
    this.filterStatus = undefined;
    this.filterPriority = undefined;
    this.filterMedicalRecordNumber = '';
    this.loadOperationRequests();
    this.filterPanel.hide();
  }

  applyFilters(): void {
    console.log(this.filterOperationType)
    const OperationType=this.dataOpTypesRev.find((item)=>item[0]==this.filterOperationType);
    if(OperationType){
      this.filterOperationType=OperationType[1];
    }
    console.log(this.filterOperationType)
    this.loadOperationRequests();
    this.filterPanel.hide();
  }

  loadOperationRequests(): void {
    this.operationRequestService.getFilteredOperationRequests(
      this.filterMedicalRecordNumber,
      this.filterOperationType,
      this.filterPriority,
      this.filterStatus
    ).subscribe({
      next: (data) => {
        this.operationRequests = data;
        this.operationRequests.forEach((request) => {
          this.OperationTypeService.getById(request.operationTypeId).subscribe({
            next: (operationType) => {
              request.operationTypeName = operationType.name;
            },
            error: (error) => {
              console.error('Erro ao buscar o tipo de operação:', error);
            }
          });
          console.log(request)
        });
      },
      error: (error) => {
        console.error('Erro ao buscar as requisições de operação:', error);
      }
    });
  }

  ngOnInit(): void{
    this.loadOperationTypes();
    this.loadPatients();
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

  loadOperationTypes(){
    this.OperationTypeService.getAll().subscribe((data)=>{
      this.dataOpTypes = data.map((item: { id: string; name: string }) => [item.id, item.name]);
      this.dataOpTypesRev = data.map((item: { name: string; id: string }) => [item.name, item.id]);
      this.operationTypes=data;
      this.availableOperationTypes=[...this.operationTypes]
    });
  }

  loadPatients(){
    this.patientService.getAll().subscribe((data) => {
      this.patients = data;
      this.availablePatients = [...this.patients];
    });
  }

  onOperationRequestRemoved() {
    this.loadOperationRequests();
  }

  operationRequestUpdate(){
    this.loadOperationRequests();
  }
}








