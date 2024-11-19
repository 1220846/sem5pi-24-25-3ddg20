import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogModule } from 'primeng/dialog';
import { ToastModule } from 'primeng/toast';
import { OperationRequest } from '../../../../domain/OperationRequests';
import { OperationRequestService } from '../../../../services/operation-request.service';
import { ConfirmationService, MessageService } from 'primeng/api';

@Component({
  selector: 'app-modal-remove-operation-request',
  standalone: true,
  imports: [
    DialogModule,
    ButtonModule,
    CommonModule, 
    ToastModule,
    ConfirmDialogModule 
  ],
  templateUrl: './modal-remove-operation-request.component.html',
  styleUrl: './modal-remove-operation-request.component.scss'
})
export class ModalRemoveOperationRequestComponent {
  
  @Output() operationRequestRemoved = new EventEmitter<OperationRequest>();
  @Input() operationRequest: OperationRequest | null = null;
  
  constructor(private operationRequestService: OperationRequestService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService
  ) {

  }
  
  visible: boolean = false;

  showDialog() {
    this.visible = true;
  }

  removeOperationRequest() {
    this.confirmationService.confirm({
      header: 'Are you sure?',
      message: 'This cannot be undone!',
      acceptIcon: 'pi pi-trash mr-2',
      rejectIcon: 'pi pi-times mr-2',
      acceptButtonStyleClass: 'p-button-sm p-button-danger', 
      rejectButtonStyleClass: 'p-button-sm p-button-outlined',
      accept: () => {
        this.operationRequestService.remove(this.operationRequest?.id!).subscribe(
          (response) => {
            this.messageService.add({severity: 'success', summary: 'Success', detail: 'Operation request successfully removed', life: 2000});
            this.operationRequestRemoved.emit(response);
          },
          (error) => {
            console.error("Error removing operation request", error);
            this.messageService.add({severity: 'error', summary: 'Error', detail:'Failed to remove staff profile', life: 2000});
          }
        );
      },
      reject: () => {}
    });
  }
}
