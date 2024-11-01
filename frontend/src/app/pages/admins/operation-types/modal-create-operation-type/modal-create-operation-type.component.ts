import { Component, OnInit } from '@angular/core';
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
import { FormsModule } from '@angular/forms';
import { SpecializationService } from '../../../../services/specialization.service';
import { Specialization } from '../../../../domain/specialization';
import { OperationTypeService } from '../../../../services/operation-type.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'modal-create-operation-type',
  standalone: true,
  imports: [TagModule, IconFieldModule, InputTextModule, InputIconModule, MultiSelectModule, DropdownModule, CommonModule, ButtonModule, DialogModule, InputTextModule, InputNumberModule, FormsModule],
  templateUrl: './modal-create-operation-type.component.html',
  styleUrls: ['./modal-create-operation-type.component.scss']
})

export class ModalCreateOperationTypeComponent implements OnInit{

  constructor(private specializationService: SpecializationService, private operationTypeService: OperationTypeService) {

  }

  specializations: Specialization[] = [];
  selectedSpecializations: { specialization: Specialization; staffCount: number }[] = []; 
  availableSpecializations: Specialization[] = [];

  ngOnInit(): void {
    this.loadSpecializations();
  }

  loadSpecializations() {
    this.specializationService.getAll().subscribe((data) => {
      this.specializations = data;
      this.availableSpecializations = [...this.specializations];
    });
  }
  
  visible: boolean = false;
  name: string | undefined;
  estimatedDuration: number | undefined;
  surgeryTime: number | undefined;
  anesthesiaTime: number | undefined;
  cleaningTime: number | undefined;

  selectedSpecialization: Specialization | null = null;

  showDialog() {
    this.visible = true;
    this.selectedSpecializations = [];
    this.availableSpecializations = [...this.specializations];
  }

  addSpecialization() {
    if (this.selectedSpecialization) {
      if (!this.selectedSpecializations.some(item => item.specialization.id === this.selectedSpecialization?.id)) {
        this.selectedSpecializations.push({ specialization: this.selectedSpecialization, staffCount: 0 });
        this.availableSpecializations = this.availableSpecializations.filter(sp => sp.id !== this.selectedSpecialization?.id);
        this.selectedSpecialization = null;
      }
    }
  }

  removeSpecialization(item: { specialization: Specialization; staffCount: number }) {
    this.selectedSpecializations = this.selectedSpecializations.filter(i => i !== item);
    this.availableSpecializations.push(item.specialization); 
  }
    
  saveData() {
    this.visible = false;
    this.selectedSpecializations = [];
  }
}
