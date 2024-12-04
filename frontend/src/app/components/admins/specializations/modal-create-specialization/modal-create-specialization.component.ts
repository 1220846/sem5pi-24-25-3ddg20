import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { ToastModule } from 'primeng/toast';
import { CreatingSpecializationDto } from '../../../../domain/CreatingSpecializationDto';
import { Specialization } from '../../../../domain/Specialization';
import { SpecializationService } from '../../../../services/specialization.service';
import { MessageService } from 'primeng/api';
import { InputTextareaModule } from 'primeng/inputtextarea';

@Component({
  selector: 'app-modal-create-specialization',
  standalone: true,
  imports: [
    DialogModule,
    ButtonModule,
    InputTextModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    ToastModule,
    InputTextareaModule
  ],
  providers: [MessageService],
  templateUrl: './modal-create-specialization.component.html',
  styleUrl: './modal-create-specialization.component.scss'
})
export class ModalCreateSpecializationComponent {
  specializationForm: FormGroup;
  @Output() specializationCreated = new EventEmitter<CreatingSpecializationDto>();

  constructor(
    private specializationService: SpecializationService,
    private fb: FormBuilder,
    private messageService: MessageService
  ) {
    this.specializationForm = this.fb.group({
      name: ['', Validators.required],
      code:  ['', [(control: AbstractControl) => !control.value || /^\d{6,18}$/.test(control.value) ? null : { invalidPattern: true }]],
      description: ['']
    });
  }

  visible: boolean = false;
  descriptionBoxVisible: boolean = false;

  showDialog() {
    this.visible = true;
    this.descriptionBoxVisible = false;
    this.specializationForm.reset();
  }

  showDescriptionBox() {
    this.descriptionBoxVisible = true;
  }

  saveData() {
    console.log(this.specializationForm.value.code);
    if (this.specializationForm.valid) {
      const specialization: CreatingSpecializationDto = {
        name: this.specializationForm.value.name,
        code: this.specializationForm.value.code,
        description: this.specializationForm.value.description
      };

      this.specializationService.add(specialization).subscribe(
        (response) => {
          this.visible = false;
          this.descriptionBoxVisible = false;
          this.specializationForm.reset();
          this.specializationCreated.emit(specialization);
          this.messageService.add({severity: 'success', summary: 'Success', detail: 'Specialization created successfully', life: 2000});
        },
        (error) => {
          console.error("Error creating specialization: ", error);
          this.messageService.add({severity: 'error', summary: 'Error', detail: error, life: 2000})
        }
      );
    } else {
      console.warn("Form is invalid!");
    }
  }
}
