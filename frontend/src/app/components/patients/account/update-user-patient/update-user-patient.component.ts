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
import { UpdateUserPatientDto } from '../../../../domain/UpdateUserPatientDto';
import { passwordStrengthValidator } from '../../../../domain/PasswordStrengthValidator';

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

  isEditing = false;
  originalValues: any;
  
  constructor(private fb: FormBuilder,private userService: UserService,
    private messageService: MessageService) {
    this.patientForm = this.fb.group({
      username: [{ value: this.user?.username, disabled: true }, [Validators.required]],
      firstName: [{ value: this.patient?.firstName, disabled: true }, [Validators.required]],
      lastName: [{ value: this.patient?.lastName, disabled: true }, [Validators.required]],
      fullName: [{ value: this.patient?.fullName, disabled: true }, [Validators.required]], 
      email: [{ value: this.patient?.email, disabled: true },[Validators.email]],
      phoneNumber: [{ value: this.patient?.phoneNumber, disabled: true }, [Validators.required, Validators.pattern('^9[1236]\\d{7}$')]],
      emergencyContact: [{ value: this.patient?.emergencyContact, disabled: true }],
      gender: [{ value: this.patient?.gender, disabled: true }],
      medicalConditions: [{ value: this.patient?.medicalConditions, disabled: true }],
      birthDate: [{ value: this.patient?.dateOfBirth, disabled: true }],
      medicalRecordNumber: [{ value: this.patient?.id, disabled: true }],
      password: [''],
  repeatPassword: [''],
    }, { validator: this.passwordMatchValidator });
  }
  
  ngOnInit(): void {
    this.originalValues = this.patientForm.value;
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['patient']) {
      Promise.resolve().then(() => {
        if (this.patient && this.user) {
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
      this.patientForm.controls['password'].enable();
      this.patientForm.controls['repeatPassword'].enable();
    } else {
      this.patientForm.controls['firstName'].disable();
      this.patientForm.controls['lastName'].disable();
      this.patientForm.controls['fullName'].disable();
      this.patientForm.controls['email'].disable();
      this.patientForm.controls['phoneNumber'].disable();
      this.patientForm.controls['password'].disable();
      this.patientForm.controls['repeatPassword'].disable();
    }
  }

  openResetPasswordDialog() {
    this.resetPasswordDialogVisible = true;
    this.patientForm.get('password')?.addValidators(passwordStrengthValidator());
    this.patientForm.get('password')?.updateValueAndValidity();
  }

  closeResetPasswordDialog() {
    this.resetPasswordDialogVisible = false;
  
    this.patientForm.patchValue({
      password: '',
      repeatPassword: ''
    });
  
    this.patientForm.get('password')?.setErrors(null);
    this.patientForm.get('repeatPassword')?.setErrors(null);
  
    this.patientForm.get('password')?.clearValidators();
    this.patientForm.get('repeatPassword')?.clearValidators();
  
    this.patientForm.get('password')?.updateValueAndValidity();
    this.patientForm.get('repeatPassword')?.updateValueAndValidity();
  }
  

  passwordMatchValidator(form: FormGroup) {
    const password = form.get('password')?.value;
    const repeatPassword = form.get('repeatPassword')?.value;
        if (!password && !repeatPassword) {
      return null;
    }

    return password === repeatPassword ? null : { 'mismatch': true };
  }

  resetPassword() {
    const passwordControl = this.patientForm.get('password');
    const repeatPasswordControl = this.patientForm.get('repeatPassword');

    if (passwordControl?.value !== repeatPasswordControl?.value) {
      repeatPasswordControl?.setErrors({ mismatch: true });
    } else {
      this.temporaryPassword = passwordControl?.value;
      this.closeResetPasswordDialog();
    }
  }

  saveData() {
    if (this.patientForm.valid && this.patient && this.user) {
      const passwordValue = this.patientForm.get('password')?.value;
      const password = passwordValue && passwordValue !== '' ? passwordValue : null;
  
      const formData: UpdateUserPatientDto = {
        email: this.patientForm.get('email')?.value === this.originalValues?.email ? null : this.patientForm.get('email')?.value,
        firstName: this.patientForm.get('firstName')?.value === this.originalValues?.firstName ? null : this.patientForm.get('firstName')?.value,
        lastName: this.patientForm.get('lastName')?.value === this.originalValues?.lastName ? null : this.patientForm.get('lastName')?.value,
        fullName: this.patientForm.get('fullName')?.value === this.originalValues?.fullName ? null : this.patientForm.get('fullName')?.value,
        phoneNumber: this.patientForm.get('phoneNumber')?.value === this.originalValues?.phoneNumber ? null : this.patientForm.get('phoneNumber')?.value,
        password: password,
      };
  
      this.userService.updateUserPatient(this.user.username, formData).subscribe({
        next: (updatedUser) => {
          this.messageService.add({ severity: 'success', summary: 'Updated successfully', detail: 'Your data was updated with success' });
          this.isEditing = false;
          this.patientForm.disable();
        },
        error: (error) => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: error.message });
        }
      });
    } else {
      this.messageService.add({ severity: 'warn', summary: 'Warming', detail: 'Invalid Data in forms!' });
    }
  }

  cancelChanges() {
    this.patientForm.patchValue(this.originalValues);
    this.isEditing = false;
    this.patientForm.disable();
  }
}