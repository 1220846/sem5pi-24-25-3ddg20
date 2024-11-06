import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { StaffService } from '../../../../services/staff.service';
import { CommonModule } from '@angular/common';
import { SpecializationService } from '../../../../services/specialization.service';
import { Specialization } from '../../../../domain/specialization';
import { DropdownModule } from 'primeng/dropdown';
import { CreatingStaffDto } from '../../../../domain/CreatingStaffDto';

@Component({
  selector: 'modal-create-staff-profile',
  standalone: true,
  imports: [
    DialogModule,
    ButtonModule,
    InputTextModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    DropdownModule
  ],
  templateUrl: './modal-create-staff-profile.component.html',
  styleUrl: './modal-create-staff-profile.component.scss'
})
export class CreateStaffProfileComponent {
  staffForm: FormGroup;
  @Output() staffTypeCreated = new EventEmitter<CreatingStaffDto>();

  constructor(private staffService: StaffService, private specializationService: SpecializationService, private fb: FormBuilder) {
    this.staffForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      fullName: ['', Validators.required],
      email: ['', Validators.required],
      phoneNumber: ['', Validators.required],
      licenseNumber: ['', Validators.required],
      specialization: [ null, [Validators.required]],
      userEmail: ['', Validators.required]
    });
  }  

  specializations: Specialization[] = [];
  availableSpecializations: Specialization[] = [];
  visible: boolean = false;

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
    this.availableSpecializations = [...this.specializations];
    this.staffForm.reset();
  }

  saveData() {
    if (this.staffForm.valid) {
      const staff: CreatingStaffDto = {
        firstName: this.staffForm.value.firstName,
        lastName: this.staffForm.value.lastName,
        fullName: this.staffForm.value.fullName,
        email: this.staffForm.value.email,
        phoneNumber: this.staffForm.value.phoneNumber,
        licenseNumber: this.staffForm.value.licenseNumber,
        specializationId: this.staffForm.value.specialization.id,
        userEmail: this.staffForm.value.userEmail
      };

      this.staffService.add(staff).subscribe(
        (response) => {
          this.visible = false;
          this.staffForm.reset();
          this.staffTypeCreated.emit(staff);
        },
        (error) => {
          console.error("Error creating staff profile:", error);
        }
      );
    } else {
      console.warn("Form is invalid!");
    }
  }
}
