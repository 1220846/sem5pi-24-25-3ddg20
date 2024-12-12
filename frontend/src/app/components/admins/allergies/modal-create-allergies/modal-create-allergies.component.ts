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
import { AllergyService } from '../../../../services/allergy.service';
import { CreatingAllergyDto } from '../../../../domain/CreatingAllergy';

@Component({
  selector: 'app-modal-create-allergies',
  standalone: true,
  imports: [TagModule, InputTextModule, DropdownModule, CommonModule, InputTextareaModule,
    ButtonModule, DialogModule, FormsModule, ReactiveFormsModule, ToastModule],
  providers: [MessageService],
  templateUrl: './modal-create-allergies.component.html',
  styleUrl: './modal-create-allergies.component.scss'
})
export class ModalCreateAllergiesComponent implements OnInit {

  allergiesForm: FormGroup;
  @Output() allergyCreated = new EventEmitter<CreatingAllergyDto>();

  ngOnInit(): void {
  }

  visible: boolean = false;
  showDialog() {
    this.allergiesForm.reset();
    this.allergiesForm.markAsPristine();
    this.visible = true;
  }

  constructor(
    private fb: FormBuilder,
    private messageService: MessageService,
    private allergyService: AllergyService
  ) {
    this.allergiesForm = this.fb.group({
      code: ['', Validators.required],
      designation: ['', Validators.required],
      description: ['', Validators.required],
    })
  }

  saveData(){
    if(this.allergiesForm.valid){
      const allergy: CreatingAllergyDto = {
        code: this.allergiesForm.value.code,
        designation: this.allergiesForm.value.designation,
        description:this.allergiesForm.value.description};

      this.allergyService.add(allergy).subscribe((response) => {
        console.log("Allergy saved successfully:", response);
        this.allergyCreated.emit(allergy);
        this.visible=false;
        this.allergiesForm.reset();
      },
      (error) => {
        console.log("Error Adding Allergy:", error);
      });
    }else{
      console.warn("Form is Invalid!");
    }
  }
}
