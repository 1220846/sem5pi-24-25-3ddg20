import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Patient } from '../../../../domain/Patient';
import { CardModule } from 'primeng/card';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { CalendarModule } from 'primeng/calendar';
import { DialogModule } from 'primeng/dialog';
import { PasswordModule } from 'primeng/password';
import { User } from '../../../../domain/User';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { UserService } from '../../../../services/user.service';

@Component({
  selector: 'app-update-user-patient',
  standalone: true,
  imports: [CommonModule,CardModule,ButtonModule,ReactiveFormsModule,FormsModule, InputTextModule,DropdownModule, CalendarModule,DialogModule,PasswordModule,ToastModule],
  templateUrl: './update-user-patient.component.html',
  styleUrls: ['./update-user-patient.component.scss'],
  providers: [MessageService]
})
export class UpdateUserPatientComponent implements OnInit,OnChanges {

  patientForm: FormGroup;
  resetPasswordDialogVisible: boolean = false;
  temporaryPassword: string | null = null;
  @Input()user: User | null = null;
  @Input()patient: Patient | null = null;
  
  /*patient: Patient = {
    id: 'MRN12345',
    dateOfBirth: '1990-05-15',
    emergencyContact: '987654321',
    gender: 'Male',
    email: 'john.doe@example.com',
    phoneNumber: '912345678',
    firstName: 'John',
    lastName: 'Doe',
    fullName: 'John Doe',
    medicalConditions: 'Asthma, Hypertension'
  };*/

  genderOptions = [
    { label: 'Male', value: 'Male' },
    { label: 'Female', value: 'Female' },
    { label: 'Other', value: 'Other' },
    { label: 'Undefined', value: 'Undefined' }
  ];

  isEditing = false;
  originalValues: any;
  
  constructor(private fb: FormBuilder,private userService: UserService,
    private messageService: MessageService) {
    this.patientForm = this.fb.group({
      username: [{ value: this.user?.username, disabled: true }, [Validators.required]],
      firstName: [{ value: this.patient?.firstName, disabled: true }, [Validators.required]],
      lastName: [{ value: this.patient?.lastName, disabled: true }, [Validators.required]],
      fullName: [{ value: this.patient?.fullName, disabled: true }, [Validators.required]], 
      email: [{ value: this.patient?.email, disabled: true }],
      phoneNumber: [{ value: this.patient?.phoneNumber, disabled: true }, [Validators.required, Validators.pattern('^[0-9]{9,15}$')]],
      emergencyContact: [{ value: this.patient?.emergencyContact, disabled: true }],
      gender: [{ value: this.patient?.gender, disabled: true }],
      medicalConditions: [{ value: this.patient?.medicalConditions, disabled: true }],
      birthDate: [{ value: this.patient?.dateOfBirth, disabled: true }],
      medicalRecordNumber: [{ value: this.patient?.id, disabled: true }],
      password: ['', Validators.required],
      repeatPassword: ['', Validators.required],
    }, { validator: this.passwordMatchValidator });
  }
  
  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['patient'] && this.patient && this.user) {
      this.patientForm.patchValue({
        username: this.user.username,
        firstName: this.patient.firstName,
        lastName: this.patient.lastName,
        fullName: this.patient.fullName,
        email: this.patient.email,
        phoneNumber: this.patient.phoneNumber,
        emergencyContact: this.patient.emergencyContact,
        gender: this.patient.gender,
        medicalConditions: this.patient.medicalConditions,
        birthDate: this.patient.dateOfBirth,
        medicalRecordNumber: this.patient.id
      });
    }
  }

  toggleEdit() {
    this.isEditing = !this.isEditing;
  
    if (this.isEditing) {
      this.patientForm.controls['firstName'].enable();
      this.patientForm.controls['lastName'].enable();
      this.patientForm.controls['fullName'].enable();
      this.patientForm.controls['email'].enable();
      this.patientForm.controls['phoneNumber'].enable();
    } else {
      this.patientForm.controls['firstName'].disable();
      this.patientForm.controls['lastName'].disable();
      this.patientForm.controls['fullName'].disable();
      this.patientForm.controls['email'].disable();
      this.patientForm.controls['phoneNumber'].disable();
    }
  }

  openResetPasswordDialog() {
    this.resetPasswordDialogVisible = true;
  }

  closeResetPasswordDialog() {
    this.resetPasswordDialogVisible = false;
  }

  saveData() {
    if (this.patientForm.valid) {
      console.log('Data:', this.patientForm.value);
    } else {
      alert('Erro in form');
    }
  }

  passwordMatchValidator(form: FormGroup) {
    return form.get('password')?.value === form.get('repeatPassword')?.value
      ? null : { 'mismatch': true };
  }

  resetPassword() {
    if (this.patientForm.valid) {
      this.temporaryPassword = this.patientForm.get('password')?.value;
      this.closeResetPasswordDialog(); 
    } else {
      alert('Por favor, verifique se todos os campos estão preenchidos corretamente.');
    }
  }

  saveChanges() {
    if (this.patientForm.valid) {
      const formData = this.patientForm.getRawValue(); 
      this.isEditing = false; 
    }
  }
  cancelChanges() {
    this.patientForm.patchValue(this.originalValues);
    this.isEditing = false; 
    this.patientForm.disable();
  }
}