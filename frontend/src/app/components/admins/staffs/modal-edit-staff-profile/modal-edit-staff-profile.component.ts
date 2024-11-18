import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
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

  @Output() staffProfileEdited = new EventEmitter<EditingStaffDto>();
  @Input() staff: Staff | null = null;

  constructor(
    private staffService: StaffService,
    private specializationService: SpecializationService,
    private fb: FormBuilder,
    private messageService: MessageService)
  {
    this.staffForm = this.fb.group({
      email: [{ value: this.staff?.email },[Validators.email]],
      phoneNumber: [{ value: this.staff?.phoneNumber }, [Validators.required, Validators.pattern('^9[1236]\\d{7}$')]],
      specialization: [{ value: this.staff?.specialization.name }]
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

  changeInfo() {
    //TODO
  }

  addAvailabilitySlot() {
    //TODO
  }

  removeAvailabilitySlot() {
    //TODO
  }
}
