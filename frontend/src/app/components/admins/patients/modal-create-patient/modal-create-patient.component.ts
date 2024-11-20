import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { TagModule } from 'primeng/tag';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputTextModule } from 'primeng/inputtext';
import { MultiSelectModule } from 'primeng/multiselect';
import { DropdownModule } from 'primeng/dropdown';
import { CalendarModule } from 'primeng/calendar';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputNumberModule } from 'primeng/inputnumber';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import { PatientService } from '../../../../services/patient.service';
import { CreatingPatientDto } from '../../../../domain/CreatingPatientDto';
import { Patient } from '../../../../domain/Patient';

@Component({
  selector: 'app-modal-create-patient',
  standalone: true,
  imports: [TagModule, IconFieldModule, InputTextModule, InputIconModule, MultiSelectModule, DropdownModule, CalendarModule, CommonModule, ButtonModule, DialogModule, InputTextModule, InputNumberModule, FormsModule,ReactiveFormsModule],
  templateUrl: './modal-create-patient.component.html',
  styleUrl: './modal-create-patient.component.scss'
})
export class ModalCreatePatientComponent implements OnInit{

  patientForm: FormGroup;
  @Output() patientCreated = new EventEmitter<CreatingPatientDto>();

  constructor(private patientService: PatientService, private fb: FormBuilder
  ) {
    this.patientForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      fullName: ['', Validators.required],
      dateOfBirth: ['', [Validators.required]],
      gender: ['', [Validators.required]],
      emergencyContact: ['', [Validators.required, Validators.pattern('^9[1236]\\d{7}$')]],
      phoneNumber: ['', [Validators.required, Validators.pattern('^9[1236]\\d{7}$')]],
      email: ['', [Validators.required, Validators.email]],
      address: ['', [Validators.required]],
      postalCode: ['', [Validators.required, Validators.pattern('^[0-9]{4}-[0-9]{3}$')]],
    });
  }

  visible: boolean = false;
  genderOptions = [
    { label: 'Male', value: 'Male' },
    { label: 'Female', value: 'Female' },
    { label: 'Other', value: 'Other' },
    { label: 'Undefined', value: 'Undefined' }
  ];


  ngOnInit(): void {
    
  }

  showDialog() {
    this.patientForm.reset();
    this.patientForm.markAsPristine();
    this.visible = true;
  }

  saveData() {
    if (this.patientForm.valid) {
      const patient: CreatingPatientDto = {
        firstName: this.patientForm.value.firstName,
        lastName: this.patientForm.value.lastName,
        fullName: this.patientForm.value.fullName,
        dateOfBirth: this.patientForm.value.dateOfBirth,
        gender: this.patientForm.value.gender,
        emergencyContact: this.patientForm.value.emergencyContact,
        phoneNumber: this.patientForm.value.phoneNumber,
        email: this.patientForm.value.email,
        address: this.patientForm.value.address,
        postalCode: this.patientForm.value.postalCode
      };

      this.patientService.add(patient).subscribe(
        (response) => {
          console.log("Patient added successfully:", response);
          this.patientCreated.emit(patient);
          this.visible = false;
          this.patientForm.reset();
        },
        (error) => {
          console.error("Error adding patient:", error);
        }
      );
    } else {
      console.warn("Form is invalid!");
    }
  }
}
