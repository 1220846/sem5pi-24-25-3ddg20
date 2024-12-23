import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-auth-callback',
  standalone: true,
  imports: [],
  templateUrl: './auth-callback.component.html',
  providers: [MessageService],
  styleUrl: './auth-callback.component.scss'
})

export class AuthCallbackComponent implements OnInit {
  isLoading = true; 
  private maxTimeout = 10000; 
  private timeoutHandle: any;

  constructor(
    private router: Router,
    private userService: UserService,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
    const fragment = window.location.hash.substring(1);
    const params = new URLSearchParams(fragment);
    const accessToken = params.get('access_token');

    if (!accessToken) {
      this.showError('Access token is missing.');
      return;
    }

    this.timeoutHandle = setTimeout(() => {
      this.isLoading = false;
      this.showError('Authentication timed out. Please try again.');
    }, this.maxTimeout);

    this.userService.authenticateWithToken(accessToken).subscribe({
      next: (response) => {
        clearTimeout(this.timeoutHandle);
        this.isLoading = false; 
        this.redirect(response.roles);
      },
      error: (error) => {
        clearTimeout(this.timeoutHandle); 
        this.isLoading = false; 
        this.showError(error.message);
      }
    });
  }

  private redirect(roles: string[]): void {
    if (roles.includes('Admin')) {
      this.router.navigate(['/admin']);
    } else if (roles.includes('Patient')) {
      this.router.navigate(['/patient']);
    } else {
      this.router.navigate(['/doctor']);
    }
  }

  private showError(message: string): void {
    this.messageService.add({
      severity: 'error',
      summary: 'Authentication Failed',
      detail: message,
      life: 3000
    });
    this.router.navigate(['/home']);
  }
}