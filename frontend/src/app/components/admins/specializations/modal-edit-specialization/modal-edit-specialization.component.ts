import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { CalendarModule } from 'primeng/calendar';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { ToastModule } from 'primeng/toast';
import { Specialization } from '../../../../domain/Specialization';
import { SpecializationService } from '../../../../services/specialization.service';
import { EditingSpecializationDto } from '../../../../domain/EditingSpecializationDto';

@Component({
  selector: 'app-modal-edit-specialization',
  standalone: true,
  imports: [    
    DialogModule,
    ButtonModule,
    InputTextModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    ToastModule,
    CalendarModule,
    InputTextareaModule],
  providers: [MessageService],
  templateUrl: './modal-edit-specialization.component.html',
  styleUrl: './modal-edit-specialization.component.scss'
})
export class ModalEditSpecializationComponent {
  specializationForm: FormGroup;

  @Output() specializationEdited = new EventEmitter();
  @Input() specialization: Specialization | null = null;

  constructor(
    private specializationService: SpecializationService,
    private fb: FormBuilder,
    private messageService: MessageService
  ) {
    this.specializationForm = this.fb.group({
      name: [''],
      description: ['']
    })
    
  }

  visible: boolean = false;
  descriptionBoxVisible: boolean = false;
  
  showDialog() {
    this.resetForm();
    this.visible = true;
    this.descriptionBoxVisible = false;
  }

  resetForm() {
    this.specializationForm.patchValue({
      name: '',
      description: this.specialization?.description || '' 
    });
  }

  showDescriptionBox() {
    this.descriptionBoxVisible = true;
  }

  closeDialog() {
    this.visible = false;
    this.descriptionBoxVisible = false;
  }

  saveData() {
    if (this.specializationForm.valid) {
      const specialization: EditingSpecializationDto = {
        name: this.specializationForm.value.name ? this.specializationForm.value.name : null,
        description: this.specializationForm.value.description == (this.specialization?.description ? this.specialization?.description : '') ? null : this.specializationForm.value.description
      }
      console.log(specialization.description);
      this.specializationService.edit(this.specialization?.id!, specialization).subscribe(
        (response) => {
          this.closeDialog();
          this.specializationEdited.emit();
          this.messageService.add({severity: 'success', summary: 'Success', detail: 'Specialization edited successfully', life: 2000});
        },
        (error) => {
          console.error("Error editing specialization:", error);
          this.messageService.add({severity: 'error', summary: 'Error', detail: error, life: 2000});
        }
      );
    } else {
      this.messageService.add({severity: 'warn', summary: 'Error', detail:'Some inputted data is invalid', life: 2000});
    }
  }
}
