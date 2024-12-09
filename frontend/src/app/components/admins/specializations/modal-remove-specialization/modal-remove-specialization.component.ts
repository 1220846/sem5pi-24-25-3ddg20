import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, SimpleChanges } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogModule } from 'primeng/dialog';
import { ToastModule } from 'primeng/toast';
import { Specialization } from '../../../../domain/Specialization';
import { SpecializationService } from '../../../../services/specialization.service';
import { StaffService } from '../../../../services/staff.service';
import { OperationTypeService } from '../../../../services/operation-type.service';
import { MessageService, ConfirmationService } from 'primeng/api';

@Component({
  selector: 'app-modal-remove-specialization',
  standalone: true,
  imports: [
    DialogModule,
    ButtonModule,
    CommonModule, 
    ToastModule,
    ConfirmDialogModule 
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './modal-remove-specialization.component.html',
  styleUrl: './modal-remove-specialization.component.scss'
})
export class ModalRemoveSpecializationComponent {
  @Output() SpecializationRemoved = new EventEmitter<Specialization>();
  @Input() specialization : Specialization | null = null;

  constructor(
    private specializationService: SpecializationService,
    private staffService: StaffService,
    private operationTypeService: OperationTypeService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService
  ) {}

  visible: boolean = false;
  isRemoveable: boolean = false;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['specialization'] && this.specialization) {
      this.operationTypeService.getAllAndFilter('', this.specialization?.id, '').subscribe(
        (response) => {
          if (response.length == 0) {
            this.staffService.getAllAndFilter(1,0x7fffffff,'','','','',this.specialization?.id,'','','','').subscribe(
              (response) => {
                if (response.length == 0)
                  this.isRemoveable = true;
              },
              (error) => {
                console.error("Error fetching specialization's staffs.");
              }
            );
          }
        },
        (error) => {
          console.error("Error fetching specialization's operation types.");
        }
      );
    }
  }

  showDialog() {
    this.visible = true;
  }

  removeSpecialization() {
    this.confirmationService.confirm({
      header: 'Are you sure?',
      message: 'This cannot be undone!',
      acceptIcon: 'pi pi-trash mr-2',
      rejectIcon: 'pi pi-times mr-2',
      acceptButtonStyleClass: 'p-button-sm p-button-danger', 
      rejectButtonStyleClass: 'p-button-sm p-button-outlined',
      accept: () => {
        this.specializationService.remove(this.specialization?.id!).subscribe(
          (response) => {
            this.messageService.add({severity: 'success', summary: 'Success', detail: 'Specialization removed successfully', life: 2000});
            this.SpecializationRemoved.emit(response);
          },
          (error) => {
            console.error("Error removing specialization: ", error);
            this.messageService.add({severity: 'error', summary: 'Error', detail:'Failed to remove specialziation', life: 2000});
          }
        )
      },
      reject: () => {
      }
    })
  }

}
