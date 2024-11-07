import { Component, OnInit } from '@angular/core';
import { CardModule } from 'primeng/card';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { PasswordModule } from 'primeng/password';
import { InputTextModule } from 'primeng/inputtext';
import { UserService } from '../../../services/user.service';
import { CommonModule } from '@angular/common';
import { LoginRequestDto } from '../../../domain/LoginRequestDto';
import { Router } from '@angular/router';
        

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule,CardModule,ButtonModule,PasswordModule,InputTextModule,FormsModule,ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit{

  loginForm: FormGroup;

  constructor(private fb: FormBuilder, private userService: UserService,private router: Router) {
    this.loginForm = this.fb.group({
      email: ['', Validators.required],
      password: ['', Validators.required]});
  }
  ngOnInit(): void {
  }

  login(){
    if(this.loginForm.valid){
      const loginRequestDto: LoginRequestDto = {
        username : this.loginForm.value.email,
        password : this.loginForm.value.password
      }

      this.userService.login(loginRequestDto).subscribe(
        (response) => {
          this.loginForm.reset();
          this.redirect(response.roles);
        },
        (error) => {
          console.error("Error login:", error);
        }
      )
    } else {
      console.warn("Form is invalid!");
    }
  }

  private redirect(roles: string[]) {
    if (roles.includes('Admin')) {
      this.router.navigate(['/admin']);
    } else if (roles.includes('Patient')) {
      this.router.navigate(['/user']);
    } else {
      this.router.navigate(['/doctor']);
    }
  }

}
