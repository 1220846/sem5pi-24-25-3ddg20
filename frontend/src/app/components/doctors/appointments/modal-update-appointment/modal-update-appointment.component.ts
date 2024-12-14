  import { CommonModule } from '@angular/common';
  import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
  import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
  import { MessageService } from 'primeng/api';
  import { Appointment } from '../../../../domain/Appointment';
  import { UpdateAppointmentDto } from '../../../../domain/UpdateAppointmentDto';
  import { AppointmentService } from '../../../../services/appointment.service';
  import { SurgeryRoomService } from '../../../../services/surgery-room.service';
  import { StaffService } from '../../../../services/staff.service';
  import { SurgeryRoom } from '../../../../domain/SurgeryRoom';
  import { Staff } from '../../../../domain/Staff';
  import { ButtonModule } from 'primeng/button';
  import { Dialog, DialogModule } from 'primeng/dialog';
  import { ToastModule } from 'primeng/toast';
  import { Dropdown, DropdownModule } from 'primeng/dropdown';
  import { MultiSelectModule } from 'primeng/multiselect';
  import { CalendarModule } from 'primeng/calendar';
import { InputText, InputTextModule } from 'primeng/inputtext';

  @Component({
    selector: 'app-modal-update-appointment',
    standalone: true,
    imports: [ButtonModule,DialogModule,ToastModule,FormsModule,ReactiveFormsModule,DropdownModule,MultiSelectModule,CalendarModule,CommonModule,InputTextModule],
    providers: [MessageService],  
    templateUrl: './modal-update-appointment.component.html',
    styleUrl: './modal-update-appointment.component.scss'
  })
  export class ModalUpdateAppointmentComponent implements OnInit,OnChanges{

    @Output() appointmentUpdated = new EventEmitter<Appointment>();
    @Input() appointment: Appointment | null = null;

    originalValues: any;
    appointmentForm: FormGroup;

    surgeryRooms: SurgeryRoom[] = [];
    staffs: Staff[] = [];
    minDate : Date = new Date();
    isEditing = true;

    constructor(
      private appointmentService: AppointmentService,
      private fb: FormBuilder,
      private messageService: MessageService, private surgeryRoomService: SurgeryRoomService,
    private staffService: StaffService)
    {
      this.appointmentForm = this.fb.group({
        surgeryRoomNumber: [null, Validators.required],
        dateAndTime: [null, Validators.required],
        staffsIds: [[], Validators.required],
        operationRequest:[{value: '', disabled: true}]
      });
    }

    visible: boolean = false;

    ngOnInit(): void {
      
    }

    showDialog() {
      this.originalValues = this.appointmentForm.value;
      this.visible = true;
    }

    closeDialog() {
      this.visible = false;
      this.appointmentForm.reset(this.originalValues);
    }

    ngOnChanges(changes: SimpleChanges): void {
      if (changes['appointment']) {
        Promise.resolve().then(() => {
          if (this.appointment?.surgeryRoomDto) {
            this.appointmentForm.patchValue({
              surgeryRoomNumber: this.appointment.surgeryRoomDto.number,
              dateAndTime: new Date(this.appointment.dateAndTime),
              staffsIds: this.appointment.team.map(staff => staff.id),  
              operationRequest: `${this.appointment.operationRequestDto.medicalRecordNumber} - ${this.appointment.operationRequestDto.operationType.name}`
            });
          }
          this.loadOSurgeryRooms();
          this.loadStaffs();
        });
      }
    }
    loadOSurgeryRooms() {
      this.surgeryRoomService.getAll().subscribe((data) => {
        this.surgeryRooms = data;
      });
    }

    loadStaffs(): void {
      this.staffService.getAll().subscribe({
        next: (data) => { this.staffs = data; },
        error: (error) => console.error('Error fetching staffs:', error)
      });
      console.log(this.staffs);
    }

    saveData() {
      if (this.appointmentForm.valid && this.appointment) {
        const formData: UpdateAppointmentDto = {
          surgeryRoomId: this.appointmentForm.get('surgeryRoomNumber')?.value === this.originalValues?.surgeryRoomNumber ? null : this.appointmentForm.get('surgeryRoomNumber')?.value,
          dateAndTime: this.appointmentForm.get('dateAndTime')?.value === this.originalValues?.dateAndTime ? null : this.appointmentForm.get('dateAndTime')?.value,
          staffsIds: this.appointmentForm.get('staffsIds')?.value === this.originalValues?.staffsIds ? null : this.appointmentForm.get('staffsIds')?.value
        };
    
        this.appointmentService.updateAppointment(this.appointment.id, formData).subscribe({
          next: (appointment) => {
            this.messageService.add({ severity: 'success', summary: 'Updated successfully', detail: 'Appointment data was updated with success' });
            this.appointmentUpdated.emit(appointment);
            setTimeout(() => {
              this.closeDialog();
            }, 4000);
          },
          error: (error) => {
            this.messageService.add({ severity: 'error', summary: 'Error', detail: error, life: 3000 });
          }
        });
      } else {
        this.messageService.add({ severity: 'warn', summary: 'Warming', detail: 'Invalid Data in forms!' });
      }
    }
    updateMinDateOnFocus() {
      this.minDate = new Date();
    }
  }

