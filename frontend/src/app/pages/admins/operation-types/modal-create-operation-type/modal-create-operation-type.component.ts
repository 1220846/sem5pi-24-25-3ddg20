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
import { Specialization } from '../../../../domain/Specialization';
import { SpecializationService } from '../../../../services/specialization.service';

@Component({
  selector: 'modal-create-operation-type',
  standalone: true,
  imports: [TagModule, IconFieldModule, InputTextModule, InputIconModule, MultiSelectModule, DropdownModule, CommonModule, ButtonModule, DialogModule, InputTextModule, InputNumberModule, FormsModule],
  templateUrl: './modal-create-operation-type.component.html',
  styleUrls: ['./modal-create-operation-type.component.scss']
})

export class ModalCreateOperationTypeComponent implements OnInit{

  constructor(private specializationService: SpecializationService) {}
  specializations: Specialization[] = [];

  ngOnInit(): void {
    this.loadSpecializations();
  }

  loadSpecializations() {
    this.specializationService.getAll().subscribe((data) => {
      this.specializations = data;
    });
  }
  
  visible: boolean = false;
  name: string | undefined;
  estimatedDuration: number | undefined;
  surgeryTime: number | undefined;
  anesthesiaTime: number | undefined;
  cleaningTime: number | undefined;

  selectedSpecializationIds!: Specialization[];

  showDialog() {
    this.visible = true;
  }
    

  saveData() {
    this.visible = false;
  }
}
