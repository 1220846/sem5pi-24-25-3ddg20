import { Component, OnInit } from '@angular/core';
import { InputTextModule } from 'primeng/inputtext';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FloatLabelModule } from 'primeng/floatlabel';
import { CalendarModule } from 'primeng/calendar';
import { DropdownModule } from 'primeng/dropdown';
import { DataViewModule } from 'primeng/dataview';
import { OperationRequest } from '../../../../domain/OperationRequests';
import { OperationRequestService } from '../../../../services/operation-request.service';

interface optionsPriority {
  label: string;
}

interface optionsStatus {
  label: string;
}

@Component({
  selector: 'modal-list-operation-request',
  standalone: true,
  imports: [InputTextModule, FormsModule, FloatLabelModule,CalendarModule,DropdownModule,DataViewModule,CommonModule],
  templateUrl: './modal-list-operation-requests.component.html',
  styleUrl: './modal-list-operation-requests.component.scss',
  providers: [OperationRequestService]
})
export class ModalListOperationRequestsComponent implements OnInit{
  DoctorId: string | undefined;
  OperationTypeId: string | undefined;
  MedicalRecordNumber: string | undefined;
  Deadline: Date | undefined;
  Priority: string | undefined;
  optionsPriority: optionsPriority[] | undefined;
  optionsStatus: optionsStatus[] | undefined;
  selectedStatus: optionsStatus | undefined;
  selectedPriority: optionsPriority | undefined;

  operationRequests!: OperationRequest[];

  constructor(private operationRequestService: OperationRequestService) {}

  ngOnInit() {
      this.operationRequestService.getAll().subscribe({
          next: (data: OperationRequest[]) => {
              this.operationRequests = data;
          },
          error: (error) => {
              console.error('Erro ao buscar as requisições de operação:', error);
          }
      });

      this.optionsPriority = [
          { label: 'Elective' },
          { label: 'Urgent' },
          { label: 'Emergency' }
      ];

      this.optionsStatus = [
          { label: 'Waiting' },
          { label: 'Scheduled' }
      ];

      this.operationRequestService.getFilteredOperationRequests(
          this.DoctorId,
          this.OperationTypeId,
          this.selectedPriority?.label,
          this.selectedStatus?.label
      ).subscribe({
          next: (data) => {
              this.operationRequests = data;
          },
          error: (error) => {
              console.error('Erro ao buscar as requisições de operação:', error);
          }
      });
  }
}
