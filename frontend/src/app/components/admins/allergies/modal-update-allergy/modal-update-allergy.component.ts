import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { ToastModule } from 'primeng/toast';
import { Allergy } from '../../../../domain/Allergy';
import { AllergyService } from '../../../../services/allergy.service';
import { MessageService } from 'primeng/api';
import { UpdateAllergyDto } from '../../../../domain/UpdateAllergyDto';

@Component({
  selector: 'app-modal-update-allergy',
  standalone: true,
  imports: [InputTextModule, CommonModule, InputTextareaModule,
    ButtonModule, DialogModule, FormsModule, ReactiveFormsModule, ToastModule],
  providers: [MessageService],
  templateUrl: './modal-update-allergy.component.html',
  styleUrl: './modal-update-allergy.component.scss'
})
export class ModalUpdateAllergyComponent implements OnChanges {

  @Output() allergyUpdated = new EventEmitter<Allergy>();
  @Input() allergy: Allergy | null = null;

  originValues: any;
  allergyForm: FormGroup;

  isEditing = true;

  constructor(private allergyService: AllergyService, private fb: FormBuilder, private messageService: MessageService) {
    this.allergyForm = this.fb.group({
      code: [{value: null, disabled: true}],
      designation: [null, Validators.required],
      description: [null, Validators.required]
    });
  }

  visible: boolean = false;

  showDialog() {
    this.allergyForm.reset(this.originValues);
    this.visible = true;
  }

  closeDialog() {
    this.visible = false;
    this.allergyForm.reset(this.originValues);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['allergy'] && this.allergy) {
      this.allergyForm.patchValue({
        code: this.allergy.code,
        designation: this.allergy.designation,
        description: this.allergy.description
      });
      this.originValues = {
        code: this.allergy.code,
        designation: this.allergy.designation,
        description: this.allergy.description
      };
    }
  }
  

  saveData() {
    if (this.allergyForm.valid && this.allergy) {
      const formData: UpdateAllergyDto = {
        id: this.allergy.id,
        code: this.allergyForm.get('code')?.value,
        designation: this.allergyForm.get('designation')?.value === this.originValues?.designation ? null : this.allergyForm.get('designation')?.value,
        description: this.allergyForm.get('description')?.value === this.originValues?.description ? null : this.allergyForm.get('description')?.value
      };

      this.allergyService.updateAllergy(formData).subscribe({
        next: (updatedAllergy) => {
          this.messageService.add({
            severity: 'success',
            summary: 'Updated successfully',
            detail: 'Allergy data was updated successfully.'
          });
          this.allergyUpdated.emit(updatedAllergy);
          setTimeout(() => {
            this.closeDialog();
          }, 4000);

          this.originValues = {
            code: updatedAllergy.code,
            designation: updatedAllergy.designation,
            description: updatedAllergy.description
          };
        },
        error: (error) => {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: error,
            life: 3000
          });
        }
      });
    } else {
      this.messageService.add({
        severity: 'warn',
        summary: 'Warning',
        detail: 'Invalid data in the form!'
      });
    }
  }


}
