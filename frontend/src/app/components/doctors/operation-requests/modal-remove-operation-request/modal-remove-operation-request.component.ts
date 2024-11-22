import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, SimpleChanges } from '@angular/core';
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
  providers: [MessageService, ConfirmationService],
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
    console.log(this.operationRequest);
  }
  
  visible: boolean = false;
  disabled: boolean = true;

  showDialog() {
    this.visible = true;
  }

  ngOnChanges(changes: SimpleChanges): void {
    console.log(this.operationRequest);
    console.log(this.operationRequest?.status);
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
        console.log(this.operationRequest?.id);
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
