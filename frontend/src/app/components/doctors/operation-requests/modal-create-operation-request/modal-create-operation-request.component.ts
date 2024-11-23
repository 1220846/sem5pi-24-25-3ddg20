import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { TagModule } from 'primeng/tag';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { CalendarModule } from 'primeng/calendar';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { OperationRequestService } from '../../../../services/operation-request.service';
import { CreatingOperationRequestDto } from '../../../../domain/CreatingOperationRequestDto';
import { OperationType } from '../../../../domain/OperationType';
import { OperationTypeService } from '../../../../services/operation-type.service';
import { Patient } from '../../../../domain/Patient';
import { PatientService } from '../../../../services/patient.service';
import { User } from '../../../../domain/User';
import { Observable } from 'rxjs';
import { UserService } from '../../../../services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-modal-create-operation-request',
  standalone: true,
  imports: [
    TagModule, InputTextModule, DropdownModule, CommonModule, CalendarModule,
    ButtonModule, DialogModule, FormsModule, ReactiveFormsModule, ToastModule
  ],
  providers: [MessageService],
  templateUrl: './modal-create-operation-request.component.html',
  styleUrls: ['./modal-create-operation-request.component.scss']
})
export class ModalCreateOperationRequestComponent implements OnInit {

  operationRequestForm: FormGroup;
  @Output() operationRequestCreated = new EventEmitter<CreatingOperationRequestDto>();

  @Input()user: User | null = null;

  operationTypes: OperationType[] = [];
  availableOperationTypes: OperationType[] = [];

  patients: Patient[] = [];
  availablePatients: Patient[] = [];
  
  prioritySelected: string | undefined;

  constructor(
    private fb: FormBuilder,
    private messageService: MessageService,
    private operationRequestService: OperationRequestService,
    private operationTypesService: OperationTypeService,
    private patientsService: PatientService,
  ) {
    this.operationRequestForm = this.fb.group({
      operationType: ['', Validators.required],
      patient: ['', Validators.required],
      selectedPriority: [null, Validators.required],
      deadline: ['', Validators.required]
    });
  }

  optionsPriority = [
    { label: 'Elective', value: 'ELECTIVE' },
    { label: 'Urgent', value: 'URGENT' },
    { label: 'Emergency', value: 'EMERGENCY' }
  ];
  
  visible: boolean = false;



  ngOnInit(): void {
    this.loadOperationTypes();
    this.loadPatients();
  }

  showDialog() {
    this.visible = true;
    this.resetForm();
  }

  loadPatients(){
    this.patientsService.getAll().subscribe((data) => {
      this.patients = data;
      this.availablePatients = [...this.patients];
    });
  }

  loadOperationTypes() {
    this.operationTypesService.getAll().subscribe((data) => {
      this.operationTypes = data;
      this.availableOperationTypes = [...this.operationTypes];
    });
  }

  resetForm() {
    this.operationRequestForm.reset({
      selectedPriority: null  
    });
  }  

  saveData() {
    if (this.operationRequestForm.valid) {

      const doctorId = this.user?.username.split("@")[0];
      if (!doctorId) {
          throw new Error('Doctor ID is missing');
      }
      const operationRequest: CreatingOperationRequestDto = {
        doctorId,
        operationTypeId: this.operationRequestForm.value.operationType.id,
        medicalRecordNumber: this.operationRequestForm.value.patient.id,
        priority: this.operationRequestForm.value.selectedPriority,
        deadline: this.operationRequestForm.value.deadline
      };

      this.operationRequestService.add(operationRequest).subscribe(
        (response) => {
          this.visible = false;
          this.operationRequestForm.reset({
            selectedPriority: null
          });
          this.operationRequestCreated.emit(operationRequest);
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Operation request created successfully', life: 2000 });
        },
        (error) => {
          console.error("Error creating operation request:", error);
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to create operation request', life: 2000 });
        }
      );
    } else {
      this.messageService.add({ severity: 'warn', summary: 'Warning', detail: 'Form is invalid. Please fill all required fields.', life: 2000 });
    }
  }

}
