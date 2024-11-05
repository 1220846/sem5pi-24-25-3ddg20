import { Component, OnInit } from '@angular/core';
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

@Component({
  selector: 'app-modal-create-patient',
  standalone: true,
  imports: [TagModule, IconFieldModule, InputTextModule, InputIconModule, MultiSelectModule, DropdownModule, CalendarModule, CommonModule, ButtonModule, DialogModule, InputTextModule, InputNumberModule, FormsModule,ReactiveFormsModule],
  templateUrl: './modal-create-patient.component.html',
  styleUrl: './modal-create-patient.component.scss'
})
export class ModalCreatePatientComponent implements OnInit{

  patientForm: FormGroup;

  constructor(private patientService: PatientService, private fb: FormBuilder
  ) {
    this.patientForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      fullName: ['', Validators.required],
      birthDate: [null, [Validators.required]],
      gender: ['', [Validators.required]],
      emergencyContact: ['', [Validators.required]],
      phoneNumber: ['', [Validators.required]],
      email: ['', [Validators.required]],
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
        birthDate: this.patientForm.value.birthDate,
        gender: this.patientForm.value.gender,
        emergencyContact: this.patientForm.value.emergencyContact,
        phoneNumber: this.patientForm.value.phoneNumber,
        email: this.patientForm.value.email
      };

      this.patientService.add(patient).subscribe(
        (response) => {
          console.log("Patient added successfully:", response);
          this.visible = false; // Fecha o diálogo
          this.patientForm.reset(); // Reseta o formulário
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
