import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { TagModule } from 'primeng/tag';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ToastModule } from 'primeng/toast';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { MessageService } from 'primeng/api';
import { MedicalConditionService } from '../../../../services/medical-condition.service';
import { CreatingMedicalConditionDto } from '../../../../domain/CreatingMedicalConditionDto';

@Component({
  selector: 'app-modal-create-medical-condition',
  standalone: true,
  imports: [ TagModule, InputTextModule, DropdownModule, CommonModule, InputTextareaModule,
    ButtonModule, DialogModule, FormsModule, ReactiveFormsModule, ToastModule],
  providers: [MessageService],
  templateUrl: './modal-create-medical-condition.component.html',
  styleUrl: './modal-create-medical-condition.component.scss'
})
export class ModalCreateMedicalConditionComponent implements OnInit {
    medicalConditionForm: FormGroup;
    @Output() medicalConditionCreated= new EventEmitter<CreatingMedicalConditionDto>();
   
    ngOnInit(): void {
    }

    visible: boolean = false;
    showDialog() {
      this.medicalConditionForm.reset();
      this.medicalConditionForm.markAsPristine();
      this.visible = true;
    }

    constructor(
      private fb: FormBuilder,
      private messageService: MessageService,
      private medicalConditionService: MedicalConditionService
    ){
      this.medicalConditionForm=this.fb.group({
        code:['', Validators.required],
        designation:['', Validators.required],
        description:['', Validators.required],
      })
    }

    saveData(){
      if(this.medicalConditionForm.valid){
        const medicalCondition: CreatingMedicalConditionDto = {
          code: this.medicalConditionForm.value.code,
          designation: this.medicalConditionForm.value.designation,
          description:this.medicalConditionForm.value.description};

        this.medicalConditionService.add(medicalCondition).subscribe((response) => {
          console.log("Medical Condition saved successfully:", response);
          this.medicalConditionCreated.emit(medicalCondition);
          this.visible=false;
          this.medicalConditionForm.reset();
        },
        (error) => {
          console.log("Error Adding medical condition:", error);
        });
      }else{
        console.warn("Form is Invalid!");
      }
    }
}
