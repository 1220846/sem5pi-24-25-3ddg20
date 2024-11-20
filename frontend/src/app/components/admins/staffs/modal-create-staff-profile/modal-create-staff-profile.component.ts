import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { StaffService } from '../../../../services/staff.service';
import { CommonModule } from '@angular/common';
import { SpecializationService } from '../../../../services/specialization.service';
import { Specialization } from '../../../../domain/Specialization';
import { DropdownModule } from 'primeng/dropdown';
import { CreatingStaffDto } from '../../../../domain/CreatingStaffDto';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-create-staff-profile',
  standalone: true,
  imports: [
    DialogModule,
    ButtonModule,
    InputTextModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    DropdownModule,
    ToastModule
  ],
  providers: [MessageService],
  templateUrl: './modal-create-staff-profile.component.html',
  styleUrl: './modal-create-staff-profile.component.scss'
})
export class CreateStaffProfileComponent {
  staffForm: FormGroup;
  @Output() staffProfileCreated = new EventEmitter<CreatingStaffDto>();

  constructor(private staffService: StaffService, private specializationService: SpecializationService, private fb: FormBuilder,
    private messageService: MessageService
  ) {
    this.staffForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      fullName: ['', Validators.required],
      email: ['', Validators.required],
      phoneNumber: ['', Validators.required],
      licenseNumber: ['', Validators.required],
      specialization: [null, [Validators.required]],
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
          this.staffProfileCreated.emit(staff);
          this.messageService.add({severity: 'success', summary: 'Success', detail: 'Staff profile create successfully', life: 2000});
        },
        (error) => {
          console.error("Error creating staff profile:", error);
          this.messageService.add({severity: 'error', summary: 'Error', detail: error, life: 2000});
        }
      );
    } else {
      console.warn("Form is invalid!");
    }
  }
}
