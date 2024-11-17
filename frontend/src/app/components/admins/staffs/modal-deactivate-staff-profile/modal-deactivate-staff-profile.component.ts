import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { StaffService } from '../../../../services/staff.service';
import { Staff } from '../../../../domain/Staff';

@Component({
  selector: 'app-deactivate-staff-profile',
  standalone: true,
  imports: [
    DialogModule,
    ButtonModule,
    CommonModule, 
    ToastModule,
    ConfirmDialogModule 
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './modal-deactivate-staff-profile.component.html',
  styleUrl: './modal-deactivate-staff-profile.component.scss'
})

export class ModalDeactivateStaffProfileComponent {
  @Output() staffProfileDeacivated = new EventEmitter<Staff>();
  @Input() staff: Staff | null = null;

  constructor(private staffService: StaffService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService) {

  }

  visible: boolean = false;

  showDialog() {
    this.visible = true;
  }

  deactivateStaff() {
    this.confirmationService.confirm({
        header: 'Are you sure?',
        message: 'This cannot be undone!',
        acceptIcon: 'pi pi-trash mr-2',
        rejectIcon: 'pi pi-times mr-2',
        acceptButtonStyleClass: 'p-button-sm p-button-danger', 
        rejectButtonStyleClass: 'p-button-sm p-button-outlined',
        accept: () => {
            console.log(this.staff?.id!);
            this.staffService.deactivate(this.staff?.id!).subscribe(
              (response) => {
                this.messageService.add({severity: 'success', summary: 'Success', detail: 'Staff profile deactivated successfully', life: 2000});
                this.staffProfileDeacivated.emit(response);
              },
              (error) => {
                console.error("Error creating staff profile:", error);
                this.messageService.add({severity: 'error', summary: 'Error', detail:'Failed to deactivate staff profile', life: 2000});
              }
            );
        },
        reject: () => {
        }
    });
}
}
