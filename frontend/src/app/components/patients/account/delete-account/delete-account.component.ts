import { Component, Input, OnInit } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { ToastModule } from 'primeng/toast';
import { ConfirmPopupModule } from 'primeng/confirmpopup';
import { ConfirmationService, MessageService } from 'primeng/api';
import { UserService } from '../../../../services/user.service';
import { User } from '@auth0/auth0-angular';

@Component({
  selector: 'app-delete-account',
  standalone: true,
  imports: [ButtonModule, ToastModule, ConfirmPopupModule],
  providers: [ConfirmationService, MessageService],
  templateUrl: './delete-account.component.html',
  styleUrl: './delete-account.component.scss'
})
export class DeleteAccountComponent implements OnInit{

  @Input() user: User | null | undefined;
  
  constructor(private confirmationService: ConfirmationService, private messageService: MessageService,
    private userService: UserService) { }
  
  ngOnInit(): void {
  }

  confirm(event: Event) {
    this.confirmationService.confirm({
        target: event.target as EventTarget,
        message: 'Do you want to delete your account?',
        icon: 'pi pi-info-circle',
        acceptButtonStyleClass: 'p-button-danger p-button-sm',
        accept: () => {
            this.deleteUserAccount();
        }
    });
  }

  private deleteUserAccount(): void {
    if (!this.user) {
      this.messageService.add({ severity: 'error', summary: 'Error', detail: 'User data not available' });
      return;
    }

    this.userService.deleteUserPatientAccount(this.user['username']).subscribe({
      next: (response) => {
        this.messageService.add({ severity: 'success', summary: 'Success', detail: response });
      },
      error: (err) => {
        console.error('Error deleting user', err);
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Error deleting account' });
      }
    });
  }
}
