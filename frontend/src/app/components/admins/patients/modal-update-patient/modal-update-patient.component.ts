import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Patient } from '../../../../domain/Patient';
import { PatientService } from '../../../../services/patient.service';
import { MessageService } from 'primeng/api';
import { UpdatePatientDto } from '../../../../domain/UpdatePatientDto';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { CommonModule } from '@angular/common';
import { DropdownModule } from 'primeng/dropdown';
import { ToastModule } from 'primeng/toast';
import { InputTextareaModule } from 'primeng/inputtextarea';

@Component({
  selector: 'app-modal-update-patient',
  standalone: true,
  imports: [DialogModule,
    ButtonModule,
    InputTextModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    DropdownModule,
    ToastModule, InputTextareaModule],
  providers: [MessageService],
  templateUrl: './modal-update-patient.component.html',
  styleUrl: './modal-update-patient.component.scss'
})
export class ModalUpdatePatientComponent implements OnInit, OnChanges{
  patientForm: FormGroup;

  @Output() patientUpdated = new EventEmitter<UpdatePatientDto>();
  @Input() patient: Patient | null = null;

  originalValues: any;

  constructor(
    private patientService: PatientService,
    private fb: FormBuilder,
    private messageService: MessageService)
  {
    this.patientForm = this.fb.group({
      firstName: [{ value: this.patient?.firstName }, Validators.required],
      lastName: [{ value: this.patient?.lastName }, Validators.required],
      fullName: [{ value: this.patient?.fullName }, Validators.required],
      phoneNumber: [{ value: this.patient?.phoneNumber }, [Validators.required, Validators.pattern('^9[1236]\\d{7}$')]],
      email: [{ value: this.patient?.email }, [Validators.required, Validators.email]],
      address: [{ value: this.patient?.address }, [Validators.required]],
      postalCode: [{ value: this.patient?.postalCode }, [Validators.required, Validators.pattern('^[0-9]{4}-[0-9]{3}$')]],
      medicalConditions: [{ value: this.patient?.medicalConditions }]
    });
  }

  visible: boolean = false;
  mVisible: boolean = false;

  ngOnInit(): void {
    
  }

  showDialog() {
    this.originalValues = this.patientForm.value;
    this.visible = true;
  }

  closeDialog() {
    this.visible = false;
    this.patientForm.patchValue(this.originalValues);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['patient']) {
      Promise.resolve().then(() => {
        if (this.patient) {
          this.patientForm.patchValue({
            firstName: this.patient.firstName,
            lastName: this.patient.lastName,
            fullName: this.patient.fullName,
            email: this.patient.email,
            phoneNumber: this.patient.phoneNumber,
            medicalConditions: this.patient.medicalConditions,
            address : this.patient.address,
            postalCode: this.patient.postalCode,
          });
        }
      });
    }
  }

  saveData() {
    if (this.patientForm.valid && this.patient) {

      const formData: UpdatePatientDto = {
        email: this.patientForm.get('email')?.value === this.originalValues?.email ? null : this.patientForm.get('email')?.value,
        firstName: this.patientForm.get('firstName')?.value === this.originalValues?.firstName ? null : this.patientForm.get('firstName')?.value,
        lastName: this.patientForm.get('lastName')?.value === this.originalValues?.lastName ? null : this.patientForm.get('lastName')?.value,
        fullName: this.patientForm.get('fullName')?.value === this.originalValues?.fullName ? null : this.patientForm.get('fullName')?.value,
        phoneNumber: this.patientForm.get('phoneNumber')?.value === this.originalValues?.phoneNumber ? null : this.patientForm.get('phoneNumber')?.value,
        address: this.patientForm.get('address')?.value === this.originalValues?.address ? null : this.patientForm.get('address')?.value,
        postalCode: this.patientForm.get('postalCode')?.value === this.originalValues?.postalCode ? null : this.patientForm.get('postalCode')?.value, 
        medicalConditions: this.patientForm.get('medicalConditions')?.value === this.originalValues?.medicalConditions ? null : this.patientForm.get('medicalConditions')?.value,
      };
  
      this.patientService.updatePatient(this.patient.id, formData).subscribe({
        next: (patient) => {
          this.messageService.add({ severity: 'success', summary: 'Updated successfully', detail: 'Your data was updated with success' });
          this.patientUpdated.emit(patient);
          this.closeDialog();
        },
        error: (error) => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: error.message });
        }
      });
    } else {
      this.messageService.add({ severity: 'warn', summary: 'Warming', detail: 'Invalid Data in forms!' });
    }
  }

  showEditMedicalConditions(){
    this.mVisible = true;
  }
}
