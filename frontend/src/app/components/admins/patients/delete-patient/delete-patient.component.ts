import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { Patient } from '../../../../domain/Patient';
import { PatientService } from '../../../../services/patient.service';

@Component({
  selector: 'app-delete-patient',
  standalone: true,
  imports: [DialogModule, ButtonModule, CommonModule, ToastModule, ConfirmDialogModule ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './delete-patient.component.html',
  styleUrl: './delete-patient.component.scss'
})
export class DeletePatientComponent {
  @Output() patientDeleted = new EventEmitter<Patient>();
  @Input() patient: Patient | null = null;

  constructor(private patientService: PatientService, private messageService: MessageService, private confirmationService: ConfirmationService){
  }

  visible: boolean = false;

  showDialog() {
    this.visible = true;
  }

  deletePatient(){
    this.confirmationService.confirm({
      header: 'Are you sure?',
      message: 'This cannot be undone!',
      acceptIcon: 'pi pi-trash mr-2',
      rejectIcon: 'pi pi-times mr-2',
      acceptButtonStyleClass: 'p-button-sm p-button-danger',
      rejectButtonStyleClass: 'p-button-sm p-button-outlined',
      accept: () => {
          console.log(this.patient?.id!);
          this.patientService.delete(this.patient?.id!).subscribe(
            (response) => {
              this.messageService.add({severity: 'success', summary: 'Success', detail: 'Patient deleted successfully', life: 2000});
              this.patientDeleted.emit(response);
            },
            (error) => {
              console.error("Error deleting patient:", error);
              this.messageService.add({severity: 'error', summary: 'Error', detail:'Failed to delete patient', life: 2000});
            }
          );
      },
      reject: () => {
      }
  });
  }
}
