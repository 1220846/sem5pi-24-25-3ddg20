import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { DropdownModule } from 'primeng/dropdown';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { MultiSelectModule } from 'primeng/multiselect';
import { ToastModule } from 'primeng/toast';
import { OperationTypeService } from '../../../../services/operation-type.service';
import { OperationType } from '../../../../domain/OperationType';
import { EditingOperationTypeDto } from '../../../../domain/EditingOperationTypeDto';
import { StaffSpecialization } from '../../../../domain/StaffSpecialization';

@Component({
  selector: 'app-modal-edit-operation-type',
  standalone: true,
  imports: [IconFieldModule, InputTextModule, InputIconModule, MultiSelectModule, DropdownModule, 
    CommonModule, ButtonModule, DialogModule, InputTextModule, InputNumberModule, FormsModule,ReactiveFormsModule,ToastModule],
  providers: [MessageService],
  templateUrl: './modal-edit-operation-type.component.html',
  styleUrl: './modal-edit-operation-type.component.scss'
})
export class ModalEditOperationTypeComponent implements OnInit, OnChanges{
  operationTypeForm: FormGroup;
  @Input() operationType: OperationType | null = null;
  @Output() operationTypeEdited = new EventEmitter();

  originalValues: any;
  
  constructor(private operationTypeService: OperationTypeService, private fb: FormBuilder, private messageService: MessageService) {
    this.operationTypeForm = this.fb.group({
      name: [{value: this.operationType?.name}, Validators.required],
      estimatedDuration: [{value: this.operationType?.estimatedDuration}, [Validators.required, Validators.min(1)]],
      specializations: this.fb.array([]),
    });
  }

  visible: boolean = false;

  ngOnInit(): void {
    
  }

  showDialog() {
    this.originalValues = this.operationTypeForm.value;
    this.visible = true;
  }

  closeDialog() {
    this.visible = false;
    this.operationTypeForm.patchValue(this.originalValues);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['operationType']) {
      Promise.resolve().then(() => {
        if (this.operationType) {
          this.operationTypeForm.patchValue({
            name: this.operationType.name,
            estimatedDuration: this.operationType.estimatedDuration,
          });
          const specializationsArray = this.operationTypeForm.get('specializations') as FormArray;
          specializationsArray.clear();

          this.operationType.staffSpecializationDtos?.forEach((specialization) => {
            specializationsArray.push(
              this.fb.group({
                specializationId: [{ value: specialization.specializationId, disabled: true }],
                specializationName: [{ value: specialization.specializationName, disabled: true }],
                numberOfStaff: [specialization.numberOfStaff, [Validators.required, Validators.min(1)]],
              })
            );
          });
        }
      });
    }
  }

  get specializations(): FormArray {
    return this.operationTypeForm.get('specializations') as FormArray;
  }

  saveData() {
    if (this.operationTypeForm.valid && this.operationType) {

      const updatedSpecializations = this.specializations.controls.reduce((acc: Record<string, number>, control) => {
        const specializationId = control.get('specializationId')?.value;
        const numberOfStaff = control.get('numberOfStaff')?.value;
      
        if (specializationId != null && numberOfStaff != null) {
          acc[specializationId] = numberOfStaff;
        }
        return acc;
      }, {});

      console.log('Updated Specializations:', updatedSpecializations);

      const originalSpecializations = (this.originalValues?.specializations || []).reduce((acc: Record<string, number>, specialization: any) => {
        acc[specialization.specializationId] = specialization.numberOfStaff;
        return acc;
      }, {});
      
      console.log('Original Specializations:', originalSpecializations);

      const isSpecializationsChanged = JSON.stringify(updatedSpecializations) !== JSON.stringify(originalSpecializations);

      const formData: EditingOperationTypeDto = {
        name: this.operationTypeForm.get('name')?.value === this.originalValues?.name ? null : this.operationTypeForm.get('name')?.value,
        estimatedDuration: this.operationTypeForm.get('estimatedDuration')?.value === this.originalValues?.estimatedDuration ? null : this.operationTypeForm.get('estimatedDuration')?.value, 
        staffBySpecializations: isSpecializationsChanged ? updatedSpecializations : undefined,
      };
  
      console.log(formData);

      this.operationTypeService.editOperationType(this.operationType.id, formData).subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'Updated successfully', detail: 'Your data was updated with success' });
          this.operationTypeEdited.emit();
          this.closeDialog();
        },
        error: (error) => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: error.message });
        }
      });
    } else {
      this.messageService.add({ severity: 'warn', summary: 'Warming', detail: 'Invalid Data in forms!' });
    }
  }

}
