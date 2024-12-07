import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { DropdownModule } from 'primeng/dropdown';
import { MultiSelectModule } from 'primeng/multiselect';
import { CreatingAppointmentDto } from '../../../../domain/CreatingAppointmentDto';
import { OperationRequestService } from '../../../../services/operation-request.service';
import { AppointmentService } from '../../../../services/appointment.service';
import { SurgeryRoom } from '../../../../domain/SurgeryRoom';
import { SurgeryRoomService } from '../../../../services/surgery-room.service';
import { ToastModule } from 'primeng/toast';
import { CalendarModule } from 'primeng/calendar';
import { User } from '../../../../domain/User';
import { OperationRequestWithAllDataDto } from '../../../../domain/OperationRequestWithAllDataDto';
import { StaffService } from '../../../../services/staff.service';
import { Staff } from '../../../../domain/Staff';

@Component({
  selector: 'app-modal-create-appointment',
  standalone: true,
  imports: [MultiSelectModule, DropdownModule, CommonModule, ButtonModule, DialogModule, ToastModule, ReactiveFormsModule,CalendarModule],
  providers: [MessageService],
  templateUrl: './modal-create-appointment.component.html',
  styleUrl: './modal-create-appointment.component.scss'
})
export class ModalCreateAppointmentComponent implements OnChanges {

  appointmentForm: FormGroup;
  @Output() appointmentCreated = new EventEmitter<CreatingAppointmentDto>();
  @Input()user: User | null = null;
  doctorId: string | null = null;
  minDate : Date = new Date();

  constructor(private operationRequestService: OperationRequestService, private surgeryRoomService: SurgeryRoomService, private appointmentService: AppointmentService, private staffService :StaffService,private fb: FormBuilder,
    private messageService: MessageService) {
    this.appointmentForm = this.fb.group({
      operationRequest: [null, Validators.required],
      surgeryRoom: [null, Validators.required],
      date: [null, Validators.required],
      staffs: [[], Validators.required]
    });
  }

  operationRequests: OperationRequestWithAllDataDto[] = [];
  surgeryRooms: SurgeryRoom[] = [];
  staffs: Staff[] = [];
  visible: boolean = false;
  submitted: boolean = false;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['user'] && this.user) {
      this.doctorId = this.user.username.split('@')[0];
      this.loadOperationRequests();
      this.loadOSurgeryRooms();
      this.loadStaffs();
    }
  }

  loadOperationRequests() {
    this.operationRequestService.getByDoctorAndStatus(this.doctorId!, "WAITING").subscribe((data) => {
      this.operationRequests = data.map(operation => ({
        ...operation,
        label: `${operation.medicalRecordNumber} - ${operation.operationType.name}`
      }));
    });
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

  updateMinDateOnFocus() {
    this.minDate = new Date();
  }

  showDialog() {
    this.visible = true;
    this.appointmentForm.reset();
    this.submitted = false;
    this.loadOperationRequests();
  }

  saveData() {
    if (this.appointmentForm.valid) {
      const appointment: CreatingAppointmentDto = {
        surgeryRoomId: this.appointmentForm.value.surgeryRoom.number,
        operationRequestId: this.appointmentForm.value.operationRequest.id,
        dateAndTime: this.appointmentForm.value.date,
        staffsIds: this.appointmentForm.value.staffs.map((staff: any) => staff.id)
      };

      this.appointmentService.add(appointment).subscribe(
        (response) => {
          this.visible = false; this.appointmentForm.reset();
          this.submitted = false;
          this.appointmentCreated.emit(appointment);
          this.loadOperationRequests();
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Operation type added successfully', life: 2000 });
        },
        (error) => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: error, life: 3000 });
        }
      );
    } else {
      this.messageService.add({ severity: 'warn', summary: 'Warning', detail: 'Invalid form data!', life: 3000 });
    }
  }
}
