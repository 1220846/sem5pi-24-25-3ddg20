import { Component, EventEmitter, Input, Output } from '@angular/core';
import { OperationType } from '../../../../domain/OperationType';
import { ConfirmationService, MessageService } from 'primeng/api';
import { OperationTypeService } from '../../../../services/operation-type.service';
import { ButtonModule } from 'primeng/button';
import { CommonModule } from '@angular/common';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogModule } from 'primeng/dialog';

@Component({
  selector: 'app-modal-delete-operation-type',
  standalone: true,
  providers:[MessageService, ConfirmationService],
  imports: [DialogModule,
    ButtonModule,
    CommonModule, 
    ToastModule,
    ConfirmDialogModule],
  templateUrl: './modal-delete-operation-type.component.html',
  styleUrl: './modal-delete-operation-type.component.scss'
})
export class ModalDeleteOperationTypeComponent {

  @Output() operationTypeProfileDeacivated = new EventEmitter<OperationType>();
  @Input() operationType: OperationType | null = null;

  constructor(private operationTypeService: OperationTypeService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService) {

  }

  visible: boolean = false;

  showDialog() {
    this.visible = true;
  }


  deactivateOperationType(){
    this.confirmationService.confirm({
      header: 'Are you sure?',
      message: 'This cannot be undone!',
      acceptIcon: 'pi pi-trash mr-2',
      rejectIcon: 'pi pi-times mr-2',
      acceptButtonStyleClass: 'p-button-sm p-button-danger', 
      rejectButtonStyleClass: 'p-button-sm p-button-outlined',
      accept: () => {
          this.operationTypeService.deactivateOperationType(this.operationType?.id!).subscribe(
            (response)=>{
              this.messageService.add({severity: 'success', summary: 'Success', detail: 'Operation Type deactivated successfully', life: 2000});
              this.operationTypeProfileDeacivated.emit(response);
            }
          );
      },
      reject: () => {
      }
  });
  }

}
