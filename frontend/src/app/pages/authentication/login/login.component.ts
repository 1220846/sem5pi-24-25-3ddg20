import { Component, OnInit, ViewChild } from '@angular/core';
import { CardModule } from 'primeng/card';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { PasswordModule } from 'primeng/password';
import { InputTextModule } from 'primeng/inputtext';
import { UserService } from '../../../services/user.service';
import { CommonModule } from '@angular/common';
import { LoginRequestDto } from '../../../domain/LoginRequestDto';
import { Router } from '@angular/router';
import { ModalCreateUserPatientComponent } from '../modal-create-user-patient/modal-create-user-patient.component';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, CardModule, ButtonModule, PasswordModule, InputTextModule, FormsModule, ReactiveFormsModule, ToastModule, ModalCreateUserPatientComponent],
  providers: [MessageService],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {

  @ViewChild('create-user-patient') modalCreateUserPatientComponent!: ModalCreateUserPatientComponent;

  loginForm: FormGroup;

  constructor(private fb: FormBuilder, private userService: UserService, private router: Router, private messageService: MessageService) {
    this.loginForm = this.fb.group({
      email: ['',Validators.required, Validators.email],
      password: ['', Validators.required]
    });
  }
  ngOnInit(): void {
  }

  login() {
    if (this.loginForm.valid) {
      const loginRequestDto: LoginRequestDto = {
        username: this.loginForm.value.email,
        password: this.loginForm.value.password
      };
  
      this.userService.login(loginRequestDto).subscribe({
        next: (response) => {
          this.loginForm.reset();
          this.redirect(response.roles);
        },
        error: (error) => {
          this.messageService.add({severity: 'error',summary: 'Login Failed',detail: error.message,life: 3000});
        }
      });
    } else {
      this.messageService.add({severity: 'warn',summary: 'Warning',detail: 'Invalid Data in forms!',life: 3000});
    }
  }

  loginWithGoogle() {
  }

  private redirect(roles: string[]) {
    if (roles.includes('Admin')) {
      this.router.navigate(['/admin']);
    } else if (roles.includes('Patient')) {
      this.router.navigate(['/patient']);
    } else {
      this.router.navigate(['/doctor']);
    }
  }

}
