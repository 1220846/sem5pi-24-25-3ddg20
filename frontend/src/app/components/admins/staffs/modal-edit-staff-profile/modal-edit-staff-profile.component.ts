import { CommonModule } from '@angular/common';
import { AbstractType, Component, EventEmitter, Input, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';
import { ToastModule } from 'primeng/toast';
import { CalendarModule } from 'primeng/calendar';
import { EditingStaffDto } from '../../../../domain/EditingStaffDto';
import { StaffService } from '../../../../services/staff.service';
import { MessageService } from 'primeng/api';
import { SpecializationService } from '../../../../services/specialization.service';
import { Specialization } from '../../../../domain/specialization';
import { Staff } from '../../../../domain/Staff';
import { startWith } from 'rxjs';

@Component({
  selector: 'app-edit-staff-profile',
  standalone: true,
  imports: [    
    DialogModule,
    ButtonModule,
    InputTextModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    DropdownModule,
    ToastModule,
    CalendarModule],
  providers: [MessageService],
  templateUrl: './modal-edit-staff-profile.component.html',
  styleUrl: './modal-edit-staff-profile.component.scss'
})
export class ModalEditStaffProfileComponent {
  staffForm: FormGroup;
  addAvailabilitySlotForm: FormGroup;
  removeAvailabilitySlotForm: FormGroup;

  @Output() staffProfileEdited = new EventEmitter();
  @Input() staff: Staff | null = null;

  constructor(
    private staffService: StaffService,
    private specializationService: SpecializationService,
    private fb: FormBuilder,
    private messageService: MessageService)
  {
      this.staffForm = this.fb.group({
        email: ['', [(control: AbstractControl) => !control.value || /^[^@\s]+@[^@\s]+\.[^@\s]+$/.test(control.value) ? null : { invalidEmail: true }]],
        phoneNumber: ['', [(control: AbstractControl) => !control.value || /^9[1236]\d{7}$/.test(control.value) ? null : { invalidPattern: true }]],
        specialization: [null]
      })
    this.addAvailabilitySlotForm = this.fb.group({
      startTime: [null, [Validators.required]],
      endTIme: [null, [Validators.required]]
    },
    {
      validators: (formGroup) => {
        const startTime = formGroup.value.startTime;
        const endTime = formGroup.value.endTime;
  
        if (startTime && endTime && startTime >= endTime) {
          return { startTimeAfterEndTime: true };
        }
        return null;
      }
    })
    this.removeAvailabilitySlotForm = this.fb.group({
      availabilitySlotId: [null, [Validators.required]]
    })
  }

  specializations: Specialization[] = [];
  availableSpecializations: Specialization[] = [];
  visible: boolean = false;
  changes: boolean = false;


  ngOnInit(): void {
    this.loadSpecializations();
    if (this.staff?.availabilitySlots) {
      this.staff.availabilitySlots = this.staff.availabilitySlots.map((availabilitySlot) => ({
        ...availabilitySlot,
        name: `${new Date(availabilitySlot.startTime).toLocaleString()} - ${new Date(availabilitySlot.endTime).toLocaleString()}`
      }));
    }
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
    this.addAvailabilitySlotForm.reset();
    this.removeAvailabilitySlotForm.reset();
  }

  closeDialog() {
    if (this.changes) {
      this.staffProfileEdited.emit();
      this.changes = false;
    }
    this.visible = false;
  }

  changeInfo() {
    if (this.staffForm.valid) {
      const staff: EditingStaffDto = {
        email: this.staffForm.value.email ? this.staffForm.value.email : null,
        phoneNumber: this.staffForm.value.phoneNumber ? this.staffForm.value.phoneNumber : null,
        specializationId: this.staffForm.value.specialization ? this.staffForm.value.specialization.id : null,
        newAvailabilitySlotStartTime: null,
        newAvailabilitySlotEndTime: null,
        toRemoveAvailabilitySlotId: null
      }
      this.staffService.edit(this.staff?.id!, staff).subscribe(
        (response) => {
          this.changes = true;
          this.messageService.add({severity: 'success', summary: 'Success', detail: 'Staff profile edited successfully', life: 2000});
        },
        (error) => {
          console.error("Error editing staff profile:", error);
          this.messageService.add({severity: 'error', summary: 'Error', detail:'Failed to edit staff profile', life: 2000});
        }
      );
    } else {
      this.messageService.add({severity: 'warn', summary: 'Error', detail:'Some inputted data is invalid', life: 2000});
    }
  }

  addAvailabilitySlot() {
    //TODO
  }

  removeAvailabilitySlot() {
    //TODO
  }
}
