import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { AuthService } from '@auth0/auth0-angular';
import { CreatingUserPatientDto } from '../../../domain/CreatingUserPatientDto';
import { UserService } from '../../../services/user.service';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-modal-create-user-patient',
  standalone: true,
  imports: [CommonModule,DialogModule, ButtonModule, InputTextModule,PasswordModule,FormsModule,ReactiveFormsModule,ToastModule],
  providers: [MessageService],
  templateUrl: './modal-create-user-patient.component.html',
  styleUrl: './modal-create-user-patient.component.scss'
})
export class ModalCreateUserPatientComponent implements OnInit {

  userPatientForm: FormGroup;

  visible: boolean = false;

  ngOnInit(): void {
   
  }

  constructor(private fb: FormBuilder, private userService: UserService,private messageService: MessageService) {
    this.userPatientForm = this.fb.group({
      email: ['', Validators.required],
      password: ['', Validators.required],
      repeatPassword: ['', Validators.required],
        }, { validator: this.passwordMatchValidator });
  }

  showDialog() {
    this.visible = true;
    this.userPatientForm.reset();
  }

  passwordMatchValidator(form: FormGroup) {
    return form.get('password')?.value === form.get('repeatPassword')?.value
      ? null : { 'mismatch': true };
  }

  createAccount(){
    if(this.userPatientForm.valid){
      const user: CreatingUserPatientDto = {
        email : this.userPatientForm.value.email,
        password : this.userPatientForm.value.password
      }

      this.userService.createUserPatient(user).subscribe(
        (response) => {
          this.visible = false; 
          this.userPatientForm.reset();
          this.messageService.add({severity:'success', summary: 'Success', detail: 'Account registered successfully',life: 2000});
        },
        (error) => {
          console.error("Error creating account:", error);
          this.messageService.add({severity:'error', summary: 'Error', detail: 'Failed to register account',life: 2000});
        }
      )
    } else {
      console.warn("Form is invalid!");
    }
  }
}
