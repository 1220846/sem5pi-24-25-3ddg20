import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { TagModule } from 'primeng/tag';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputTextModule } from 'primeng/inputtext';
import { MultiSelectModule } from 'primeng/multiselect';
import { DropdownModule } from 'primeng/dropdown';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputNumberModule } from 'primeng/inputnumber';
import { FormArray, FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import { SpecializationService } from '../../../../services/specialization.service';
import { Specialization } from '../../../../domain/Specialization';
import { OperationTypeService } from '../../../../services/operation-type.service';
import { CreatingOperationTypeDto } from '../../../../domain/CreatingOperationTypeDto';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-modal-create-operation-type',
  standalone: true,
  imports: [TagModule, IconFieldModule, InputTextModule, InputIconModule, MultiSelectModule, DropdownModule, CommonModule, ButtonModule, DialogModule, InputTextModule, InputNumberModule, FormsModule,ReactiveFormsModule,ToastModule],
  providers: [MessageService],
  templateUrl: './modal-create-operation-type.component.html',
  styleUrls: ['./modal-create-operation-type.component.scss']
})

export class ModalCreateOperationTypeComponent implements OnInit{

  operationTypeForm: FormGroup;
  @Output() operationTypeCreated = new EventEmitter<CreatingOperationTypeDto>();
  
  constructor(private specializationService: SpecializationService, private operationTypeService: OperationTypeService, private fb: FormBuilder,
  private messageService: MessageService) {
    this.operationTypeForm = this.fb.group({
      name: ['', Validators.required],
      surgeryTime: [null, [Validators.required]],
      anesthesiaTime: [null, [Validators.required]],
      cleaningTime: [null, [Validators.required]],
      selectedSpecialization: [null, Validators.required],
      staffSpecializations: this.fb.array([])
    });
  }

  specializations: Specialization[] = [];
  selectedSpecializations: { specialization: Specialization; numberOfStaff: number }[] = []; 
  availableSpecializations: Specialization[] = [];
  visible: boolean = false;
  selectedSpecialization: Specialization | null = null;

  ngOnInit(): void {
    this.loadSpecializations();
  }

  loadSpecializations() {
    this.specializationService.getAll().subscribe((data) => {
      this.specializations = data;
      this.availableSpecializations = [...this.specializations];
    });
  }

  showDialog() {
    this.visible = true;
    this.selectedSpecializations = [];
    this.availableSpecializations = [...this.specializations];
    this.staffSpecializations.clear();
    this.operationTypeForm.reset();
  }

  get staffSpecializations(): FormArray {
    return this.operationTypeForm.get('staffSpecializations') as FormArray;
  }

  addSpecialization() {
    const selectedSpec = this.operationTypeForm.get('selectedSpecialization')?.value;
    if (selectedSpec && !this.staffSpecializations.controls.some((ctrl) => ctrl.value.specializationId === selectedSpec.id)) {
      const specializationGroup = this.fb.group({
        specializationId: [selectedSpec.id, Validators.required],
        numberOfStaff: [null, [Validators.required, Validators.min(1)]],
      });
      this.staffSpecializations.push(specializationGroup);
    
      this.availableSpecializations = this.availableSpecializations.filter(
        (spec) => spec.id !== selectedSpec.id
      );
    }
  }
  
  removeSpecialization(index: number) {
    const removedSpec = this.staffSpecializations.at(index).value.specializationId;
    this.staffSpecializations.removeAt(index);
      const removed = this.specializations.find((spec) => spec.id === removedSpec);
    if (removed) {
      this.availableSpecializations.push(removed);
    }
  }
  
  getSpecializationName(specializationId: string): string | undefined {
    const specialization = this.specializations.find(spec => spec.id === specializationId);
    return specialization?.name;
  }
  
  saveData() {
    if (this.operationTypeForm.valid) {
      const operationType: CreatingOperationTypeDto = {
        name: this.operationTypeForm.value.name,
        estimatedDuration: this.operationTypeForm.value.surgeryTime 
                          +this.operationTypeForm.value.anesthesiaTime
                          +this.operationTypeForm.value.cleaningTime,
        surgeryTime: this.operationTypeForm.value.surgeryTime,
        anesthesiaTime: this.operationTypeForm.value.anesthesiaTime,
        cleaningTime: this.operationTypeForm.value.cleaningTime,
        staffSpecializations: this.staffSpecializations.value
      };
      console.log(operationType.staffSpecializations);
      this.operationTypeService.add(operationType).subscribe(
        (response) => {
          this.visible = false; 
          this.selectedSpecializations = [];
          this.staffSpecializations.clear();
          this.operationTypeForm.reset();
          this.loadSpecializations();
          this.operationTypeCreated.emit(operationType);
          this.messageService.add({severity:'success', summary: 'Success', detail: 'Operation type added successfully',life: 2000});
        },
        (error) => {
          this.messageService.add({severity: 'error', summary: 'Error', detail: error, life: 3000});
        }
      );
    } else {
      this.messageService.add({severity: 'warn', summary: 'Warning', detail: 'Invalid form data!', life: 3000});
    }
  }
}